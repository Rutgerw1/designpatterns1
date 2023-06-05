using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.Game
{
	class Composite : ISudokuComponent
	{
		public List<ISudokuComponent> Components { get; set; }

		public Composite()
		{
			Components = new List<ISudokuComponent>();
		}

		public void AddComponent(ISudokuComponent component)
		{
			Components.Add(component);
		}

		public bool IsValid(int maxNumber)
		{
			List<Cell> cells = Components.OfType<Cell>().ToList();

			if (!cells.Any())
			{ // this is a higher level composite, delegate to children
				if (!Components.All(component => component.IsValid(maxNumber))) return false;
			}

			List<List<Cell>> cellsWithValue = new List<List<Cell>>(maxNumber);
			bool conflicts = false;

			foreach (Cell cell in cells)
			{
				if (cell.Value == 0) continue;

				if (cellsWithValue[cell.Value] == null) cellsWithValue[cell.Value] = new List<Cell>();

				cellsWithValue[cell.Value].Add(cell);
			}

			cellsWithValue.ForEach(valueCells =>
			{
				if (valueCells.Count > 1)
				{ // composite contains multiple cells with the same value
					valueCells.ForEach(cell => cell.Conflicts.AddRange(valueCells));
					conflicts = true;
				}
			});

			return !conflicts;
		}

		public void ChangeValueAtPosition(int value, Point position)
		{
			foreach (ISudokuComponent component in Components)
			{
				component.ChangeValueAtPosition(value, position);
			}
		}

		public void ToggleNoteAtPosition(int value, Point position)
		{
			foreach (ISudokuComponent component in Components)
			{
				component.ToggleNoteAtPosition(value, position);
			}
		}

		public Cell CellAtPosition(Point position)
		{
			if (!Contains(position)) return null;

			foreach (ISudokuComponent component in Components)
			{
				if (component.Contains(position)) return component.CellAtPosition(position);
			}

			return null;
		}

		public bool Contains(Point point)
		{
			return Components.Any(component => component.Contains(point));
		}
	}
}
