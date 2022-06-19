using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.View.States
{
    abstract class BaseViewState : IViewState
    {
        public IViewStateContext Context { get; }
        public BaseViewState(IViewStateContext context)
        {
            Context = context;
        }

        public virtual void RegisterInput(int viewMode, IView view)
        {

        }
    }
}
