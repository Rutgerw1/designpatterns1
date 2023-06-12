using System.Collections.Generic;
using System.Drawing;

namespace sudoku.View.States
{
	public interface IViewState : IView
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
		void FitConsole(int minWidth = 0);
	}
}
