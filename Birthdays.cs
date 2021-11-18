using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Timers;

namespace Elki
{
    internal class Birthdays : ElkiTimer
    {        
        List<Employee> _employees = new List<Employee>();
        private decimal _lists;
        private decimal _whichlist = 1;

        public Birthdays(double dt, string filename) : base(dt)
        {           
            _employees = OpenFile(filename);
        }

        protected override void onTimer(object source, ElapsedEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Дни рождения");
            Console.ForegroundColor = ConsoleColor.White;

            var numOfLists = 4; // Количество имен на листе            

            // Ищем всех людей с соответствующим днем рождения
            var emp = _employees.Where(em => em.DateOfBirth.Contains(_dataNow));
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

        List<Employee> OpenFile(string filename)
        {
            List<Employee> employees = new List<Employee>();
            // считываем из файла данные по дням рождения сотрудников
            var fw = new FileWork(filename);

            // Создаем List сотрудников для последующего простого поиска соответствий дню рождений в нем и помещаем в глобальный List
            var id = 0;
            foreach (var item in fw.Rows)
            {
                employees.Add(new Employee
                {
                    Id = id,
                    Name = item.Cells[0].ToString().Trim() + " " +
                           item.Cells[1].ToString().Trim() + " " +
                           item.Cells[2].ToString().Trim(),
                    DateOfBirth = item.Cells[3].ToString()
                });
                id++;
            }
            return employees;
        }
    }

    class Employee
    {
        public string DateOfBirth;
        public int Id;
        public string Name;
    }
}
