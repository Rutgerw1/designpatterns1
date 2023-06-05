using sudoku.Game;
using sudoku.Factory;
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
