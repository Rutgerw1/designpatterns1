using System.Collections.Generic;
using System.Drawing;

namespace sudoku.Game
{
	public interface ISudokuComponent
	{
		bool IsValid(int maxNumber, Point? position);
		bool ChangeValueAtPosition(int value, Point position);
		void ToggleNoteAtPosition(int value, Point position);
		Cell CellAtPosition(Point position);
		bool Contains(Point point);
	}
}
