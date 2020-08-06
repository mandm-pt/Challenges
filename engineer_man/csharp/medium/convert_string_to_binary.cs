using System;

class MainClass {
    static void Main(string[] args) {
        /*
        Letter
            value1 abc
            output 011000010110001001100011
        */
        string chars = args[0];

        foreach (char c in chars)
            Console.Write(Convert.ToString(c, 2).PadLeft(8, '0'));
    }
}