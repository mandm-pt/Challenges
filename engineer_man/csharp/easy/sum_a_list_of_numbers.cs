using System;
using System.Linq;

class MainClass {
    static void Main(string[] args) {
        /*
        Short list
            value1 2,5,1
            output 8
        
        Long list
            value1 3,9,10,5,2,7,9,2
            output 47
        */
        string numbers = args[0];

        var result = numbers.Split(',')
            .Select(s => int.Parse(s))
            .Sum();

        Console.WriteLine(result);
    }
}