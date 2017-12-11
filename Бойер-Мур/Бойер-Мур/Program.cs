using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Бойер_Мур
{
    class Program
    {
        static int ShiftNonIdentSymbol;
        /// <summary>
        /// Таблица смещений(словарь)
        /// </summary>
        static Dictionary<int, char> TableShifts;
        ///<summary>Составление таблицы смещений</summary>
        ///<param name="readtemplate">Введнный шаблон</param>
        public static void TableShift(string readtemplate)
        {
            ShiftNonIdentSymbol = readtemplate.Length;
            TableShifts = new Dictionary<int, char>();
            for (int i = readtemplate.Length-1; i >0 ; i--)
            {
                TableShifts.Add( readtemplate.Length - i,readtemplate[i]);//длина слова минус позиция символа
            }
        }
        /// <summary>
        /// Таблица прыжков
        /// </summary>
        static Dictionary<string,int> TableJumps;
        ///<summary>Составление таблицы прыжков</summary>
        ///<param name="readtemplate">Введнный шаблон</param>
        static public void TableJump(string readtemplate)
        {
            TableJumps = new Dictionary<string, int>();
            for(int i = 1; i < readtemplate.Length-1; i++)
            {
                TableJumps.Add(readtemplate.Substring(0,i), i);
            }
        }
        /// <summary>
        /// сравнение с суффиксами
        /// </summary>
        static public int ReadTableJump(int NumDifferent, string equalSource, string template)
        {
            for(int i = 0; i < equalSource.Length; i++)
            {
                foreach(KeyValuePair<string, int> Node in TableJumps)
                {
                    if (equalSource.Substring(i) == Node.Key) return Node.Value; 
                }
            }
            return 0;
        }
        ///<summary>Алгоритма Бойера-Мура</summary>
        ///<param name="readsource">Введенная исходная строка</param>
        ///<param name="readtemplate">Введенный шаблон</param>
        ///<param name="sensitivity">Чувствительность к регистру</param>
        static public int GetKeyByValue(char value)
        {
            foreach (var recordOfDictionary in TableShifts)
            {
                if (recordOfDictionary.Value.Equals(value))
                    return recordOfDictionary.Key;
            }
            return ShiftNonIdentSymbol;
        }
        public static void BoyerMoore(string readsource, string readtemplate, bool sensitivity = false)
        {
            bool FlagJump = false;
            var source = readsource;
            var template = readtemplate;
            if (!sensitivity)
            {
                source = readsource.ToLower();
                template = readtemplate.ToLower();
            }
            TableShift(template);
            TableJump(template);
            if (template.Length > source.Length)
            {
                Console.WriteLine("Error: длинна искомого образца больше длинны текста");
                return;
            }
            if (template == source)
            {
                Console.WriteLine("Шаблон и исходная строка равны");
                return;
            }
            for (int i = template.Length; i < source.Length + 1;)                         // Основной цикл
            {
                FlagJump = false;
                for (int j = template.Length - 1; j >= 0; j--)                            // Цикл проверки на совпадения
                {
                    if (template[j] == source[i - template.Length + j])                   // Проверка на совпадения
                    {
                        FlagJump = true;
                        if (j == 0)                                                       // Если первый символ шаблона схож с текущим символом исходной строки
                        {
                            Console.WriteLine();
                            Console.WriteLine(source);
                            Console.WriteLine(template);
                            Console.WriteLine("Шаблон найден на {0} символе строки.", ((i - template.Length) + 1));
                            return;
                        }
                    }
                    else
                    {
                        if (FlagJump)
                        {
                            i += ReadTableJump(i - template.Length + j + 1, source.Substring(i - template.Length + j + 1, template.Length - 1-j),template);
                        }
                        i += GetKeyByValue(source[i]);
                        break;
                    }
                }
            }
            Console.WriteLine("Шаблон не был найден в исходной строке.");
        }
        static void Main(string[] args)
        {
            bool flag = false;
            Console.WriteLine("Введите текст в котором будем искать шаблон");
            string source = Console.ReadLine();
            Console.WriteLine("Введите искомый шаблон");
            string template = Console.ReadLine();
            Console.WriteLine("Важен ли регистр? Да/Нет");
            string Answer = Console.ReadLine();
            Answer = Answer.ToLower();
            if (Answer == "да") flag = true;
            BoyerMoore(source, template, flag);

            Console.ReadKey();
        }
    }
}
