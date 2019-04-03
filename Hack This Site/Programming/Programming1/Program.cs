using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Programming1
{
    class Program
    {
        const string WordListPath = "./wordlist.txt";

        static async Task Main(string[] args)
        {
            var words = await ReadWords(WordListPath);

            Console.WriteLine("Paste list of words:");

            var scrambledWords = new List<string>();
            string word;
            while ((word = Console.ReadLine()) != string.Empty)
                scrambledWords.Add(word);

            var solution = ProcessWords(words, scrambledWords);
            Console.WriteLine(solution);
            Console.ReadKey();
        }

        private static Task<string[]> ReadWords(string filePath)
        {
            if (!File.Exists(filePath))
                return Task.FromResult(Array.Empty<string>());

            return File.ReadAllLinesAsync(filePath);
        }

        private static string ProcessWords(IEnumerable<string> words, IEnumerable<string> scrambledWords)
        {
            var solutionStrings = new List<string>();

            foreach (var scrambledWord in scrambledWords)
            {
                foreach (var word in words.Where(w => w.Length == scrambledWord.Length))
                {
                    var scrambledChars = scrambledWord.OrderBy(c => c).Select(c => c);
                    var wordChars = word.ToCharArray().OrderBy(c => c).Select(c => c);

                    if (scrambledChars.SequenceEqual(wordChars))
                    {
                        solutionStrings.Add(word);
                        break;
                    }
                }
            }

            return string.Join(", ", solutionStrings.ToArray());
        }
    }
}