using sudoku.Creation.Builders;
using sudoku.Game;
using System;
using System.Drawing;

namespace sudoku.Creation
{
	public class SamuraiFactory : ISudokuFactory
	{
		public Puzzle CreatePuzzle(string file)
		{
			string[] subStrings = file.Split(new string[] { "\r\n" }, StringSplitOptions.None);

			SamuraiBuilder builder = new SamuraiBuilder();

			ClassicFactory subFactory = new ClassicFactory();

			builder
				.AddSubPuzzle(subFactory.CreatePuzzle(subStrings[0]), new Point(0, 0)) // top left
				.AddSubPuzzle(subFactory.CreatePuzzle(subStrings[1]), new Point(12, 0)) // top right
				.AddSubPuzzle(subFactory.CreatePuzzle(subStrings[2]), new Point(6, 6)) // middle
				.AddSubPuzzle(subFactory.CreatePuzzle(subStrings[3]), new Point(0, 12)) // bottom left
				.AddSubPuzzle(subFactory.CreatePuzzle(subStrings[4]), new Point(12, 12)) // bottom right
				.SetSize(21)
				.SetMaxNumber(9);

			return builder.Build();
		}
	}
}
