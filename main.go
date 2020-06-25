package main

import (
	"fmt"
	"pazuzu156/cleargog/funcs"
)

func main() {
	cg := funcs.New()
	list, x := cg.GatherFilesToRemove()

	if x == 0 {
		fmt.Println("There's nothing to remove from your Start Menu")
	} else {
		if x == -1 {
			fmt.Println("No folders were selected to delete")
		} else {
			fmt.Println("GOG Folders to remove:")
			cg.DisplayFilesToBeDeleted(list)
			fmt.Printf("Continue? [Y/n] ")
			ans, _ := cg.Readln()

			if cg.IsAccepted(ans) {
				cg.DeleteFiles(list)
				fmt.Println("Task completed")
			} else {
				fmt.Println("No changes were made")
			}
		}
	}

	cg.TermLine()
}
