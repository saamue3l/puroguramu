using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Puroguramu.Domains;
using Puroguramu.Domains.Models;
using Puroguramu.Domains.Repositories;

namespace Puroguramu.Infrastructures.Roslyn;

public class RoslynAssessor : IAssessExercise
{
    private readonly IExerciseRepository _exerciseRepository;

    private static readonly ScriptOptions Options = ScriptOptions.Default
        .WithImports("System", "System.Linq", "Puroguramu.Domains.Models")
        .WithReferences(typeof(Exercise).Assembly);


    public RoslynAssessor(IExerciseRepository repository)
    {
        _exerciseRepository = repository;
    }

    public async Task<ExerciseResult> Assess(int exerciseId, string proposal)
    {
        var exercise = _exerciseRepository.GetExercise(exerciseId);
        var codeToRun = exercise.InjectIntoTemplate(proposal);
        Console.WriteLine("CODE TO RUN: ");
        Console.WriteLine(codeToRun);
        Console.WriteLine("FIN CODE RUN");

        try
        {
            ScriptState<TestResult[]> run = await CSharpScript.RunAsync<TestResult[]>(
                codeToRun,
                Options);

            Console.WriteLine("RETURN VALUE: " + run.ReturnValue);

            return new ExerciseResult(exercise, proposal, run.ReturnValue);
        }
        catch (CompilationErrorException ex)
        {
            Console.WriteLine("ERROR CODE ZEBI: " + ex.Message);

            return new ExerciseResult(exercise, proposal,
                ex.Diagnostics.Select(d => new TestResult("Compilation Error", TestStatus.Inconclusive, d.ToString())));
        }
    }

    public async Task<ExerciseResult> AssessForTest(string modele, string solution)
    {
        Console.WriteLine("MODELE " + modele);
        Console.WriteLine("Solution " + solution);

        Console.WriteLine("AVANT CREATION");

        var exercise = new Exercise
        {
            Modele = modele,
            Solution = solution
        };
        Console.WriteLine("APRES CREATION");

        if(exercise == null)
        {
            Console.WriteLine("EXERCICE NULL");
        }
        else
        {
            Console.WriteLine("EXERCICE PAS NULL");
        }
        Console.WriteLine("MODELE " + exercise.Modele);
        Console.WriteLine("Solution " + exercise.Solution);
        var codeToRun = InjectIntoTemplateForTest(exercise, solution);
        Console.WriteLine("CODE TO RUN: ");
        Console.WriteLine(codeToRun);
        Console.WriteLine("FIN CODE RUN");

        try
        {
            ScriptState<TestResult[]> run = await CSharpScript.RunAsync<TestResult[]>(
                codeToRun,
                Options);

            Console.WriteLine("RETURN VALUE: " + run.ReturnValue);

            return new ExerciseResult(exercise, solution, run.ReturnValue);
        }
        catch (CompilationErrorException ex)
        {
            Console.WriteLine("ERROR CODE ZEBI: " + ex.Message);

            return new ExerciseResult(exercise, solution,
                ex.Diagnostics.Select(d => new TestResult("Compilation Error", TestStatus.Inconclusive, d.ToString())));
        }
    }

    public string InjectIntoTemplateForTest(Exercise exercise, string solution)
        => exercise.Modele.Replace("// code-insertion-point", solution);


    public async Task<ExerciseResult> StubForExercise(int exerciseId)
    {
        var exercise = _exerciseRepository.GetExercise(exerciseId);

        return await Task.FromResult(new ExerciseResult(exercise, exercise.Stub));
    }
}
