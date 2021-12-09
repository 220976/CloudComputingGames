using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AnalysisService.Models
{
    public class Authentication
    {
        public static bool IsAuthenticate(string username,string token)
        {
            try
            {

                string html = string.Empty;
                string url = @"http://DBUsers/User/IsSignIn";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("UserName", username);
                request.Headers.Add("Token", token);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                    return Convert.ToBoolean(html);
                }
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}
