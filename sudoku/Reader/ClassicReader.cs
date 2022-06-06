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
				cellArray[i] = new Cell((int)Char.GetNumericValue(fileArray[i]));
			}

			int groupLength = (int)Math.Sqrt(file.Length);

			Row[] rows = new Row[groupLength];
			Column[] columns = new Column[groupLength];
			Region[] regions = new Region[groupLength];

			int sqrtCeiling = (int)Math.Ceiling(Math.Sqrt(groupLength));
			int sqrtFloor = (int)Math.Sqrt(groupLength);

			for (int y = 0; y < groupLength; y++)
			{
				Cell[] row = new Cell[groupLength];
				Cell[] column = new Cell[groupLength];
				Cell[] region = new Cell[groupLength];

				for (int x = 0; x < groupLength; x++)
				{
					int rowIndex = x + (y * groupLength);
					int columnIndex = y + (x * groupLength);
					//int regionIndex =
					//	x % sqrtCeiling + 
					//	y * sqrtCeiling + 
					//	(int)x / sqrtCeiling * groupLength + 
					//	(int)y / sqrtFloor * (sqrtFloor - 1) * groupLength;
					int regionIndex =
						x % sqrtCeiling +
						y * sqrtCeiling +
						groupLength * (int)(x / sqrtCeiling + y / sqrtFloor * (sqrtFloor - 1));

					row[x] = cellArray[rowIndex];
					column[x] = cellArray[columnIndex];
					region[x] = cellArray[regionIndex];
				}

				rows[y] = new Row(row);
				columns[y] = new Column(column);
				regions[y] = new Region(region);
			}

			return new Puzzle(rows, columns, regions);
		}
	}
}
