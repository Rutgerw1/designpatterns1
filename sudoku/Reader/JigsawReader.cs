using sudoku.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.Reader
{
	class JigsawReader : ISudokuReader
	{
		public Puzzle CreatePuzzle(string file)
		{
			string[] fileArray = new ArraySegment<string>(file.Split('='), 1, 81).ToArray();
			Dictionary<int, List<Cell>> cellDict = new Dictionary<int, List<Cell>>();
			Cell[] cellArray = new Cell[fileArray.Length];

			int i = 0;
			foreach (string cellString in fileArray)
			{
				int regionKey = (int)Char.GetNumericValue(cellString.ToCharArray()[2]);
				if (!cellDict.ContainsKey(regionKey))
				{
					cellDict.Add(regionKey, new List<Cell>());
				}
				Cell toAdd = new Cell((int)Char.GetNumericValue(cellString.ToCharArray()[0]));
				cellArray[i] = toAdd;
				cellDict[regionKey] = cellDict[regionKey].Append(toAdd).ToList();
				i++;
			}

			int groupLength = (int)Math.Sqrt(fileArray.Length);

			Row[] rows = new Row[groupLength];
			Column[] columns = new Column[groupLength];
			Region[] regions = new Region[groupLength];

			for (int y = 0; y < groupLength; y++)
			{
				Cell[] row = new Cell[groupLength];
				Cell[] column = new Cell[groupLength];
				Cell[] region = cellDict[y].ToArray();

				for (int x = 0; x < groupLength; x++)
				{
					int rowIndex = x + (y * groupLength);
					int columnIndex = y + (x * groupLength);

					row[x] = cellArray[rowIndex];
					column[x] = cellArray[columnIndex];
				}

				rows[y] = new Row(row.ToList());
				columns[y] = new Column(column.ToList());
				regions[y] = new Region(region.ToList());
			}

			return new Puzzle(rows.ToList(), columns.ToList(), regions.ToList());
		}
	}
}
