using sudoku.Game;
using System.Collections.Generic;

namespace sudoku.Creation.Builders
{
	class SudokuBuilder
	{
		private List<ISudokuComponent> _components { get; }
		protected int _size { get; set; }
		protected int _maxNumber { get; set; }

		public SudokuBuilder()
		{
			_components = new List<ISudokuComponent>();
		}

		public SudokuBuilder SetSize(int size)
		{
			_size = size;
			return this;
		}

		public SudokuBuilder SetMaxNumber(int maxNumber)
		{
			_maxNumber = maxNumber;
			return this;
		}

		public SudokuBuilder AddComponent(ISudokuComponent component)
		{
			_components.Add(component);
			return this;
		}

		public virtual Puzzle Build()
		{
			return new Puzzle(_size, _maxNumber)
			{
				Components = _components
			};
		}
	}
}
