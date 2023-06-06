using sudoku.Creation.Builders;
using sudoku.Game;
using System;
using System.Drawing;
using System.Linq;

namespace sudoku.Creation
{
	class ClassicFactory : ISudokuFactory
	{
		public Puzzle CreatePuzzle(string file)
		{
			char[] valuesArray = file.ToArray();

			// size of the validation groups (rows, columns, regions)
			int groupSize = (int)Math.Sqrt(valuesArray.Length);

			int regionHeight = (int)Math.Sqrt(groupSize);   // 3 for 9x9, 2 for 8x8/6x6/4x4
			int regionWidth = groupSize / regionHeight;     // 3 for 9x9/6x6, 4 for 8x8, 2 for 4x4

			SudokuBuilder builder = new SudokuBuilder()
				.SetSize(groupSize);

			Composite[] columns = new Composite[groupSize];
			Composite[] regions = new Composite[groupSize];

			// example 9x9 region numbering:
			// 0 - 1 - 2 
			// 3 - 4 - 5
			// 6 - 7 - 8

			for (int y = 0; y < groupSize; y++)
			{
				Composite row = new Composite();
				for (int x = 0; x < groupSize; x++)
				{
					if (columns[x] == null) columns[x] = new Composite();

					// comment examples are for 9x9
					int regionNumber =
						(int)(y / regionHeight) // 0, 1 or 2
						* regionHeight      // times 3 gives 0, 3 or 6
						+ x / regionWidth;  // offsets by 0, 1 or 2 horizontally

					if (regions[regionNumber] == null) regions[regionNumber] = new Composite();

					Point cellPosition = new Point(x, y);
					int value = (int)char.GetNumericValue(valuesArray[x + y * groupSize]);
					Cell cell = new Cell(cellPosition, value, regionNumber);

					row.AddComponent(cell);
					columns[x].AddComponent(cell);
					regions[regionNumber].AddComponent(cell);
				}

				builder.AddComponent(row);
			}

			columns.ToList().ForEach(column => builder.AddComponent(column));
			regions.ToList().ForEach(region => builder.AddComponent(region));

			return builder.Build();
		}
	}
}
