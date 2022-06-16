using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.Game
{
	public class Region : Group
	{
		public Region(List<Cell> cells) : base(cells)
		{
			foreach(Cell cell in Cells)
			{
				cell.Region = this;
			}
		}
	}
}
