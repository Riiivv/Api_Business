using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using Api_Business.DataTransferObject;

namespace Api_Business
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Velkommen til Quiz Programmet. Nu skal du vælge en fil med spørgsmål du gerne vil have stilt:");
                Console.WriteLine("1: API Quiz");
                Console.WriteLine("2: Vand Quiz");
                Console.WriteLine("3: Programmerings Quiz");
                Console.WriteLine("4: SQL Quiz");
                Console.WriteLine("5: Afslut");
                Console.WriteLine("Indtast dit valg:");
                string valg = Console.ReadLine();

                switch (valg)
                {
                    case "1":
                        KørApiQuiz(); // Korrigeret kald til metoden KørApiQuiz
                        break;
                    case "2":
                        KørLokaleQuiz(@"C:\Users\g12\source\repos\ConsoleApp3\ConsoleApp3\QuizVand.Json"); // Korrigeret kald til metoden KørLokaleQuiz
                        break;
                    case "3":
                        KørLokaleQuiz(@"C:\Users\g12\source\repos\ConsoleApp3\ConsoleApp3\QuizProgrammerings.json"); // Korrigeret kald til metoden KørLokaleQuiz
                        break;
                    case "4":
                        KørLokaleQuiz(@"C:\Users\g12\source\repos\ConsoleApp3\ConsoleApp3\QuizSQL.json"); // Korrigeret kald til metoden KørLokaleQuiz
                        break;
                    case "5":
                        Console.WriteLine("Farvel");
                        return;
                    default:
                        Console.WriteLine("Ugyldigt valg. Prøv igen");
                        break;
                }

                Console.WriteLine();


                 void VisDataTilBruger<T>(List<T> data)
                {
                    foreach (var item in data)
                    {
                        if (item is APIViewer apiViewer)
                        {
                            Console.WriteLine(apiViewer.question); // Brug af instansvariabel i stedet for klassens navn
                            VisApiSvar(apiViewer);
                        }
                        else if (item is QuizVand quizVand)
                        {
                            Console.WriteLine(quizVand.spørgsmål); // Brug af instansvariabel i stedet for klassens navn
                            VisLokaleSvar(quizVand);
                        }
                    }
                }

                 void VisApiSvar(APIViewer apiViewer)
                {
                    int i = 0;
                    foreach (var answerChoice in apiViewer.answers)
                    {
                        if (answerChoice.Value == null) continue;
                        Console.WriteLine(++i + ". " + answerChoice.Value);
                    }

                    string brugerSvar;
                    if (apiViewer.multiple_correct_answers.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Indtast dine svar (f.eks. a,b for flere svar):");
                        brugerSvar = Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Indtast dit svar (f.eks. a):");
                        brugerSvar = Console.ReadLine();
                    }

                    TjekBrugerSvar(apiViewer, brugerSvar);
                }

                 void VisLokaleSvar(QuizVand quizVand)
                {
                    if (quizVand == null)
                    {
                        Console.WriteLine("QuizVand-objektet er null");
                        return;
                    }

                    if (quizVand.svarmuligheder == null)
                    {
                        Console.WriteLine("Svarmuligheder er null for spørgsmålet: " + quizVand.spørgsmål);
                        return;
                    }

                    int i = 0;
                    foreach (var answer in quizVand.svarmuligheder)
                    {
                        Console.WriteLine(++i + ". " + answer);
                    }

                    Console.WriteLine("Indtast dit svar (nummer):");
                    string brugersvar = Console.ReadLine();

                    int svarIndex;
                    bool isValid = int.TryParse(brugersvar, out svarIndex);

                    if (!isValid || svarIndex < 1 || svarIndex > quizVand.svarmuligheder.Count)
                    {
                        Console.WriteLine("Ugyldigt input. Indtast venligst et gyldigt nummer.");
                        return;
                    }

                    svarIndex--;

                    if (svarIndex == quizVand.TrueAnswer)
                    {
                        Console.WriteLine("Korrekt");
                    }
                    else
                    {
                        Console.WriteLine("Forkert");
                    }
                }

                 void TjekBrugerSvar(APIViewer apiViewer, string brugersvar)
                {
                    bool erKorrekt = true;
                    var brugerSvarListe = brugersvar.Split(',');

                    foreach (var svar in brugerSvarListe)
                    {
                        string svarNøgle = "answer_" + svar.Trim() + "_correct"; // Rettet til "answer_" i stedet for "Answer_"
                        if (!apiViewer.correct_answers.ContainsKey(svarNøgle) || apiViewer.correct_answers[svarNøgle] != "true")
                        {
                            erKorrekt = false;
                            break;
                        }
                    }

                    if (erKorrekt)
                    {
                        Console.WriteLine("Korrekt");
                    }
                    else
                    {
                        Console.WriteLine("Forkert");
                    }
                }
            }
        }

    }
}
}
