﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.Game
{
	public class Column : Group
	{
		public Column(List<Cell> cells) : base(cells)
		{
		}
	}
}
