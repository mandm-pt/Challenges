package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
)

func runSimulation(initialState []int64, nDays int) int64 {
	currentState := processStates(&initialState)

	for i := 1; i <= nDays; i++ {
		currentState = processDay(currentState)
	}

	return sumStates(currentState)
}

func processDay(currentState [9]int64) [9]int64 {
	nextState := [9]int64{0, 0, 0, 0, 0, 0, 0, 0, 0}

	for i := range currentState {

		if i > 0 {
			nextState[i-1] += currentState[i]
		} else {
			nextState[8] += currentState[i]
			nextState[6] += currentState[i]
		}
	}

	return nextState
}

func sumStates(countPerStates [9]int64) int64 {
	var sum int64 = 0

	for _, n := range countPerStates {
		sum += n
	}

	return sum
}

func processStates(initialState *[]int64) [9]int64 {
	countPerStates := [9]int64{0, 0, 0, 0, 0, 0, 0, 0, 0}

	for _, n := range *initialState {
		countPerStates[n]++
	}

	return countPerStates
}

func part2(initialState []int64) int64 {
	return runSimulation(initialState, 256)
}

func part1(initialState []int64) int64 {
	return runSimulation(initialState, 80)
}

func getInput(filePath string) ([]int64, error) {
	initialState := []int64{}

	file, err := os.Open(filePath)
	if err != nil {
		return initialState, err
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)

	scanner.Scan()
	numbers := strings.Split(scanner.Text(), ",")

	for _, number := range numbers {
		n, _ := strconv.ParseInt(number, 10, 64)
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
