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
			ISudokuReader classicReader = new ClassicReader();
			List<Puzzle> subPuzzles = new List<Puzzle>();

			foreach(String puzzleString in puzzleStrings)
			{
				subPuzzles.Add(classicReader.CreatePuzzle(puzzleString));
			}

			List<Cell> cellList = new List<Cell>();

			for (int row = 0; row < 21; row++)
			{
				if (row < 6)
				{
					cellList = cellList.Concat(subPuzzles[0].Rows[row].Cells).ToList();
					cellList = cellList.Concat(GenerateInactiveCells()).ToList();
					cellList = cellList.Concat(subPuzzles[1].Rows[row].Cells).ToList();
				}
				else if (row < 9)
				{
					cellList = cellList.Concat(subPuzzles[0].Rows[row].Cells).ToList();
					cellList = cellList.Concat(subPuzzles[2].Rows[row - 6].Cells.GetRange(3, 3)).ToList();
					cellList = cellList.Concat(subPuzzles[1].Rows[row].Cells).ToList();
				}
				else if (row < 12)
				{
					cellList = cellList.Concat(GenerateInactiveCells()).ToList();
					cellList = cellList.Concat(GenerateInactiveCells()).ToList();
					cellList = cellList.Concat(subPuzzles[2].Rows[row - 6].Cells).ToList();
					cellList = cellList.Concat(GenerateInactiveCells()).ToList();
					cellList = cellList.Concat(GenerateInactiveCells()).ToList();
				}
				else if (row < 15)
				{
					cellList = cellList.Concat(subPuzzles[3].Rows[row - 12].Cells).ToList();
					cellList = cellList.Concat(subPuzzles[2].Rows[row - 6].Cells.GetRange(3, 3)).ToList();
					cellList = cellList.Concat(subPuzzles[4].Rows[row - 12].Cells).ToList();
				}
				else
				{
					cellList = cellList.Concat(subPuzzles[3].Rows[row - 12].Cells).ToList();
					cellList = cellList.Concat(GenerateInactiveCells()).ToList();
					cellList = cellList.Concat(subPuzzles[4].Rows[row - 12].Cells).ToList();
				}
			}

			int groupLength = (int)Math.Sqrt(cellList.Count);

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

					row[x] = cellList[rowIndex];
					column[x] = cellList[columnIndex];
				}

				rows[y] = new Row(row.ToList());
				columns[y] = new Column(column.ToList());
			}

			List<Region> regions = new List<Region>();
			for (int i = 0; i < 41; i++)
			{
				if (i < 12)
				{
					regions = regions.Concat(subPuzzles[0].Regions.GetRange(0, 3)).ToList();
					regions = regions.Concat(subPuzzles[1].Regions.GetRange(0, 3)).ToList();
					regions = regions.Concat(subPuzzles[0].Regions.GetRange(3, 3)).ToList();
					regions = regions.Concat(subPuzzles[1].Regions.GetRange(3, 3)).ToList();
				}
				else if (i < 19)
				{
					regions = regions.Concat(subPuzzles[0].Regions.GetRange(6, 3)).ToList();
					regions = regions.Concat(subPuzzles[2].Regions.GetRange(1, 1)).ToList();
					regions = regions.Concat(subPuzzles[1].Regions.GetRange(6, 3)).ToList();
				}
				else if (i < 22)
				{
					regions = regions.Concat(subPuzzles[2].Regions.GetRange(3, 3)).ToList();
				}
				else if (i < 29)
				{
					regions = regions.Concat(subPuzzles[3].Regions.GetRange(0, 3)).ToList();
					regions = regions.Concat(subPuzzles[2].Regions.GetRange(8, 1)).ToList();
					regions = regions.Concat(subPuzzles[4].Regions.GetRange(0, 3)).ToList();
				}
				else
				{
					regions = regions.Concat(subPuzzles[3].Regions.GetRange(3, 3)).ToList();
					regions = regions.Concat(subPuzzles[4].Regions.GetRange(3, 3)).ToList();
					regions = regions.Concat(subPuzzles[3].Regions.GetRange(6, 3)).ToList();
					regions = regions.Concat(subPuzzles[4].Regions.GetRange(6, 3)).ToList();
				}
			}

			return new Puzzle(rows.ToList(), columns.ToList(), regions, subPuzzles);
		}

		public List<Cell> GenerateInactiveCells()
		{
			return new List<Cell> { new Cell(0, false), new Cell(0, false), new Cell(0, false) };
		}
	}
}
