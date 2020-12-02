using System;
using System.Linq;

class MainClass {
    static void Main(string[] args) {
        /*
        Permutation
            value1 brian,airbn
            output Yes
        */
        string words = args[0];

        var correct = words.Split(',')[0];
        var attempt = words.Split(',')[1];

        var group1 = correct.GroupBy(c => c);
        var group2 = attempt.GroupBy(c => c);

        bool result = true;
        foreach (var g in group1)
        {
            result &= group2.Any(g2 => g2.Key == g.Key && g2.Count() == g.Count());
        }

        Console.WriteLine(result ? "Yes" : "No");
    }
}