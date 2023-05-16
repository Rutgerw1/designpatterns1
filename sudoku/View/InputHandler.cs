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
		private IGameView _view;
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
				ISudokuReader reader;
				string path = dialog.FileName;
				string ext = path.Split('.').Last();
				string file = File.ReadAllText(path);
				try
				{
					reader = factory.GetReader(ext);
				}
				catch (KeyNotFoundException)
				{
					List<string> typesList = new List<string>(factory.Types.Keys);
					string typesString = string.Join(", ", typesList.ToArray());
					_mainView.PrintInvalidFile(typesString);
                    return StartGame();
				}
				_puzzle = reader.CreatePuzzle(file);
			}

			return _puzzle;
		}

		public void PlayGame(Puzzle puzzle)
		{
			IGameView inputView = new InputView(puzzle);
			IGameView notesView = new NotesView(puzzle);

			_view = inputView;

			bool quitGame = false;
			if (_puzzle.NotesMode)
			{
				notesView.PrintGame();
			}
			else
			{
				_view.PrintGame();
			}

			while (!quitGame)
			{
				ConsoleKeyInfo input = Console.ReadKey(true);
				List<(int X, int Y)> redrawLocations = new List<(int X, int Y)>
				{
					(_puzzle.Location.X, _puzzle.Location.Y)
				};

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
						_view.ClearErrorMessage();
						break;
					case ConsoleKey.Spacebar:
						puzzle.ToggleNotesMode();
						_view = puzzle.NotesMode ? notesView : inputView;
						_view.PrintGame();
						break;
					case ConsoleKey.C:
						List<(int X, int Y)> errors = validator.ValidateWhole(puzzle);
						if (errors.Count > 0)
						{
							_view.PrintErrorsPresent();
							redrawLocations.AddRange(errors);
						}
						else
						{
							_view.PrintNoErrorsPresent();
						}
						break;
					case ConsoleKey.S:
						errors = validator.ValidateWhole(puzzle);
						if (errors.Count == 0)
						{
							bool solved = _solver.Solve(puzzle, validator);
							_view.PrintGame();
							if (!solved)
							{
								_view.PrintUnsolvable();
							}
							else
							{
								quitGame = true;
							}
						}
						else
						{
							_view.PrintErrorsPresent();
							redrawLocations.AddRange(errors);
						}
						break;
                    case ConsoleKey.Enter:
                        errors = validator.ValidateWhole(puzzle);
                        if (errors.Count > 0 || puzzle.FirstEmptyCellLocation() != null)
                        {
                            _view.PrintIncorrectWin();
                        }
                        else
                        {
                            _view.PrintCorrectWin();
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
							_view.ClearErrorMessage();
						}
						break;
				}
				redrawLocations.Add((_puzzle.Location.X, _puzzle.Location.Y));
				_view.RePrintCells(redrawLocations);
			}
			_view.PrintFinish();
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
