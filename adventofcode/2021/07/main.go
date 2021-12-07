package main

import (
	"bufio"
	"fmt"
	"math"
	"os"
	"sort"
	"strconv"
	"strings"
)

func part2(hPositions []int) int {
	return 2
}

func getMedian(hPositions []int) int {
	sort.Ints(hPositions) // sort the numbers

	mNumber := len(hPositions) / 2

	if len(hPositions)%2 != 0 {
		return hPositions[mNumber]
	}

	return (hPositions[mNumber-1] + hPositions[mNumber]) / 2
}

func part1(hPositions []int) int {
	median := getMedian(hPositions)

	fuel := 0
	for _, n := range hPositions {
		fuel += int(math.Abs(float64(n - median)))
	}

	return fuel
}

func getInput(filePath string) ([]int, error) {
	hPositions := []int{}

	file, err := os.Open(filePath)
	if err != nil {
		return hPositions, err
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)

	scanner.Scan()
	numbers := strings.Split(scanner.Text(), ",")

	for _, number := range numbers {
		n, _ := strconv.ParseInt(number, 10, 32)
		hPositions = append(hPositions, int(n))
	}

	return hPositions, nil
}

func main() {
	if len(os.Args) != 2 {
		fmt.Println("Please, add input file path as parameter")
		os.Exit(1)
	}

	hPositions, err := getInput(os.Args[1])

	if err != nil {
		fmt.Println("Error:", err)
	}

	fmt.Println("P1: ", part1(hPositions))
	fmt.Println("P2: ", part2(hPositions))
}
