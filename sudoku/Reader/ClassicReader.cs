using sudoku.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.Reader
{
	class ClassicReader : ISudokuReader
	{
		public Puzzle CreatePuzzle(string file)
		{
			char[] fileArray = file.ToCharArray();
			Cell[] cellArray = new Cell[fileArray.Length];

			for (int i = 0; i < cellArray.Length; i++)
			{
				List<int> allNotes = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
				cellArray[i] = new Cell((int)Char.GetNumericValue(fileArray[i]));
				cellArray[i].Notes = allNotes;
			}

			int groupLength = (int)Math.Sqrt(file.Length);

			Row[] rows = new Row[groupLength];
			Column[] columns = new Column[groupLength];
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

					row[x] = cellArray[rowIndex];
					column[x] = cellArray[columnIndex];
					region[x] = cellArray[regionIndex];
				}

				rows[y] = new Row(row.ToList());
				columns[y] = new Column(column.ToList());
				regions[y] = new Region(region.ToList());
			}

			return new Puzzle(rows.ToList(), columns.ToList(), regions.ToList());
		}
	}
}
