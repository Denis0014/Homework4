using System;
using System.Security.Cryptography.X509Certificates;

namespace Homework4
{
    internal class Program
    {
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
        static void ReadInfoMP3(string fileName)
        {
            using (BinaryReader br = new BinaryReader(new FileStream(fileName, FileMode.Open)))
            {
                br.BaseStream.Seek(-125, SeekOrigin.End);
                Console.WriteLine($"Название: { string.Join("" ,br.ReadChars(30)) }");
                Console.WriteLine($"Исполнитель: {string.Join("", br.ReadChars(30))}");
                Console.WriteLine($"Альбом: {string.Join("", br.ReadChars(30))}");
                Console.WriteLine($"Год: {string.Join("", br.ReadChars(4))}");
                Console.WriteLine($"Комментарий: {string.Join("", br.ReadChars(30))}");
            }
        }
        static void Main(string[] args)
        {
            // Задание 1
            //FileRandomNumbers("output/task1.dat");
            // Задание 2
            //Console.WriteLine(GetValue("output/task1.dat", 19));
            // Задание 3
            ReadInfoMP3("ответы на контрольную.mp3");
        }
    }
}