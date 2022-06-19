using sudoku.Game;
using sudoku.Validation;
using sudoku.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.SolvingAlgorithm
{
    interface ISolvingAlgorithm
    {
        bool Solve(Puzzle puzzle, IValidator validator);
    }
}
