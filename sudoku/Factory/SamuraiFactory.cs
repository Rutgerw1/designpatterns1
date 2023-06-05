using sudoku.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.Factory
{
	class SamuraiFactory : ISudokuFactory
	{
		public Puzzle CreatePuzzle(string file, Point offset = default)
		{
			string[] subStrings = file.Split(new string[] { "\r\n" }, StringSplitOptions.None);

			Puzzle puzzle = new Puzzle(21);

			ClassicFactory subFactory = new ClassicFactory();

			puzzle.AddComponent(subFactory.CreatePuzzle(subStrings[0], new Point(0, 0))); // top left
			puzzle.AddComponent(subFactory.CreatePuzzle(subStrings[1], new Point(12, 0))); // top right
			puzzle.AddComponent(subFactory.CreatePuzzle(subStrings[2], new Point(6, 6))); // middle
			puzzle.AddComponent(subFactory.CreatePuzzle(subStrings[3], new Point(0, 12))); // bottom left
			puzzle.AddComponent(subFactory.CreatePuzzle(subStrings[4], new Point(12, 12))); // bottom right

			return puzzle;
		}
	}
}
