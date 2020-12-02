using System;
using System.Linq;

class MainClass {
    static void Main(string[] args) {
        /*
        Regular Cookie
            value1 10000101,100001111000010110100001
            output 23
        Big Cookie
            value1 1010100100110110,1111010000011111111000111010100100110110
            output 63
        */
        string[] values = args[0].Split(',');
        string cookie = values[0];
        string jar = values[1];

        int startIdx = jar.IndexOf(cookie);
        int endIdx = startIdx + (cookie.Length - 1);

        Console.WriteLine(startIdx + endIdx);
    }
}