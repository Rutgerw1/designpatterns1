using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.View
{
    interface IViewState
    {
        void RegisterInput(int viewMode, IView view);

    }
}
