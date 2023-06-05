using sudoku.SolvingAlgorithm;
using sudoku.View;
using System;

namespace sudoku
{
	class Program
	{
		[STAThread]
		static void Main()
		{
			MainView mainView = new MainView();
			ISolvingAlgorithm solver = new BacktrackAlgorithm();
			InputHandler inputHandler = new InputHandler(mainView, solver);
			inputHandler.StartGame();

			inputHandler.PlayGame();
		}
	}
}
