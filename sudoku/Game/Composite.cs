﻿using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace sudoku.Game
{
	public class Composite : ISudokuComponent
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

		public bool IsValid(int maxNumber, Point? position = null)
		{
			if (position != null && !Contains(position.Value)) return true;

			bool childrenValid = true;
			// can't use Any or All because we want to check every element, always
			Components.ForEach(component =>
			{
				if (!component.IsValid(maxNumber, position)) childrenValid = false;
			});

			List<Cell> cells = Components.OfType<Cell>().ToList();

			if (!cells.Any())
			{
				return childrenValid;
			}

			List<Cell>[] cellsWithValue = new List<Cell>[maxNumber];
			bool conflicts = false;

			// we need to check the cell values
			foreach (Cell cell in cells)
			{
				if (cell.Value == 0 || cell.Conflicts.Contains(cell)) continue;

				int index = cell.Value - 1;
				if (cellsWithValue[index] == null) cellsWithValue[index] = new List<Cell>();

				cellsWithValue[index].Add(cell);
			}

			foreach (List<Cell> valueCells in cellsWithValue)
			{
				if (valueCells?.Count > 1)
				{ // composite contains multiple cells with the same value
					foreach (Cell cell in valueCells)
					{
						IEnumerable<Cell> others = valueCells.Where(other => other != cell);
						cell.Conflicts.AddRange(others);
					}
					conflicts = true;
				}
			}

			return childrenValid && !conflicts;
		}

		// added for use in solving algorithms for improved efficiency, and because we don't care about marking conflicts
		public bool IsValidIgnoreConflicts(int maxNumber, Point? position = null)
		{
			if (position != null && !Contains(position.Value)) return true;

			if (Components.Any(component => !component.IsValid(maxNumber, position))) return false;

			List<Cell> cells = Components.OfType<Cell>().ToList();

			if (!cells.Any()) return true;

			BitArray encountered = new BitArray(maxNumber);

			foreach (Cell cell in cells)
			{
				int index = cell.Value - 1;

				if (encountered[index]) return false;
				encountered[index] = true;
			}

			return true;
		}

		public bool ChangeValueAtPosition(int value, Point position)
		{
			foreach (ISudokuComponent component in Components)
			{
				if (component.ChangeValueAtPosition(value, position))
				{
					return true;
				}
			}
			return false;
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

		public void ReplaceComponent(ISudokuComponent old, ISudokuComponent new_)
		{
			if (Components.Contains(old))
			{
				Components.Remove(old);
				Components.Add(new_);
			}
			else
			{
				Components.ForEach(component =>
				{
					if (component is Composite composite)
					{
						composite.ReplaceComponent(old, new_);
					}
				});
			}
		}
	}
}
