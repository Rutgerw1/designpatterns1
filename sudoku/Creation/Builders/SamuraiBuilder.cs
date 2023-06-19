using sudoku.Game;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace sudoku.Creation.Builders
{
	class SamuraiBuilder : SudokuBuilder
	{
		private readonly List<Puzzle> _subPuzzles;
		private readonly Dictionary<Point, Cell> _keptCells;

		public SamuraiBuilder()
		{
			_subPuzzles = new List<Puzzle>();
			_keptCells = new Dictionary<Point, Cell>();
		}

		public SamuraiBuilder AddSubPuzzle(Puzzle subPuzzle, Point offset)
		{
			List<Cell> subPuzzleCells = new List<Cell>();
			List<(Cell, Cell)> toReplace = new List<(Cell, Cell)>();

			foreach (ISudokuComponent component in subPuzzle.Components)
			{
				if (component is Composite composite)
				{
					foreach (ISudokuComponent cellComponent in composite.Components)
					{
						if (cellComponent is Cell cell)
						{
							subPuzzleCells.Add(cell);
						}
					}
				}
			}

			subPuzzleCells = subPuzzleCells.Distinct().ToList();

			foreach (Cell cell in subPuzzleCells)
			{
				cell.Offset(offset);

				if (_keptCells.ContainsKey(cell.Position))
				{
					if (_keptCells[cell.Position] != cell)
					{
						toReplace.Add((cell, _keptCells[cell.Position]));
					}
				}
				else
				{
					_keptCells.Add(cell.Position, cell);
				}
			}

			foreach ((Cell old, Cell new_) in toReplace)
			{
				subPuzzle.ReplaceComponent(old, new_);
			}

			_subPuzzles.Add(subPuzzle);

			return this;
		}

		public override Puzzle Build()
		{
			var samuraiPuzzle = new Puzzle(_size, _maxNumber);

			foreach (var subPuzzle in _subPuzzles)
			{
				samuraiPuzzle.AddComponent(subPuzzle);
			}

			return samuraiPuzzle;
		}
	}
}
