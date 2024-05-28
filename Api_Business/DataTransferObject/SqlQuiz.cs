using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api_Business.DataTransferObject
{
    internal class SqlQuiz
    {
        public string spørgsmål { get; set; }

        public List<string> svarmuligheder { get; set; }

        public string hint { get; set; }

       public int TrueAnswer { get; set; } = 0;


    }
}
