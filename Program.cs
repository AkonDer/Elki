﻿using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;


namespace Elki
{
    class Program
    {
        private static System.Timers.Timer aTimerElki;
        private static System.Timers.Timer aTimerBD;
        private static System.Timers.Timer aTimerClock;
        private static string[] dataElki;
        private static List<Employee> employees = new List<Employee>();
        private static decimal lists = 0;
        private static decimal Whichlist = 1;
       


        static void Main(string[] args)
        {
            // start pogram
            Console.WriteLine("Start programm in " + DateTime.Now);

            dataElki = File.ReadAllLines("data.txt");

            //// Переформируем файл с договорняками
            //var dp = new FileWork("dp.xlsx");
            //List<Employee> empDp = new List<Employee>();
            //for (int i = 0; i < dp.Rows.Count; i += 4)
            //{
            //    empDp.Add(new Employee()
            //    {
            //        Name = dp.Rows[i].Cells[0].ToString(),
            //        DateOfBirth = dp.Rows[i+1].Cells[0].ToString()
            //    }); 
            //}

            // SaveData("wd.xlsx", empDp);


            // считываем из файла данные по дням рождения сотрудников
            var fw = new FileWork("emp.xlsx");
            
            // Создаем List сотрудников для последующего простого поиска соответствий дню рождений в нем и помещаем в глобальный List
            int id = 0;
            foreach (var item in fw.Rows)
            {
                employees.Add(new Employee()
                {
                    Id = id,
                    Name = item.Cells[0].ToString().Trim() + " " +
                            item.Cells[1].ToString().Trim() + " " +
                            item.Cells[2].ToString().Trim(),
                    DateOfBirth = item.Cells[3].ToString()
                });
                id++;
            }

            // Запускаем таймеры в трех потоках
            Thread t1, t2, t3;

            t1 = new Thread(e =>
            {
                SetTimerElki();
                Console.ReadLine();
                aTimerElki.Stop();
                aTimerElki.Dispose();
            });
            t1.Start();

            t2 = new Thread(e => { SetTimerBD(); 
                Console.ReadLine();
                aTimerBD.Stop();
                aTimerBD.Dispose();
            });
            t2.Start();
            
            t3 = new Thread(e => { SetTimerClock(); 
                Console.ReadLine();
                aTimerClock.Stop();
                aTimerClock.Dispose();
            });
            t3.Start();
        }

        // Конвертирует время из DateTime в Double
        public static double ConvertTimeToDouble(DateTime dt)
        {
            return dt.Hour + Convert.ToDouble(dt.Minute) / 60;
        }
       
        // Событие, срабатывающее при тике таймера
        private static void OnTimedEventElki(Object source, ElapsedEventArgs e)
        {
            elki();
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                e.SignalTime);
        }

        private static void SetTimerElki()
        {
            // Create a timer with a two second interval.
            aTimerElki = new System.Timers.Timer(2000);
            // Hook up the Elapsed event for the timer. 
            aTimerElki.Elapsed += OnTimedEventElki;
            aTimerElki.AutoReset = true;
            aTimerElki.Enabled = true;
        }

        // Событие, срабатывающее при тике таймера
        private static void OnTimedEventClock(Object source, ElapsedEventArgs e)
        {
            Clock();
        }

        private static void SetTimerClock()
        {
            // Create a timer with a two second interval.
            aTimerClock = new System.Timers.Timer(1000);
            // Hook up the Elapsed event for the timer. 
            aTimerClock.Elapsed += OnTimedEventClock;
            aTimerClock.AutoReset = true;
            aTimerClock.Enabled = true;
        }

        // Событие, срабатывающее при тике таймера
        private static void OnTimedEventBD(Object source, ElapsedEventArgs e)
        {
            birthday();
        }

        private static void SetTimerBD()
        {
            // Create a timer with a two second interval.
            aTimerBD = new System.Timers.Timer(6000);
            // Hook up the Elapsed event for the timer. 
            aTimerBD.Elapsed += OnTimedEventBD;
            aTimerBD.AutoReset = true;
            aTimerBD.Enabled = true;
        }

        /// <summary>
        /// Функция формирует электрички
        /// </summary>
        private static void elki()
        {
            // считываем массив времен отправления электричек и переводим их в числа с плавающей точкой
            var timesElki = dataElki;

            double timeNow;
            string time1 = "";
            string time2 = "";
            string time3 = "";

            double time;

            Bitmap b = new Bitmap(170, 100);
            using (Graphics g = Graphics.FromImage(b))
            {

                // Create fonts and brush.
                SolidBrush drawBrush = new SolidBrush(Color.DarkBlue);
                Font drawFont1 = new Font("Arial", 18);
                Font drawFont2 = new Font("Arial", 24, FontStyle.Bold);

                // Set format of string.
                StringFormat drawFormat = new StringFormat();

                // Рисуем линии
                Pen ePen = new Pen(Color.DarkBlue, 1);

                // Вставляем картинку
                Image newImage = Image.FromFile("icon.png");

                int i;

                // находим текущее время и переводим его в число с плавающей точкой
                timeNow = ConvertTimeToDouble(DateTime.Now);

                for (i = 0; i < timesElki.Length; i++)
                {

                    // Если текущее время меньше минимального за день
                    if (i == 0)
                    {
                        time1 = timesElki[timesElki.Length - 1];
                        time2 = timesElki[i];
                        time3 = timesElki[i + 1];
                    }
                    else if (i > 0 && i < timesElki.Length - 1)
                    {
                        time1 = timesElki[i - 1];
                        time2 = timesElki[i];
                        time3 = timesElki[i + 1];
                    }
                    else if (i == timesElki.Length - 1)
                    {
                        time1 = timesElki[i - 1];
                        time2 = timesElki[i];
                        time3 = timesElki[0];
                    }

                    time = ConvertTimeToDouble(Convert.ToDateTime(time2));
                    if ((time - timeNow) >= 0) break;
                }

                // Если текущее время больше максимального в расписании за день
                if (timeNow > ConvertTimeToDouble(Convert.ToDateTime(timesElki[timesElki.Length - 1])))
                {
                    time1 = timesElki[timesElki.Length - 1];
                    time2 = timesElki[0];
                    time3 = timesElki[1];
                }

                g.Clear(Color.White);

                // рисуем иконку
                g.DrawImage(newImage, 0, 5, 85, 85);

                // рисуем линии
                g.DrawLine(ePen, 78, 34, 160, 34);
                g.DrawLine(ePen, 78, 68, 160, 68);

                //Рисуем вычесленные времена отправления электричек
                g.DrawString(time1, drawFont1, drawBrush, 87, 7, drawFormat);
                g.DrawString(time3, drawFont1, drawBrush, 87, 70, drawFormat);

                g.DrawString(time2, drawFont2, drawBrush, 72, 33, drawFormat);

                b.Save(@"timeelki.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                b.Save(@"timeelki2.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            }

        }

        /// <summary>
        /// Фукция формирует дни рождения
        /// </summary>
        private static void birthday()
        {
            int numOfLists = 4; // Количество имен на листе

            string dataNow = DateTime.Now.ToString("dd.MM");
            //string dataNow = "26.12";

            // Ищем всех людей с соответствующим днем рождения
            var emp = employees.Where(e => e.DateOfBirth.Contains(dataNow));
            string[] ebd = new string[50];

            int a = 0;
            foreach (var item in emp)
            {
                ebd[a] = item.Name;
                a++;
            }

            lists = Math.Ceiling((decimal) (emp.ToList().Count / (decimal)numOfLists));

            int start = (int) (Whichlist * numOfLists - numOfLists);
            int end = (emp.ToList().Count - Whichlist * numOfLists < 0)
                ? (int)(Whichlist * numOfLists) - numOfLists + (int)emp.ToList().Count % numOfLists
                : (int)Whichlist * numOfLists;


            Bitmap b = new Bitmap(362, 512);
            using (Graphics g = Graphics.FromImage(b))
            {

                // Create fonts and brush.
                SolidBrush drawBrush = new SolidBrush(Color.DarkRed);
                Font drawFont1 = new Font("Arial", 18, FontStyle.Italic);
                Font drawFont2 = new Font("Arial", 24, FontStyle.Bold);

                // Set format of string.
                StringFormat drawFormat = new StringFormat();

                // Рисуем линии
                Pen ePen = new Pen(Color.DarkBlue, 1);

                // Вставляем картинку
                Image newImage = Image.FromFile("fon.jpg");
               

                g.Clear(Color.White);

                // рисуем иконку
                g.DrawImage(newImage, 0, 0, 362, 512);
               
                g.DrawString("Наши именинники", drawFont2, drawBrush, 30, 33, drawFormat);

                int index = 1;
                for (int i = start; i < end; i++)
                {
                    string[] s = ebd[i].Split(' ');
                    string s1 = $"{s[0]} {s[1]}";
                    string s2 = $"     {s[2]}";
                    g.DrawString(s1, drawFont1, drawBrush, 50, (index) * 55 + 45, drawFormat);
                    g.DrawString(s2, drawFont1, drawBrush, 50, (index) * 55 + 67, drawFormat);
                    Console.WriteLine(ebd[i]);
                    index++;
                }

                Font drawFont3 = new Font("Arial", 20, FontStyle.Bold);
                g.DrawString("Поздравляем", drawFont3, drawBrush, 33, 360, drawFormat);
                g.DrawString("с днем", drawFont3, drawBrush, 33, 390, drawFormat);
                g.DrawString("рождения!", drawFont3, drawBrush, 33, 420, drawFormat);

                b.Save(@"bd1.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                b.Save(@"bd2.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            }




            // Если это был последний лист то перейти снова к первому
            if (Whichlist == lists) Whichlist = 1;
            else Whichlist++;
            Console.WriteLine(Whichlist);
        }

        private static void Clock()
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
            int hourAngleOx = (int)(Math.Round((hour - 8 + minute/60.0) * 30) - 90);
            int hourAngleBur = (int)(Math.Round((hour - 2 + minute / 60.0) * 30) - 90);

            Bitmap b = new Bitmap(345, 422);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                SolidBrush drawBrush = new SolidBrush(Color.DarkBlue);
                Font drawFont1 = new Font("Arial", 16, FontStyle.Bold);
                Font drawFont2 = new Font("Arial", 12);
                StringFormat drawFormat = new StringFormat();

                //g.DrawString("IPG Photonics", drawFont1, drawBrush, 100, 5, drawFormat);
                //g.DrawString("Оксфорд, США", drawFont2, drawBrush, 120, 25, drawFormat);

                //g.DrawString("IPG Laser", drawFont1, drawBrush, 120, 213, drawFormat);
                //g.DrawString("Бурбах, Германия", drawFont2, drawBrush, 100, 233, drawFormat);


                Image newImage = Image.FromFile("clock.png");
                g.DrawImage(newImage, 0, 0, 345, 422);
                

                g.DrawEllipse(new Pen(Color.Black, 6), xCenter - 3, yCenter - 3, 6, 6);

                
                /* Минутная стрелка */
                g.DrawLine(new Pen(Color.Black, 4), new Point(xCenter, yCenter),
                    new Point((int) ((r * 0.7) * Math.Cos(minuteAngle * Math.PI / 180) + xCenter),
                        (int) ((r * 0.7) * Math.Sin(minuteAngle * Math.PI / 180) + yCenter)));
                /* Часовая стрелка */
                g.DrawLine(new Pen(Color.Black, 8), new Point(xCenter, yCenter),
                    new Point((int) ((r * 0.50) * Math.Cos(hourAngleOx * Math.PI / 180) + xCenter),
                        (int) ((r * 0.50) * Math.Sin(hourAngleOx * Math.PI / 180) + yCenter)));
                /* Стрелка секундная */
                g.DrawLine(new Pen(Color.OrangeRed, 2), new Point(xCenter, yCenter),
                    new Point((int)((r * 0.75) * Math.Cos(secondAngle * Math.PI / 180) + xCenter),
                        (int)((r * 0.75) * Math.Sin(secondAngle * Math.PI / 180) + yCenter)));


                g.DrawEllipse(new Pen(Color.Black, 6), xCenter - 3, yCenter - 3 + delta, 6, 6);
                
                /* Минутная стрелка */
                g.DrawLine(new Pen(Color.Black, 4), new Point(xCenter, yCenter + delta),
                    new Point((int) ((r * 0.7) * Math.Cos(minuteAngle * Math.PI / 180) + xCenter),
                        (int) ((r * 0.7) * Math.Sin(minuteAngle * Math.PI / 180) + yCenter + delta)));
                /* Часовая стрелка */
                g.DrawLine(new Pen(Color.Black, 8), new Point(xCenter, yCenter + delta),
                    new Point((int) ((r * 0.50) * Math.Cos(hourAngleBur * Math.PI / 180) + xCenter),
                        (int) ((r * 0.50) * Math.Sin(hourAngleBur * Math.PI / 180) + yCenter + delta)));
                /* Стрелка секундная */
                g.DrawLine(new Pen(Color.OrangeRed, 2), new Point(xCenter, yCenter + delta),
                    new Point((int)((r * 0.75) * Math.Cos(secondAngle * Math.PI / 180) + xCenter),
                        (int)((r * 0.75) * Math.Sin(secondAngle * Math.PI / 180) + yCenter + delta)));

            }

            b.Save(@"clock1.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            b.Save(@"clock2.bmp", System.Drawing.Imaging.ImageFormat.Bmp);


        }

        
        public static void SaveData(string fileName, List<Employee> emp)
        {
            //Рабочая книга Excel
            XSSFWorkbook wb;
            //Лист в книге Excel
            XSSFSheet sh;

            //Создаем рабочую книгу
            wb = new XSSFWorkbook();
            //Создаём лист в книге
            sh = (XSSFSheet)wb.CreateSheet("Лист 1");

            // Текущая строка
            int row = 0;

            foreach (var item in emp)
            {
                //Создаем строку
                var currentRow = sh.CreateRow(row);
                currentRow.CreateCell(0).SetCellValue(item.Name);
                currentRow.CreateCell(1).SetCellValue(item.DateOfBirth);
                row++;
            }

            // Удалим файл если он есть уже
            if (!File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            //запишем всё в файл
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                wb.Write(fs);
            }            
        }


    }
}
