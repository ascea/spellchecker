using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpellChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            string dict;
            string[] text;
            string[] result;
            dict = String.Join<string>(' ', Input());
            text = Input();

            if (text.Length > 0)
            {
                result = Correction(text, dict);
                foreach (string str in result)
                {
                    Console.WriteLine(str);
                }
            }
            Console.ReadLine();
        }
        // Обработка ввода
        static string[] Input()
        {
            List<string> result = new List<string>();
            string str;
            str = Console.ReadLine();
            while (str != "===")
            {
                if (!String.IsNullOrEmpty(str))
                {
                    result.Add(str);
                }
                str = Console.ReadLine();
            }
            return result.ToArray();
        }
        // Основной метод
        static string[] Correction(string[] text, string dict)
        {
            string[] result = new string[text.Length];
            string[] dictWords;
            CutWords(dict, out dictWords);
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = GetCorStr(text[i], dictWords);
            }
            return result;
        }
        // Получение исправленной строки
        static string GetCorStr(string text, string[] dict)
        {
            StringBuilder result = new StringBuilder(text);
            string[] textWords;
            CutWords(text, out textWords);
            string corWord;
            for (int i = 0; i < textWords.Length; i++)
            {
                corWord = GetCorWord(textWords[i], dict);
                result.Replace(textWords[i], corWord);
            }
            return result.ToString();
        }
        // Получение исправленного слова
        static string GetCorWord(string word, string[] dict)
        {
            List<string> strList = new List<string>();
            string result;
            int rate = 2;
            int count;

            for (int i = 0; i < dict.Length; i++)
            {
                count = Conf(word, dict[i]);

                if (count == rate)
                {
                    strList.Add(dict[i]);
                }
                if (count < rate)
                {
                    rate = count;
                    strList.Clear();
                    strList.Add(dict[i]);
                }
                if (count == 0)
                {
                    break;
                }
            }

            if (strList.Count == 0)
            {
                result = $"{{{word}?}}";
            }
            else if (rate == 0)
            {
                result = word;
            }
            else if (strList.Count == 1)
            {
                result = strList[0];
            }
            else
            {
                result = "{" + String.Join<string>(" ", strList) + "}";
            }
            return result;
        }
        // Нахождение веса различий в словах
        public static int Conf(string source, string target)
        {
            int insertRate = 1;
            int deleteRate = 1;
            int replaceRate;

            int m = source.Length + 1;
            int n = target.Length + 1;
            int[,] matrix = new int[m, n];
            // Заполнение matrixR:
            // 0 - замена символа
            // 1 - удаление символа
            // 2 - вставка символа
            int[,] matrixR = new int[m, n];

            for (int i = 0; i < m; i++)
            {
                matrix[i, 0] = i;
                matrixR[i, 0] = 1;
            }
            for (int j = 0; j < n; j++)
            {
                matrix[0, j] = j;
                matrixR[0, j] = 2;
            }
            matrix[0, 0] = 0;
            for (int i = 1; i < m; i++)
            {
                for (int j = 1; j < n; j++)
                {
                    deleteRate = matrixR[i - 1, j] == 1 ? 2 : 1;
                    insertRate = matrixR[i, j - 1] == 2 ? 2 : 1;
                    replaceRate = source[i - 1] == target[j - 1] ? 0 : 2;
                    matrix[i, j] = Min(matrix[i - 1, j] + deleteRate,
                        matrix[i, j - 1] + insertRate,
                        matrix[i - 1, j - 1] + replaceRate, out matrixR[i, j]);
                }
            }

            return matrix[m - 1, n - 1];
        }
        // Поиск минимального значения из 3-х int
        private static int Min(int first, int second, int third, out int oper)
        {
            if (first < second)
            {
                if (first < third)
                {
                    oper = 1;
                    return first;
                }
                oper = 0;
                return third;
            }
            else if (second < third)
            {
                oper = 2;
                return second;
            }
            else
            {
                oper = 0;
                return third;
            }
        }
        // Выделение слов из строки
        static void CutWords(string inStr, out string[] outStr)
        {
            Regex regex = new Regex(@"\b\w+\b");
            MatchCollection matchCol = regex.Matches(inStr);
            outStr = new string[matchCol.Count];
            for (int i = 0; i < matchCol.Count; i++)
            {
                outStr[i] = matchCol[i].ToString();
            }
        }
    }
}
