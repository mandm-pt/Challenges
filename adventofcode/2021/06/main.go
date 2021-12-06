package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
)

func part2(initialState []int) int {
	return 2
}

func processDay(initialState *[]int) {
	toAdd := 0

	nItems := len(*initialState)

	for i := 0; i < nItems; i++ {
		if (*initialState)[i] == 0 {
			toAdd++
			(*initialState)[i] = 6
		} else {
			(*initialState)[i]--
		}
	}

	for i := toAdd; i > 0; i-- {
		*initialState = append(*initialState, 8)
	}
}

func part1(initialState []int) int {

	//fmt.Printf("Initial state: %v \n", initialState)

	for i := 1; i <= 80; i++ {
		processDay(&initialState)

		//fmt.Printf("After  %d days: %v \n", i, initialState)
	}

	return len(initialState)
}

func getInput(filePath string) ([]int, error) {
	initialState := []int{}

	file, err := os.Open(filePath)
	if err != nil {
		return initialState, err
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)

	scanner.Scan()
	numbers := strings.Split(scanner.Text(), ",")

	for _, number := range numbers {
		n, _ := strconv.Atoi(number)
		initialState = append(initialState, n)
	}

	return initialState, nil
}

func main() {
	if len(os.Args) != 2 {
		fmt.Println("Please, add input file path as parameter")
		os.Exit(1)
	}

	initialState, err := getInput(os.Args[1])

	if err != nil {
		fmt.Println("Error:", err)
	}

	fmt.Println("P1: ", part1(initialState))
	fmt.Println("P2: ", part2(initialState))
}
