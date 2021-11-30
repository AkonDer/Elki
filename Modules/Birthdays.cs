using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Timers;

namespace Elki
{
    internal class Birthdays : ElkiTimer
    {
        readonly List<Employee> _employees = new List<Employee>();
        private decimal _lists;
        private decimal _whichlist = 1;
        readonly int numOfLists = 4; // Количество имен на листе  
        string[] ebd; // люди у которых сегодня день рождения
        List<Employee> emp;
        Image newImage; // фоновая картинка

        public Birthdays(double dt, string filename) : base(dt)
        {
            _employees = OpenFile(filename);
            
            newImage = Image.FromFile(@"resources\fon.jpg");
        }

        protected override void OnTimer(object source, ElapsedEventArgs e)
        {     
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTime.Now} Дни рождения");
            Console.ForegroundColor = ConsoleColor.White;

            Trace.WriteLine($"{DateTime.Now} Дни рождения");  
            
            // Ищем всех людей с соответствующим днем рождения
            emp = _employees.Where(em => em.DateOfBirth.Contains(_dataNow)).ToList();

            ebd = new string[50];
            int a = 0;
           
            foreach (var item in emp)
            {
                ebd[a] = item.Name;
                a++;
            }

            _lists = Math.Ceiling(emp.Count / (decimal)numOfLists);

            int start = (int)(_whichlist * numOfLists - numOfLists);
            int end = emp.Count - _whichlist * numOfLists < 0
                ? (int)(_whichlist * numOfLists) - numOfLists + emp.Count % numOfLists
                : (int)_whichlist * numOfLists;


            Bitmap b = new Bitmap(362, 512);
            using (Graphics g = Graphics.FromImage(b))
            {
                // Create fonts and brush.
                SolidBrush drawBrush = new SolidBrush(Color.DarkRed);
                Font drawFont1 = new Font("Arial", 22, FontStyle.Italic);
                Font drawFont2 = new Font("Arial", 24, FontStyle.Bold);

                // Set format of string.
                var drawFormat = new StringFormat();

                // Рисуем линии
                //var ePen = new Pen(Color.DarkBlue, 1);               

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
                    g.DrawString(s1, drawFont1, drawBrush, 42, index * 62 + 45, drawFormat);
                    g.DrawString(s2, drawFont1, drawBrush, 42, index * 62 + 73, drawFormat);
                    Console.WriteLine(ebd[i]);
                    index++;
                }

                Font drawFont3 = new Font("Arial", 20, FontStyle.Bold);
                g.DrawString("Поздравляем", drawFont3, drawBrush, 33, 385, drawFormat);
                g.DrawString("с днем", drawFont3, drawBrush, 33, 415, drawFormat);
                g.DrawString("рождения!", drawFont3, drawBrush, 33, 445, drawFormat);

                b.Save(@"output\bd1.bmp", ImageFormat.Bmp);
                b.Save(@"output\bd2.bmp", ImageFormat.Bmp);
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
        public int Id;
        public string DateOfBirth;
        public string Name;
    }
}
