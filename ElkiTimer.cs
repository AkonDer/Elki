using System;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Elki
{
    /// <summary>
    /// Класс запускает та
    /// </summary>
    abstract class ElkiTimer
    {
        private Thread t;
        private Timer timer;
        private readonly double delayTime;        

        public ElkiTimer(double dt)
        {
            delayTime = dt;
            
        }

        protected virtual void OnTimer(object source, ElapsedEventArgs e)
        {
        }

        public void StartTimer()
        {
            t = new Thread(e =>
            {
                timer = new Timer(delayTime);
                timer.Elapsed += OnTimer;
                timer.AutoReset = true;
                timer.Enabled = true;

                Console.ReadLine();
                timer.Stop();
                timer.Dispose();
            });
            t.Start();
        }
    }

}
