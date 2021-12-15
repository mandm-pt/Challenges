package main

import (
	"bufio"
	"fmt"
	"math"
	"os"
	"strings"
)

func getNextPair(pairMaps *map[string]string, key string) []string {

	nextChar := (*pairMaps)[key]

	first := key[:1] + nextChar
	second := nextChar + key[1:2]

	return []string{first, second}
}

func getPairCount(input string) map[string]int {
	pairCount := map[string]int{}

	len := len(input) - 1
	for i := 0; i < len; i++ {
		pairCount[input[i:i+2]] += 1
	}

	return pairCount
}

func runSteps(template string, maps map[string]string, nSteps int) int {

	pairCount := getPairCount(template)

	for step := 0; step < nSteps; step++ {

		newPairCount := map[string]int{}
		for k, v := range pairCount {
			nextPair := getNextPair(&maps, k)

			newPairCount[nextPair[0]] += v
			newPairCount[nextPair[1]] += v
		}
		pairCount = newPairCount
	}

	// Template:     NNCB -                                        NN           NC              CB
	// After step 1: NCNBCHB -                                 NC      CN     NB    BC         CH  HB
	// After step 2: NBCCNBBBCBHCB                            NB BC CC CN    NB BB BB BC     CB BH HC CB
	// After step 3: NBBBCNCCNBBNBNBBCHBHHBCHB
	// After step 4: NBBNBNBBCCNBCNCCNBBNBBNBBBNBBNBBCBHCBHHNHCBBCBHCB

	count := map[string]int{
		"B": 0,
		"C": 0,
		"F": 0,
		"H": 0,
		"K": 0,
		"N": 0,
		"O": 0,
		"P": 0,
		"S": 0,
		"V": 0,
	}

	for k, v := range pairCount {
		count[k[:1]] += v
	}

	min := math.MaxInt
	max := 0
	for _, c := range count {

		if c < min && c > 0 {
			min = c
		}
		if c > max {
			max = c
		}
	}

	return max - min
}

func part2(template string, maps map[string]string) int {
	return runSteps(template, maps, 40) + 1
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
