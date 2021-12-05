package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
)

type cardNumber struct {
	number int
	marked bool
}

type card struct {
	won     bool
	numbers [][]cardNumber
}

type game struct {
	cards             []card
	drawNumbers       []int
	winningCardsCount int
}

func part2(game game) int {
	cardsPlaying := len(game.cards)
	for {
		drewNumber := game.drawNumber()

		for i := 0; i < cardsPlaying; i++ {
			if game.cards[i].won {
				continue
			}

			game.cards[i].markNumberIfExists(drewNumber)

			if game.cards[i].hasFullLineOrCollumn() {
				game.winningCardsCount++
				game.cards[i].won = true

				if game.winningCardsCount == cardsPlaying {
					sum := game.cards[i].sumUnMarked()
					return sum * drewNumber
				}
			}
		}
	}
}

func part1(game game) int {
	for {
		drewNumber := game.drawNumber()

		for _, playerCard := range game.cards {
			playerCard.markNumberIfExists(drewNumber)

			if playerCard.hasFullLineOrCollumn() {
				sum := playerCard.sumUnMarked()
				return sum * drewNumber
			}
		}
	}
}

func (game *game) drawNumber() int {
	number := game.drawNumbers[0]
	game.drawNumbers = game.drawNumbers[1:]

	return number
}

func (card *card) hasFullLineOrCollumn() bool {
	h := len(card.numbers)
	columnCounts := make([]int, h)

	for l := 0; l < h; l++ {
		w := len(card.numbers)
		lineCount := 0
		for c := 0; c < w; c++ {
			if card.numbers[l][c].marked {
				columnCounts[c]++
				lineCount++
			}
		}

		if lineCount == w {
			return true
		}
	}

	for _, c := range columnCounts {
		if c == h {
			return true
		}
	}

	return false
}

func (card *card) markNumberIfExists(number int) {
	h := len(card.numbers)

	for l := 0; l < h; l++ {
		w := len(card.numbers)

		for c := 0; c < w; c++ {
			if card.numbers[l][c].number == number {
				card.numbers[l][c].marked = true
			}
		}
	}
}

func (card *card) sumUnMarked() int {
	sum := 0
	for _, line := range card.numbers {
		for _, cardNumber := range line {
			if !cardNumber.marked {
				sum += cardNumber.number
			}
		}
	}

	return sum
}

func getInput(filePath string) (game, error) {
	g := game{}

	file, err := os.Open(filePath)
	if err != nil {
		return g, err
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)
	scanner.Split(bufio.ScanLines)

	scanner.Scan()
	drawNumbersString := strings.Split(scanner.Text(), ",")
	for _, n := range drawNumbersString {
		number, _ := strconv.Atoi(n)
		g.drawNumbers = append(g.drawNumbers, number)
	}

	// skip blank line
	scanner.Scan()

	cardLineIndex := 0
	newCard := card{}
	for scanner.Scan() {
		line := scanner.Text()

		if line == "" {
			g.cards = append(g.cards, newCard)
			newCard = card{}
			cardLineIndex = 0
			continue
		}

		cardNumbers := strings.Split(line, " ")
		for i, n := range cardNumbers {
			if n == "" {
				continue
			}

			number, _ := strconv.Atoi(n)
			if len(newCard.numbers) <= cardLineIndex {
				newCard.numbers = append(newCard.numbers, []cardNumber{})
			}

			if len(newCard.numbers[cardLineIndex]) <= i {
				newCard.numbers[cardLineIndex] = append(newCard.numbers[cardLineIndex], cardNumber{number: number, marked: false})
			}
		}
		cardLineIndex++
	}

	g.cards = append(g.cards, newCard)

	return g, nil
}

func main() {
	if len(os.Args) != 2 {
		fmt.Println("Please, add input file path as parameter")
		os.Exit(1)
	}

	game, err := getInput(os.Args[1])

	if err != nil {
		fmt.Println("Error:", err)
	}

	fmt.Println("P1: ", part1(game))
	fmt.Println("P2: ", part2(game))
}
