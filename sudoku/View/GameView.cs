using sudoku.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.View
{
	class GameView : IView
	{
		private Puzzle _puzzle;

		public GameView(Puzzle puzzle)
		{
			_puzzle = puzzle;
		}

		public void PrintMessage(string message, ConsoleColor color = ConsoleColor.White)
		{
			Console.ForegroundColor = color;
			Console.Write(message);
		}

		public void PrintGame()
		{
			Console.Clear();
			_puzzle.Rows.ToList().ForEach(row =>
			{
				PrintRow(row);
			});
			PrintRowSeparator(_puzzle.Rows[0].Cells.Length, _puzzle.Rows.Length);
		}

		public void PrintRow(Row row)
		{
			PrintRowSeparator(row.Cells.Length, Array.IndexOf(_puzzle.Rows, row));
			for (int i = 0; i < row.Cells.Length; i++)
			{
				if (IsSameRegion(i > 0 ? row.Cells[i - 1] : null, row.Cells[i]))
				{
					PrintMessage(" | ", ConsoleColor.DarkBlue);
				}
				else
				{
					PrintMessage(" | ");
				}

				PrintMessage(row.Cells[i].ToString());
			}
			PrintMessage(" |\n");
		}

		private void PrintRowSeparator(int length, int rowNumber)
		{
			PrintMessage(" ");
			for (int i = 0; i < length; i++)
			{
				PrintMessage("+");
				Cell cell1 = null;
				Cell cell2 = null;
				if (rowNumber > 0)
				{
					cell1 = _puzzle.Rows[rowNumber - 1].Cells[i];
				}
				if (rowNumber < _puzzle.Rows.Length)
				{
					cell2 = _puzzle.Rows[rowNumber].Cells[i];
				}

				if (IsSameRegion(cell1, cell2))
				{
					PrintMessage("---", ConsoleColor.DarkBlue);
				}
				else
				{
					PrintMessage("---");
				}
			}
			PrintMessage("+\n");
		}

		private bool IsSameRegion(Cell cell1, Cell cell2)
		{
			return cell1?.Region == cell2?.Region;
		}
	}
}
