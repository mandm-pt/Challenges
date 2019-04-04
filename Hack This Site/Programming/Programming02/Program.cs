using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Programming02
{
    class Program
    {
        private static readonly string ImagePath = "download.png";
        private static IDictionary<string, char> MorseCode;

        static void Main(string[] args)
        {
            MorseCode = GetMorseCodeDictionary();

            Console.WriteLine($"Press ENTER once '{ImagePath}' is in place.");
            Console.ReadKey();

            string morseCode = GetMorseCodeFromImage(ImagePath);
            string solution = ProcessMorseCode(morseCode);

            Console.WriteLine(solution);
            Console.ReadKey();
        }

        private static IDictionary<string, char> GetMorseCodeDictionary()
            => new Dictionary<string, char> {
                // Letters
                {".-", 'A'},
                {"-...", 'B'},
                {"-.-.", 'C'},
                {"-..", 'D'},
                {".", 'E'},
                {"..-.", 'F'},
                {"--.", 'G'},
                {"....", 'H'},
                {"..", 'I'},
                {".---", 'J'},
                {"-.-", 'K'},
                {".-..", 'L'},
                {"--", 'M'},
                {"-.", 'N'},
                {"---", 'O'},
                {".--.", 'P'},
                {"--.-", 'Q'},
                {".-.", 'R'},
                {"...", 'S'},
                {"-", 'T'},
                {"..-", 'U'},
                {"...-", 'V'},
                {".--", 'W'},
                {"-..-", 'X'},
                {"-.--", 'Y'},
                {"--..", 'Z'},

                // Numbers
                {"-----", '0'},
                {".----", '1'},
                {"..---", '2'},
                {"...--", '3'},
                {"....-", '4'},
                {".....", '5'},
                {"-....", '6'},
                {"--...", '7'},
                {"---..", '8'},
                {"----.", '9'}
            };

        private static string GetMorseCodeFromImage(string imagePath)
        {
            if (!File.Exists(imagePath))
                return string.Empty;

            var img = new Bitmap(imagePath);

            int lastPosition = 0;
            int currentPosition = -1;

            var morseCode = new StringBuilder();
            // percorre os pixeis da imagem na vertical
            for (int y = 0; y < img.Height; y++)
            {
                // percorre os pixeis da imagem na horizontal
                for (int x = 0; x < img.Width; x++)
                {
                    currentPosition += 1;
                    // Checks if is a white pixel
                    if (img.GetPixel(x, y).ToArgb() == Color.White.ToArgb())
                    {
                        morseCode.Append(char.ConvertFromUtf32(currentPosition - lastPosition));
                        lastPosition = currentPosition;
                    }
                }
            }
            img.Dispose();

            return morseCode.ToString();
        }

        private static string ProcessMorseCode(string morseCode)
        {
            if (string.IsNullOrWhiteSpace(morseCode))
                return string.Empty;

            var solution = new StringBuilder();
            foreach (var @char in morseCode.Trim().Split(' '))
            {
                solution.Append(MorseCode[@char]);
            }

            return solution.ToString();
        }
    }
}