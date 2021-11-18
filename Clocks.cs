using System;
using System.Timers;

namespace Elki
{
    internal class Clocks : ElkiTimer
    {
        public Clocks(double dt) : base(dt) { }

        protected override void onTimer(object source, ElapsedEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Часы");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
