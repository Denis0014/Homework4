using System;
using System.Globalization;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

namespace Homework4
{
    internal class Program
    {
        public class Movie
        {
            public string Title { get; }
            public string Original_language { get; }
            private int Vote_count;
            public double Vote_average { get; }
            public Movie(string title, string original_language, int vote_count, double vote_average)
            {
                Title = title;
                Original_language = original_language;
                Vote_count = vote_count;
                Vote_average = vote_average;
            }
        }
        static Random rnd = new Random();
        /// <summary>
        /// Сформировывает бинарный файл из N случайных вещественных чисел в диапазоне от a до b (a ≤ b)
        /// </summary>
        /// <param name="fileName">Исходное имя файла</param>
        /// <param name="n">Количество чисел</param>
        /// <param name="a">Нижняя грань чисел</param>
        /// <param name="b">Верхняя грань чисел</param>
        /// <returns>Бинарный файл файл вещественных чисел</returns>
        /// <exception cref="ArgumentException"></exception>
        static void FileRandomNumbers(string fileName, int n = 20, int a = -50, int b = 50)
        {
            if (fileName == "") throw new ArgumentException("Имя файла не должно быть пустым");
            if (n <= 0) throw new ArgumentException("Количество элементов должно быть больше 0");
            if (a > b) throw new ArgumentException("Нижняя грань (a) должна быть меньше чем верхняя грань (b)");
            using (BinaryWriter bw = new BinaryWriter(new FileStream(fileName, FileMode.Create)))
                for (int i = 0; i < n; i++)
                    bw.Write(rnd.Next(a, b));
        }
        /// <summary>
        /// Возвращает K-й элемент файла. Если такой элемент отсутствует, возвращает −1.
        /// </summary>
        /// <param name="fileName">Исходное имя файла</param>
        /// <param name="k">Номер элемента для поиска</param>
        /// <returns>Integer значение на K-й позиции</returns>
        /// <exception cref="ArgumentException"></exception>
        static int GetValue(string fileName, int k)
        {
            if (k < 0) throw new ArgumentException("Индекс не должен быть отрицательным");
            using (BinaryReader br = new BinaryReader(new FileStream(fileName, FileMode.Open)))
            {
                br.BaseStream.Seek(k * sizeof(int), SeekOrigin.Begin);
                try
                {
                    return br.ReadInt32();
                }
                catch (EndOfStreamException)
                {
                    return -1;
                }
            }
        }
        /// <summary>
        /// Извлекает из mp3 файла мета-данные  
        /// </summary>
        /// <param name="fileName">Исходное имя файла</param>
        static void ReadInfoMP3(string fileName)
        {
            using (BinaryReader br = new BinaryReader(new FileStream(fileName, FileMode.Open)))
            {
                br.BaseStream.Seek(-125, SeekOrigin.End);
                Console.WriteLine($"Название: {string.Join("", br.ReadChars(30))}");
                Console.WriteLine($"Исполнитель: {string.Join("", br.ReadChars(30))}");
                Console.WriteLine($"Альбом: {string.Join("", br.ReadChars(30))}");
                Console.WriteLine($"Год: {string.Join("", br.ReadChars(4))}");
                Console.WriteLine($"Комментарий: {string.Join("", br.ReadChars(30))}");
            }
        }
        /// <summary>
        /// Считает количество слов в этом файле
        /// </summary>
        /// <param name="fileName">Исходное имя файла</param>
        /// <returns>Считает количество цельных слов разделённые разделителями</returns>
        static int CountWordsInFile(string fileName) => File.ReadAllText(fileName).Split(new char[] { ' ', ',', '.', ':', '\t' }).Count();
        static List<Movie> SetTop(string fileName)
        {
            var list = new List<Movie>();
            var top = File.ReadAllLines(fileName)[1..];
            foreach (var line in top)
            {
                var info = line.Split(new char[] { ',' });
                try
                {
                    if (int.Parse(info[3]) > 100)
                    {

                        list.Add(new Movie(info[1], info[2], int.Parse(info[3]), double.Parse(info[4], CultureInfo.InvariantCulture.NumberFormat)));
                    }
                }
                catch { }
            }
            return list;
        }
        static void TopAndAntiTop5(List<Movie> list)
        {
            var lst = list.OrderBy(x => x.Vote_average).ToArray();
            var min = lst[5].Vote_average;
            var max = lst[^5].Vote_average;
            Console.WriteLine("--Top--");
            Console.WriteLine(string.Join("\n", list.Where(x => x.Vote_average >= max).OrderByDescending(x => x.Vote_average).Select(x => x.Title + " | " + x.Original_language + " | " + x.Vote_average)));
            Console.WriteLine("--AntiTop--");
            Console.WriteLine(string.Join("\n", list.Where(x => x.Vote_average <= min).OrderByDescending(x => x.Vote_average).Select(x => x.Title + " | " + x.Original_language + " | " + x.Vote_average)));
        }
        static void PrintRuTop(List<Movie> list) => Console.WriteLine(string.Join("\n",list.Where(x => (x.Vote_average > 7) && (x.Original_language == "ru")).Select(x => x.Title + " | " + x.Original_language + " | " + x.Vote_average)));
        static void MovieOnEvning(List<Movie> list, string language, double rating)
        {
            var lst = list.Where(x => (x.Vote_average > rating) && (x.Original_language == language)).Select(x => x.Title + " | " + x.Original_language + " | " + x.Vote_average).ToArray();
            Console.WriteLine(lst[rnd.Next(0, lst.Length - 1)]);
        }
        static void Main(string[] args)
        {
            // Задание 1
            FileRandomNumbers("output/task1.dat");
            // Задание 2
            Console.WriteLine(GetValue("output/task1.dat", 19));
            // Задание 3
            ReadInfoMP3("ответы на контрольную.mp3");
            // Задание 4
            Console.WriteLine(CountWordsInFile("Война и Мир.txt"));
            PrintRuTop(SetTop("IMDB.csv"));
            TopAndAntiTop5(SetTop("IMDB.csv"));
            MovieOnEvning(SetTop("IMDB.csv"), "ru", 6);
        }
    }
}