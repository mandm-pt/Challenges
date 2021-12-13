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

	result := []string{}
	startVertex := graph["start"]

	path := "start"
	for _, connection := range startVertex {
		result = append(result, traverse(connection, graph, path+","+connection, false)...)
	}

	return len(result)
}

func traverse(entryVertex string, graph map[string][]string, path string, visitedSmall bool) []string {

	result := []string{}
	vertex := graph[entryVertex]

	for _, connection := range vertex {
		if connection == "start" {
			continue
		}
		if connection == "end" {
			result = append(result, path+","+connection)
			continue
		}
		if !isBigCave(connection) {
			if !strings.Contains(path, connection) {
				result = append(result, traverse(connection, graph, path+","+connection, visitedSmall)...)
			} else if strings.Contains(path, connection) && !visitedSmall {
				result = append(result, traverse(connection, graph, path+","+connection, true)...)
			}
			continue
		}

		result = append(result, traverse(connection, graph, path+","+connection, visitedSmall)...)
	}

	return result
}

func part1(graph map[string][]string) int {

	result := []string{}
	startVertex := graph["start"]

	path := "start"
	for _, connection := range startVertex {

		result = append(result, traverse(connection, graph, path+","+connection, true)...)
	}

	return len(result)
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
		graph[nodes[1]] = append(graph[nodes[1]], nodes[0])
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
