package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
)

func part2(report *[]int, windowSize int) int {
	var countInc int

	var previousWindowSum int
	for i := range *report {
		windowSum := 0

		if i+windowSize > len((*report)) {
			break
		}

		for j := i; j < i+windowSize; j++ {
			windowSum += (*report)[j]
		}

		if i == 0 {
			continue
		}

		if previousWindowSum < windowSum {
			countInc++
		}

		previousWindowSum = windowSum
	}

	return countInc
}

func part1(report *[]int) int {
	var countInc int
	for i := range *report {
		if i == 0 {
			continue
		}
		if (*report)[i-1] < (*report)[i] {
			countInc++
		}
	}

	return countInc
}

func getInput(filePath string) ([]int, error) {
	file, err := os.Open(filePath)
	if err != nil {
		return nil, err
	}

	measurements := []int{}

	scanner := bufio.NewScanner(file)
	scanner.Split(bufio.ScanLines)

	for scanner.Scan() {
		v, _ := strconv.Atoi(scanner.Text())
		measurements = append(measurements, v)
	}

	return measurements, nil
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
	fmt.Println("P2: ", part2(&report, 3))
}
