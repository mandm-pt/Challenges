using System;
using System.Linq;

class MainClass {
    static void Main(string[] args) {
        /*
            Short number
                value1 31
                output even

            Long number
                value1 662638
                output odd
        */
        int number = Convert.ToInt32(args[0]);
        bool isEven = number.ToString()
            .ToArray()
            .Sum(c => int.Parse(c.ToString())) % 2 == 0;

        Console.WriteLine(isEven ? "even" : "odd");
    }
}