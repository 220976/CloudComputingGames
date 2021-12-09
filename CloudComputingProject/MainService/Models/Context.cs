using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace MainService.Models
{
    public class Context
    {
        public List<Game> Games { get; set; }
        public Context()
        {
            try
            {
                Games = new List<Game>();
                string html = string.Empty;
                string url = @"http://DBGames/Data/GetData";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                    Games = JsonConvert.DeserializeObject<List<Game>>(html);
                }
            }
            catch (System.Exception)
            {

            }

        }
    }
}