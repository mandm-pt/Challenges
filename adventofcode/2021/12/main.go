package main

import (
	"bufio"
	"fmt"
	"os"
	"strings"
)

func isBigCave(cave string) bool {
	return strings.ToUpper(cave) == cave
}

func part2(graph map[string][]string) int {

	// TODO
	return 2
}

func part1(graph map[string][]string) int {

	startVertex := graph["start"]

	path := "start"
	for _, connection := range startVertex {

		path = path + "," + connection
		// TODO
	}

	return 1
}

func getInput(filePath string) (map[string][]string, error) {
	graph := map[string][]string{}

	file, err := os.Open(filePath)
	if err != nil {
		return graph, err
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)

	for scanner.Scan() {
		nodes := strings.Split(scanner.Text(), "-")

		graph[nodes[0]] = append(graph[nodes[0]], nodes[1])
	}

	return graph, nil
}

func main() {
	if len(os.Args) != 2 {
		fmt.Println("Please, add input file path as parameter")
		os.Exit(1)
	}

	graph, err := getInput(os.Args[1])

	if err != nil {
		fmt.Println("Error:", err)
	}

	fmt.Println("P1: ", part1(graph))
	fmt.Println("P2: ", part2(graph))
}
