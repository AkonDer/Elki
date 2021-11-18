using System;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Elki
{
    /// <summary>
    /// Класс запускает та
    /// </summary>
    internal class ElkiTimer
    {
        private Thread t;
        private Timer timer;
        private double delayTime;
        protected string _dataNow; // текущая дата

        public ElkiTimer(double dt)
        {
            delayTime = dt;
            _dataNow = DateTime.Now.ToString("dd.MM");
        }       

        protected virtual void onTimer(object source, ElapsedEventArgs e)
        {            
        }

        public void StartTimer()
        {
            t = new Thread(e =>
            {
                timer = new Timer(delayTime);
                timer.Elapsed += onTimer;
                timer.AutoReset = true;
                timer.Enabled = true;
            });
            t.Start();
        }
    }

}
