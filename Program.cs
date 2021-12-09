using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using WebScrapper;

namespace Elki
{
    internal class Program
    {
        private static void Main()
        {
            //WeatherScrap webScrapper = new WeatherScrap("https://www.gismeteo.ru/weather-fryazino-12648/");
            //webScrapper.RunScrapper();
            //File.WriteAllBytes("image.jpg", webScrapper.Image);

            // start pogram
            Console.WriteLine("Start programm in " + DateTime.Now);

            Trace.Listeners.Add(new TextWriterTraceListener(File.CreateText("log.txt")));  // Логирование работы программы
            Trace.AutoFlush = true;

            List<ElkiTimer> timers = new List<ElkiTimer>()
            {
                new Birthdays(10000, @"resources\emp.xlsx"),
                new Clocks(1000),
                new Elki(1000, @"resources\data.txt"),
                new TimeUntilNewYear(1000),
                new Holidays(60000, @"resources\holidays.xlsx")
            };
            foreach (var timer in timers) timer.StartTimer();
        }
    }
}