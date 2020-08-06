using System;
using System.Linq;

class MainClass {
    static void Main(string[] args) {
        /*
        Short phrase
            value1 A test case
            output t,e,s
        Long phrase
            value1 The way this mode works is by looking for the mode of the characters of any given string
            output o
        */
        string text = args[0];

        var topChars = text.ToCharArray()
            .Where(c => c != ' ')
            .GroupBy(c => c)
            .OrderByDescending(g => g.Count());

        var veryTopChar = topChars.First();
        var equallyTopChars = topChars.Where(g => g.Count() == veryTopChar.Count()).Select(g => g.Key).Distinct();

        System.Console.WriteLine(string.Join(",", equallyTopChars));
    }
}
