using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Puroguramu.Infrastructures.Migrations.SqlServerMigrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Difficulties",
                columns: table => new
                {
                    IdDifficulte = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Libelle = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Difficulties", x => x.IdDifficulte);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    IDGroupe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.IDGroupe);
                });

            migrationBuilder.CreateTable(
                name: "ProgressStatus",
                columns: table => new
                {
                    IDStatut = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressStatus", x => x.IDStatut);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    IDStatut = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.IDStatut);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IDGroupe = table.Column<int>(type: "int", nullable: false),
                    GroupeIDGroupe = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Groups_GroupeIDGroupe",
                        column: x => x.GroupeIDGroupe,
                        principalTable: "Groups",
                        principalColumn: "IDGroupe");
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    IDLecon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Intitule = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false),
                    IDStatut = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.IDLecon);
                    table.ForeignKey(
                        name: "FK_Lessons_Status_IDStatut",
                        column: x => x.IDStatut,
                        principalTable: "Status",
                        principalColumn: "IDStatut",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    IDExercice = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titre = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Enonce = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modele = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Solution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false),
                    IDLecon = table.Column<int>(type: "int", nullable: false),
                    IDDifficulte = table.Column<int>(type: "int", nullable: false),
                    IDStatut = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.IDExercice);
                    table.ForeignKey(
                        name: "FK_Exercises_Difficulties_IDDifficulte",
                        column: x => x.IDDifficulte,
                        principalTable: "Difficulties",
                        principalColumn: "IdDifficulte");
                    table.ForeignKey(
                        name: "FK_Exercises_Lessons_IDLecon",
                        column: x => x.IDLecon,
                        principalTable: "Lessons",
                        principalColumn: "IDLecon",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exercises_Status_IDStatut",
                        column: x => x.IDStatut,
                        principalTable: "Status",
                        principalColumn: "IDStatut");
                });

            migrationBuilder.CreateTable(
                name: "Progress",
                columns: table => new
                {
                    IDProgres = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateDerniereTentative = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CodeDerniereTentative = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IDUtilisateur = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IDStatut = table.Column<int>(type: "int", nullable: false),
                    IDExercice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Progress", x => x.IDProgres);
                    table.ForeignKey(
                        name: "FK_Progress_AspNetUsers_IDUtilisateur",
                        column: x => x.IDUtilisateur,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Progress_Exercises_IDExercice",
                        column: x => x.IDExercice,
                        principalTable: "Exercises",
                        principalColumn: "IDExercice",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Progress_ProgressStatus_IDStatut",
                        column: x => x.IDStatut,
                        principalTable: "ProgressStatus",
                        principalColumn: "IDStatut",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "IdDifficulte", "Libelle" },
                values: new object[,]
                {
                    { 1, "Facile" },
                    { 2, "Moyen" },
                    { 3, "Difficile" }
                });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "IDGroupe", "Nom" },
                values: new object[,]
                {
                    { 1, "2i1" },
                    { 2, "2i2" },
                    { 3, "2i3" },
                    { 4, "2i4" }
                });

            migrationBuilder.InsertData(
                table: "ProgressStatus",
                columns: new[] { "IDStatut", "Nom" },
                values: new object[,]
                {
                    { 1, "À faire" },
                    { 2, "En cours" },
                    { 3, "Résolu" },
                    { 4, "Abandonné" }
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "IDStatut", "Nom" },
                values: new object[,]
                {
                    { 1, "Masqué" },
                    { 2, "Publié" }
                });

            migrationBuilder.InsertData(
                table: "Lessons",
                columns: new[] { "IDLecon", "Description", "IDStatut", "Intitule", "Position" },
                values: new object[,]
                {
                    { 1, "Cette leçon couvre les bases de C#, incluant la syntaxe de base, les types de données et les méthodes simples.", 2, "Introduction à C#", 1 },
                    { 2, "Cette leçon explore les structures de contrôle en C#, telles que les boucles et les instructions conditionnelles.", 2, "Structures de contrôle en C#", 2 },
                    { 3, "Cette leçon couvre les concepts de base de la programmation orientée objet en C#.", 2, "Programmation orientée objet en C#", 3 },
                    { 4, "Cette leçon couvre la gestion des exceptions en C#.", 2, "Gestion des exceptions en C#", 4 },
                    { 5, "Cette leçon couvre la programmation asynchrone en C#.", 2, "Programmation asynchrone en C#", 5 }
                });

            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "IDExercice", "Enonce", "IDDifficulte", "IDLecon", "IDStatut", "Modele", "Position", "Solution", "Titre" },
                values: new object[,]
                {
                    { 1, "Ceci est l'exercie qui a été donné en exemple dans le projet de base.", 3, 1, 2, "// code-insertion-point\n\npublic class Test \n{\n    public static TestResult Ensure(float b, int exponent, float expected) \n    {\n      TestStatus status = TestStatus.Passed;\n      float actual = float.NaN;\n      try \n      {\n         actual = Exercice.Power(b, exponent);\n         if(Math.Abs(actual - expected) > 0.00001f) \n         {\n             status = TestStatus.Failed;\n         }\n      } \n      catch(Exception ex) \n      {\n         status = TestStatus.Inconclusive;\n      }\n\n      return new TestResult(\n        string.Format(\"\"Power of {0} by {1} should be {2}\"\", b, exponent, expected),\n        status,\n        status == TestStatus.Passed ? string.Empty : string.Format(\"\"Expected {0}. Got {1}.\"\", expected, actual)\n      );  \n    }\n}\n\nreturn new TestResult[] {\n  Test.Ensure(2, 4, 16.0f),\n  Test.Ensure(2, -4, 1.0f/16.0f)\n};", 1, "Aucune solution pour celui-ci, c'est l'exercice qui a été donné en exemple dans le projet de base.", "Exercice d'exemple de base" },
                    { 2, "Écrivez une méthode statique Power dans la classe Exercice qui prend en entrée un nombre flottant baseNumber et un entier exponent et retourne baseNumber élevé à la puissance exponent. Vous devez gérer les cas où l'exposant est négatif ou zéro.", 1, 1, 2, "// code-insertion-point\n\npublic class Test \n{\n    public static TestResult Ensure(float b, int exponent, float expected) \n    {\n        TestStatus status = TestStatus.Passed;\n        float actual = float.NaN;\n        try \n        {\n            actual = Exercice.Power(b, exponent);\n            if(Math.Abs(actual - expected) > 0.00001f) \n            {\n                status = TestStatus.Failed;\n            }\n        } \n        catch(Exception ex) \n        {\n            status = TestStatus.Inconclusive;\n        }\n\n        return new TestResult(\n            string.Format(\"Power of {0} by {1} should be {2}\", b, exponent, expected),\n            status,\n            status == TestStatus.Passed ? string.Empty : string.Format(\"Expected {0}. Got {1}.\", expected, actual)\n        );  \n    }\n}\n\nreturn new TestResult[] {\n    Test.Ensure(2, 4, 16.0f),\n    Test.Ensure(2, -4, 1.0f/16.0f),\n    Test.Ensure(10, 0, 1.0f),\n    Test.Ensure(2, 1, 2.0f),\n    Test.Ensure(3, 3, 27.0f)\n};\n", 2, "public class Exercice\n{\n    public static float Power(float baseNumber, int exponent)\n    {\n        if (exponent == 0) return 1.0f; // Cas de base pour exponent = 0\n        float result = 1.0f;\n        int absExponent = Math.Abs(exponent);\n        for (int i = 0; i < absExponent; i++)\n        {\n            result *= baseNumber;\n        }\n        return exponent > 0 ? result : 1.0f / result;\n    }\n}\n", "Puissance d'un nombre" },
                    { 3, "Implémentez une méthode ReverseString dans la classe Exercice qui prend une chaîne de caractères en entrée et retourne cette chaîne inversée.", 2, 1, 2, "// code-insertion-point\n\npublic class Test \n{\n    public static TestResult Ensure(string input, string expected) \n    {\n        TestStatus status = TestStatus.Passed;\n        string actual = string.Empty;\n        try \n        {\n            actual = Exercice.ReverseString(input);\n            if(actual != expected) \n            {\n                status = TestStatus.Failed;\n            }\n        } \n        catch(Exception ex) \n        {\n            status = TestStatus.Inconclusive;\n        }\n\n        return new TestResult(\n            string.Format(\"Reverse of '{0}' should be '{1}'\", input, expected),\n            status,\n            status == TestStatus.Passed ? string.Empty : string.Format(\"Expected '{0}'. Got '{1}'.\", expected, actual)\n        );  \n    }\n}\n\nreturn new TestResult[] {\n    Test.Ensure(\"hello\", \"olleh\"),\n    Test.Ensure(\"world\", \"dlrow\"),\n    Test.Ensure(\"\", \"\"),\n    Test.Ensure(\"a\", \"a\")\n};\n", 3, "public class Exercice\n{\n    public static string ReverseString(string input)\n    {\n        char[] charArray = input.ToCharArray();\n        Array.Reverse(charArray);\n        return new string(charArray);\n    }\n}\n", "Inversion d'une chaîne" },
                    { 4, "Créez une méthode CountVowels qui reçoit une chaîne et retourne le nombre de voyelles (a, e, i, o, u, y) dans cette chaîne. Considerer les voyelles en majuscules également.", 3, 1, 2, "// code-insertion-point\n\npublic class Test \n{\n    public static TestResult Ensure(string input, int expected) \n    {\n        TestStatus status = TestStatus.Passed;\n        int actual = 0;\n        try \n        {\n            actual = Exercice.CountVowels(input);\n            if(actual != expected) \n            {\n                status = TestStatus.Failed;\n            }\n        } \n        catch(Exception ex) \n        {\n            status = TestStatus.Inconclusive;\n        }\n\n        return new TestResult(\n            string.Format(\"Number of vowels in '{0}' should be {1}\", input, expected),\n            status,\n            status == TestStatus.Passed ? string.Empty : string.Format(\"Expected {0}. Got {1}.\", expected, actual)\n        );  \n    }\n}\n\nreturn new TestResult[] {\n    Test.Ensure(\"hello\", 2),\n    Test.Ensure(\"world\", 1),\n    Test.Ensure(\"aeiouy\", 6),\n    Test.Ensure(\"bcdfgh\", 0),\n    Test.Ensure(\"YELLOWS\", 1)\n};\n", 4, "public class Exercice\n{\n    public static int CountVowels(string input)\n    {\n        int count = 0;\n        string vowels = \"aeiouyAEIOUY\";\n        foreach (char c in input)\n        {\n            if (vowels.Contains(c))\n            {\n                count++;\n            }\n        }\n        return count;\n    }\n}\n", "Compteur de voyelles" },
                    { 5, "Implémentez une méthode FindMax qui prend un tableau d'entiers en entrée et retourne l'élément le plus grand du tableau.", 1, 2, 2, "// code-insertion-point\n\npublic class Test \n{\n    public static TestResult Ensure(int[] numbers, int expected) \n    {\n        TestStatus status = TestStatus.Passed;\n        int actual = int.MinValue;\n        try \n        {\n            actual = Exercice.FindMax(numbers);\n            if(actual != expected) \n            {\n                status = TestStatus.Failed;\n            }\n        } \n        catch(Exception ex) \n        {\n            status = TestStatus.Inconclusive;\n        }\n\n        return new TestResult(\n            string.Format(\"The maximum value in the array should be {0}\", expected),\n            status,\n            status == TestStatus.Passed ? string.Empty : string.Format(\"Expected {0}. Got {1}.\", expected, actual)\n        );  \n    }\n}\n\nreturn new TestResult[] {\n    Test.Ensure(new int[]{ 1, 2, 3, 4, 5 }, 5),\n    Test.Ensure(new int[]{ 5, 4, 3, 2, 1 }, 5),\n    Test.Ensure(new int[]{ -10, 0, 10, 20 }, 20),\n    Test.Ensure(new int[]{ }, int.MinValue) // Cas d'un tableau vide\n};\n", 1, "public class Exercice\n{\n    public static int FindMax(int[] numbers)\n    {\n        if (numbers.Length == 0)\n            return int.MinValue;\n        \n        int max = numbers[0];\n        foreach (int num in numbers)\n        {\n            if (num > max)\n                max = num;\n        }\n        return max;\n    }\n}\n", "Trouver le maximum" },
                    { 6, "Développez une méthode IsPalindrome dans la classe Exercice qui prend un string comme entrée et retourne un bool indiquant si le string est un palindrome ou non (un palindrome se lit de la même manière dans les deux sens).", 2, 2, 2, "// code-insertion-point\n\npublic class Test \n{\n    public static TestResult Ensure(string input, bool expected) \n    {\n        TestStatus status = TestStatus.Passed;\n        bool actual = false;\n        try \n        {\n            actual = Exercice.IsPalindrome(input);\n            if(actual != expected) \n            {\n                status = TestStatus.Failed;\n            }\n        } \n        catch(Exception ex) \n        {\n            status = TestStatus.Inconclusive;\n        }\n\n        return new TestResult(\n            string.Format(\"'{0}' should be a palindrome: {1}\", input, expected),\n            status,\n            status == TestStatus.Passed ? string.Empty : string.Format(\"Expected {0}. Got {1}.\", expected, actual)\n        );  \n    }\n}\n\nreturn new TestResult[] {\n    Test.Ensure(\"racecar\", true),\n    Test.Ensure(\"hello\", false),\n    Test.Ensure(\"Anna\", true),\n    Test.Ensure(\"A man a plan a canal Panama\", true)\n};\n", 2, "public class Exercice\n{\n    public static bool IsPalindrome(string input)\n    {\n        string cleanedInput = input.ToLower().Replace(\" \", \"\").Replace(\",\", \"\").Replace(\".\", \"\");\n        char[] charArray = cleanedInput.ToCharArray();\n        Array.Reverse(charArray);\n        string reversedString = new string(charArray);\n        return cleanedInput == reversedString;\n    }\n}\n", "Est-ce un palindrome?" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GroupeIDGroupe",
                table: "AspNetUsers",
                column: "GroupeIDGroupe");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Difficulties_Libelle",
                table: "Difficulties",
                column: "Libelle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_IDDifficulte",
                table: "Exercises",
                column: "IDDifficulte");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_IDLecon",
                table: "Exercises",
                column: "IDLecon");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_IDStatut",
                table: "Exercises",
                column: "IDStatut");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_IDStatut",
                table: "Lessons",
                column: "IDStatut");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_Intitule",
                table: "Lessons",
                column: "Intitule",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Progress_IDExercice",
                table: "Progress",
                column: "IDExercice");

            migrationBuilder.CreateIndex(
                name: "IX_Progress_IDStatut",
                table: "Progress",
                column: "IDStatut");

            migrationBuilder.CreateIndex(
                name: "IX_Progress_IDUtilisateur",
                table: "Progress",
                column: "IDUtilisateur");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Progress");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "ProgressStatus");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Difficulties");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "Status");
        }
    }
}
