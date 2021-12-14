package main

import (
	"bufio"
	"fmt"
	"math"
	"os"
	"strings"
)

func runSteps(template string, maps map[string]string, nSteps int) int {
	for step := 0; step < nSteps; step++ {
		for i := 0; i < len(template)-1; i += 2 {
			template = template[:i+1] + maps[template[i:i+2]] + template[i+1:]
		}
	}

	count := map[rune]int{}
	for _, r := range template {
		count[r] += 1
	}

	min := math.MaxInt
	max := 0
	for _, c := range count {

		if c < min {
			min = c
		}
		if c > max {
			max = c
		}
	}

	return max - min
}

func part2(template string, maps map[string]string) int {
	return runSteps(template, maps, 40)
}

func part1(template string, maps map[string]string) int {

	return runSteps(template, maps, 10)
}

func getInput(filePath string) (template string, maps map[string]string, err error) {

	file, err := os.Open(filePath)
	if err != nil {
		return "", nil, err
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)

	maps = map[string]string{}
	scanner.Scan()
	template = scanner.Text()
	scanner.Scan()

	for scanner.Scan() {
		line := scanner.Text()

		split := strings.Split(line, " -> ")

		maps[split[0]] = split[1]
	}

	return template, maps, nil
}

func main() {
	if len(os.Args) != 2 {
		fmt.Println("Please, add input file path as parameter")
		os.Exit(1)
	}

	template, maps, err := getInput(os.Args[1])

	if err != nil {
		fmt.Println("Error:", err)
	}

	fmt.Println("P1: ", part1(template, maps))
	fmt.Println("P2: ", part2(template, maps))
}
