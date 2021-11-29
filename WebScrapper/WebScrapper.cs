using System;
using System.Net.Http;

namespace WebScrapper // Note: actual namespace depends on the project name.
{
    public abstract class WebScrapper
    {
        public string _uri;

        public WebScrapper(string uri)
        {
            _uri = uri;
        }

        public virtual void RunScrapper()
        {

        }

        public string LoadHtmlForScrapper(string url)
        {
            HttpClient client = new HttpClient();
            try
            {
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    return responseBody;
                } 
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return null;
        }
    }
}
