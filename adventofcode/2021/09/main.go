package main

import (
	"bufio"
	"fmt"
	"os"
)

func getRiskLevel(values []int) int {
	sum := 0
	for i := range values {
		sum += values[i] + 1
	}

	return sum
}

func isLowPoint(point int, neighbours []int) bool {
	for _, v := range neighbours {
		if point >= v {
			return false
		}
	}

	return true
}

func getNeighbours(heightmap [][]int, y, x int) []int {
	neighbours := []int{}
	height := len(heightmap)
	width := len(heightmap[0])

	if y == 0 {
		neighbours = append(neighbours, heightmap[y+1][x])
	} else if y == height-1 {
		neighbours = append(neighbours, heightmap[y-1][x])
	} else {
		neighbours = append(neighbours, heightmap[y+1][x])
		neighbours = append(neighbours, heightmap[y-1][x])
	}

	if x == 0 {
		neighbours = append(neighbours, heightmap[y][x+1])
	} else if x == width-1 {
		neighbours = append(neighbours, heightmap[y][x-1])
	} else {
		neighbours = append(neighbours, heightmap[y][x+1])
		neighbours = append(neighbours, heightmap[y][x-1])
	}

	return neighbours
}

func part2(heightmap [][]int) int {
	return 2
}

func part1(heightmap [][]int) int {
	height := len(heightmap)
	width := len(heightmap[0])

	lowPoints := []int{}

	for y := 0; y < height; y++ {
		for x := 0; x < width; x++ {
			pointHeight := heightmap[y][x]
			neighbours := getNeighbours(heightmap, y, x)

			if isLowPoint(pointHeight, neighbours) {
				lowPoints = append(lowPoints, pointHeight)
			}
		}
	}

	return getRiskLevel(lowPoints)
}

func getInput(filePath string) ([][]int, error) {
	heightmap := [][]int{}

	file, err := os.Open(filePath)
	if err != nil {
		return heightmap, err
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)

	for scanner.Scan() {
		y := []int{}
		line := scanner.Text()
		for _, column := range line {
			y = append(y, int(column-'0'))
		}
		heightmap = append(heightmap, y)
	}

	return heightmap, nil
}

func main() {
	if len(os.Args) != 2 {
		fmt.Println("Please, add input file path as parameter")
		os.Exit(1)
	}

	heightmap, err := getInput(os.Args[1])

	if err != nil {
		fmt.Println("Error:", err)
	}

	fmt.Println("P1: ", part1(heightmap))
	fmt.Println("P2: ", part2(heightmap))
}
