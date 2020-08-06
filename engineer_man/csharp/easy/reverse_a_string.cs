using System;
using System.Linq;

class MainClass {
    static void Main(string[] args) {
        /*
        Simple string
            value1 cat
            output tac

        Complex string
            value1 sd#$)(&*09&M0
            output 0M&90*&()$#ds
        */
        string value1 = args[0];

        var chars = value1.ToArray();
        Array.Reverse(chars);
        Console.WriteLine(new string(chars));
    }
}