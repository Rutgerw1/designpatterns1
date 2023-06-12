using sudoku.Game;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace sudoku.Creation
{
	public class SudokuFactory : ISudokuFactory
	{
		private readonly string _extension;
		public Dictionary<string, Func<ISudokuFactory>> Types { get; }

		public SudokuFactory(string extension)
		{
			_extension = extension;
			Types = new Dictionary<string, Func<ISudokuFactory>>()
			{
				{ "4x4", () => new ClassicFactory() },
				{ "6x6", () => new ClassicFactory() },
				{ "8x8", () => new ClassicFactory() },
				{ "9x9", () => new ClassicFactory() },
				{ "jigsaw", () => new JigsawFactory() },
				{ "samurai", () => new SamuraiFactory() }
			};
		}

		private ISudokuFactory GetFactory(string ext)
		{
			return Types[ext]();
		}

		public Puzzle CreatePuzzle(string file)
		{
			ISudokuFactory concreteFactory = GetFactory(_extension);

			return concreteFactory.CreatePuzzle(file);
		}
	}
}
