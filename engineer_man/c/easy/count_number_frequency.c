#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int main(int argc, char **argv) {
    /*
    String of numbers
        value1 08989082882348823838
        output 8
    */
    char value1[128];
    strcpy(value1, argv[1]);
    int numCount[10] = {0};

    for (int i = 0; i < strlen(value1); i++)
    {
        int num = value1[i] - 48;
        numCount[num] += 1;
    }
    
    int biggerIdx = 0;
    for (int i = 0; i < 10; i++)
    {
        if (numCount[i] > numCount[biggerIdx])
            biggerIdx = i;
    }
    
    printf("%d", biggerIdx);
}