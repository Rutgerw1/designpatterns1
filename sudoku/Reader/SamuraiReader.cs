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

			for (int i = 0; i < puzzleStrings.Length; i++)
			{
				if (i != 2)
				{
					subPuzzles.Add(classicReader.CreatePuzzle(puzzleStrings[i]));
				}
				else
				{ // to handle overlapping cells, we want to build up subPuzzles[2] manually
					subPuzzles.Add(null);
				}
			}
			subPuzzles[2] = BuildCentralPuzzle(subPuzzles, puzzleStrings[2]);

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

			Group[] rows = new Group[groupLength];
			Group[] columns = new Group[groupLength];

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

				rows[y] = new Group(row.ToList());
				columns[y] = new Group(column.ToList());
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

		private Puzzle BuildCentralPuzzle(List<Puzzle> subPuzzles, string puzzleString)
		{
			List<Cell> cellsList = new List<Cell>();

			for (int i = 0; i < 9; i++)
			{
				if (i < 3)
				{
					cellsList.AddRange(subPuzzles[0].Rows[i + 6].Cells.GetRange(6, 3));
					puzzleString.Substring(i * 9 + 3, 3).ToCharArray().ToList().ForEach(c => cellsList.Add(new Cell((int)char.GetNumericValue(c))));
					cellsList.AddRange(subPuzzles[1].Rows[i + 6].Cells.GetRange(0, 3));
				}
				else if (i < 6)
				{
					puzzleString.Substring(i * 9, 9).ToCharArray().ToList().ForEach(c => cellsList.Add(new Cell((int)char.GetNumericValue(c))));
				}
				else
				{
					cellsList.AddRange(subPuzzles[3].Rows[i - 6].Cells.GetRange(6, 3));
					puzzleString.Substring(i * 9 + 3, 3).ToCharArray().ToList().ForEach(c => cellsList.Add(new Cell((int)char.GetNumericValue(c))));
					cellsList.AddRange(subPuzzles[4].Rows[i - 6].Cells.GetRange(0, 3));
				}
			}

			int groupLength = (int)Math.Sqrt(cellsList.Count);

			Group[] rows = new Group[groupLength];
			Group[] columns = new Group[groupLength];
			Region[] regions = new Region[groupLength];

			int sqrtFloor = (int)Math.Sqrt(groupLength);
			int sqrtInverse = groupLength / sqrtFloor;

			for (int y = 0; y < groupLength; y++)
			{
				Cell[] row = new Cell[groupLength];
				Cell[] column = new Cell[groupLength];
				Cell[] region = new Cell[groupLength];

				for (int x = 0; x < groupLength; x++)
				{
					int rowIndex = x + (y * groupLength);
					int columnIndex = y + (x * groupLength);
					int regionIndex =
						x % sqrtInverse +
						y * sqrtInverse +
						groupLength * (int)(x / sqrtInverse + y / sqrtFloor * (sqrtFloor - 1));

					row[x] = cellsList[rowIndex];
					column[x] = cellsList[columnIndex];
					region[x] = cellsList[regionIndex];
				}

				rows[y] = new Group(row.ToList());
				columns[y] = new Group(column.ToList());
				regions[y] = new Region(region.ToList());
			}

			return new Puzzle(rows.ToList(), columns.ToList(), regions.ToList());
		}

		public List<Cell> GenerateInactiveCells()
		{
			return new List<Cell> { new Cell(0, false), new Cell(0, false), new Cell(0, false) };
		}
	}
}
