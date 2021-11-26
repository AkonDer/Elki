using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Elki
{
    internal class Program
    {
        private static void Main()
        {
            // start pogram
            Console.WriteLine("Start programm in " + DateTime.Now);
            Trace.Listeners.Add(new TextWriterTraceListener(File.CreateText("log.txt")));
            Trace.AutoFlush = true;

            List<ElkiTimer> timers = new List<ElkiTimer>()
            {
                new Birthdays(10000, @"resources\emp.xlsx"),
                new Clocks(1000),
                new Elki(1000, @"resources\data.txt"),
                new Holidays(60000, @"resources\holidays.xlsx")
            };
            foreach (var timer in timers) timer.StartTimer();
        }
    }
}