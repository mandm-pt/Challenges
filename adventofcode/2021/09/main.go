package main

import (
	"bufio"
	"fmt"
	"os"
	"sort"
	"strconv"
)

type point struct {
	x          int
	y          int
	value      int
	neighbours []point
}

func getRiskLevel(values []point) int {
	sum := 0
	for i := range values {
		sum += values[i].value + 1
	}

	return sum
}

func isLowPoint(pointValue int, neighbours []point) bool {
	for _, n := range neighbours {
		if pointValue >= n.value {
			return false
		}
	}

	return true
}

func getNeighbours(heightmap *[][]int, y, x int) []point {
	neighbours := []point{}
	height := len(*heightmap)
	width := len((*heightmap)[0])

	if y == 0 {
		neighbours = append(neighbours, point{y: y + 1, x: x, value: (*heightmap)[y+1][x]})
	} else if y == height-1 {
		neighbours = append(neighbours, point{y: y - 1, x: x, value: (*heightmap)[y-1][x]})
	} else {
		neighbours = append(neighbours, point{y: y + 1, x: x, value: (*heightmap)[y+1][x]})
		neighbours = append(neighbours, point{y: y - 1, x: x, value: (*heightmap)[y-1][x]})
	}

	if x == 0 {
		neighbours = append(neighbours, point{y: y, x: x + 1, value: (*heightmap)[y][x+1]})
	} else if x == width-1 {
		neighbours = append(neighbours, point{y: y, x: x - 1, value: (*heightmap)[y][x-1]})
	} else {
		neighbours = append(neighbours, point{y: y, x: x + 1, value: (*heightmap)[y][x+1]})
		neighbours = append(neighbours, point{y: y, x: x - 1, value: (*heightmap)[y][x-1]})
	}

	return neighbours
}

func contains(elements *[]string, elementToCheck string) bool {
	if elements == nil {
		return false
	}

	for i := range *elements {
		if (*elements)[i] == elementToCheck {
			return true
		}
	}
	return false
}

func markAsVisited(visited *[]string, p point) *[]string {
	pointToMark := p.toString()

	if visited == nil {
		visited = &[]string{pointToMark}
		return visited
	}

	if !contains(visited, pointToMark) {
		*visited = append(*visited, pointToMark)
	}

	return visited
}

func getBasinSize(heightmap *[][]int, startingPoint point, visited *[]string) int {
	size := 0
	for _, p := range startingPoint.neighbours {
		if contains(visited, p.toString()) {
			continue
		}

		if p.value > startingPoint.value && p.value < 9 {
			visited = markAsVisited(visited, p)
			size++

			if p.neighbours == nil {
				p.neighbours = getNeighbours(heightmap, p.y, p.x)
			}

			size += getBasinSize(heightmap, p, visited)
		}
	}

	return size
}

func (p point) toString() string {
	return strconv.Itoa(p.y) + "," + strconv.Itoa(p.x)
}

func getLowPoints(heightmap [][]int) []point {
	height := len(heightmap)
	width := len(heightmap[0])

	lowPoints := []point{}

	for y := 0; y < height; y++ {
		for x := 0; x < width; x++ {
			pointHeight := heightmap[y][x]
			neighbours := getNeighbours(&heightmap, y, x)

			if isLowPoint(pointHeight, neighbours) {
				lowPoints = append(lowPoints, point{y: y,
					x:          x,
					value:      pointHeight,
					neighbours: neighbours,
				})
			}
		}
	}

	return lowPoints
}

func part2(heightmap [][]int) int {

	lowPoints := getLowPoints(heightmap)

	basinSizes := []int{}
	for _, p := range lowPoints {
		basinSizes = append(basinSizes, getBasinSize(&heightmap, p, nil)+1)
	}

	sort.Ints(basinSizes)

	len := len(basinSizes)
	return basinSizes[len-1] * basinSizes[len-2] * basinSizes[len-3]
}

func part1(heightmap [][]int) int {
	lowPoints := getLowPoints(heightmap)

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
