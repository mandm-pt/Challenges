using System;
using System.Linq;

class MainClass {
    static void Main(string[] args) {
        /*
        String of numbers
            value1 08989082882348823838
            output 8
        */
        string numbers = args[0];

        var result = numbers.GroupBy(c => c)
            .OrderByDescending(c => c.Count())
            .Select(g => g.Key)
            .First();

        Console.WriteLine(result);
    }
}