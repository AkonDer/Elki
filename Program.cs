using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            dataNow = DateTime.Now.ToString("dd.MM");
            //string dataNow = "26.12";

            // start pogram
            Console.WriteLine("Start programm in " + DateTime.Now);

            _dataElki = File.ReadAllLines("data.txt");

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

            // считываем данные из файла с праздниками
            var fwHolidays = new FileWork("holidays.xlsx");

            // Создаем список праздников для последующего поиска по нему
            foreach (var hd in fwHolidays.Rows)
            {
                string HolyData = hd.Cells[0].ToString();
                List<string> holday = new List<string>();
                for (int i = 1; i < hd.Cells.Count; i++)
                {
                    holday.Add(hd.Cells[i].ToString());
                }
                Holidays.Add(new Holiday { Date = HolyData, HolDays = holday });
            }

            // считываем из файла данные по дням рождения сотрудников
            var fw = new FileWork("emp.xlsx");

            // Создаем List сотрудников для последующего простого поиска соответствий дню рождений в нем и помещаем в глобальный List
            var id = 0;
            foreach (var item in fw.Rows)
            {
                Employees.Add(new Employee
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
            Thread t1, t2, t3, t4;

            t1 = new Thread(e =>
            {
                SetTimerElki();
                Console.ReadLine();
                _aTimerElki.Stop();
                _aTimerElki.Dispose();
            });
            t1.Start();

            t2 = new Thread(e =>
            {
                SetTimerBd();
                Console.ReadLine();
                _aTimerBd.Stop();
                _aTimerBd.Dispose();
            });
            t2.Start();

            t3 = new Thread(e =>
            {
                SetTimerClock();
                Console.ReadLine();
                _aTimerClock.Stop();
                _aTimerClock.Dispose();
            });
            t3.Start();

            t4 = new Thread(e =>
            {
                SetTimerHolidays();
                Console.ReadLine();
                _aTimerClock.Stop();
                _aTimerClock.Dispose();
            });
            t4.Start();
        }

        // Конвертирует время из DateTime в Double
        public static double ConvertTimeToDouble(DateTime dt)
        {
            return dt.Hour + Convert.ToDouble(dt.Minute) / 60;
        }

        // Событие, срабатывающее при тике таймера
        private static void OnTimedEventElki(object source, ElapsedEventArgs e)
        {
            Elki();
        }

        // Событие, срабатывающее при тике таймера
        private static void OnTimedEventClock(object source, ElapsedEventArgs e)
        {
            Clock();
        }

        // Событие, срабатывающее при тике таймера
        private static void OnTimedEventBD(object source, ElapsedEventArgs e)
        {
            Birthday();
        }

        // Событие, срабатывающее при тике таймера
        private static void OnTimedEventHolidays(object source, ElapsedEventArgs e)
        {
            Holydays();
        }

        private static void SetTimerHolidays()
        {
            // Create a timer with a two second interval.
            _aTimerHolidays = new Timer(2000);
            // Hook up the Elapsed event for the timer. 
            _aTimerHolidays.Elapsed += OnTimedEventHolidays;
            _aTimerHolidays.AutoReset = true;
            _aTimerHolidays.Enabled = true;
        }

        private static void SetTimerElki()
        {
            // Create a timer with a two second interval.
            _aTimerElki = new Timer(2000);
            // Hook up the Elapsed event for the timer. 
            _aTimerElki.Elapsed += OnTimedEventElki;
            _aTimerElki.AutoReset = true;
            _aTimerElki.Enabled = true;
        }

        private static void SetTimerClock()
        {
            // Create a timer with a two second interval.
            _aTimerClock = new Timer(1000);
            // Hook up the Elapsed event for the timer. 
            _aTimerClock.Elapsed += OnTimedEventClock;
            _aTimerClock.AutoReset = true;
            _aTimerClock.Enabled = true;
        }

        private static void SetTimerBd()
        {
            // Create a timer with a two second interval.
            _aTimerBd = new Timer(6000);
            // Hook up the Elapsed event for the timer. 
            _aTimerBd.Elapsed += OnTimedEventBD;
            _aTimerBd.AutoReset = true;
            _aTimerBd.Enabled = true;
        }


        /// <summary>
        ///     Функция формирует электрички
        /// </summary>
        private static void Elki()
        {
            // считываем массив времен отправления электричек и переводим их в числа с плавающей точкой
            var timesElki = _dataElki;

            double timeNow;
            var time1 = "";
            var time2 = "";
            var time3 = "";

            double time;

            var b = new Bitmap(170, 100);
            using (var g = Graphics.FromImage(b))
            {
                // Create fonts and brush.
                var drawBrush = new SolidBrush(Color.DarkBlue);
                var drawFont1 = new Font("Arial", 18);
                var drawFont2 = new Font("Arial", 24, FontStyle.Bold);

                // Set format of string.
                var drawFormat = new StringFormat();

                // Рисуем линии
                var ePen = new Pen(Color.DarkBlue, 1);

                // Вставляем картинку
                var newImage = Image.FromFile("icon.png");

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
                    if (time - timeNow >= 0) break;
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

                b.Save(@"timeelki.bmp", ImageFormat.Bmp);
                b.Save(@"timeelki2.bmp", ImageFormat.Bmp);
            }
        }

        /// <summary>
        ///     Фукция формирует дни рождения
        /// </summary>
        private static void Birthday()
        {
            var numOfLists = 4; // Количество имен на листе            

            // Ищем всех людей с соответствующим днем рождения
            var emp = Employees.Where(e => e.DateOfBirth.Contains(dataNow));
            var ebd = new string[50];

            var a = 0;
            var enumerable = emp.ToList();
            foreach (var item in enumerable)
            {
                ebd[a] = item.Name;
                a++;
            }

            _lists = Math.Ceiling(enumerable.ToList().Count / (decimal)numOfLists);

            var start = (int)(_whichlist * numOfLists - numOfLists);
            var end = enumerable.ToList().Count - _whichlist * numOfLists < 0
                ? (int)(_whichlist * numOfLists) - numOfLists + enumerable.ToList().Count % numOfLists
                : (int)_whichlist * numOfLists;


            var b = new Bitmap(362, 512);
            using (var g = Graphics.FromImage(b))
            {
                // Create fonts and brush.
                var drawBrush = new SolidBrush(Color.DarkRed);
                var drawFont1 = new Font("Arial", 18, FontStyle.Italic);
                var drawFont2 = new Font("Arial", 24, FontStyle.Bold);

                // Set format of string.
                var drawFormat = new StringFormat();

                // Рисуем линии
                //var ePen = new Pen(Color.DarkBlue, 1);

                // Вставляем картинку
                var newImage = Image.FromFile("fon.jpg");


                g.Clear(Color.White);

                // рисуем иконку
                g.DrawImage(newImage, 0, 0, 362, 512);

                g.DrawString("Наши именинники", drawFont2, drawBrush, 30, 33, drawFormat);

                var index = 1;
                for (var i = start; i < end; i++)
                {
                    var s = ebd[i].Split(' ');
                    var s1 = $"{s[0]} {s[1]}";
                    var s2 = $"     {s[2]}";
                    g.DrawString(s1, drawFont1, drawBrush, 50, index * 55 + 45, drawFormat);
                    g.DrawString(s2, drawFont1, drawBrush, 50, index * 55 + 67, drawFormat);
                    Console.WriteLine(ebd[i]);
                    index++;
                }

                var drawFont3 = new Font("Arial", 20, FontStyle.Bold);
                g.DrawString("Поздравляем", drawFont3, drawBrush, 33, 360, drawFormat);
                g.DrawString("с днем", drawFont3, drawBrush, 33, 390, drawFormat);
                g.DrawString("рождения!", drawFont3, drawBrush, 33, 420, drawFormat);

                b.Save(@"bd1.bmp", ImageFormat.Bmp);
                b.Save(@"bd2.bmp", ImageFormat.Bmp);
            }


            // Если это был последний лист то перейти снова к первому
            if (_whichlist == _lists) _whichlist = 1;
            else _whichlist++;
            Console.WriteLine(_whichlist);


        }

        /// <summary>
        ///  Формирует праздники
        /// </summary>
        private static void Holydays()
        {
            var holday = Holidays.FirstOrDefault(h => h.Date == dataNow);

            var b = new Bitmap(362, 512);
            using (var g = Graphics.FromImage(b))
            {
                // Create fonts and brush.
                var drawBrush = new SolidBrush(Color.DarkRed);
                var drawFont1 = new Font("Arial", 18);
                var drawFont2 = new Font("Arial", 24, FontStyle.Bold);

                // Set format of string.
                var drawFormat = new StringFormat();

                // Рисуем линии
                //var ePen = new Pen(Color.DarkBlue, 1);

                // Вставляем картинку
                var newImage = Image.FromFile("fon.jpg");

                g.Clear(Color.White);

                // рисуем иконку
                g.DrawImage(newImage, 0, 0, 362, 512);

                const int characters = 15; // количество символов в строке
                int numberOfString = 1; // номер текущей строки

                foreach (var hd in holday.HolDays)
                {
                    string[] holArr = hd.ToString().Split(' '); // разбиваем фразу на слова
                    string str = ""; // текущая строка для печати
                    int currentWord = 0; // текущее слово в архиве                    

                    for (int i = currentWord; i < holArr.Length; i++)
                    {
                        str += holArr[i] + " ";
                        if (str.Length > characters)
                        {
                            g.DrawString(str, drawFont1, drawBrush, 33, numberOfString * 30, drawFormat);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(str);
                            Console.ForegroundColor = ConsoleColor.White;
                            str = "";
                            numberOfString++;
                            currentWord = i++;
                            break;
                        }
                       
                    }
                }

                

                b.Save(@"hd1.bmp", ImageFormat.Bmp);
                b.Save(@"hd2.bmp", ImageFormat.Bmp);

            }
        }

        /// <summary>
        ///  Формирует время в США и Германии
        /// </summary>
        private static void Clock()
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
            var row = 0;

            foreach (var item in emp)
            {
                //Создаем строку
                var currentRow = sh.CreateRow(row);
                currentRow.CreateCell(0).SetCellValue(item.Name);
                currentRow.CreateCell(1).SetCellValue(item.DateOfBirth);
                row++;
            }

            // Удалим файл если он есть уже
            if (!File.Exists(fileName)) File.Delete(fileName);

            //запишем всё в файл
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                wb.Write(fs);
            }
        }

        static void Factorial()
        {
            int result = 1;
            for (int i = 1; i <= 6; i++)
            {
                result *= i;
            }
            Thread.Sleep(8000);
            Console.WriteLine($"Факториал равен {result}");
        }

        // определение асинхронного метода
        static async void FactorialAsync()
        {
            Console.WriteLine("Начало метода FactorialAsync"); // выполняется синхронно
            await Task.Run(() => Factorial());                // выполняется асинхронно
            Console.WriteLine("Конец метода FactorialAsync");
        }
    }


}