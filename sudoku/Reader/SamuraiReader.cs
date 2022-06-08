using sudoku.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.Reader
{
	class SamuraiReader : ISudokuReader
	{
		public Puzzle CreatePuzzle(string file)
		{
			string[] puzzleStrings = file.Split(new string[] { "\r\n" }, StringSplitOptions.None);
			ClassicReader classicReader = new ClassicReader();
			Puzzle[] subPuzzles = new Puzzle[5];

			for (int i = 0; i < subPuzzles.Length; i++)
			{
				subPuzzles[i] = classicReader.CreatePuzzle(puzzleStrings[i]);
			}

			Cell[] cellArray = new Cell[0];

			for (int row = 0; row < 21; row++)
			{
				if (row < 6)
				{
					cellArray = cellArray.Concat(subPuzzles[0].Rows[row].Cells).ToArray();
					cellArray = cellArray.Concat(GenerateInactiveCells()).ToArray();
					cellArray = cellArray.Concat(subPuzzles[1].Rows[row].Cells).ToArray();
				}
				else if (row < 9)
				{
					cellArray = cellArray.Concat(subPuzzles[0].Rows[row].Cells).ToArray();
					cellArray = cellArray.Concat(new ArraySegment<Cell>(subPuzzles[2].Rows[row - 6].Cells, 3, 3)).ToArray();
					cellArray = cellArray.Concat(subPuzzles[1].Rows[row].Cells).ToArray();
				}
				else if (row < 12)
				{
					cellArray = cellArray.Concat(GenerateInactiveCells()).ToArray();
					cellArray = cellArray.Concat(GenerateInactiveCells()).ToArray();
					cellArray = cellArray.Concat(subPuzzles[2].Rows[row - 6].Cells).ToArray();
					cellArray = cellArray.Concat(GenerateInactiveCells()).ToArray();
					cellArray = cellArray.Concat(GenerateInactiveCells()).ToArray();
				}
				else if (row < 15)
				{
					cellArray = cellArray.Concat(subPuzzles[3].Rows[row - 12].Cells).ToArray();
					cellArray = cellArray.Concat(new ArraySegment<Cell>(subPuzzles[2].Rows[row - 6].Cells, 3, 3)).ToArray();
					cellArray = cellArray.Concat(subPuzzles[4].Rows[row - 12].Cells).ToArray();
				}
				else
				{
					cellArray = cellArray.Concat(subPuzzles[3].Rows[row - 12].Cells).ToArray();
					cellArray = cellArray.Concat(GenerateInactiveCells()).ToArray();
					cellArray = cellArray.Concat(subPuzzles[4].Rows[row - 12].Cells).ToArray();
				}
			}

			int groupLength = (int)Math.Sqrt(cellArray.Length);

			Row[] rows = new Row[groupLength];
			Column[] columns = new Column[groupLength];

			for (int y = 0; y < groupLength; y++)
			{
				Cell[] row = new Cell[groupLength];
				Cell[] column = new Cell[groupLength];

				for (int x = 0; x < groupLength; x++)
				{
					int rowIndex = x + (y * groupLength);
					int columnIndex = y + (x * groupLength);

					row[x] = cellArray[rowIndex];
					column[x] = cellArray[columnIndex];
				}

				rows[y] = new Row(row);
				columns[y] = new Column(column);
			}

			Region[] regions = new Region[0];
			for (int i = 0; i < 41; i++)
			{
				if (i < 12)
				{
					regions = regions.Concat(new ArraySegment<Region>(subPuzzles[0].Regions, 0, 3)).ToArray();
					regions = regions.Concat(new ArraySegment<Region>(subPuzzles[1].Regions, 0, 3)).ToArray();
					regions = regions.Concat(new ArraySegment<Region>(subPuzzles[0].Regions, 3, 3)).ToArray();
					regions = regions.Concat(new ArraySegment<Region>(subPuzzles[1].Regions, 3, 3)).ToArray();
				}
				else if (i < 19)
				{
					regions = regions.Concat(new ArraySegment<Region>(subPuzzles[0].Regions, 6, 3)).ToArray();
					regions = regions.Concat(new ArraySegment<Region>(subPuzzles[2].Regions, 1, 1)).ToArray();
					regions = regions.Concat(new ArraySegment<Region>(subPuzzles[1].Regions, 6, 3)).ToArray();
				}
				else if (i < 22)
				{
					regions = regions.Concat(new ArraySegment<Region>(subPuzzles[2].Regions, 3, 3)).ToArray();
				}
				else if (i < 29)
				{
					regions = regions.Concat(new ArraySegment<Region>(subPuzzles[3].Regions, 0, 3)).ToArray();
					regions = regions.Concat(new ArraySegment<Region>(subPuzzles[2].Regions, 8, 1)).ToArray();
					regions = regions.Concat(new ArraySegment<Region>(subPuzzles[4].Regions, 0, 3)).ToArray();
				}
				else
				{
					regions = regions.Concat(new ArraySegment<Region>(subPuzzles[3].Regions, 3, 3)).ToArray();
					regions = regions.Concat(new ArraySegment<Region>(subPuzzles[4].Regions, 3, 3)).ToArray();
					regions = regions.Concat(new ArraySegment<Region>(subPuzzles[3].Regions, 6, 3)).ToArray();
					regions = regions.Concat(new ArraySegment<Region>(subPuzzles[4].Regions, 6, 3)).ToArray();
				}
			}

			return new Puzzle(rows, columns, regions);
		}

		public Cell[] GenerateInactiveCells()
		{
			return new Cell[] { new Cell(0, false), new Cell(0, false), new Cell(0, false) };
		}
	}
}
