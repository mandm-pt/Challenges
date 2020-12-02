#!/usr/bin/perl
=for
Plotting The Trail 2
Imagine you are driving a car. You receive directions like TR 
which tells you to make a 90 degree clockwise turn, or FW which 
tells you to move forward by one square. Your job is to write a 
program to interpret these instructions and return the ending 
position along with the direction the 'car' is facing.

Possible Movements
    TL = Turn Left/CounterClockwise
    TR = Turn Right/Clockwise
    FW = Move forward 1 space
    BW = Move backwards 1 space

Sample Input Argument 1 (the positional movements)
    FWFWTLFW

Sample Output
    -1,2,L

Guidelines
    The starting coordinates are 0,0 and you are facing up (U)
    The output can be positive or negative x and y values along with 
        the direction you are currently facing (L, R, U, D)
=cut
# Solution
map/T/?$d=/L/?--$d%4:++$d%4:($d%2?$x:$y+=($d+ord)%6>1?1:-1),pop=~/../g;print"$x,$y,".(U,R,D,L)[$d]



# ./08_Plotting_The_Trail_2.pl FWFWFWBWTLFWTLTLFWFWTRBWTRTRBWBW
# 1,1,U

# /mnt/w/github/mine/challenges/engineer_man/contests
