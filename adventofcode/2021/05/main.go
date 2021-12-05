package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
)

type point struct {
	x int
	y int
}

type line struct {
	start point
	end   point
}

type grid struct {
	maxX  int
	maxY  int
	lines []line
}

func part2(g grid) int {
	matrix := initMatrix(g)

	fillMatrix(&matrix, g, false)
	//drawMatrix(matrix)
	return countOverlaps(matrix)
}

func countOverlaps(matrix [][]int) int {
	count := 0

	height := len(matrix)
	width := len(matrix[height-1])

	for y := 0; y < height; y++ {
		for x := 0; x < width; x++ {
			if matrix[y][x] >= 2 {
				count++
			}
		}
	}

	return count
}

func drawMatrix(matrix [][]int) {
	height := len(matrix)
	width := len(matrix[height-1])

	for y := 0; y < height; y++ {
		for x := 0; x < width; x++ {
			switch matrix[y][x] {
			case 0:
				fmt.Print(".")
			default:
				fmt.Print(matrix[y][x])
			}
		}
		fmt.Println()
	}
}

func (l *line) isStraight() bool {
	return l.start.x == l.end.x ||
		l.start.y == l.end.y
}

func fillMatrix(matrix *[][]int, g grid, skipDiagonal bool) {
	for _, line := range g.lines {
		if skipDiagonal && !line.isStraight() {
			continue
		}

		x := line.start.x
		y := line.start.y
		for {
			(*matrix)[y][x]++

			if x == line.end.x && y == line.end.y {
				break
			}

			if x < line.end.x {
				x++
			} else if x > line.end.x {
				x--
			}
			if y < line.end.y {
				y++
			} else if y > line.end.y {
				y--
			}
		}
	}
}

func initMatrix(g grid) [][]int {
	matrix := make([][]int, g.maxY+1)
	for i := range matrix {
		matrix[i] = make([]int, g.maxX+1)
	}

	return matrix
}

func part1(g grid) int {
	matrix := initMatrix(g)

	fillMatrix(&matrix, g, true)
	//drawMatrix(matrix)
	return countOverlaps(matrix)
}

func parsePoint(pairString string) point {
	xyPair := strings.Split(pairString, ",")

	x, _ := strconv.Atoi(xyPair[0])
	y, _ := strconv.Atoi(xyPair[1])

	return point{
		x: x,
		y: y,
	}
}

func getInput(filePath string) (grid, error) {
	grid := grid{}

	file, err := os.Open(filePath)
	if err != nil {
		return grid, err
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)

	for scanner.Scan() {
		startEndPairs := strings.Split(scanner.Text(), " -> ")

		startPair := startEndPairs[0]
		endPair := startEndPairs[1]

		line := line{
			start: parsePoint(startPair),
			end:   parsePoint(endPair),
		}

		if line.start.x > grid.maxX {
			grid.maxX = line.start.x
		}
		if line.end.x > grid.maxX {
			grid.maxX = line.end.x
		}
		if line.start.y > grid.maxY {
			grid.maxY = line.start.y
		}
		if line.end.y > grid.maxY {
			grid.maxY = line.end.y
		}

		grid.lines = append(grid.lines, line)
	}

	return grid, nil
}

func main() {
	if len(os.Args) != 2 {
		fmt.Println("Please, add input file path as parameter")
		os.Exit(1)
	}

	grid, err := getInput(os.Args[1])

	if err != nil {
		fmt.Println("Error:", err)
	}

	fmt.Println("P1: ", part1(grid))
	fmt.Println("P2: ", part2(grid))
}
