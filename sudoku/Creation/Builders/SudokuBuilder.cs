using sudoku.Game;
using System.Collections.Generic;

namespace sudoku.Creation.Builders
{
	class SudokuBuilder
	{
		private readonly List<ISudokuComponent> _components;
		protected int _size;
		protected int _maxNumber;

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
