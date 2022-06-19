using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.View.States
{
    interface IViewStateContext
    {
        void SetState(IViewState newState);

        void PlayGame(IView view);
    }
}
