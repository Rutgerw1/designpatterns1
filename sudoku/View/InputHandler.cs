using sudoku.Game;
using sudoku.Reader;
using sudoku.SolvingAlgorithm;
using sudoku.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sudoku.View
{
	class InputHandler
	{
		private readonly MainView _mainView;
		private Puzzle _puzzle;
		private readonly ISolvingAlgorithm _solver;

		public InputHandler(MainView mainView, ISolvingAlgorithm solver)
		{
			_mainView = mainView;
			_solver = solver;
		}

		public Puzzle StartGame()
		{
			_mainView.PrintFilePrompt();

			ReaderFactory factory = new ReaderFactory();

			OpenFileDialog dialog = new OpenFileDialog();
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				ISudokuReader reader = null;
				string path = dialog.FileName;
				string ext = path.Split('.').Last();
				try
				{
					reader = factory.GetReader(ext);
				}
				catch (KeyNotFoundException)
				{
					List<string> typesList = new List<string>(factory.Types.Keys);
					string typesString = string.Join(", ", typesList.ToArray());
					_mainView.PrintInvalidFile(typesString);
					this.StartGame();
				}

				string file = File.ReadAllText(path);
				_puzzle = reader.CreatePuzzle(file);
			}

			return _puzzle;
		}

		public void PlayGame(Puzzle puzzle)
		{
			GameView gameView = new GameView(puzzle);
			bool quitGame = false;
			gameView.PrintGame();

			while (!quitGame)
			{
				ConsoleKeyInfo input = Console.ReadKey(true);
				List<(int X, int Y)> redrawLocations = new List<(int X, int Y)>();
				redrawLocations.Add((_puzzle.Location.X, _puzzle.Location.Y));

				IValidator validator = new Validator();

				switch (input.Key)
				{
					case ConsoleKey.Escape:
						quitGame = true;
						break;
					case ConsoleKey.UpArrow:
						puzzle.TryMove(Direction.Up);
						break;
					case ConsoleKey.RightArrow:
						puzzle.TryMove(Direction.Right);
						break;
					case ConsoleKey.DownArrow:
						puzzle.TryMove(Direction.Down);
						break;
					case ConsoleKey.LeftArrow:
						puzzle.TryMove(Direction.Left);
						break;
					case ConsoleKey.Backspace:
					case ConsoleKey.Delete:
						if (puzzle.ChangeCellValue(0))
						{
							Cell changedCell = puzzle.SelectedCell;
							redrawLocations.AddRange(ClearErrors(changedCell, puzzle));
						}
						gameView.ClearErrorMessage();
						break;
					case ConsoleKey.Spacebar:
						//switch editor modes;
						break;
					case ConsoleKey.C:
						List<(int X, int Y)> errors = validator.ValidateWhole(puzzle);
						if (errors.Count > 0)
						{
							gameView.PrintErrorsPresent();
							redrawLocations.AddRange(errors);
						} else
						{
							gameView.PrintNoErrorsPresent();
						}
						break;
					case ConsoleKey.S:
						errors = validator.ValidateWhole(puzzle);
						if (errors.Count == 0)
						{
							bool solved = _solver.Solve(puzzle, validator);
							gameView.PrintGame();
							if (!solved)
							{
								gameView.PrintUnsolvable();
							}
							else
							{
								quitGame = true;
							}
						}
						else
						{
							gameView.PrintErrorsPresent();
							redrawLocations.AddRange(errors);
						}
						break;
					default:
						if (char.IsDigit(input.KeyChar))
						{
							if (puzzle.ChangeCellValue(int.Parse(input.KeyChar.ToString())))
							{
								Cell changedCell = puzzle.SelectedCell;
								redrawLocations.AddRange(ClearErrors(changedCell, puzzle));
							}
							gameView.ClearErrorMessage();
						}
						break;
				}
				redrawLocations.Add((_puzzle.Location.X, _puzzle.Location.Y));
				gameView.RePrintCells(redrawLocations);
			}
			gameView.PrintFinish();
		}

		private List<(int X, int Y)> ClearErrors(Cell changedCell, Puzzle puzzle)
		{
			List<(int X, int Y)> toClear = new List<(int X, int Y)>();
			changedCell.OutOfBounds = false;
			changedCell.Conflicts.ForEach(otherCell => otherCell.Conflicts.Remove(changedCell));
			changedCell.Conflicts.ForEach(otherCell => toClear.Add(puzzle.GetCellLocation(otherCell).Value));
			changedCell.Conflicts.Clear();

			toClear.Add(puzzle.GetCellLocation(changedCell).Value);

			return toClear;
		}
	}
}
