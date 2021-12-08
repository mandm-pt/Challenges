package main

import (
	"bufio"
	"fmt"
	"math"
	"os"
	"strings"
)

type DigitFlag int

const (
	None DigitFlag = 0
	A    DigitFlag = 1 << iota
	B    DigitFlag = 2 << iota
	C    DigitFlag = 4 << iota
	D    DigitFlag = 8 << iota
	E    DigitFlag = 16 << iota
	F    DigitFlag = 32 << iota
	G    DigitFlag = 64 << iota
)

type display struct {
	digitPatterns []string
	outputDigits  []string
}

func (this DigitFlag) hasFlag(flag DigitFlag) bool {
	return this|flag == this
}

func wiresToDigitFlags(wires string) DigitFlag {
	flag := None

	for i := range wires {
		switch wires[i] {
		case 'a':
			flag |= A
		case 'b':
			flag |= B
		case 'c':
			flag |= C
		case 'd':
			flag |= D
		case 'e':
			flag |= E
		case 'f':
			flag |= F
		case 'g':
			flag |= G
		}
	}
	return flag
}

func sumNumbers(displayNumbers [][]int) int {
	sum := 0

	digitPosition := 3
	for _, display := range displayNumbers {
		for i, number := range display {
			sum += number * int(math.Pow10(digitPosition-i))
		}
	}

	return sum
}

func countNumbers(displays [][]int, valuesToCount ...int) int {
	count := 0

	for _, display := range displays {
		for _, digit := range display {
			for _, v := range valuesToCount {
				if digit == v {
					count++
				}
			}
		}
	}

	return count
}

func decodeOutputDigit(digitPatterns []DigitFlag, digitWires string) int {
	wireFlags := wiresToDigitFlags(digitWires)

	for i, p := range digitPatterns {
		if p == wireFlags {
			return i
		}
	}

	return -1
}

func decodePattern(patterns []string) []DigitFlag {
	flags := make([]DigitFlag, 10)
	indexToSolve := []int{}

	for i, p := range patterns {
		if len := len(p); (len >= 2 && len <= 4) || len == 7 {
			switch len {
			case 2: // number 1
				flags[1] = wiresToDigitFlags(p)
			case 4: // number 4
				flags[4] = wiresToDigitFlags(p)
			case 3: // number 7
				flags[7] = wiresToDigitFlags(p)
			case 7: // number 8
				flags[8] = wiresToDigitFlags(p)
			}
		} else {
			indexToSolve = append(indexToSolve, i)
		}
	}

	for _, indexValue := range indexToSolve {
		pattern := patterns[indexValue]
		unknownDigitFlags := wiresToDigitFlags(pattern)

		if len := len(pattern); len == 5 { // len 5 possible numbers: 2, 3, 5
			if unknownDigitFlags.hasFlag(flags[7]) {
				flags[3] = unknownDigitFlags // number 3
			} else if unknownDigitFlags.hasFlag(flags[7] ^ flags[4]) {
				flags[5] = unknownDigitFlags // number 5
			} else {
				flags[2] = unknownDigitFlags // number 2
			}
		} else { // len 6 possible numbers: 0, 6 9
			if unknownDigitFlags.hasFlag(flags[4]) && unknownDigitFlags.hasFlag(flags[7]) {
				flags[9] = unknownDigitFlags // number 9
			} else if unknownDigitFlags.hasFlag(flags[1]) {
				flags[0] = unknownDigitFlags // number 0
			} else {
				flags[6] = unknownDigitFlags // number 6
			}
		}
	}

	return flags
}

func fixDisplay(d display) []int {

	digitsPatterns := decodePattern(d.digitPatterns)

	numbers := []int{}
	for _, digitWires := range d.outputDigits {
		digit := decodeOutputDigit(digitsPatterns, digitWires)

		numbers = append(numbers, digit)
	}

	return numbers
}

func part2(displays []display) int {
	fixedDisplays := [][]int{}

	for _, display := range displays {
		fixedDisplays = append(fixedDisplays, fixDisplay(display))
	}

	return sumNumbers(fixedDisplays)
}

func part1(displays []display) int {

	fixedDisplays := [][]int{}

	for _, display := range displays {
		fixedDisplays = append(fixedDisplays, fixDisplay(display))
	}

	return countNumbers(fixedDisplays, 1, 4, 7, 8)
}

func getInput(filePath string) ([]display, error) {
	displays := []display{}

	file, err := os.Open(filePath)
	if err != nil {
		return displays, err
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)

	for scanner.Scan() {
		patternsAndDigits := strings.Split(scanner.Text(), " | ")

		displays = append(displays, display{
			digitPatterns: strings.Fields(patternsAndDigits[0]),
			outputDigits:  strings.Fields(patternsAndDigits[1]),
		})
	}

	return displays, nil
}

func main() {
	if len(os.Args) != 2 {
		fmt.Println("Please, add input file path as parameter")
		os.Exit(1)
	}

	displays, err := getInput(os.Args[1])

	if err != nil {
		fmt.Println("Error:", err)
	}

	fmt.Println("P1: ", part1(displays))
	fmt.Println("P2: ", part2(displays))
}
