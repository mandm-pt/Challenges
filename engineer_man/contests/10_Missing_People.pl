#!/usr/bin/perl
=for
You will be provided a persons name and a long string of number, letters
 and characters. You must find the provided name in the long string and 
 return the starting position of the name. The letters of the name can 
 be spaced out with a random character in between.


Possible combinations for a 3 letter name(X represents a random character):
    joe
    jXoe
    joXe
    jXoXe

Sample Input Argument 1 (the name)
    bobby
Sample Input Argument 2 (the string of data)
    akubwobbzywdawda

Sample Output
    3

Guidelines
    There is a maximum of 1 random character between each letter of the name. (0 or 1)
    The name and the string only consist of lowercase and uppercase english letters (a-zA-Z)
    Uppercase/Lowercase letters have to match.
    The name will be between 1 and 10 characters long 
    The string will be between 1 and 100 characters long
    Counting starts at 0
=cut
# Solution
pop=~join'.?',pop=~/./g;print@-
