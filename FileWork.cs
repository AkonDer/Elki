using System.Collections.Generic;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Elki
{
    internal class FileWork
    {
        /// <summary>
        ///     Путь к файлу
        /// </summary>
        private readonly string _path;

        /// <summary>
        ///     Количество строк в файле
        /// </summary>
        public int NumberOfLines;

        /// <summary>
        ///     коллекция всех строк в файле
        /// </summary>
        public List<IRow> Rows = new List<IRow>();

        /// <summary>
        ///     Конструктор - возращает все строки из файла
        /// </summary>
        /// <param name="p">Имя файла в текущей дирректории</param>
        public FileWork(string p)
        {
            _path = p;
            GetRowsFromFile();
        }

        /// <summary>
        ///     Получить все строки из файла
        /// </summary>
        /// <returns></returns>
        public List<IRow> GetRowsFromFile()
        {
            XSSFWorkbook xssfwb;

            //Открываем файл
            using (var file = new FileStream(_path, FileMode.Open, FileAccess.Read))
            {
                xssfwb = new XSSFWorkbook(file);
            }

            //Получаем первый лист книги
            var sheet = xssfwb.GetSheetAt(0);
            NumberOfLines = sheet.PhysicalNumberOfRows - 1;

            //Получаем все строки            
            for (var i = 0; i < NumberOfLines; i++) Rows.Add(sheet.GetRow(i));
            return Rows;
        }

        /// <summary>
        ///     Получить значение из ячейки
        /// </summary>
        /// <param name="row">Номер строки</param>
        /// <param name="col">Номер колонки</param>
        /// <returns></returns>
        public string GetCell(int row, int col)
        {
            return Rows[row].Cells[col].ToString();
        }
    }
}