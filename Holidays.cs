using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Elki
{
    internal class Holidays : ElkiTimer
    {
        public Holidays(double dt) : base(dt) { }

        protected override void onTimer(object source, ElapsedEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Праздники");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
    
    /// <summary>
    /// Праздник
    /// </summary>
    class Holiday
    {
        public string Date;
        public List<string> HolDays;
    }
}
