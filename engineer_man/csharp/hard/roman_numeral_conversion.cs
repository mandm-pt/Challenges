using System;
using System.Collections.Generic;

class MainClass {
    static void Main(string[] args) {
        /*
        Small Roman Numeral
            value1 VI
            output 6
        Large Roman Numeral
            value1 CDVII
            output 407
        */
        string romanNumeral = args[0];

        var convert = new Dictionary<char, int>
        {
            ['I'] = 1,
            ['V'] = 5,
            ['X'] = 10,
            ['L'] = 50,
            ['C'] = 100,
            ['D'] = 500,
            ['M'] = 1000,
        };

        int result = 0;
        int lastValue = int.MaxValue;
        foreach (char symbol in romanNumeral)
        {
            int value = convert[symbol];
            if (value > lastValue)
            {
                result += (value - lastValue * 2);
            }
            else
            {
                result += value;
            }
            lastValue = value;
        }


        Console.WriteLine(result);
    }
}