using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
