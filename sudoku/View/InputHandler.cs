using sudoku.Creation;
using sudoku.Game;
using sudoku.SolvingAlgorithm;
using sudoku.View.States;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace sudoku.View
{
	class InputHandler
	{
		private MainView MainView { get; }
		private Puzzle Puzzle { get; set; }
		private IViewState ViewState { get; set; }
		private ISolvingAlgorithm Solver { get; }

		public InputHandler(MainView mainView, ISolvingAlgorithm solver)
		{
			MainView = mainView;
			Solver = solver;
		}

		public void StartGame()
		{
			MainView.PrintFilePrompt();

			OpenFileDialog dialog = new OpenFileDialog();
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				string path = dialog.FileName;
				string ext = path.Split('.').Last();
				string file = File.ReadAllText(path);

				SudokuFactory factory = new SudokuFactory(ext);

				try
				{
					Puzzle = factory.CreatePuzzle(file);
				}
				catch (KeyNotFoundException)
				{
					List<string> typesList = new List<string>(factory.Types.Keys);
					string typesString = string.Join(", ", typesList.ToArray());
					MainView.PrintInvalidFile(typesString);
					StartGame();
				}
			}
		}

		public void PlayGame()
		{
			IViewState normalViewState = new NormalViewState(Puzzle);
			IViewState notesViewState = new NotesViewState(Puzzle);
			ViewState = normalViewState;

			bool quitGame = false;
			ViewState.PrintGame();

			while (!quitGame)
			{
				ConsoleKeyInfo input = Console.ReadKey(true);
				List<Point> redrawLocations = new List<Point>
				{
					Puzzle.Cursor
				};

				switch (input.Key)
				{
					case ConsoleKey.Escape:
						quitGame = true;
						break;
					case ConsoleKey.UpArrow:
						Puzzle.TryMove(Direction.Up);
						break;
					case ConsoleKey.RightArrow:
						Puzzle.TryMove(Direction.Right);
						break;
					case ConsoleKey.DownArrow:
						Puzzle.TryMove(Direction.Down);
						break;
					case ConsoleKey.LeftArrow:
						Puzzle.TryMove(Direction.Left);
						break;
					case ConsoleKey.Backspace:
					case ConsoleKey.Delete:
						redrawLocations.AddRange(HandleNumber(0));
						break;
					case ConsoleKey.Spacebar:
						Puzzle.ChangeState(Puzzle.State is NormalViewState ? notesViewState : normalViewState);
						ViewState = Puzzle.State;
						Console.Clear();
						System.Threading.Thread.Sleep(5);
						ViewState.FitConsole();
						ViewState.PrintGame();
						break;
					case ConsoleKey.C:
						//List<(int X, int Y)> errors = validator.ValidateWhole(Puzzle);
						//if (errors.Count > 0)
						//{
						//	View.PrintErrorsPresent();
						//	redrawLocations.AddRange(errors);
						//}
						//else
						//{
						//	View.PrintNoErrorsPresent();
						//}
						break;
					case ConsoleKey.S:
						//errors = validator.ValidateWhole(Puzzle);
						//if (errors.Count == 0)
						//{
						//	bool solved = Solver.Solve(Puzzle, validator);
						//	View.PrintGame();
						//	if (!solved)
						//	{
						//		View.PrintUnsolvable();
						//	}
						//	else
						//	{
						//		quitGame = true;
						//	}
						//}
						//else
						//{
						//	View.PrintErrorsPresent();
						//	redrawLocations.AddRange(errors);
						//}
						break;
					case ConsoleKey.Enter:
						//errors = validator.ValidateWhole(Puzzle);
						//if (errors.Count > 0 || Puzzle.FirstEmptyCellLocation() != null)
						//{
						//    View.PrintIncorrectWin();
						//}
						//else
						//{
						//    View.PrintCorrectWin();
						//}
						break;
					default:
						if (char.IsDigit(input.KeyChar))
						{
							redrawLocations.AddRange(HandleNumber(int.Parse(input.KeyChar.ToString())));
						}
						break;
				}
				redrawLocations.Add(Puzzle.Cursor);
				ViewState.RePrintCells(redrawLocations.Distinct().ToList());
			}
			ViewState.PrintFinish();
		}

		private List<Point> HandleNumber(int number)
		{
			List<Point> changedLocations = new List<Point> { Puzzle.Cursor };
			changedLocations.AddRange(Puzzle.CellAtPosition(Puzzle.Cursor).Conflicts.Select(otherCell => otherCell.Position));

			Puzzle.ChangeValueAtCursor(number);
			ViewState.ClearErrorMessage();
			return changedLocations;
		}
	}
}
