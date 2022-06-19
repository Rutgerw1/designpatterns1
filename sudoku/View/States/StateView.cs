using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.View.States
{
    class StateView : IViewStateContext
    {
        public IViewState State { get; private set; }
        public void PlayGame(IView view)
        {
            throw new NotImplementedException();
        }

        public void SetState(IViewState newState)
        {
            State = newState;
        }

        public void StartViewing(int viewMode, IView view)
        {
            State.RegisterInput(viewMode, view);
        }
    }
}
