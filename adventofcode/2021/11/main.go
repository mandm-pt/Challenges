package main

import (
	"bufio"
	"fmt"
	"os"
)

type octopus struct {
	x     int
	y     int
	value *int
}

func getNeighbours(energyLevels *[][]int, y, x int) []octopus {
	neighbours := []octopus{}
	height := len(*energyLevels) - 1
	width := len((*energyLevels)[0]) - 1

	if y > 0 {
		neighbours = append(neighbours, octopus{y: y - 1, x: x, value: &(*energyLevels)[y-1][x]})
	}
	if y < height {
		neighbours = append(neighbours, octopus{y: y + 1, x: x, value: &(*energyLevels)[y+1][x]})
	}
	if x > 0 {
		neighbours = append(neighbours, octopus{y: y, x: x - 1, value: &(*energyLevels)[y][x-1]})
		if y > 0 {
			neighbours = append(neighbours, octopus{y: y - 1, x: x - 1, value: &(*energyLevels)[y-1][x-1]})
		}
		if y < height {
			neighbours = append(neighbours, octopus{y: y + 1, x: x - 1, value: &(*energyLevels)[y+1][x-1]})
		}
	}
	if x < width {
		neighbours = append(neighbours, octopus{y: y, x: x + 1, value: &(*energyLevels)[y][x+1]})
		if y > 0 {
			neighbours = append(neighbours, octopus{y: y - 1, x: x + 1, value: &(*energyLevels)[y-1][x+1]})
		}
		if y < height {
			neighbours = append(neighbours, octopus{y: y + 1, x: x + 1, value: &(*energyLevels)[y+1][x+1]})
		}
	}

	return neighbours
}

func contains(visited *map[octopus]struct{}, elementToCheck octopus) bool {
	if visited == nil {
		return false
	}

	_, itExists := (*visited)[elementToCheck]

	return itExists
}

func startFlashing(energyLevels *[][]int, neighbours []octopus, flashed *map[octopus]struct{}) {
	for i := range neighbours {
		oct := octopus{
			y:     neighbours[i].y,
			x:     neighbours[i].x,
			value: neighbours[i].value,
		}
		*oct.value++

		if contains(flashed, oct) {
			continue
		}

		if *neighbours[i].value > 9 {
			(*flashed)[oct] = struct{}{}
			neighbours := getNeighbours(energyLevels, oct.y, oct.x)
			startFlashing(energyLevels, neighbours, flashed)
			*oct.value = 0
		}
	}
}

func startSimulation(energyLevels [][]int, numberOfSteps int, stopWhenAllFlash bool) (flashCount int, firstSimultaneousFlashStep int) {
	// copying 2d slice to not affect 2nd run
	workingEnergyLevels := make([][]int, len(energyLevels))
	for i := range energyLevels {
		workingEnergyLevels[i] = make([]int, len(energyLevels[i]))
		copy(workingEnergyLevels[i], energyLevels[i])
	}

	height := len(workingEnergyLevels)
	width := len(workingEnergyLevels[0])

	if stopWhenAllFlash {
		numberOfSteps = 9999
	}

	for step := 1; step <= numberOfSteps; step++ {
		flashed := &(map[octopus]struct{}{})

		for y := 0; y < height; y++ {
			for x := 0; x < width; x++ {
				oct := octopus{
					y:     y,
					x:     x,
					value: &workingEnergyLevels[y][x],
				}
				*oct.value++

				neighbours := getNeighbours(&workingEnergyLevels, y, x)

				if workingEnergyLevels[y][x] > 9 {
					// flash
					(*flashed)[oct] = struct{}{}
					startFlashing(&workingEnergyLevels, neighbours, flashed)
					*oct.value = 0
				}
			}
		}

		for k := range *flashed {
			*k.value = 0
		}

		flashCount += len(*flashed)

		if len(*flashed) == width*height {
			firstSimultaneousFlashStep = step
			return flashCount, firstSimultaneousFlashStep
		}
	}

	return flashCount, firstSimultaneousFlashStep
}

func part2(energyLevels [][]int) int {
	_, firstSimultaneousFlashStep := startSimulation(energyLevels, -1, true)

	return firstSimultaneousFlashStep
}

func part1(energyLevels [][]int) int {
	flashesCount, _ := startSimulation(energyLevels, 100, false)

	return flashesCount
}

func getInput(filePath string) ([][]int, error) {
	energyLevels := [][]int{}

	file, err := os.Open(filePath)
	if err != nil {
		return energyLevels, err
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)

	for scanner.Scan() {
		y := []int{}
		line := scanner.Text()
		for _, column := range line {
			y = append(y, int(column-'0'))
		}
		energyLevels = append(energyLevels, y)
	}

	return energyLevels, nil
}

func main() {
	if len(os.Args) != 2 {
		fmt.Println("Please, add input file path as parameter")
		os.Exit(1)
	}

	energyLevels, err := getInput(os.Args[1])

	if err != nil {
		fmt.Println("Error:", err)
	}

	fmt.Println("P1: ", part1(energyLevels))
	fmt.Println("P2: ", part2(energyLevels))
}
