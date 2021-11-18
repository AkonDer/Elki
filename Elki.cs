using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Elki
{
    internal class Elki : ElkiTimer
    {
        public Elki(double dt) : base(dt) { }

        protected override void onTimer(object source, ElapsedEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Электрички");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }    
}
