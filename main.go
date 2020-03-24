package main

import (
	"bufio"
	"fmt"
	"io/ioutil"
	"os"
	"strings"
)

func main() {
	smPath := "C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs"
	reader := bufio.NewReader(os.Stdin)
	files, err := ioutil.ReadDir(smPath)

	if err != nil {
		panic(err)
	}

	for _, file := range files {
		if file.IsDir() && strings.Contains(file.Name(), "[GOG.com]") {
			fmt.Printf("Remove %s? [Y/n] ", file.Name())
			ans, _ := reader.ReadString('\n')

			if strings.ToLower(strings.TrimSpace(ans)) == "y" {
				err := os.RemoveAll(fmt.Sprintf("%s\\%s", smPath, file.Name()))

				if err != nil {
					panic(err)
				}
			}
		}
	}
}
