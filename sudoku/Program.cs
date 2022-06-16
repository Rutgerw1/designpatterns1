using sudoku.Game;
using sudoku.Reader;
using sudoku.SolvingAlgorithm;
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
            ISolvingAlgorithm solver = new BacktrackAlgorithm();
            InputHandler inputHandler = new InputHandler(mainView, solver);
            Puzzle puzzle = inputHandler.StartGame();

            if (puzzle != null)
            {
                inputHandler.PlayGame(puzzle);
            }
        }
    }
}
