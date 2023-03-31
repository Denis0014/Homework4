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
        /// <param name="n">Количество чисел</param>
        /// <param name="a">Нижняя грань чисел</param>
        /// <param name="b">Верхняя грань чисел</param>
        /// <returns>Бинарный файл файл вещественных чисел</returns>
        static void FileRandomNumbers(string fileName, int n = 20, int a = -50, int b = 50)
        {
            if (fileName == "")
            if (a > b)
                 throw new ArgumentException("Нижняя грань (a) должна быть меньше чем верхняя грань (b)");
            using (BinaryWriter bw = new BinaryWriter(new FileStream(fileName, FileMode.Create)))
                for (int i = 0; i < n; i++)
                    bw.Write(rnd.Next(a, b));
        }

        static void Main(string[] args)
        {
            // Задание 1
            FileRandomNumbers("output/task1.dat");
        }
    }
}