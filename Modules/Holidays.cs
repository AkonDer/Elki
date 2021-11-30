using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        Image newImage; // Фоновая картинка

        public Holidays(double dt, string fileName) : base(dt)
        {
            _holidays = OpenFile(fileName);
            
            newImage = Image.FromFile(@"resources\fonHolidays.jpg");
        }

        protected override void OnTimer(object source, ElapsedEventArgs e)
        {
            Trace.WriteLine($"{DateTime.Now} Праздники");

            Holiday holday = _holidays.FirstOrDefault(h => h.Date == _dataNow);

            Bitmap b = new Bitmap(345, 422);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Create fonts and brush.
                SolidBrush drawBrush = new SolidBrush(Color.White);
                Font drawFont1 = new Font("Calibri", 21, FontStyle.Bold);
                Font drawFont2 = new Font("Arial", 26, FontStyle.Bold);

                // Set format of string.
                StringFormat drawFormat = new StringFormat();                

                g.Clear(Color.White);

                // рисуем иконку
                g.DrawImage(newImage, 0, 0, 345, 422);

                const int characters = 11; // количество символов в строке
                float numberOfString = 1; // номер текущей строки               
                string str = ""; // текущая строка для печати

                List<string> listSpliting = new List<string>();

                g.DrawString("СЕГОДНЯ", drawFont2, drawBrush, 80, 25, drawFormat);

                foreach (string listHD in holday.HolDays)
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

            FileWork fwHolidays = new FileWork(filename);

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
