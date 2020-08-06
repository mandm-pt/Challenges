using System;
using System.Collections.Generic;

class MainClass {
    static void Main(string[] args) {
        int startingNum = Convert.ToInt32(args[0]);
        int numValuesToDisplay = Convert.ToInt32(args[1]);

        int n2 = 0;
        int n1 = 1;
        int current = 1;

        var result = new List<int>(numValuesToDisplay);
        for (int i = 2; result.Count != numValuesToDisplay; i++)
        {
            current = n1 + n2;
            n2 = n1;
            n1 = current;

            if (current > startingNum)
                result.Add(current);
        }

        System.Console.WriteLine(string.Join(",", result));
    }
}