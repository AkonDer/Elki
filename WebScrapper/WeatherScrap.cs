using CoreHtmlToImage;
using HtmlAgilityPack;
using System;

namespace WebScrapper
{
    public class WeatherScrap : WebScrapper
    {
        public string Weather { get; set; }
        public string Temperature { get; set; }
        public byte[] Image { get; set; }
        public WeatherScrap(string uri) : base(uri)
        {
            _uri = uri;
        }

        public override void RunScrapper()
        {
            HtmlDocument doc = new HtmlDocument();

            if (LoadHtmlForScrapper(_uri) != null)
            {
                try
                {
                    doc.LoadHtml(LoadHtmlForScrapper(_uri));

                    var node = doc.DocumentNode.SelectSingleNode("//a[@data-text and @href='/weather-fryazino-12648/now/']");
                    var weather = node.SelectSingleNode("//a[@data-text]").Attributes["data-text"];
                    var temp = node.SelectSingleNode("/html/body/section/div[2]/div/div[1]/div/div[2]/div[1]/div[1]/a[1]/div/div[1]/div[3]/div[1]/span[1]/span");
                    var image = node.SelectSingleNode("/html/body/section/div[2]/div/div[1]/div/div[2]/div[1]/div[1]/a[1]/div/div[2]/div");

                    HtmlConverter converter = new HtmlConverter();
                    byte[] bytes = converter.FromHtmlString(image.OuterHtml.Trim());

                    Weather = weather.Value.Trim();
                    Temperature = temp.InnerText.Trim();
                    Image = bytes;
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
        }
    }
}
