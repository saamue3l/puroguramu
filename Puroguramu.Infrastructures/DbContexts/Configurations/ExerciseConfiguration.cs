using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Puroguramu.Domains;
using Puroguramu.Domains.Models;

namespace Puroguramu.Infrastructures.DbContexts.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.HasKey(e => e.IDExercice);

        builder.Property(e => e.Titre)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.Enonce).IsRequired(false);

        builder.Property(e => e.Modele).IsRequired(false);

        builder.Property(e => e.Solution).IsRequired(false);

        builder.Property(e => e.Position)
            .IsRequired();

        builder.HasOne(e => e.Lesson)
            .WithMany()
            .HasForeignKey(e => e.IDLecon);

        builder.HasOne(e => e.Difficulty)
            .WithMany()
            .HasForeignKey(e => e.IDDifficulte)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(l => l.Statut)
            .WithMany()
            .HasForeignKey(l => l.IDStatut)
            .OnDelete(DeleteBehavior.NoAction);

        SeedData(builder);
    }

    private void SeedData(EntityTypeBuilder<Exercise> builder)
    {
        builder.HasData(
            new Exercise
            {
                IDExercice = 1,
                Titre = "Exercice d'exemple de base",
                Enonce =
                    "Ceci est l'exercie qui a été donné en exemple dans le projet de base.",
                Modele =
                    "// code-insertion-point\n\npublic class Test \n{\n    public static TestResult Ensure(float b, int exponent, float expected) \n    {\n      TestStatus status = TestStatus.Passed;\n      float actual = float.NaN;\n      try \n      {\n         actual = Exercice.Power(b, exponent);\n         if(Math.Abs(actual - expected) > 0.00001f) \n         {\n             status = TestStatus.Failed;\n         }\n      } \n      catch(Exception ex) \n      {\n         status = TestStatus.Inconclusive;\n      }\n\n      return new TestResult(\n        string.Format(\"\"Power of {0} by {1} should be {2}\"\", b, exponent, expected),\n        status,\n        status == TestStatus.Passed ? string.Empty : string.Format(\"\"Expected {0}. Got {1}.\"\", expected, actual)\n      );  \n    }\n}\n\nreturn new TestResult[] {\n  Test.Ensure(2, 4, 16.0f),\n  Test.Ensure(2, -4, 1.0f/16.0f)\n};",
                Solution =
                    "Aucune solution pour celui-ci, c'est l'exercice qui a été donné en exemple dans le projet de base.",
                IDLecon = 1,
                IDDifficulte = 3,
                IDStatut = 2,
                Position = 1
            },
            new Exercise
            {
                IDExercice = 2,
                Titre = "Puissance d'un nombre",
                Enonce =
                    "Écrivez une méthode statique Power dans la classe Exercice qui prend en entrée un nombre flottant baseNumber et un entier exponent et retourne baseNumber élevé à la puissance exponent. Vous devez gérer les cas où l'exposant est négatif ou zéro.",
                Modele =
                    "// code-insertion-point\n\npublic class Test \n{\n    public static TestResult Ensure(float b, int exponent, float expected) \n    {\n        TestStatus status = TestStatus.Passed;\n        float actual = float.NaN;\n        try \n        {\n            actual = Exercice.Power(b, exponent);\n            if(Math.Abs(actual - expected) > 0.00001f) \n            {\n                status = TestStatus.Failed;\n            }\n        } \n        catch(Exception ex) \n        {\n            status = TestStatus.Inconclusive;\n        }\n\n        return new TestResult(\n            string.Format(\"Power of {0} by {1} should be {2}\", b, exponent, expected),\n            status,\n            status == TestStatus.Passed ? string.Empty : string.Format(\"Expected {0}. Got {1}.\", expected, actual)\n        );  \n    }\n}\n\nreturn new TestResult[] {\n    Test.Ensure(2, 4, 16.0f),\n    Test.Ensure(2, -4, 1.0f/16.0f),\n    Test.Ensure(10, 0, 1.0f),\n    Test.Ensure(2, 1, 2.0f),\n    Test.Ensure(3, 3, 27.0f)\n};\n",
                Solution =
                    "public class Exercice\n{\n    public static float Power(float baseNumber, int exponent)\n    {\n        if (exponent == 0) return 1.0f; // Cas de base pour exponent = 0\n        float result = 1.0f;\n        int absExponent = Math.Abs(exponent);\n        for (int i = 0; i < absExponent; i++)\n        {\n            result *= baseNumber;\n        }\n        return exponent > 0 ? result : 1.0f / result;\n    }\n}\n",
                IDLecon = 1,
                IDDifficulte = 1,
                IDStatut = 2,
                Position = 2
            },
            new Exercise
            {
                IDExercice = 3,
                Titre = "Inversion d'une chaîne",
                Enonce = "Implémentez une méthode ReverseString dans la classe Exercice qui prend une chaîne de caractères en entrée et retourne cette chaîne inversée.",
                Modele =
                    "// code-insertion-point\n\npublic class Test \n{\n    public static TestResult Ensure(string input, string expected) \n    {\n        TestStatus status = TestStatus.Passed;\n        string actual = string.Empty;\n        try \n        {\n            actual = Exercice.ReverseString(input);\n            if(actual != expected) \n            {\n                status = TestStatus.Failed;\n            }\n        } \n        catch(Exception ex) \n        {\n            status = TestStatus.Inconclusive;\n        }\n\n        return new TestResult(\n            string.Format(\"Reverse of '{0}' should be '{1}'\", input, expected),\n            status,\n            status == TestStatus.Passed ? string.Empty : string.Format(\"Expected '{0}'. Got '{1}'.\", expected, actual)\n        );  \n    }\n}\n\nreturn new TestResult[] {\n    Test.Ensure(\"hello\", \"olleh\"),\n    Test.Ensure(\"world\", \"dlrow\"),\n    Test.Ensure(\"\", \"\"),\n    Test.Ensure(\"a\", \"a\")\n};\n",
                Solution =
                    "public class Exercice\n{\n    public static string ReverseString(string input)\n    {\n        char[] charArray = input.ToCharArray();\n        Array.Reverse(charArray);\n        return new string(charArray);\n    }\n}\n",
                IDLecon = 1,
                IDDifficulte = 2,
                IDStatut = 2,
                Position = 3
            },
            new Exercise
            {
                IDExercice = 4,
                Titre = "Compteur de voyelles",
                Enonce =
                    "Créez une méthode CountVowels qui reçoit une chaîne et retourne le nombre de voyelles (a, e, i, o, u, y) dans cette chaîne. Considerer les voyelles en majuscules également.",
                Modele =
                    "// code-insertion-point\n\npublic class Test \n{\n    public static TestResult Ensure(string input, int expected) \n    {\n        TestStatus status = TestStatus.Passed;\n        int actual = 0;\n        try \n        {\n            actual = Exercice.CountVowels(input);\n            if(actual != expected) \n            {\n                status = TestStatus.Failed;\n            }\n        } \n        catch(Exception ex) \n        {\n            status = TestStatus.Inconclusive;\n        }\n\n        return new TestResult(\n            string.Format(\"Number of vowels in '{0}' should be {1}\", input, expected),\n            status,\n            status == TestStatus.Passed ? string.Empty : string.Format(\"Expected {0}. Got {1}.\", expected, actual)\n        );  \n    }\n}\n\nreturn new TestResult[] {\n    Test.Ensure(\"hello\", 2),\n    Test.Ensure(\"world\", 1),\n    Test.Ensure(\"aeiouy\", 6),\n    Test.Ensure(\"bcdfgh\", 0),\n    Test.Ensure(\"YELLOWS\", 3)\n};\n",
                Solution =
                    "public class Exercice\n{\n    public static int CountVowels(string input)\n    {\n        int count = 0;\n        string vowels = \"aeiouyAEIOUY\";\n        foreach (char c in input)\n        {\n            if (vowels.Contains(c))\n            {\n                count++;\n            }\n        }\n        return count;\n    }\n}\n",
                IDLecon = 1,
                IDDifficulte = 3,
                IDStatut = 2,
                Position = 4
            },
            new Exercise
            {
                IDExercice = 5,
                Titre = "Trouver le maximum",
                Enonce = "Implémentez une méthode FindMax qui prend un tableau d'entiers en entrée et retourne l'élément le plus grand du tableau.",
                Modele =
                    "// code-insertion-point\n\npublic class Test \n{\n    public static TestResult Ensure(int[] numbers, int expected) \n    {\n        TestStatus status = TestStatus.Passed;\n        int actual = int.MinValue;\n        try \n        {\n            actual = Exercice.FindMax(numbers);\n            if(actual != expected) \n            {\n                status = TestStatus.Failed;\n            }\n        } \n        catch(Exception ex) \n        {\n            status = TestStatus.Inconclusive;\n        }\n\n        return new TestResult(\n            string.Format(\"The maximum value in the array should be {0}\", expected),\n            status,\n            status == TestStatus.Passed ? string.Empty : string.Format(\"Expected {0}. Got {1}.\", expected, actual)\n        );  \n    }\n}\n\nreturn new TestResult[] {\n    Test.Ensure(new int[]{ 1, 2, 3, 4, 5 }, 5),\n    Test.Ensure(new int[]{ 5, 4, 3, 2, 1 }, 5),\n    Test.Ensure(new int[]{ -10, 0, 10, 20 }, 20),\n    Test.Ensure(new int[]{ }, int.MinValue) // Cas d'un tableau vide\n};\n",
                Solution =
                    "public class Exercice\n{\n    public static int FindMax(int[] numbers)\n    {\n        if (numbers.Length == 0)\n            return int.MinValue;\n        \n        int max = numbers[0];\n        foreach (int num in numbers)\n        {\n            if (num > max)\n                max = num;\n        }\n        return max;\n    }\n}\n",
                IDLecon = 2,
                IDDifficulte = 1,
                IDStatut = 2,
                Position = 1
            },
            new Exercise
            {
                IDExercice = 6,
                Titre = "Est-ce un palindrome?",
                Enonce =
                    "Développez une méthode IsPalindrome dans la classe Exercice qui prend un string comme entrée et retourne un bool indiquant si le string est un palindrome ou non (un palindrome se lit de la même manière dans les deux sens).",
                Modele =
                    "// code-insertion-point\n\npublic class Test \n{\n    public static TestResult Ensure(string input, bool expected) \n    {\n        TestStatus status = TestStatus.Passed;\n        bool actual = false;\n        try \n        {\n            actual = Exercice.IsPalindrome(input);\n            if(actual != expected) \n            {\n                status = TestStatus.Failed;\n            }\n        } \n        catch(Exception ex) \n        {\n            status = TestStatus.Inconclusive;\n        }\n\n        return new TestResult(\n            string.Format(\"'{0}' should be a palindrome: {1}\", input, expected),\n            status,\n            status == TestStatus.Passed ? string.Empty : string.Format(\"Expected {0}. Got {1}.\", expected, actual)\n        );  \n    }\n}\n\nreturn new TestResult[] {\n    Test.Ensure(\"racecar\", true),\n    Test.Ensure(\"hello\", false),\n    Test.Ensure(\"Anna\", true),\n    Test.Ensure(\"A man a plan a canal Panama\", true)\n};\n",
                Solution =
                    "public class Exercice\n{\n    public static bool IsPalindrome(string input)\n    {\n        string cleanedInput = input.ToLower().Replace(\" \", \"\").Replace(\",\", \"\").Replace(\".\", \"\");\n        char[] charArray = cleanedInput.ToCharArray();\n        Array.Reverse(charArray);\n        string reversedString = new string(charArray);\n        return cleanedInput == reversedString;\n    }\n}\n",
                IDLecon = 2,
                IDDifficulte = 2,
                IDStatut = 2,
                Position = 2
            }
        );
    }
}
