package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
)

type instruction struct {
	points map[coordinate]struct{}
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

func startFolding(instructions instruction, foldCount int) map[coordinate]struct{} {
	for i, f := range instructions.fold {
		if i == foldCount {
			break
		}

		newPointsMap := make(map[coordinate]struct{})

		for point := range instructions.points {
			newCoordinate := coordinate{x: -1, y: -1}

			if f.isX && point.x >= f.value {
				diffX := f.value*2 - point.x

				newCoordinate = coordinate{
					x: diffX,
					y: point.y,
				}
			} else if !f.isX && point.y >= f.value {
				diffY := f.value*2 - point.y

				newCoordinate = coordinate{
					x: point.x,
					y: diffY,
				}
			} else {
				newPointsMap[point] = struct{}{}
			}

			if !newCoordinate.isEmpty() {
				newPointsMap[newCoordinate] = struct{}{}
			}
		}

		instructions.points = newPointsMap
	}

	return instructions.points
}

func print(points map[coordinate]struct{}) {
	maxX := 0
	maxY := 0
	for k := range points {
		if maxX < k.x {
			maxX = k.x
		}
		if maxY < k.y {
			maxY = k.y
		}
	}

	for y := 0; y <= maxY; y++ {
		for x := 0; x <= maxX; x++ {
			_, contains := points[coordinate{x: x, y: y}]

			if contains {
				fmt.Print("#")
			} else {
				fmt.Print(".")
			}
		}

		fmt.Println()
	}
}

func part2(instructions instruction) int {
	points := startFolding(instructions, len(instructions.fold))

	print(points)

	return len(points)
}

func part1(instructions instruction) int {
	points := startFolding(instructions, 1)

	return len(points)
}

func getInput(filePath string) (instruction, error) {
	instructions := instruction{}
	instructions.points = map[coordinate]struct{}{}

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
			}] = struct{}{}

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
