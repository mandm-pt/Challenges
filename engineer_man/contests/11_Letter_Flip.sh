#!/bin/bash
: '
This time you have not only been tasked with flipping the case of all 
vowel letters in the string, but also switching the sign on a number. 
If you are given a positive number you should return that number 
negative, and if the number is negative it should be returned positive. 
If you encounter a 0, just ignore it.

Sample Input 
    Ab-11-10z

Sample Output
    ab1-110z

Guidelines
    The letters you should switch the case of are vowels (A, E, I, O, U)
    The input will contain a string of numbers and letters
    The length of the input will be between 1 and 100
    Positive numbers are between 0 and 9, Negative numbers are between -9 and -1
'
# Solution
sed 's/[1-9]/-&/g;s/--//g'<<<${1~~[aeiouAEIOU]}
