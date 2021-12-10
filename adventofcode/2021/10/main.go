package main

import (
	"bufio"
	"fmt"
	"os"
)

type Stack []string

// IsEmpty: check if stack is empty
func (s *Stack) IsEmpty() bool {
	return len(*s) == 0
}

// Push a new value onto the stack
func (s *Stack) Push(str string) {
	*s = append(*s, str) // Simply append the new value to the end of the stack
}

// Remove and return top element of stack. Return false if stack is empty.
func (s *Stack) Pop() (string, bool) {
	if s.IsEmpty() {
		return "", false
	} else {
		index := len(*s) - 1   // Get the index of the top most element.
		element := (*s)[index] // Index into the slice and obtain the element.
		*s = (*s)[:index]      // Remove it from the stack by slicing it off.
		return element, true
	}
}

func sumArray(array []int) int {
	sum := 0

	for _, n := range array {
		sum += n
	}

	return sum
}

func isExpected(openChar string, closingChar string) bool {
	switch openChar {
	case "(":
		return closingChar == ")"
	case "[":
		return closingChar == "]"
	case "{":
		return closingChar == "}"
	case "<":
		return closingChar == ">"
	}
	return false
}

func getPointsForIllegalChar(char string) int {
	return map[string]int{
		")": 3,
		"]": 57,
		"}": 1197,
		">": 25137,
	}[char]
}

func part2(navSubSystem [][]string) int {
	return 2
}

func part1(navSubSystem [][]string) int {

	errors := []int{}
	for _, line := range navSubSystem {
		stack := Stack{}
		for _, char := range line {
			if char == "(" || char == "[" || char == "{" || char == "<" {
				stack.Push(char)
			} else {
				pop, _ := stack.Pop()

				if !isExpected(pop, char) {
					errors = append(errors, getPointsForIllegalChar(char))
				}
			}

		}
	}
	return sumArray(errors)
}

func getInput(filePath string) ([][]string, error) {
	navSubSystem := [][]string{}

	file, err := os.Open(filePath)
	if err != nil {
		return navSubSystem, err
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)

	for scanner.Scan() {
		line := []string{}
		scannedLine := scanner.Text()

		for _, rune := range scannedLine {
			line = append(line, string(rune))
		}
		navSubSystem = append(navSubSystem, line)
	}

	return navSubSystem, nil
}

func main() {
	if len(os.Args) != 2 {
		fmt.Println("Please, add input file path as parameter")
		os.Exit(1)
	}

	navSubSystem, err := getInput(os.Args[1])

	if err != nil {
		fmt.Println("Error:", err)
	}

	fmt.Println("P1: ", part1(navSubSystem))
	fmt.Println("P2: ", part2(navSubSystem))
}
