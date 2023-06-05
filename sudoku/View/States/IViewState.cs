using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.View.States
{
    interface IViewState : IView
    {
		void PrintGame();
		void ClearErrorMessage();
		void PrintErrorsPresent();
		void PrintNoErrorsPresent();
		void PrintUnsolvable();
		void RePrintCells(List<Point> redrawLocations);
		void PrintFinish();
		void PrintCorrectWin();
		void PrintIncorrectWin();
		void FitConsole();
    }
}
