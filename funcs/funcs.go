package funcs

import (
	"bufio"
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"strings"
)

// Cleargog is stuff for what i want it to be.
type Cleargog struct {
	smPath string
	reader *bufio.Reader
}

// New creates a new Cleargog instance.
func New() Cleargog {
	reader := bufio.NewReader(os.Stdin)
	return Cleargog{
		smPath: "C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs",
		reader: reader,
	}
}

// Readln reads user input on current line.
func (c Cleargog) Readln() (string, error) {
	return c.reader.ReadString('\n')
}

// IsAccepted checks if user input was accepted with a "Y".
func (c Cleargog) IsAccepted(answer string) bool {
	if strings.ToLower(strings.TrimSpace(answer)) == "y" {
		return true
	}

	return false
}

// GatherFilesToRemove asks the user what files they want to delete.
// function will return a -1 for x if the user decides not to select anything.
func (c Cleargog) GatherFilesToRemove() (deletionList []string, x int) {
	x = 0
	files, err := ioutil.ReadDir(c.smPath)

	if err != nil {
		log.Fatal(err)
	}

	for _, file := range files {
		if file.IsDir() && strings.Contains(file.Name(), "[GOG.com]") {
			fmt.Printf("Remove %s? [Y/n] ", file.Name())
			ans, _ := c.Readln()

			if c.IsAccepted(ans) {
				deletionList = append(deletionList, file.Name())
				x++
			}

			if x == 0 {
				x = -1 // -1 tells the app that the user decided not to select anything
			}
		}
	}

	return deletionList, x
}

// DisplayFilesToBeDeleted displays a list of files the user has chosen to delete.
func (c Cleargog) DisplayFilesToBeDeleted(deletionList []string) {
	for _, f := range deletionList {
		fmt.Println(f)
	}
}

// DeleteFiles removes all files the user has chosen to delete.
func (c Cleargog) DeleteFiles(deletionList []string) {
	for _, f := range deletionList {
		err := os.RemoveAll(fmt.Sprintf("%s\\%s", c.smPath, f))

		if err != nil {
			log.Fatal(err)
		}
	}
}

// TermLine adds a "pause" feature to the terminal and waits for user intpu.
func (c Cleargog) TermLine() {
	fmt.Println("Press [ENTER] to continue")
	fmt.Scanln()
}
