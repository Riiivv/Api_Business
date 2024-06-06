using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
            var program = new Program();
            var quizHelper = new QuizHelper();

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
                        quizHelper.KørApiQuiz();
                        break;
                    case "2":
                        quizHelper.KørLokaleQuiz(@"C:\Users\matal\source\repos\Api_Business\Api_Business\Quiz.Json");
                        break;
                    case "3":
                        quizHelper.KørLokaleQuiz(@"C:\Users\matal\source\repos\Api_Business\Api_Business\ProgrammeringsQuiz.Json");
                        break;
                    case "4":
                        quizHelper.KørLokaleQuiz(@"C:\Users\matal\source\repos\Api_Business\Api_Business\SqlQuiz.Json");
                        break;
                    case "5":
                        Console.WriteLine("Farvel");
                        return;
                    default:
                        Console.WriteLine("Ugyldigt valg. Prøv igen");
                        break;
                }

                Console.WriteLine();
            }
        }
    }

    internal class QuizHelper
    {
        public void KørApiQuiz()
        {
            var apiData = HentApiData();
            VisDataTilBruger(apiData);
        }

        public void KørLokaleQuiz(string filsti)
        {
            var lokalData = LæsQuizFraFil(filsti);
            VisDataTilBruger(lokalData);
        }

        public List<APIViewer> HentApiData()
        {
            var responseFraAPI = HentApiResponse();
            return JsonConvert.DeserializeObject<List<APIViewer>>(responseFraAPI);
        }

        private string HentApiResponse(string sværhedsgrad = "easy", string grænse = "10", string kategori = "code", string id = "ID")
        {
            var client = new RestClient("https://quizapi.io/api/v1/questions");
            var request = new RestRequest();

            request.AddParameter("apiKey", "UYLc4lIkwtKwcg67EMNOCBk1susVt9b6dYmEtfjO");
            request.AddParameter("limit", grænse);
            request.AddParameter("difficulty", sværhedsgrad);
            request.AddParameter("category", kategori);
            request.AddParameter("id", id);

            return client.Execute(request).Content;
        }

        private List<QuizVand> LæsQuizFraFil(string filsti)
        {
            string jsonindhold = File.ReadAllText(filsti);
            return JsonConvert.DeserializeObject<List<QuizVand>>(jsonindhold);
        }

        private void VisDataTilBruger<T>(List<T> data)
        {
            foreach (var item in data)
            {
                if (item is APIViewer apiViewer)
                {
                    Console.WriteLine(apiViewer.question);
                    VisApiSvar(apiViewer);
                }
                else if (item is QuizVand quizVand)
                {
                    Console.WriteLine(quizVand.spørgsmål);
                    VisLokaleSvar(quizVand);
                }
            }
        }

        private void VisApiSvar(APIViewer apiViewer)
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

        private void VisLokaleSvar(QuizVand quizVand)
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

        private void TjekBrugerSvar(APIViewer apiViewer, string brugersvar)
        {
            bool erKorrekt = true;
            var brugerSvarListe = brugersvar.Split(',');

            foreach (var svar in brugerSvarListe)
            {
                string svarNøgle = "answer_" + svar.Trim() + "_correct";
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
