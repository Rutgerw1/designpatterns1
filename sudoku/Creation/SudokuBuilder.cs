using sudoku.Game;

namespace sudoku.Creation
{
	class SudokuBuilder
	{
		private Puzzle Puzzle { get; }

		public SudokuBuilder()
		{
			Puzzle = new Puzzle();
		}

		public SudokuBuilder SetSize(int size)
		{
			Puzzle.Size = size;
			return this;
		}

		public SudokuBuilder AddComponent(ISudokuComponent component)
		{
			Puzzle.AddComponent(component);
			return this;
		}

		public Puzzle Build()
		{
			return Puzzle;
		}
	}
}
