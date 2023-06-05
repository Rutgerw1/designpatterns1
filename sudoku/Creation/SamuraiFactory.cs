using sudoku.Game;
using System;
using System.Drawing;

namespace sudoku.Creation
{
	class SamuraiFactory : ISudokuFactory
	{
		public Puzzle CreatePuzzle(string file, Point offset = default)
		{
			string[] subStrings = file.Split(new string[] { "\r\n" }, StringSplitOptions.None);

			SudokuBuilder builder = new SudokuBuilder();

			ClassicFactory subFactory = new ClassicFactory();

			builder
				.SetSize(21)
				.AddComponent(subFactory.CreatePuzzle(subStrings[0], new Point(0, 0))) // top left
				.AddComponent(subFactory.CreatePuzzle(subStrings[1], new Point(12, 0))) // top right
				.AddComponent(subFactory.CreatePuzzle(subStrings[2], new Point(6, 6))) // middle
				.AddComponent(subFactory.CreatePuzzle(subStrings[3], new Point(0, 12))) // bottom left
				.AddComponent(subFactory.CreatePuzzle(subStrings[4], new Point(12, 12))); // bottom right

			return builder.Build();
		}
	}
}
