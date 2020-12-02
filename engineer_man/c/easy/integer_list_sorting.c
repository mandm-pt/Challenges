#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int compare_desc(const void* n1, const void* n2)
{
    if (*(int*)n1 > *(int*)n2) return -1;
    else if (*(int*)n1 < *(int*)n2) return 1;

    return 0;
}

int main(int argc, char **argv) {
    /*
    Short list
        value1 3,4,-9
        output 4,3,-9

    Long list
        value1 3,9,10,5,2,7,9,2
        output 10,9,9,7,5,3,2,2
    */
    char num_string[128];
    int numbers[64] = {0};
    strcpy(num_string, argv[1]);
    int numbers_len = strlen(num_string);

    int nCount = 0, start_num_idx = 0;
    char buff[10];
    for (int i = 0; i <= numbers_len; i++)
    {
        if (num_string[i] == ',' || num_string[i] == '\0')
        {
            int num_len = i - start_num_idx;
            strncpy(buff, num_string + (start_num_idx*sizeof(char)), num_len);
            buff[num_len] = '\0'; // adjust end

            numbers[nCount] = atoi(buff);
            nCount++;
            start_num_idx = i+1;
        }
    }
    
    qsort(numbers, nCount, sizeof(int), compare_desc);

    for (int i = 0; i < nCount; i++)
    {
        printf("%i", numbers[i]);

        if (i+1 < nCount)
            printf(",");
    }
}