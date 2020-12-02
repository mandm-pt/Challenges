#!/bin/bash
: '
You must write a program that spirals a given sequence of characters n 
number of times. The word to spiral and the number of times to spiral 
are provided in separate arguments.

Guidelines
    Input Argument 1 will not contain spaces
    Input Argument 2 will be more than 1 but less than 100
    The original word should be included unchanged first
    Each spiral iteration should be separated by a newline

Input Argument 1 (sample)
SpiralMePlz

Input Argument 2 (sample)
7

Expected Output (sample)
SpiralMePlz
piralMePlzS
iralMePlzSp
ralMePlzSpi
alMePlzSpir
lMePlzSpira
MePlzSpiral
'
# Solution
for((;i<$2;)){
echo ${1:i}${1::i++}
}