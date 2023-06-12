using System;

namespace sudoku.View
{
	public interface IView
	{
		void PrintMessage(string message, ConsoleColor foregroundColor, ConsoleColor backgroundColor);
	}
}
