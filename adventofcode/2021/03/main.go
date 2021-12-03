package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
)

type report struct {
	bitLen  int
	numbers []int64
}

func process(mask int64, slice []int64, getNextBatch func(start0, start1 []int64) []int64) []int64 {

	for i := mask; i > 0; i = i / 2 {
		common0 := []int64{}
		common1 := []int64{}

		for _, n := range slice {
			if n&i == 0 {
				common0 = append(common0, n)
			} else {
				common1 = append(common1, n)
			}
		}

		slice = getNextBatch(common0, common1)

		if len(slice) == 1 {
			break
		}
	}

	return slice
}

func part2(r *report) int64 {
	mask := int64(1 << (r.bitLen - 1))

	oxygenGeneratorRating := process(mask, r.numbers, func(start0, start1 []int64) []int64 {
		if len(start0) > len(start1) {
			return start0
		}
		return start1
	})
	co2ScrubberRating := process(mask, r.numbers, func(start0, start1 []int64) []int64 {
		if len(start0) <= len(start1) {
			return start0
		}
		return start1
	})

	return oxygenGeneratorRating[0] * co2ScrubberRating[0]
}

func part1(r *report) int {
	mask := int64(1 << (r.bitLen - 1))

	halfBits := len(r.numbers) / 2
	gamaRate, epsilonRate := 0, 0

	for i := mask; i > 0; i = i / 2 {
		mostCommon0 := 0

		for _, n := range r.numbers {
			if n&i == 0 {
				mostCommon0++
			}
		}

		if mostCommon0 > halfBits {
			gamaRate <<= 1
			epsilonRate = (epsilonRate << 1) + 1
		} else {
			epsilonRate <<= 1
			gamaRate = (gamaRate << 1) + 1
		}
	}

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
