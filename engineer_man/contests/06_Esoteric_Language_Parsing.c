/*
Esoteric Language Parsing
A new programming language has been created called EM language. 
The language is very simple and only has three operations: shift 
current character right one place and output current character 
lower or upper case. You must parse the source code and output 
the proper string by using the rules below.

Operations
Shift position right
>

Output current character and return back to `a` (lowercase)
.

Output current character and return back to `a` (uppercase)
!

Sample Output (column 1) and Sample Input Argument 1 (column 2)
a    .
b    >.
c    >>.
A    !
B    >!
C    >>!
ab   .>.
abc  .>.>>.
abcd .>.>>.>>>.
aBcD .>!>>.>>>!

Guidelines
    Input Argument 1 will be between 1-5000 characters long
    The number of consecutive `>` characters will not exceed 25
    Each time `.` or `!` is used the current character should reset to `a`
*/
//Solution
#include<stdio.h>
int main(int c,char**a){for(int i,j;c=a[1][i++];j=c>61?j+1:!putchar(c%45+j^96));}