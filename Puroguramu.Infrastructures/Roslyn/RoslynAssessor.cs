using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.Logging;
using Puroguramu.Domains;
using Puroguramu.Domains.Models;
using Puroguramu.Domains.Repositories;

namespace Puroguramu.Infrastructures.Roslyn;

public class RoslynAssessor : IAssessExercise
{
    private readonly IExerciseRepository _exerciseRepository;
    private readonly ILogger<RoslynAssessor> _logger;
    private const int ExecutionTimeoutSeconds = 5;
    private const int MaxCodeLength = 10000;

    private static readonly ScriptOptions Options = ScriptOptions.Default
        .WithImports("System", "System.Linq", "Puroguramu.Domains.Models")
        .WithReferences(typeof(Exercise).Assembly)
        .WithOptimizationLevel(Microsoft.CodeAnalysis.OptimizationLevel.Release);

    private static readonly string[] ForbiddenNamespaces = new[]
    {
        "System.IO",
        "System.Net",
        "System.Reflection",
        "System.Runtime",
        "System.Diagnostics",
        "System.Threading.Tasks",
        "System.Environment",
        "System.Security",
        "System.Web",
        "Microsoft.Win32"
    };

    private static readonly string[] ForbiddenKeywords = new[]
    {
        "File.",
        "Directory.",
        "Process.",
        "Thread.",
        "Task.",
        "Environment.",
        "Assembly.",
        "Type.",
        "Activator.",
        "AppDomain.",
        "HttpClient",
        "WebClient",
        "Socket",
        "NetworkStream",
        "Registry",
        "unsafe",
        "fixed",
        "stackalloc"
    };

    public RoslynAssessor(IExerciseRepository repository, ILogger<RoslynAssessor> logger)
    {
        _exerciseRepository = repository;
        _logger = logger;
    }

    public async Task<ExerciseResult> Assess(int exerciseId, string proposal)
    {
        _logger.LogDebug("Assessing exercise {ExerciseId}", exerciseId);

        var exercise = _exerciseRepository.GetExercise(exerciseId);
        if (exercise == null)
        {
            _logger.LogWarning("Exercise {ExerciseId} not found", exerciseId);
            throw new InvalidOperationException($"Exercise with ID {exerciseId} not found");
        }

        if (proposal?.Length > MaxCodeLength)
        {
            _logger.LogWarning("Code submission too long for exercise {ExerciseId}: {Length} characters", exerciseId, proposal.Length);
            return new ExerciseResult(exercise, proposal, new[]
            {
                new TestResult("Validation Error", TestStatus.Inconclusive,
                    $"Le code soumis est trop long ({proposal.Length} caractères, maximum {MaxCodeLength})")
            });
        }

        var codeToRun = exercise.InjectIntoTemplate(proposal);

        var securityValidation = ValidateCodeSecurity(codeToRun);
        if (!securityValidation.IsValid)
        {
            _logger.LogWarning("Security validation failed for exercise {ExerciseId}: {Reason}", exerciseId, securityValidation.Reason);
            return new ExerciseResult(exercise, proposal, new[]
            {
                new TestResult("Security Error", TestStatus.Inconclusive, securityValidation.Reason)
            });
        }

        _logger.LogDebug("Executing code for exercise {ExerciseId}", exerciseId);

        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(ExecutionTimeoutSeconds));

            ScriptState<TestResult[]> run = await CSharpScript.RunAsync<TestResult[]>(
                codeToRun,
                Options,
                cancellationToken: cts.Token);

            _logger.LogInformation("Exercise {ExerciseId} assessed successfully", exerciseId);

            return new ExerciseResult(exercise, proposal, run.ReturnValue);
        }
        catch (CompilationErrorException ex)
        {
            _logger.LogDebug("Compilation error for exercise {ExerciseId}", exerciseId);

            return new ExerciseResult(exercise, proposal,
                ex.Diagnostics.Select(d => new TestResult("Compilation Error", TestStatus.Inconclusive, d.ToString())));
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Execution timeout for exercise {ExerciseId}", exerciseId);

            return new ExerciseResult(exercise, proposal, new[]
            {
                new TestResult("Timeout", TestStatus.Inconclusive,
                    $"L'exécution a dépassé le temps limite de {ExecutionTimeoutSeconds} secondes")
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while assessing exercise {ExerciseId}", exerciseId);

            return new ExerciseResult(exercise, proposal, new[]
            {
                new TestResult("Runtime Error", TestStatus.Inconclusive,
                    "Une erreur inattendue s'est produite lors de l'exécution")
            });
        }
    }

    public async Task<ExerciseResult> AssessForTest(string modele, string solution)
    {
        _logger.LogDebug("Assessing exercise for test with model length {ModelLength}", modele?.Length ?? 0);

        if (string.IsNullOrEmpty(modele) || string.IsNullOrEmpty(solution))
        {
            _logger.LogWarning("Invalid test parameters: model or solution is empty");
            throw new ArgumentException("Le modèle et la solution ne peuvent pas être vides");
        }

        if (solution.Length > MaxCodeLength)
        {
            _logger.LogWarning("Test solution too long: {Length} characters", solution.Length);
            throw new ArgumentException($"La solution est trop longue ({solution.Length} caractères, maximum {MaxCodeLength})");
        }

        var exercise = new Exercise
        {
            Modele = modele,
            Solution = solution
        };

        var codeToRun = InjectIntoTemplateForTest(exercise, solution);

        var securityValidation = ValidateCodeSecurity(codeToRun);
        if (!securityValidation.IsValid)
        {
            _logger.LogWarning("Security validation failed for test: {Reason}", securityValidation.Reason);
            return new ExerciseResult(exercise, solution, new[]
            {
                new TestResult("Security Error", TestStatus.Inconclusive, securityValidation.Reason)
            });
        }

        _logger.LogDebug("Executing test code");

        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(ExecutionTimeoutSeconds));

            ScriptState<TestResult[]> run = await CSharpScript.RunAsync<TestResult[]>(
                codeToRun,
                Options,
                cancellationToken: cts.Token);

            _logger.LogInformation("Test exercise assessed successfully");

            return new ExerciseResult(exercise, solution, run.ReturnValue);
        }
        catch (CompilationErrorException ex)
        {
            _logger.LogDebug("Compilation error for test exercise");

            return new ExerciseResult(exercise, solution,
                ex.Diagnostics.Select(d => new TestResult("Compilation Error", TestStatus.Inconclusive, d.ToString())));
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Execution timeout for test exercise");

            return new ExerciseResult(exercise, solution, new[]
            {
                new TestResult("Timeout", TestStatus.Inconclusive,
                    $"L'exécution a dépassé le temps limite de {ExecutionTimeoutSeconds} secondes")
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while assessing test exercise");

            return new ExerciseResult(exercise, solution, new[]
            {
                new TestResult("Runtime Error", TestStatus.Inconclusive,
                    "Une erreur inattendue s'est produite lors de l'exécution")
            });
        }
    }

    public string InjectIntoTemplateForTest(Exercise exercise, string solution)
        => exercise.Modele.Replace("// code-insertion-point", solution);

    public async Task<ExerciseResult> StubForExercise(int exerciseId)
    {
        var exercise = _exerciseRepository.GetExercise(exerciseId);
        if (exercise == null)
        {
            _logger.LogWarning("Exercise {ExerciseId} not found for stub", exerciseId);
            throw new InvalidOperationException($"Exercise with ID {exerciseId} not found");
        }

        return await Task.FromResult(new ExerciseResult(exercise, exercise.Stub));
    }

    private (bool IsValid, string Reason) ValidateCodeSecurity(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return (false, "Le code ne peut pas être vide");
        }

        foreach (var forbiddenNamespace in ForbiddenNamespaces)
        {
            if (code.Contains($"using {forbiddenNamespace}", StringComparison.OrdinalIgnoreCase))
            {
                return (false, $"L'utilisation du namespace '{forbiddenNamespace}' est interdite pour des raisons de sécurité");
            }
        }

        foreach (var forbiddenKeyword in ForbiddenKeywords)
        {
            if (code.Contains(forbiddenKeyword, StringComparison.OrdinalIgnoreCase))
            {
                return (false, $"L'utilisation de '{forbiddenKeyword}' est interdite pour des raisons de sécurité");
            }
        }

        var whileCount = System.Text.RegularExpressions.Regex.Matches(code, @"\bwhile\s*\(").Count;
        var forCount = System.Text.RegularExpressions.Regex.Matches(code, @"\bfor\s*\(").Count;

        if (whileCount + forCount > 10)
        {
            return (false, "Le code contient un nombre suspect de boucles (maximum 10)");
        }

        return (true, string.Empty);
    }
}
