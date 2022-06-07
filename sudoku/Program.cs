using sudoku.Game;
using sudoku.Reader;
using sudoku.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sudoku
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            MainView mainView = new MainView();
            InputHandler inputHandler = new InputHandler(mainView);
            Puzzle puzzle = inputHandler.StartGame();

            if (puzzle != null)
            {
                inputHandler.playGame(puzzle);
            }
        }
    }
}
