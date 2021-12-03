package main

import (
	"bufio"
	"fmt"
	"math"
	"os"
	"strconv"
)

type report struct {
	bitLen  int
	numbers []int64
}

func part2(r *report) int64 {

	return 2
}

func part1(r *report) int64 {
	startingMask := int64(math.Pow(2, float64(r.bitLen-1)))

	gammaRateBinary := ""

	for i := startingMask; i > 0; i = i / 2 {
		mostCommon0 := 0

		for _, n := range r.numbers {
			if n&i == 0 {
				mostCommon0++
			}
		}

		if mostCommon0 > len(r.numbers)/2 {
			gammaRateBinary += "0"
		} else {
			gammaRateBinary += "1"
		}
	}

	gamaRate, _ := strconv.ParseInt(gammaRateBinary, 2, 64)

	mask := int64(math.Pow(2, float64(r.bitLen))) - 1
	epsilonRate := gamaRate ^ mask

	return gamaRate * epsilonRate
}

func getInput(filePath string) (report, error) {
	r := report{}

	file, err := os.Open(filePath)
	if err != nil {
		return r, err
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)
	scanner.Split(bufio.ScanLines)

	for scanner.Scan() {
		value, _ := strconv.ParseInt(scanner.Text(), 2, 64)
		r.bitLen = len(scanner.Text())
		r.numbers = append(r.numbers, value)
	}

	return r, nil
}

func main() {
	if len(os.Args) != 2 {
		fmt.Println("Please, add input file path as parameter")
		os.Exit(1)
	}

	report, err := getInput(os.Args[1])

	if err != nil {
		fmt.Println("Error:", err)
	}

	fmt.Println("P1: ", part1(&report))
	fmt.Println("P2: ", part2(&report))
}
