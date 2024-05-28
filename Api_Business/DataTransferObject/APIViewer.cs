using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Api_Business.DataTransferObject
{
    internal class APIViewer
    {
        public int id {  get; set; }

        public string question { get; set; }

        public string multiple_correct_answers { get; set; }

        public Dictionary<string, string > answers { get; set; }

        public Dictionary<string, string> correct_answers { get; set; }

    }

     string HentApiResponse(string sværhedsgrad = "easy", string grænse = "10", string kategori = "code", string id = "ID")
    {
        var client = new RestClient("https://quizapi.io/api/v1/questions");
        var request = new RestRequest();

        request.AddParameter("apiKey", "UYLc4lIkwtKwcg67EMNOCBk1susVt9b6dYmEtfjO");
        request.AddParameter("limit", grænse);
        request.AddParameter("difficulty", sværhedsgrad);
        request.AddParameter("category", kategori); // Rettet stavefejl fra "catergory" til "category"
        request.AddParameter("id", id);

        return client.Execute(request).Content;
    }

    public List<APIViewer> HentApiData()
    {
        var responseFraAPI = HentApiResponse();
        return JsonConvert.DeserializeObject<List<APIViewer>>(responseFraAPI);
    }
     void KørApiQuiz()
    {
      var apiData = HentApiData();
        VisDataTilBruger(apiData);
    }
}
