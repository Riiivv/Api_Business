using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Api_Business.DataTransferObject
{
    internal class QuizVand
    { 
        public string spørgsmål {  get; set; }

        public List <string> svarmuligheder { get; set; }

        public string hint { get; set; }

        public int TrueAnswer { get; set; } = 0;


    }

     void KørLokaleQuiz(string filsti)
    {
        var lokalData = LæsQuizFraFil(filsti);
        VisDataTilBruger(lokalData);
    }

    public List<QuizVand> LæsQuizFraFil(string filsti)
    {
        string jsonindhold = File.ReadAllText(filsti);
        return JsonConvert.DeserializeObject<List<QuizVand>>(jsonindhold);
    }

}
