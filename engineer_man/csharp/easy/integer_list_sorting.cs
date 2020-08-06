using System;
using System.Linq;

class MainClass {
    static void Main(string[] args) {
        /*
        Short list
            value1 3,4,-9
            output 4,3,-9

        Long list
            value1 3,9,10,5,2,7,9,2
            output 10,9,9,7,5,3,2,2
        */
        string numbers = args[0];

        var result = numbers.Split(',')
            .Select(s => int.Parse(s))
            .OrderByDescending(n => n);

        Console.WriteLine(string.Join(",", result));
    }
}