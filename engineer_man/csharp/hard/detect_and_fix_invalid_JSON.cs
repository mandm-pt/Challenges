using System;
using System.Text;

class MainClass {
    static void Main(string[] args) {
        /*
        Single Problem
            value1 {"name":"em"
            output {"name":"em"}
        Multiple Problems
            value1 "name":"em
            output {"name":"em"}
        */
        string json = args[0];
        
        if (json[0] != '{')
            json = "{" + json;

        if (json[json.Length - 1] != '}')
            json += "}";

        var sb = new StringBuilder("{");
        for (int i = 1; i < json.Length; i++)
        {
            if (json[i] == '"' && QuoteAllowed(json[i-1]))
            {
                sb.Append(json[i]);
            }
            else if (json[i] == '"' && HasMissingComma(json[i - 1]))
            {
                sb.Append(':').Append(json[i]);
            }
            else if (json[i] == '}' && HasMissingQuotes(json[i - 1]))
            {
                sb.Append('"').Append(json[i]);
            }
            else
            {
                sb.Append(json[i]);
            }
        }

        string result = sb.ToString();
        System.Console.WriteLine(result);
    }

    static bool QuoteAllowed(char previousChar) => previousChar == '{' || previousChar == ':';
    static bool HasMissingQuotes(char previousChar) => previousChar != '"';
    static bool HasMissingComma(char previousChar) => previousChar == '"';
}