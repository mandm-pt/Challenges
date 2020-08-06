using System;

class MainClass {
    static void Main(string[] args) {
        /*
        Short List
            value1 9,5
            value2 7,10
            output 9,7,5,10

        Large List
            value1 34,18,4,102
            value2 15,19,120,64
            output 34,15,18,19,4,120,102,64
        */
        string value1 = args[0];
        string value2 = args[1];

        var split1 = value1.Split(',');
        var split2 = value2.Split(',');

        string result = "";
        for (int i = 0; i < split1.Length; i++)
        {
            result += $"{split1[i]},{split2[i]},";
        }
        Console.WriteLine(result.TrimEnd(','));
    }
}