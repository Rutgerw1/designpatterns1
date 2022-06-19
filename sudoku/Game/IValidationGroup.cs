using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.Game
{
	public interface IValidationGroup
	{
		List<Cell> Cells { get; }
	}
}
