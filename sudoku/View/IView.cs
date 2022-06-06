using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.View
{
    interface IView
    {
        void PrintMessage(string message, ConsoleColor color);
    }
}
