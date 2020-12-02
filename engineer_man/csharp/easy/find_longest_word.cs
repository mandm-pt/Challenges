using System;
using System.Linq;

class MainClass {
    static void Main(string[] args) {
        /*
        Regular
            value1 run,barn,yellow,barracuda,shark,fish,swim
            output barracuda
            
        Same Size
            value1 fishes,sam,gollum,sauron,frodo,balrog
            output fishes,gollum,sauron,balrog
        */
        string words = args[0];

        var result = words.Split(',')
            .Select(s => s.Trim())
            .GroupBy(s => s.Length)
            .OrderByDescending(s => s.Key)
            .First();

        Console.WriteLine(string.Join(",", result).ToLower());
    }
}