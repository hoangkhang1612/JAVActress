using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAVActress
{
    public class MoviesResult
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("result")]
        public List<Movies> Result { get; set; }
    }

    public class Movies
    {
        [JsonProperty("siteUrl")]
        public string SiteUrl { get; set; }

        [JsonIgnore]
        public string Code { 
            get 
            {
                //cid=
                string[] _c = SiteUrl.Split(new string[] { "/cid=" }, StringSplitOptions.RemoveEmptyEntries);
                if(_c.Length>=2)
                {
                    string[] _c2 = _c[1].Split('/');

                    return _c2[0];
                }

                return "no code!";
            } 
        }

        [JsonProperty("date")]
        public string Date { get; set; }


        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
