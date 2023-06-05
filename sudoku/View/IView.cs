using System;

namespace sudoku.View
{
	interface IView
	{
		void PrintMessage(string message, ConsoleColor foregroundColor, ConsoleColor backgroundColor);
	}
}
