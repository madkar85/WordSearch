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
            /*
            string[] docs = { @"C:\Users\madel\OneDrive\Skrivbord\Utbildning\Datalogi\Assignments\WordSearch\documents\Doc1.txt",
                @"C:\Users\madel\OneDrive\Skrivbord\Utbildning\Datalogi\Assignments\WordSearch\documents\Doc2.txt",
                @"C:\Users\madel\OneDrive\Skrivbord\Utbildning\Datalogi\Assignments\WordSearch\documents\Doc3.txt" };
            */

            string returnString = @"..\..\..\";
            string[] docs2 = { Path.Combine(Environment.CurrentDirectory, returnString, "Doc1.txt"),
                               Path.Combine(Environment.CurrentDirectory, returnString, "Doc2.txt"),
                               Path.Combine(Environment.CurrentDirectory, returnString, "Doc3.txt") };

            //Reads and adds document to list
            var allDocs = new List<string[]>();

            for (int i = 0; i <= docs2.Length - 1; i++)
            {
                allDocs.Add(ReadDocument(docs2[i]));
            }

            Menu(allDocs);
        }

        /// <summary>
        /// Reads documents and removes null or whitespace
        /// </summary>
        /// <param name="docName"></param>
        /// <returns></returns>
        public static string[] ReadDocument(string docName)
        {
            var file = File.ReadAllText(docName);

            return Regex.Split(file, @"[^\w]+").Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
        }

        /// <summary>
        /// Menu
        /// </summary>
        /// <param name="allDocs"></param>
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
                Console.WriteLine("4. Avsluta");

                string answer = Console.ReadLine();

                switch (answer)
                {
                    case "1":
                        Console.WriteLine("Vilket ord söker du?");
                        string word = Console.ReadLine();
                        dictionaryOfWords = SearchForWord(allDocs, word);
                        myResult.Add(RankDictionary(dictionaryOfWords, word));
                        break;
                    case "2":
                        PrintAllResults(myResult);
                        break;
                    case "3":
                        PrintWords(allDocs);
                        break;
                    case "4":
                        keepgoing = false;
                        break;
                    default:
                        ErrorMessage();
                        break;
                }
            }
        }



        //A recursive methode that prints out X amount of words

        public static void PrintXAmountOfWords(string[] words, int count)
        {
            if (count == 0 || words.Length < 1) return;

            var word = words[0];

            Console.WriteLine(word);

            PrintXAmountOfWords(words.Skip(1).ToArray(), count - 1);

            return;
        }

        /// <summary>
        /// Types out an error message
        /// </summary>
        public static void ErrorMessage()
        {
            Console.WriteLine("Fel input, vänligen försök igen.");
            Console.WriteLine();
        }

        /// <summary>
        /// Sorts words in document
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        /// Ordo: O(1)
        public static string[] SortDocumentWords(string[] document)
        {
            return document.OrderBy(word => word).ToArray();
        }

        /// <summary>
        /// Sorts list of strings arrays
        /// </summary>
        /// <param name="allDocs"></param>
        /// <returns></returns>
        /// Ordo: O(1)
        public static List<string[]> SortDocument(List<string[]> allDocs)
        {
            return allDocs.Select(x => SortDocumentWords(x)).ToList();
        }

        /// <summary>
        /// Gets input of X amount of words then prints them out
        /// </summary>
        /// <param name="allDocs"></param>
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

        /// <summary>
        /// Counts occurring words in documents
        /// </summary>
        /// <param name="allDocs"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public static Dictionary<string, int> SearchForWord(List<string[]> allDocs, string word)
        {
            Dictionary<string, int> dictionaryOfWords = new Dictionary<string, int>();
            int docCount = 1;


            foreach (string[] doc in allDocs)
            {

                dictionaryOfWords.Add("doc" + docCount, GetWordCount(doc, word));
                docCount++;
            }

            return dictionaryOfWords;
        }

        /// <summary>
        /// Organizing documents based on a searched word
        /// </summary>
        /// <param name="dictionaryOfWords"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        private static string RankDictionary(Dictionary<string, int> dictionaryOfWords, string word)
        {
            Dictionary<string, int> myDictionary = dictionaryOfWords;

            string result = "";

            result += $"Det sökta ordet {word} hittades enligt följande:\n";
            foreach (KeyValuePair<string, int> doc in myDictionary.OrderByDescending(key => key.Value))
            {
                result += $"{doc.Key}: {doc.Value} gånger.\n";
            }

            Console.WriteLine(result);

            return result;
        }

        /// <summary>
        /// Prints out all serchresults from a list
        /// </summary>
        /// <param name="results"></param>
        /// Ordo: O(n)
        private static void PrintAllResults(List<string> results)
        {
            
            foreach (string res in results)
            {
                Console.WriteLine(res);
            }
        }

        /// <summary>
        /// Counts a word from an array
        /// </summary>
        /// <param name="arrayWords"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public static int GetWordCount(string[] arrayWords, string word)
        {
            // Ordo: (On) ??
            int count = 0;
            for (int i = 0; i < arrayWords.Length; i++)
            {
                if (arrayWords[i] == word)
                {
                    count++;
                }
            }
            return count;
        }

    }
}
