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
            //alla sökvägarna till dokumenten i en array, istället för att lägga in dom individuellt
            string[] docs = { @"C:\Users\madel\OneDrive\Skrivbord\Utbildning\Datalogi\Assignments\WordSearch\documents\Doc1.txt",
                @"C:\Users\madel\OneDrive\Skrivbord\Utbildning\Datalogi\Assignments\WordSearch\documents\Doc2.txt",
                @"C:\Users\madel\OneDrive\Skrivbord\Utbildning\Datalogi\Assignments\WordSearch\documents\Doc3.txt" };

            string[] docs2 = { @"C:\Users\123\source\repos\WordSearch\WordSearch\Docs\Doc1.txt",
                               @"C:\Users\123\source\repos\WordSearch\WordSearch\Docs\Doc2.txt",
                               @"C:\Users\123\source\repos\WordSearch\WordSearch\Docs\Doc3.txt"};

            //läser in alla dokumenten och gör en lista av string arrays
            var allDocs = new List<string[]>();

            for (int i = 0; i <= docs2.Length - 1; i++)
            {
                allDocs.Add(ReadDocument(docs2[i]));
            }

            Menu(allDocs);
        }

        //Läser in dokumenten och splittar alla string efter specialtecken samt tar bort whitespace
        public static string[] ReadDocument(string docName)
        {
            var file = File.ReadAllText(docName);

            return Regex.Split(file, @"[^\w]+").Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
        }

        //Menyn som ger användaren valen den kan göra med dokumenten
        public static void Menu(List<string[]> allDocs)
        {
            bool keepgoing = true;

            Dictionary<string, int> dictionaryOfWords = new Dictionary<string, int>();
            List<string> myResult = new List<string>();

            Console.WriteLine("Hej!");

            while (keepgoing)
            {
                Console.WriteLine("Vad vill du göra med dokumenten?");
                Console.WriteLine("1. Sök efter antal förekomster av ett ord");
                Console.WriteLine("2. Skriv ut resultatet av sökning");
                Console.WriteLine("3. Sortera och skriv ut ord i dokumenten i bokstavsordning");
                Console.WriteLine("4. Skriv ut ett antal ord ur sorterat dokument");
                Console.WriteLine("5. Avsluta");

                string answer = Console.ReadLine();

                switch (answer)
                {
                    case "1":
                        //SearchWord();
                        Console.WriteLine("Vilket ord söker du?");
                        string word = Console.ReadLine();
                        dictionaryOfWords = SearchForWord(allDocs, word);
                        myResult.Add(RankDictionary(dictionaryOfWords, word));
                        break;
                    case "2":
                        //PrintResult();
                        PrintAllResuults(myResult);
                        break;
                    case "3":
                        PrintWords(allDocs);
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


        //Exempel på hur vi kan göra PrintXAmount-metoden rekursiv
        //Ordo = O(log n) ?
        public static void PrintXAmountOfWords(string[] words, int count)
        {
            if (count == 0 || words.Length < 1) return;

            var word = words[0];

            Console.WriteLine(word);

            PrintXAmountOfWords(words.Skip(1).ToArray(), count - 1);

            return;
        }

        //Skriver ut felmeddelande vid felaktig input
        public static void ErrorMessage()
        {
            Console.WriteLine("Fel input, vänligen försök igen.");
            Console.WriteLine();
        }

        //Sorterar orden i ett dokument
        public static string[] SortDocumentWords(string[] document)
        {
            return document.OrderBy(word => word).ToArray();
        }

        //Sorterar en lista av string arrays med dokument
        public static List<string[]> SortDocument(List<string[]> allDocs)
        {
            return allDocs.Select(x => SortDocumentWords(x)).ToList();
        }

        private static void PrintWords(List<string[]> allDocs)
        {
            List<string[]> sortedDocs = new List<string[]>();
            int amount = 0, docNumber = 1;
            while (true)
            {
                Console.WriteLine("Hur många ord vill du skriva ut?");
                try
                {
                    amount = int.Parse(Console.ReadLine());
                    break;
                }
                catch
                {
                    ErrorMessage();
                }
            }

            sortedDocs = SortDocument(allDocs);



            foreach (string[] doc in sortedDocs)
            {
                Console.WriteLine($"Document {docNumber}\n");

                PrintXAmountOfWords(doc, amount);
                Console.WriteLine();

                docNumber++;
            }

        }

        public static Dictionary<string, int> SearchForWord(List<string[]> allDocs, string word)
        {
            Dictionary<string, int> dictionaryOfWords = new Dictionary<string, int>();
            int docCount = 1;


            foreach (string[] doc in allDocs)
            {

                dictionaryOfWords.Add("doc" + docCount, DocumentAssist.GetWordCount(doc, word));
                docCount++;
            }

            return dictionaryOfWords;
        }
        private static string RankDictionary(Dictionary<string, int> dictionaryOfWords, string word)
        {
            Dictionary<string, int> myDictionary = dictionaryOfWords;

            string result = "";

            result += $"Det sökta ordet {word} hittades enligt följannde:\n";
            foreach (KeyValuePair<string, int> doc in myDictionary.OrderByDescending(key => key.Value))
            {
                result += $"{doc.Key}: {doc.Value} gånger.\n";
            }

            Console.WriteLine(result);

            return result;
        }

        private static void PrintAllResuults(List<string> results)
        {
            foreach (string res in results)
            {
                Console.WriteLine(res);
            }
        }

    }
}
