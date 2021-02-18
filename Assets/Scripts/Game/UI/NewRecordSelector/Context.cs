using System;

namespace UI.NewRecordSelector
{
    class Context
    {
        public int SelectedIndex = -1;
        public Action<int> OnCellClicked;
    }
}
