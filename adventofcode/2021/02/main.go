package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
)

type position struct {
	horizontal int
	depth      int
	aim        int
}

type instruction struct {
	command string
	unit    int
}

func part2(instructions *[]instruction) int {
	position := position{}

	for _, cmd := range *instructions {

		switch cmd.command {
		case "up":
			position.aim -= cmd.unit
		case "down":
			position.aim += cmd.unit
		case "forward":
			position.horizontal += cmd.unit
			position.depth += position.aim * cmd.unit
		}
	}

	return position.depth * position.horizontal
}

func part1(instructions *[]instruction) int {
	position := position{}

	for _, cmd := range *instructions {

		switch cmd.command {
		case "up":
			position.depth -= cmd.unit
		case "down":
			position.depth += cmd.unit
		case "forward":
			position.horizontal += cmd.unit
		}
	}

	return position.depth * position.horizontal
}

func getInput(filePath string) ([]instruction, error) {
	file, err := os.Open(filePath)
	if err != nil {
		return nil, err
	}
	defer file.Close()

	instructions := []instruction{}

	scanner := bufio.NewScanner(file)
	scanner.Split(bufio.ScanLines)

	for scanner.Scan() {
		pairResult := strings.Split(scanner.Text(), " ")

		value, _ := strconv.Atoi(pairResult[1])

		instructions = append(instructions, instruction{
			command: pairResult[0],
			unit:    value,
		})
	}

	return instructions, nil
}

func main() {
	if len(os.Args) != 2 {
		fmt.Println("Please, add input file path as parameter")
		os.Exit(1)
	}

	instructions, err := getInput(os.Args[1])

	if err != nil {
		fmt.Println("Error:", err)
	}

	fmt.Println("P1: ", part1(&instructions))
	fmt.Println("P2: ", part2(&instructions))
}
