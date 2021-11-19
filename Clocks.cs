using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Timers;

namespace Elki
{
    internal class Clocks : ElkiTimer
    {
        public Clocks(double dt) : base(dt) { }

        protected override void onTimer(object source, ElapsedEventArgs e)
        {          
            var xCenter = 233;
            var yCenter = 111;
            var delta = 197;


            var r = 100;

            var second = DateTime.Now.Second;
            var minute = DateTime.Now.Minute;
            var hour = DateTime.Now.Hour;

            var secondAngle = second * 6 - 90;
            var minuteAngle = minute * 6 - 90;
            var hourAngleOx = (int)(Math.Round((hour - 7 + minute / 60.0) * 30) - 90);
            var hourAngleBur = (int)(Math.Round((hour - 2 + minute / 60.0) * 30) - 90);

            var b = new Bitmap(345, 422);
            using (var g = Graphics.FromImage(b))
            {
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                //var drawBrush = new SolidBrush(Color.DarkBlue);
                //var drawFont1 = new Font("Arial", 16, FontStyle.Bold);
                //var drawFont2 = new Font("Arial", 12);
                //var drawFormat = new StringFormat();

                //g.DrawString("IPG Photonics", drawFont1, drawBrush, 100, 5, drawFormat);
                //g.DrawString("Оксфорд, США", drawFont2, drawBrush, 120, 25, drawFormat);

                //g.DrawString("IPG Laser", drawFont1, drawBrush, 120, 213, drawFormat);
                //g.DrawString("Бурбах, Германия", drawFont2, drawBrush, 100, 233, drawFormat);



                var newImage = Image.FromFile("clock.png");
                g.DrawImage(newImage, 0, 0, 345, 422);


                g.DrawEllipse(new Pen(Color.Black, 6), xCenter - 3, yCenter - 3, 6, 6);


                /* Минутная стрелка */
                g.DrawLine(new Pen(Color.Black, 4), new Point(xCenter, yCenter),
                    new Point((int)(r * 0.7 * Math.Cos(minuteAngle * Math.PI / 180) + xCenter),
                        (int)(r * 0.7 * Math.Sin(minuteAngle * Math.PI / 180) + yCenter)));
                /* Часовая стрелка */
                g.DrawLine(new Pen(Color.Black, 8), new Point(xCenter, yCenter),
                    new Point((int)(r * 0.50 * Math.Cos(hourAngleOx * Math.PI / 180) + xCenter),
                        (int)(r * 0.50 * Math.Sin(hourAngleOx * Math.PI / 180) + yCenter)));
                /* Стрелка секундная */
                g.DrawLine(new Pen(Color.OrangeRed, 2), new Point(xCenter, yCenter),
                    new Point((int)(r * 0.75 * Math.Cos(secondAngle * Math.PI / 180) + xCenter),
                        (int)(r * 0.75 * Math.Sin(secondAngle * Math.PI / 180) + yCenter)));


                g.DrawEllipse(new Pen(Color.Black, 6), xCenter - 3, yCenter - 3 + delta, 6, 6);

                /* Минутная стрелка */
                g.DrawLine(new Pen(Color.Black, 4), new Point(xCenter, yCenter + delta),
                    new Point((int)(r * 0.7 * Math.Cos(minuteAngle * Math.PI / 180) + xCenter),
                        (int)(r * 0.7 * Math.Sin(minuteAngle * Math.PI / 180) + yCenter + delta)));
                /* Часовая стрелка */
                g.DrawLine(new Pen(Color.Black, 8), new Point(xCenter, yCenter + delta),
                    new Point((int)(r * 0.50 * Math.Cos(hourAngleBur * Math.PI / 180) + xCenter),
                        (int)(r * 0.50 * Math.Sin(hourAngleBur * Math.PI / 180) + yCenter + delta)));
                /* Стрелка секундная */
                g.DrawLine(new Pen(Color.OrangeRed, 2), new Point(xCenter, yCenter + delta),
                    new Point((int)(r * 0.75 * Math.Cos(secondAngle * Math.PI / 180) + xCenter),
                        (int)(r * 0.75 * Math.Sin(secondAngle * Math.PI / 180) + yCenter + delta)));
            }

            b.Save(@"clock1.bmp", ImageFormat.Bmp);
            b.Save(@"clock2.bmp", ImageFormat.Bmp);
        }
       
    }
}
