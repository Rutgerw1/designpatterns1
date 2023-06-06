using sudoku.Creation.Builders;
using sudoku.Game;
using System;
using System.Drawing;
using System.Linq;

namespace sudoku.Creation
{
	class JigsawFactory : ISudokuFactory
	{
		public Puzzle CreatePuzzle(string file)
		{
			string[] cellStrings = new ArraySegment<string>(file.Split('='), 1, 81).ToArray();

			int groupSize = 9;

			Composite[] rows = new Composite[groupSize];
			Composite[] columns = new Composite[groupSize];
			Composite[] regions = new Composite[groupSize];

			int x = 0;
			int y = 0;
			foreach (string cellString in cellStrings)
			{
				int value = (int)char.GetNumericValue(cellString.ToCharArray().First());
				int region = (int)char.GetNumericValue(cellString.ToCharArray().Last());

				if (rows[y] == null) rows[y] = new Composite();
				if (columns[x] == null) columns[x] = new Composite();
				if (regions[region] == null) regions[region] = new Composite();

				Cell cell = new Cell(new Point(x, y), value, region);

				rows[y].AddComponent(cell);
				columns[x].AddComponent(cell);
				regions[region].AddComponent(cell);

				x++;
				if (x >= groupSize)
				{
					y++;
					x = 0;
				}
			}

			SudokuBuilder builder = new SudokuBuilder()
				.SetSize(groupSize)
				.SetMaxNumber(groupSize);

			rows.Concat(columns).Concat(regions).ToList().ForEach(component => builder.AddComponent(component));

			return builder.Build();
		}
	}
}
