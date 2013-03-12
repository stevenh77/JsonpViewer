using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Fiddler;

namespace JsonpViewer
{
    public class JsonpResponseViewer : Inspector2, IResponseInspector2, IBaseInspector2
    {
        private JsonpView myControl;

        private HTTPResponseHeaders oHeaders;

        private Encoding oEncoding;

        public bool bDirty
        {
            get
            {
                return false;
            }
        }

        public byte[] body
        {
            get
            {
                return null;
            }
            set
            {
                if (value == null || (int)value.Length < 1)
                {
                    this.Clear();
                    return;
                }
                else
                {
                    try
                    {
                        if (this.oHeaders != null && (this.oHeaders.Exists("Transfer-Encoding") || this.oHeaders.Exists("Content-Encoding")))
                        {
                            if ((int)value.Length < 100000 || this.oHeaders.ExistsAndContains("Content-Type", "application/json") || this.oHeaders.ExistsAndContains("Content-Type", "javascript"))
                            {
                                byte[] numArray = Utilities.Dupe(value);
                                try
                                {
                                    Utilities.utilDecodeHTTPBody(this.oHeaders, ref numArray);
                                    value = numArray;
                                }
                                catch
                                {
                                }
                            }
                            else
                            {
                                this.Clear();
                                return;
                            }
                        }
                        this.oEncoding = Utilities.getEntityBodyEncoding(this.oHeaders, value);
                        string stringFromArrayRemovingBOM = Utilities.GetStringFromArrayRemovingBOM(value, this.oEncoding);
                        this.myControl.SetJSON(stringFromArrayRemovingBOM);
                    }
                    catch (Exception exception)
                    {
                        this.Clear();
                    }
                    return;
                }
            }
        }

        public bool bReadOnly
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        public HTTPResponseHeaders headers
        {
            get
            {
                return null;
            }
            set
            {
                this.oHeaders = value;
            }
        }

        public JsonpResponseViewer()
        {
            this.myControl = new JsonpView();
        }

        public override void AddToTab(TabPage o)
        {
            o.Text = "Jsonp";
            o.Controls.Add(this.myControl);
            o.Controls[0].Dock = DockStyle.Fill;
        }

        public void Clear()
        {
            this.myControl.Clear();
            this.oHeaders = null;
            this.oEncoding = null;
        }

        public override int GetOrder()
        {
            return 100;
        }

        public override int ScoreForContentType(string sMIMEType)
        {
            if (!sMIMEType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
            {
                return -1;
            }
            else
            {
                return 55;
            }
        }

        public override void SetFontSize(float flSizeInPoints)
        {
            if (this.myControl != null)
            {
                this.myControl.tvJSON.Font = new Font(this.myControl.tvJSON.Font.FontFamily, flSizeInPoints);
            }
        }
    }
}
