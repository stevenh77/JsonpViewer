using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace JsonpViewer
{
    internal class NoToolTipTreeView : TreeView
    {
        private const int TVM_SETEXTENDEDSTYLE = 4396;

        private const int TVS_EX_DOUBLEBUFFER = 4;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                CreateParams style = createParams;
                style.Style = style.Style | 128;
                return createParams;
            }
        }

        public NoToolTipTreeView()
        {
        }

        private void EnableDoubleBuffer()
        {
            NoToolTipTreeView.SendMessage(base.Handle, 4396, (IntPtr)4, (IntPtr)4);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (Environment.OSVersion.Version.Major > 5)
            {
                this.EnableDoubleBuffer();
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.None)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    }
}
