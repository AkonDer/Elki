using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Elki
{
    internal class Program
    {
        private static Timer _aTimerElki;
        private static Timer _aTimerBd;
        private static Timer _aTimerClock;
        private static Timer _aTimerHolidays;
        private static string[] _dataElki;
        private static readonly List<Employee> Employees = new List<Employee>();
        private static readonly List<Holiday> Holidays = new List<Holiday>();
        private static decimal _lists;
        private static decimal _whichlist = 1;
        /// <summary>
        /// Какая дата сегодня
        /// </summary>
        public static string dataNow;

        private static void Main()
        {
            // start pogram
            Console.WriteLine("Start programm in " + DateTime.Now);            

            List<ElkiTimer> timers = new List<ElkiTimer>()
            {
                new Birthdays(5000, "emp.xlsx"),
                new Clocks(1000),
                new Elki(1000, "data.txt"),
                new Holidays(2000, "holidays.xlsx")
            };
            foreach (var timer in timers) timer.StartTimer();
        }
    }
}