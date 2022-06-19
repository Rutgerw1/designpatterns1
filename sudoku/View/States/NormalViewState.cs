using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.View.States
{
    class NormalViewState : BaseViewState
    {
        public NormalViewState(IViewStateContext context) : base(context)
        {

        }

        public override void RegisterInput(int viewMode, IView view)
        {
            if(viewMode == 0)
            {
                Context.SetState(new NotesViewState(Context));
            }
        }
    }
}
