using sudoku.Creation;
using sudoku.Game;
using sudoku.SolvingAlgorithm;
using sudoku.View.States;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace sudoku.View
{
	public class InputHandler
	{
		private readonly MainView _mainView;
		private Puzzle _puzzle;
		private IViewState _viewState;
		private readonly ISolvingAlgorithm _solver;

		public InputHandler(MainView mainView, ISolvingAlgorithm solver)
		{
			_mainView = mainView;
			_solver = solver;
		}

		public void StartGame()
		{
			_mainView.PrintFilePrompt();

			OpenFileDialog dialog = new OpenFileDialog();
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				string path = dialog.FileName;
				string ext = path.Split('.').Last();
				string file = File.ReadAllText(path);

				SudokuFactory factory = new SudokuFactory(ext);

				try
				{
					_puzzle = factory.CreatePuzzle(file);
				}
				catch (KeyNotFoundException)
				{
					List<string> typesList = new List<string>(factory.Types.Keys);
					string typesString = string.Join(", ", typesList.ToArray());
					_mainView.PrintInvalidFile(typesString);
					StartGame();
				}
			}
		}

		public void PlayGame()
		{
			IViewState normalViewState = new NormalViewState(_puzzle);
			IViewState notesViewState = new NotesViewState(_puzzle);
			_viewState = normalViewState;

			bool quitGame = false;
			_viewState.PrintGame();

			while (!quitGame)
			{
				ConsoleKeyInfo input = Console.ReadKey(true);
				List<Point> redrawLocations = new List<Point>
				{
					_puzzle.Cursor
				};

				switch (input.Key)
				{
					case ConsoleKey.Escape:
						quitGame = true;
						break;
					case ConsoleKey.UpArrow:
						_puzzle.TryMove(Direction.Up);
						break;
					case ConsoleKey.RightArrow:
						_puzzle.TryMove(Direction.Right);
						break;
					case ConsoleKey.DownArrow:
						_puzzle.TryMove(Direction.Down);
						break;
					case ConsoleKey.LeftArrow:
						_puzzle.TryMove(Direction.Left);
						break;
					case ConsoleKey.Backspace:
					case ConsoleKey.Delete:
						redrawLocations.AddRange(HandleNumber(0));
						break;
					case ConsoleKey.Spacebar:
						_puzzle.ChangeState(_puzzle.State is NormalViewState ? notesViewState : normalViewState);
						_viewState = _puzzle.State;
						Console.Clear();
						System.Threading.Thread.Sleep(5);
						_viewState.FitConsole();
						_viewState.PrintGame();
						break;
					case ConsoleKey.C:
						if (!_puzzle.IsValid())
						{
							_viewState.PrintErrorsPresent();
							redrawLocations.AddRange(_puzzle.GetErrorLocations());
						}
						else
						{
							_viewState.PrintNoErrorsPresent();
						}
						break;
					case ConsoleKey.S:
						if (!_puzzle.IsValid())
						{
							_viewState.PrintErrorsPresent();
							redrawLocations.AddRange(_puzzle.GetErrorLocations());
						}
						else
						{
							Stopwatch timer = new Stopwatch();
							timer.Start();
							bool solved = _solver.Solve(_puzzle);
							timer.Stop();
							if (!solved)
							{
								_viewState.PrintUnsolvable();
							}
							else
							{
								_viewState.PrintGame();
								_viewState.ClearErrorMessage();
								_viewState.PrintMessage("Solved in " + timer.Elapsed.ToString(@"m\:ss\.fff"));
							}
						}
						break;
					case ConsoleKey.Enter:
						if (!_puzzle.IsValid() || _puzzle.FirstEmptyCell() != null)
						{
                            _viewState.PrintIncorrectWin();
                        }
						else
						{
                            _viewState.PrintCorrectWin();
                        }
						break;
					default:
						if (char.IsDigit(input.KeyChar))
						{
							redrawLocations.AddRange(HandleNumber(int.Parse(input.KeyChar.ToString())));
						}
						break;
				}
				redrawLocations.Add(_puzzle.Cursor);
				_viewState.RePrintCells(redrawLocations.Distinct().ToList());
			}
			_viewState.PrintFinish();
		}

		private List<Point> HandleNumber(int number)
		{
			List<Point> changedLocations = new List<Point> { _puzzle.Cursor };
			changedLocations.AddRange(_puzzle.CellAtPosition(_puzzle.Cursor).Conflicts.Select(otherCell => otherCell.Position));

			_puzzle.ChangeValueAtCursor(number);
			_viewState.ClearErrorMessage();
			return changedLocations;
		}
	}
}
