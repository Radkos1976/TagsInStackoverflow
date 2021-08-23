
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TagsInStackoverflow.Model
{

    public class GetFromSOF 
    {
        /// <summary>
        /// Number of tags for retieve from SOF
        /// </summary>
        public long Number_Tags { get; set; }
        private int Max_pages { get; set; }
        public bool Run_backoff { get; set; }
        public int Backoff_interval { get; set; }
        /// <summary>
        /// Initialize
        /// </summary>
        public GetFromSOF()
        {
            Number_Tags = 1000;
            Max_pages = Convert.ToInt32(Math.Floor(Number_Tags / 100.00));
            Run_backoff = false;
            Backoff_interval = 0;
        }
        
        // Query for get data about Tags / Autetications is not need , records are limited / parameter max controls 
        static readonly string DefMethod = "https://api.stackexchange.com//2.3/tags?order=desc&pagesize=100&sort=popular&site=stackoverflow";
        //For test static readonly string DefMethod = "http://localhost:3000/API/ValuesControllerSOF?order=desc&pagesize=100&sort=popular&site=stackoverflow";
        // One response
        /// <summary>
        /// Get One page from SOF
        /// </summary>
        /// <param name="page"></param>
        /// <returns>Lst_of_Tags => list all task rec</returns>
        private async Task<Root> GetFromAPI(int page)
        {
            HttpClient client = new();
            //HttpResponseMessage response = await client.GetAsync(DefMethod + "&page = " + page);
            using (HttpResponseMessage response = client.GetAsync(DefMethod + "&page=" + page).Result)
            {               
               
                if (response.IsSuccessStatusCode)
                {
                    var enc = Encoding.UTF8;
                    using (Stream responseStream = response.Content.ReadAsStreamAsync().Result)
                    {
                        using (var decompressedStream = new GZipStream(responseStream, CompressionMode.Decompress))
                        {
                            using (var rd = new StreamReader(decompressedStream, enc))
                            {
                                var stringContent = rd.ReadToEnd();
                                Root tmp = JsonConvert.DeserializeObject<Root>(stringContent);
                                // Manage backoff signall and stop all signals
                                if (tmp.Backoff > 0)
                                {
                                    Run_backoff = true;
                                    Backoff_interval = tmp.Backoff;
                                }
                                return tmp;
                            }
                        }
                    }                  
                    
                }
                else
                {
                    throw new ArgumentException("Value after response => " + DefMethod + "page " +
                        "=" + page + " => response Code => " + response.StatusCode.ToString());
                }
            }
        }
        /// <summary>
        /// Get All list of Tags from StackOverFlow
        /// </summary>
        /// <returns>Lst_of_Tags => list all task rec </returns>
        public async Task<List<Item>> Lst_Of_Tags()
        {           
            //Paralel Run All API Queries in ONE :)
            var tasks = new List<Task<Root>>();
            for (int i = 1; i <= Max_pages; i++)
            {
                tasks.Add(GetFromAPI(i));
                //Minimalize problem with throttle errors (30 requests per second)
                //=> set delay at 10 request per sec
                // https://api.stackexchange.com/docs/throttle
                await Task.Delay(1000 / 10);
                // Manage Backoff => Pause all planned requests
                //https://meta.stackexchange.com/questions/243773/stack-exchange-api-too-many-requests-from-this-ip-address
                if (Run_backoff)
                {
                    await Task.Delay(1000 * Backoff_interval);
                    Run_backoff = false;
                }
            }
            List < Item > tmp = (List<Item>)(await Task.WhenAll(tasks)).SelectMany(x => x.Items).ToList();
           double sumarry = tmp.Sum(x => x.Count);
            foreach (var rc in tmp)
            {
                double vs = Math.Round((rc.Count / sumarry)*100,2);
                rc.Percentage = vs;
            }            
            tmp.Sort();
            tmp.Reverse();           
            
            return tmp;          
        }
    }
}
   

      
