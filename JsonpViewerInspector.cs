using Fiddler;
using System;
using System.Windows.Forms;

namespace JsonpViewer
{
    [assembly: Fiddler.RequiredVersion("4.4.2.4")]

    public class JsonpViewerInspector : Inspector2, IRequestInspector2
    {
        private InspectorUi _myControl;
        private byte[] _entityBody;

        public bool bDirty { get { return false; } }
        public bool bReadOnly { get { return true; } set { } }

        // Return null if your control doesn't allow header editing.
        public HTTPRequestHeaders headers { get { return null; } set { } }        
        
        public override int GetOrder() 
        { 
            return 0; 
        }
        
        public void Clear() 
        { 
            _myControl.SetText(string.Empty); 
        }
        
        public override void AddToTab(TabPage o)
        {
            _myControl = new InspectorUi();
            o.Text = "Steve's UI";
            o.Controls.Add(_myControl);
            o.Controls[0].Dock = DockStyle.Fill;
        }

        public byte[] body
        {
            get { return _entityBody; }
            set { UpdateView(value); }
        }

        private void UpdateView(byte[] bytes)
        {
            _entityBody = bytes;

            try
            {
                string messageAsText = _entityBody.ToString();
                _myControl.SetText(messageAsText);
            }
            catch (Exception ex)
            {
                _myControl.SetText(ex.ToString());
            }
        }
    }

    public class JsonpRequestInspector : JsonpViewerInspector, IRequestInspector2
    {
        public HTTPResponseHeaders headers
        {
            get { return null; }
            set { }
        }
    }

    public class JsonpResponseInspector : JsonpViewerInspector, IResponseInspector2
    {
        public HTTPResponseHeaders headers
        {
            get { return null; }
            set { }
        }
    }
}