using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Timers;

namespace Elki
{
    internal class Clocks : ElkiTimer
    {
        Image newImage; // фоновое изображение

        public Clocks(double dt) : base(dt) 
        {
            newImage = Image.FromFile(@"resources\clock.png");
        }

        protected override void OnTimer(object source, ElapsedEventArgs e)
        {
            int xCenter = 233;
            int yCenter = 111;
            int delta = 197;


            int r = 100;

            int second = DateTime.Now.Second;
            int minute = DateTime.Now.Minute;
            int hour = DateTime.Now.Hour;

            int secondAngle = second * 6 - 90;
            int minuteAngle = minute * 6 - 90;
            int hourAngleOx = (int)(Math.Round((hour - 7 + minute / 60.0) * 30) - 90);
            int hourAngleBur = (int)(Math.Round((hour - 2 + minute / 60.0) * 30) - 90);

            Bitmap b = new Bitmap(345, 422);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.Clear(Color.White);

                g.SmoothingMode = SmoothingMode.AntiAlias;
                
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

            b.Save(@"output\clock1.bmp", ImageFormat.Bmp);
            b.Save(@"output\clock2.bmp", ImageFormat.Bmp);
        }
       
    }
}
