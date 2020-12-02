#!/bin/bash
: '
You will receive a string of characters. Your job is to return a list 
of numbers that represent the distance of each letter to it is nearest 
vowel. If the current character is a vowel then the distance is considered 0.

Sample Input 
    abcxyz

Sample Output
    0,1,2,3,2,1

Guidelines

The alphabet wraps around, this means the letter "z" and "a" are considered adjacent
The input string will be between 1 and 100 chars long 
Vowels are: a, e, i, o, u
The input string will contain lowercase letters only
'
# Solution
# 0 aeiou
# 1 bdfhjnptvz
# 2 cgkmqswy
# 3 lrx

a=$["0121"*2+"012321"*3];
echo $a

# abcdefghijklmnopqrstuvwxyz
# 01210121012321012321012321



