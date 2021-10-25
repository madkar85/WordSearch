using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordSearcherML
{
    public static class DocumentAssist
    {
        //Under construction
        public static int GetWordCount(Dictionary<int, string> dictionaryWords, string word)
        {
            int count = -1;
            foreach (KeyValuePair<int, string> k in dictionaryWords)
                if (k.Value == word)
                    count++;
            return count++;
        }


        //Under construction
        public static void PrintSavedDataStructure(Dictionary<int, string> dictionaryWords)
        {
            foreach (KeyValuePair<int, string> k in dictionaryWords)
                Console.WriteLine(k.Value);
        }


        //Under construction
        public static List<string> PrintXAmountOfWords(List<string> listOfWords, int x)
        {
            List<string> words = new List<string>();
            for (int i = 0; i < x; i++)
                words.Add(listOfWords[i]);
            return words;

        }
    }
}
