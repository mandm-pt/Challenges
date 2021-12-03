package main

import (
	"bufio"
	"fmt"
	"math"
	"os"
	"strconv"
)

type number struct {
	binary  string
	decimal int64
}

func part2(report *[]number) int {
	return 2
}

func part1(report *[]number) int64 {
	bitLen := float64(len((*report)[0].binary))
	startingMask := int64(math.Pow(2, bitLen-1))

	gammaRateBinary := ""

	for i := startingMask; i > 0; i = i / 2 {
		mostCommon0 := 0

		for _, n := range *report {
			if n.decimal&i == 0 {
				mostCommon0++
			}
		}

		if mostCommon0 > len(*report)/2 {
			gammaRateBinary += "0"
		} else {
			gammaRateBinary += "1"
		}
	}

	gamaRate, _ := strconv.ParseInt(gammaRateBinary, 2, 64)

	mask := int64(math.Pow(2, bitLen)) - 1
	epsilonRate := gamaRate ^ mask

	return gamaRate * epsilonRate
}

func getInput(filePath string) ([]number, error) {
	file, err := os.Open(filePath)
	if err != nil {
		return nil, err
	}
	defer file.Close()

	report := []number{}

	scanner := bufio.NewScanner(file)
	scanner.Split(bufio.ScanLines)

	for scanner.Scan() {
		value, _ := strconv.ParseInt(scanner.Text(), 2, 64)

		report = append(report, number{
			binary:  scanner.Text(),
			decimal: value,
		})
	}

	return report, nil
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
