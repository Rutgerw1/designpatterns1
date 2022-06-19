using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.View.States
{
    class NotesViewState : BaseViewState
    {
        public NotesViewState(IViewStateContext context) : base(context)
        {

        }

        public override void RegisterInput(int viewMode, IView view)
        {
            if (viewMode == 1)
            {
                Context.SetState(new NormalViewState(Context));
            }
        }
    }
}
