using System.IO;
using System.Timers;
using WebScrapper;

namespace Elki
{
    internal class Weather : ElkiTimer
    {
        public Weather(double dt) : base(dt)
        {

        }

        protected override void OnTimer(object source, ElapsedEventArgs e)
        {
            WeatherScrap webScrapper = new WeatherScrap("https://www.gismeteo.ru/weather-fryazino-12648/");
            webScrapper.RunScrapper();    
            File.WriteAllBytes("image.jpg", webScrapper.Image);
        }
    }
}
