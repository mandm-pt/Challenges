#!/bin/bash
: '
You must write a program that takes an encrypted string and decrypt it 
according to specific rules and then print it out. Decryption will occur 
by performing two operations.

Sample Input Argument 1 (the encrypted string)
HGJILKBADCFE

Operation 1:
Swap the first half of the string with the second half, which should leave 
you with:
BADCFEHGJILK

Operation 2: 
Swap every two characters with each other such as swapping character 1 with 
2, 3 with 4, etc., which should leave you with the decrypted string:
ABCDEFGHIJKL

Guidelines:
    Input Argument 1 wil contain only uppercase letters
    Input Argument 1s length will be between 2 and 100 characters
    Input Argument 1s length will always be an even number
'
# Solution 1
sed -r "s/(.)(.)/\2\1/g"<<<${1:s=${#1}/2}${1::s}

# Solution 2
fold -w2<<<${1:s=${#1}/2}${1::s}|rev|tr -d '\n'
