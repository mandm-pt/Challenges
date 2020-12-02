#!/bin/bash
: '
You will be given a string full of random letters. Some of the 
letters will be capitalized while others will be lowercase. You 
need to return the string with the letters that are capitalized 
flipped to lowercase and the ones that where lowercase should become capitalized.

Sample Input Argument 1
    HeLLo

Sample Output
    hEllO

Guidelines
    The Input will be a string containing  only letters from the English Alphabet
    The output should be the same string with the capital and lowercase letters flipped
'
# Solution
echo ${1~~}