using sudoku.Game;
using sudoku.Reader;
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
		private MainView _mainView;
		private Puzzle _puzzle;

		public InputHandler(MainView mainView)
		{
			_mainView = mainView;
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
				catch (KeyNotFoundException e)
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

        public void playGame(Puzzle puzzle)
        {
			GameView gameView = new GameView(puzzle);
			bool quitGame = false;
			while (!quitGame)
            {
				gameView.PrintGame();
				switch (Console.ReadKey().Key)
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
					case ConsoleKey.Spacebar:
						//switch editor modes;
						break;
					case ConsoleKey.C:
						//check;
						break;
					case ConsoleKey.S:
						//solve
						break;
				}
            }
		
		}
    }
}
