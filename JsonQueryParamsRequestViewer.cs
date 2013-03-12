using System;
using System.Drawing;
using System.Text;
using System.Web;
using System.Windows.Forms;
using Fiddler;

namespace JsonpViewer
{
    public class JsonQueryParamsRequestViewer : Inspector2, IRequestInspector2, IBaseInspector2
    {
        private RawView myControl;
        private JsonpView jsonView;
        private HTTPRequestHeaders oHeaders;

        private byte[] oBody;

        private Encoding m_Encoding;

        public bool bDirty
        {
            get { return this.myControl.txtRaw.Modified; }
        }

        public byte[] body
        {
            get
            {
                int num = 0;
                int num1 = 0;
                HTTPHeaderParseWarnings hTTPHeaderParseWarning;
                if (this.myControl.txtRaw.Modified)
                {
                    byte[] bytes = this.m_Encoding.GetBytes(this.myControl.txtRaw.Text);
                    if (Parser.FindEntityBodyOffsetFromArray(bytes, out num, out num1, out hTTPHeaderParseWarning))
                    {
                        if (1 <= (int) bytes.Length - num1)
                        {
                            byte[] numArray = new byte[(int) bytes.Length - num1];
                            Buffer.BlockCopy(bytes, num1, numArray, 0, (int) numArray.Length);
                            return numArray;
                        }
                        else
                        {
                            return new byte[0];
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.oBody = value;
                this.DoRefresh();
            }
        }

        public bool bReadOnly
        {
            get { return this.myControl.txtRaw.ReadOnly; }
            set
            {
                Color color;
                this.myControl.txtRaw.ReadOnly = value;
                RichTextBox richTextBox = this.myControl.txtRaw;
                if (value)
                {
                    color = CONFIG.colorDisabledEdit;
                }
                else
                {
                    color = Color.FromKnownColor(KnownColor.Window);
                }
                richTextBox.BackColor = color;
                this.myControl.txtRaw.DetectUrls = value;
                this.DoRefresh();
            }
        }

        public HTTPRequestHeaders headers
        {
            get
            {
                int num = 0;
                int num1 = 0;
                HTTPHeaderParseWarnings hTTPHeaderParseWarning;
                if (this.myControl.txtRaw.Modified)
                {
                    byte[] bytes = this.m_Encoding.GetBytes(this.myControl.txtRaw.Text);
                    if (Parser.FindEntityBodyOffsetFromArray(bytes, out num, out num1, out hTTPHeaderParseWarning))
                    {
                        string str = string.Concat(this.m_Encoding.GetString(bytes, 0, num), "\r\n\r\n");
                        HTTPRequestHeaders hTTPRequestHeader = new HTTPRequestHeaders();
                        if (!hTTPRequestHeader.AssignFromString(str))
                        {
                            return null;
                        }
                        else
                        {
                            return hTTPRequestHeader;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.oHeaders = value;
                this.DoRefresh();
            }
        }

        public JsonQueryParamsRequestViewer()
        {
            this.m_Encoding = CONFIG.oHeaderEncoding;
        }

        public override void AddToTab(TabPage o)
        {
            this.myControl = new RawView(this);
            o.Text = "Json Query Params";
            //o.Controls.Add(this.myControl);
            
            this.jsonView = new JsonpView();
            o.Controls.Add(this.jsonView);
            o.Controls[0].Dock = DockStyle.Fill;

            this.myControl.miContextAutoTruncate.CheckedChanged +=
                new EventHandler(this.miContextAutoTruncate_CheckedChanged);
        }

        public void Clear()
        {
            this.oBody = null;
            this.oHeaders = null;
            this.myControl.txtRaw.Clear();
            this.myControl.btnSpawnTextEditor.Enabled = false;
        }

        private void DoRefresh()
        {
            string empty;
            string emptyDecoded;
            int int32Pref;
            if (this.oHeaders == null)
            {
                empty = string.Empty;
            }
            else
            {
                empty = this.oHeaders.ToString(true, true, true);
            }
            if (this.oBody != null)
            {
                this.m_Encoding = Utilities.getEntityBodyEncoding(this.oHeaders, this.oBody);
                string stringFromArrayRemovingBOM = Utilities.GetStringFromArrayRemovingBOM(this.oBody, this.m_Encoding);
                if (this.myControl.txtRaw.ReadOnly && this.myControl.miContextAutoTruncate.Checked)
                {
                    if (this.oHeaders == null ||
                        !this.oHeaders.ExistsAndContains("Content-Encoding", "deflate") &&
                        !this.oHeaders.ExistsAndContains("Content-Encoding", "gzip") &&
                        !this.oHeaders.ExistsAndContains("Content-Type", "image/") &&
                        !this.oHeaders.ExistsAndContains("Content-Type", "application/octet") &&
                        !this.oHeaders.ExistsAndContains("Content-Type", "audio/") &&
                        !this.oHeaders.ExistsAndContains("Content-Type", "video/"))
                    {
                        int32Pref =
                            FiddlerApplication.Prefs.GetInt32Pref("fiddler.inspectors.request.raw.truncatetextat",
                                                                  262144);
                    }
                    else
                    {
                        int32Pref =
                            FiddlerApplication.Prefs.GetInt32Pref("fiddler.inspectors.request.raw.truncatebinaryat", 128);
                    }
                    int num = int32Pref;
                    if (stringFromArrayRemovingBOM.Length > num)
                    {
                        stringFromArrayRemovingBOM =
                            string.Format(
                                "{0}\r\n\r\n*** FIDDLER: RawDisplay truncated at {1} characters. Right-click to disable truncation. ***",
                                stringFromArrayRemovingBOM.Substring(0, num), num);
                    }
                }
                empty = string.Concat(empty, stringFromArrayRemovingBOM);
            }
            this.myControl.Reset();

            if (empty.Substring(0, 3) == "GET")
            {
                // takes the first line, minus 'GET ' at the front and ' HTTP/1.1' at the end
                emptyDecoded = HttpUtility.UrlDecode(empty.Substring(4, empty.IndexOf((char)13) - 4 - 9));
                this.myControl.txtRaw.Text = emptyDecoded.Replace('\0', '\uFFFD');

                var indexOfRequest = emptyDecoded.IndexOf("?request=", StringComparison.InvariantCultureIgnoreCase) + "?request=".Length;
                var jsonRequest = emptyDecoded.Substring(indexOfRequest);
                this.jsonView.SetJSON(jsonRequest);
            }

            this.myControl.txtRaw.Modified = false;
            this.myControl.btnSpawnTextEditor.Enabled = true;
        }

        public override int GetOrder()
        {
            return 95;
        }

        private void miContextAutoTruncate_CheckedChanged(object sender, EventArgs e)
        {
            this.DoRefresh();
        }

        public override void SetFontSize(float flSizeInPoints)
        {
            if (this.myControl != null)
            {
                this.myControl.txtRaw.Font = new Font(this.myControl.txtRaw.Font.FontFamily, flSizeInPoints);
            }
        }

        public void SpawnTextEditor()
        {
            InspectorUtils.OpenTextEditor(string.Concat(CONFIG.GetPath("SafeTemp"), "\\RawFile.htm"), this.oHeaders,
                                          this.oBody);
        }

        public override bool UnsetDirtyFlag()
        {
            this.myControl.txtRaw.Modified = false;
            return true;
        }
    }
}
