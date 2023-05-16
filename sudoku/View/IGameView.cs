using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.View
{
    interface IGameView : IView
    {
		void PrintGame();
		void ClearErrorMessage();
		void PrintErrorsPresent();
		void PrintNoErrorsPresent();
		void PrintUnsolvable();
		void RePrintCells(List<(int X, int Y)> redrawLocations);
		void PrintFinish();
		void PrintCorrectWin();
		void PrintIncorrectWin();


    }
}
