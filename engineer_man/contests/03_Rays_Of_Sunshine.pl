#!/usr/bin/perl
=for
You must write a program that generates a picture of a sun with variable size rays.
You have to use specific numbers as the pixels. The length of the rays and starting 
number for the pixels are provided as separate arguments. The number used for each 
pixel should increase by one from left to right and then top to bottom.

The sun is a 3x3 block of pixels in the middle of the picture and there are 8 rays 
going off in different directions (up-left, up, up-right, left, right, down-left, 
down, down-right).

Sample Input Argument 1 (how long each ray is)
2

Sample Input Argument 2 (first number in numbering the rays and sun from left to right, 
top to bottom. first number is top left in the expected output)
6

Expected Output Sample
6  7  8
 9 O 1
  234
5678901
  234
 5 6 7
8  9  0
=cut
# Solution
($r,$n)=@ARGV;sub p{$n++%10}map{print!($_=abs)?map p,0..$r*2:$"x($r-$_).p.$"x--$_.p.$"x$_.p,$/}-++$r..$r
