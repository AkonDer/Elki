using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Timers;

namespace Elki
{
    internal class Holidays : ElkiTimer
    {
        List<Holiday> _holidays = new List<Holiday>();

        public Holidays(double dt, string fileName) : base(dt)
        {
            _holidays = OpenFile(fileName);
        }

        protected override void onTimer(object source, ElapsedEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Праздники");
            Console.ForegroundColor = ConsoleColor.White;

            var holday = _holidays.FirstOrDefault(h => h.Date == _dataNow);

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

                const int characters = 10; // количество символов в строке
                int numberOfString = 1; // номер текущей строки
                string str = ""; // текущая строка для печати

                List<string> listSpliting = new List<string>();

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
                }

                foreach (var item in listSpliting)
                {
                    Console.WriteLine(item.ToString());
                    g.DrawString(item, drawFont1, drawBrush, 33, numberOfString * 30, drawFormat);
                    numberOfString++;
                }


                //g.DrawString(item, drawFont1, drawBrush, 33, numberOfString * 30, drawFormat);
                //numberOfString++;


                b.Save(@"hd1.bmp", ImageFormat.Bmp);
                b.Save(@"hd2.bmp", ImageFormat.Bmp);

            }
        }
        List<Holiday> OpenFile(string filename)
        {
            List<Holiday> listHoliday = new List<Holiday>();

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
                listHoliday.Add(new Holiday { Date = HolyData, HolDays = holday });
            }
            return listHoliday;
        }

        List<string> strSplit(string str, int count)
        {
            List<string> list = new List<string>();
            string s = "";

            string[] split = str.Split(' ');

            foreach (var item in split)
            {
                str = str + item + " ";
                if (str.Length > 10)
                {
                    list.Add(str);
                    str = "";
                    continue;
                }

            }

            list.Add(str);
            str = "";

            return list;
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
