using System;

class MainClass {
    static void Main(string[] args) {
        string[] matrixSize = args[0].Split('x');

        int matrixWidth = int.Parse(matrixSize[0]);
        int matrixHeight = int.Parse(matrixSize[1]);

        int maxNumber = matrixWidth * matrixHeight;
        int jumpToCorners = matrixWidth - 1;

        int result = maxNumber + 
            (maxNumber - jumpToCorners) + 
            (maxNumber - jumpToCorners * 2) +
            (maxNumber - jumpToCorners * 3);

        System.Console.WriteLine(result);
    }
}
