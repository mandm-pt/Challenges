#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int main(int argc, char **argv) {
    /*
    Simple string
        value1 cat
        output tac

    Complex string
        value1 sd#$)(&*09&M0
        output 0M&90*&()$#ds
    */
    char value1[128];
    strcpy(value1, argv[1]);

    int endIdx;
    for (int i = 0; value1[i] != '\0'; i++)
    {
        endIdx = i;
    }

    char final[128];
    for (int i = endIdx;  i >= 0; i--)
    {
        final[endIdx-i] = value1[i];
    }
    final[endIdx+1] = '\0';

    printf("%s", final);
}