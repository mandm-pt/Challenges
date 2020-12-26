using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day25 : BaseDayChallenge
    {
        public override int Year => 2020;

        public override int Day => 25;

        protected override Task<string> Part1Async()
        {
            ulong pubKey1 = ulong.Parse(inputLines[0]);
            ulong pubKey2 = ulong.Parse(inputLines[1]);

            var pubKey1LoopSize = GuessLoopSize(pubKey1);
            var pubKey2LoopSize = GuessLoopSize(pubKey2);

            var result2 = Transform(pubKey2, pubKey1LoopSize);
            var result1 = Transform(pubKey1, pubKey2LoopSize);

            if (result1 != result2)
                return Task.FromResult("no solution found!");

            return Task.FromResult(result1.ToString());
        }

        private ulong Transform(ulong key, int loopSize)
        {
            ulong fixNum = 20201227;
            ulong value = 1;

            for (int i = 0; i < loopSize; i++)
            {
                value = value * key;
                value = value % fixNum;
            }

            return value;
        }

        private int GuessLoopSize(ulong pubKey)
        {
            int subjectNumber = 7;
            ulong value = 1;
            var loopSize = 0;

            do
            {
                value = value * (ulong)subjectNumber;
                value = value % 20201227;
                loopSize++;
            } while (value != pubKey);

            return loopSize;
        }
    }
}