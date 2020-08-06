using System;
using System.Text.RegularExpressions;

class MainClass {
    static void Main(string[] args) {
        /*
        Short phrase
            value1 ayay imple-say est-tay ase-cay
            output a simple test case
        Long phrase
            value1 ig-pay atin-lay isyay usedyay inyay ools-schay o-tay each-tay anguage-lay onstructs-cay
            output pig latin is used in schools to teach language constructs
        */
        string text = args[0];

        string result = text.Replace("yay ", " ");

        var matches = Regex.Matches(text, "\\w+-([A-Za-z]+)ay");
        foreach (Match match in matches)
        {
            var parts = match.Value.Split('-');

            result = result.Replace(match.Value, parts[1].Replace("ay","") + parts[0]);
        }
        
        System.Console.WriteLine(result);
    }
}
