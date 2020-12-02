#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int main(int argc, char **argv) {
    /*
    Short list
        value1 2,5,1
        output 8
    
    Long list
        value1 3,9,10,5,2,7,9,2
        output 47
    */
    char *numbers = argv[1];
    int numbers_len = strlen(numbers);

    int sum = 0, start_num_idx = 0;
    char buff[10];
    for (int i = 0; i <= numbers_len; i++)
    {
        if (numbers[i] == ',' || numbers[i] == '\0')
        {
            int num_len = i - start_num_idx;
            strncpy(buff, numbers + (start_num_idx*sizeof(char)), num_len);
            buff[num_len] = '\0'; // adjust end

            int num = atoi(buff);
            sum += num;
            start_num_idx = i+1;
        }
    }
    
    printf("%d", sum);
}