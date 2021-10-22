using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace WordSearch
{
    static class Program
    {
        static void Main(string[] args)
        {
            string[] docs = { @"C:\Users\madel\OneDrive\Skrivbord\documents\Doc1.txt",
                @"C:\Users\madel\OneDrive\Skrivbord\documents\Doc2.txt",
                @"C:\Users\madel\OneDrive\Skrivbord\documents\Doc3.txt" };

            var allDocs = new List<string[]>();

            for (int i = 0; i <= docs.Length - 1; i++)
            {
                allDocs.Add(ReadDocument(docs[i]));
            }

            Menu(allDocs);
        }

        public static string[] ReadDocument(string docName)
        {
            var file = File.ReadAllText(docName);

            return Regex.Split(file, @"[^\w]+").Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
        }

        public static void Menu(List<string[]> allDocs)
        {
            bool keepgoing = true;
            Console.WriteLine("Hej!");

            while (keepgoing)
            {
                Console.WriteLine("Vad vill du göra med dokumenten?");
                Console.WriteLine("1. Sök efter antal förekomster av ett ord");
                Console.WriteLine("2. Skriv ut resultatet av sökning");
                Console.WriteLine("3. Sortera dokument i bokstavsordning");
                Console.WriteLine("4. Skriv ut ett antal ord ur sorterat dokument");
                Console.WriteLine("5. Avsluta");

                string answer = Console.ReadLine();

                switch (answer)
                {
                    case "1":
                        //SearchWord();
                        break;
                    case "2":
                        //PrintResult();
                        break;
                    case "3":
                       allDocs = SortDocument(allDocs);
                        break;
                    case "4":
                        PrintXAmountOfWords(allDocs[0], 15);
                        Console.WriteLine();
                        PrintXAmountOfWords(allDocs[1], 15);
                        Console.WriteLine();
                        PrintXAmountOfWords(allDocs[2], 15);
                        break;
                    case "5":
                        keepgoing = false;
                        break;
                    default:
                        ErrorMessage();
                        break;
                }
            }
        }

        public static void PrintXAmountOfWords(string[] words, int count)
        {
            if (count == 0 || words.Length < 1) return;

            var word = words[0];

            Console.WriteLine(word);

            PrintXAmountOfWords(words.Skip(1).ToArray(), count - 1);

            return;
        }

        public static void ErrorMessage()
        {
            Console.WriteLine("Fel input, vänligen försök igen.");
            Console.WriteLine();
        }

        public static string[] SortDocumentWords(string[] document)
        {
            return document.OrderBy(word => word).ToArray();
        }

        public static List<string[]> SortDocument(List<string[]> allDocs)
        {
            return allDocs.Select(x => SortDocumentWords(x)).ToList();
        }

    }
}
