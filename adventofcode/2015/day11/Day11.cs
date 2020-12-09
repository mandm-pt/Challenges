using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC.Solutions._2015
{
    internal class Day11 : BaseDayChallenge
    {
        public override int Year => 2015;
        public override int Day => 11;

        private static readonly string AllowedChars = "abcdefghjkmnpqrstuvwxyz";
        private static readonly Regex RepeatedPairsRegex = new Regex(@"(.)\1", RegexOptions.Compiled);
        private string CurrentPassword = "";

        protected override async Task LoadyAsync() => CurrentPassword = await File.ReadAllTextAsync(InputFilePath);

        protected override Task<string> Part1Async() => Task.FromResult((CurrentPassword = GetNextPassword(CurrentPassword)));

        protected override Task<string> Part2Async() => Task.FromResult((CurrentPassword = GetNextPassword(CurrentPassword)));

        private static string GetNextPassword(string currentPassword)
        {
            char[] password = currentPassword.ToArray();

            password = GetNextPassword(ref password);
            while (!IsValid(password))
            {
                password = GetNextPassword(ref password);
            }

            return new string(password);
        }

        private static ref char[] GetNextPassword(ref char[] password)
        {
            int charToModify = password.Length - 1;
            int charIdx = AllowedChars.IndexOf(password[charToModify]);

            if (charIdx == AllowedChars.Length - 1)
            {
                password[charToModify] = 'a';
                while (--charToModify > 0 && password[charToModify] == 'z')
                {
                    password[charToModify] = 'a';
                }
                charIdx = AllowedChars.IndexOf(password[charToModify]);
                password[charToModify] = AllowedChars[++charIdx];
            }
            else
                password[charToModify] = AllowedChars[charIdx + 1];

            return ref password;
        }

        private static bool IsValid(char[] password)
            => !password.Contains('i')
                && !password.Contains('o')
                && !password.Contains('l')
                && HasStraightChars(password, 3)
                && RepeatedPairsRegex.Matches(new string(password)).Select(m => m.Value).Distinct().Count() > 1
                ;

        private static bool HasStraightChars(char[] password, int straightCount)
        {
            int lastChar = password[0];
            int sequenceCount = 0;
            for (int i = 1; i < password.Length && sequenceCount < straightCount - 1; i++)
            {
                sequenceCount = password[i] == lastChar + 1
                    ? sequenceCount + 1
                    : 0;

                lastChar = password[i];
            }

            return sequenceCount >= straightCount - 1;
        }
    }
}
