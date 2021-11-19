﻿using System;
using System.Collections.Generic;

namespace Elki
{
    internal class Program
    {
        private static void Main()
        {
            // start pogram
            Console.WriteLine("Start programm in " + DateTime.Now);

            List<ElkiTimer> timers = new List<ElkiTimer>()
            {
                new Birthdays(5000, @"resources\emp.xlsx"),
                new Clocks(1000),
                new Elki(1000, @"resources\data.txt"),
                new Holidays(2000, @"resources\holidays.xlsx")
            };
            foreach (var timer in timers) timer.StartTimer();
        }
    }
}