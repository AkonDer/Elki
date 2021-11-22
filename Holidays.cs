using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Timers;

namespace Elki
{
    internal class Holidays : ElkiTimer
    {
        readonly List<Holiday> _holidays = new List<Holiday>();

        public Holidays(double dt, string fileName) : base(dt)
        {
            _holidays = OpenFile(fileName);
        }
        
        protected override void onTimer(object source, ElapsedEventArgs e)
        {           
            var holday = _holidays.FirstOrDefault(h => h.Date == _dataNow);

            var b = new Bitmap(345, 422);
            using (var g = Graphics.FromImage(b))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Create fonts and brush.
                var drawBrush = new SolidBrush(Color.Blue);
                var drawFont1 = new Font("Arial", 21, FontStyle.Bold);
                var drawFont2 = new Font("Arial", 26, FontStyle.Bold);                

                // Set format of string.
                var drawFormat = new StringFormat();

                // Вставляем картинку
                var newImage = Image.FromFile(@"resources\fonHolidays.jpg");

                g.Clear(Color.White);

                // рисуем иконку
                g.DrawImage(newImage, 0, 0, 345, 422);

                const int characters = 10; // количество символов в строке
                float numberOfString = 1; // номер текущей строки               
                string str = ""; // текущая строка для печати

                List<string> listSpliting = new List<string>();

                g.DrawString("СЕГОДНЯ", drawFont2, drawBrush, 80, 25, drawFormat);

                foreach (var listHD in holday.HolDays)
                {
                    string[] split = listHD.Split(' ');

                    foreach (var item in split)
                    {
                        str = str + item.Trim() + " ";
                        if (str.Length > characters)
                        {
                            listSpliting.Add(str);
                            str = "";
                            continue;
                        }
                    }
                    if (str != "")
                    {
                        listSpliting.Add(str);
                    }
                    str = "";

                    foreach (var item in listSpliting)
                    {
                        
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(item.ToString());
                        Console.ForegroundColor = ConsoleColor.White;

                        g.DrawString(item, drawFont1, drawBrush, 33, numberOfString * 30 + 50, drawFormat);
                        numberOfString++;
                    }

                    numberOfString += 0.7f;

                    listSpliting.Clear();
                }

                b.Save(@"output\hd1.bmp", ImageFormat.Bmp);
                b.Save(@"output\hd2.bmp", ImageFormat.Bmp);

            }
        }
        List<Holiday> OpenFile(string filename)
        {
            List<Holiday> listHoliday = new List<Holiday>();

            var fwHolidays = new FileWork(@"resources\holidays.xlsx");

            // Создаем список праздников для последующего поиска по нему
            foreach (var hd in fwHolidays.Rows)
            {
                string HolyData = hd.Cells[0].ToString();
                List<string> holday = new List<string>();
                for (int i = 1; i < hd.Cells.Count; i++)
                {
                    holday.Add(hd.Cells[i].ToString());
                }
                listHoliday.Add(new Holiday { Date = HolyData, HolDays = holday });
            }
            return listHoliday;
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
