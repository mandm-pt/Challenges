package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
)

type instruction struct {
	points map[coordinate]int
	fold   []fold
}

type fold struct {
	isX   bool
	value int
}

type coordinate struct {
	x int
	y int
}

func (c *coordinate) isEmpty() bool {
	return c.x == -1 && c.y == -1
}

func startFolding(instructions instruction, foldCount int) map[coordinate]int {
	for i, f := range instructions.fold {
		if i == foldCount {
			break
		}

		newPointsMap := make(map[coordinate]int)

		for point := range instructions.points {
			newCoordinate := coordinate{x: -1, y: -1}

			if f.isX {
				if point.x >= f.value {
					diffX := f.value*2 - point.x

					newCoordinate = coordinate{
						x: diffX,
						y: point.y,
					}
				} else {
					newPointsMap[point] += 1
					continue
				}
			} else {
				if point.y >= f.value {
					diffY := f.value*2 - point.y

					newCoordinate = coordinate{
						x: point.x,
						y: diffY,
					}
				} else {
					newPointsMap[point] += 1
					continue
				}
			}

			if !newCoordinate.isEmpty() {
				newPointsMap[newCoordinate] += 1
			}
		}

		instructions.points = newPointsMap
	}

	return instructions.points
}

func part2(instructions instruction) int {
	result := startFolding(instructions, len(instructions.fold))

	return len(result)
}

func part1(instructions instruction) int {
	result := startFolding(instructions, 1)

	return len(result)
}

func getInput(filePath string) (instruction, error) {
	instructions := instruction{}
	instructions.points = map[coordinate]int{}

	file, err := os.Open(filePath)
	if err != nil {
		return instructions, err
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)

	for scanner.Scan() {
		line := scanner.Text()

		if !strings.HasPrefix(line, "fold") && line != "" {
			xyStrings := strings.Split(line, ",")

			x, _ := strconv.Atoi(xyStrings[0])
			y, _ := strconv.Atoi(xyStrings[1])

			instructions.points[coordinate{
				x: x,
				y: y,
			}] = 1

			continue
		} else if strings.HasPrefix(line, "fold") {
			line = strings.Replace(line, "fold along ", "", 1)

			coord := strings.Split(line, "=")
			value, _ := strconv.Atoi(coord[1])

			instructions.fold = append(instructions.fold, fold{
				isX:   coord[0] == "x",
				value: value,
			})
		}
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

	fmt.Println("P1: ", part1(instructions))
	fmt.Println("P2: ", part2(instructions))
}
