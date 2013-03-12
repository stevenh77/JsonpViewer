using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JsonpViewer
{
    public partial class InspectorUi : UserControl
    {
        public InspectorUi()
        {
            InitializeComponent();
        }

        public void SetText(string text)
        {
            _text.Text = text;
        }
    }
}
