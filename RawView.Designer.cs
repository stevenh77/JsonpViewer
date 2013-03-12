using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Fiddler;

namespace JsonpViewer
{
    internal class RawView : UserControl
    {
        public RichTextBox txtRaw;

        private Inspector2 m_owner;

        internal Button btnSpawnTextEditor;

        private TextBox txtSearchFor;

        private Panel pnlActions;

        private ContextMenuStrip mnuContextStrip;

        private ToolStripMenuItem miContextCut;

        private ToolStripMenuItem miContextCopy;

        private ToolStripMenuItem miContextPaste;

        private ToolStripSeparator toolStripMenuItem1;

        private ToolStripMenuItem miContextWordWrap;

        public ToolStripMenuItem miContextAutoTruncate;

        private IContainer components;

        private ToolStripMenuItem miTextWizard;

        private string sPrefKey;

        public RawView(Inspector2 oOwner)
        {
            string str;
            this.InitializeComponent();
            this.txtRaw.Font = new Font("Lucida Console", CONFIG.flFontSize);
            this.m_owner = oOwner;
            RawView rawView = this;
            if (this.m_owner is IRequestInspector2)
            {
                str = "fiddler.inspectors.request.raw.";
            }
            else
            {
                str = "fiddler.inspectors.response.raw.";
            }
            rawView.sPrefKey = str;
            this.SetWordWrapState(FiddlerApplication.Prefs.GetBoolPref(string.Concat(this.sPrefKey, "wordwrap"), false));
            this.txtRaw.BackColor = CONFIG.colorDisabledEdit;
            Utilities.SetCueText(this.txtSearchFor, " Find... (press Ctrl+Enter to highlight all)");
        }

        private void btnSpawnTextEditor_Click(object sender, EventArgs e)
        {
            ISpawnTextEditor mOwner = this.m_owner as ISpawnTextEditor;
            if (mOwner != null)
            {
                mOwner.SpawnTextEditor();
                return;
            }
            else
            {
                return;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DoPlainTextPaste()
        {
            if (!this.txtRaw.ReadOnly)
            {
                this.txtRaw.Paste(DataFormats.GetFormat(DataFormats.Text));
                return;
            }
            else
            {
                return;
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.txtRaw = new RichTextBox();
            this.mnuContextStrip = new ContextMenuStrip(this.components);
            this.miContextCopy = new ToolStripMenuItem();
            this.miContextCut = new ToolStripMenuItem();
            this.miContextPaste = new ToolStripMenuItem();
            this.miTextWizard = new ToolStripMenuItem();
            this.toolStripMenuItem1 = new ToolStripSeparator();
            this.miContextAutoTruncate = new ToolStripMenuItem();
            this.miContextWordWrap = new ToolStripMenuItem();
            this.pnlActions = new Panel();
            this.txtSearchFor = new TextBox();
            this.btnSpawnTextEditor = new Button();
            this.mnuContextStrip.SuspendLayout();
            this.pnlActions.SuspendLayout();
            base.SuspendLayout();
            this.txtRaw.AcceptsTab = true;
            this.txtRaw.AllowDrop = true;
            this.txtRaw.BackColor = Color.LightGray;
            this.txtRaw.ContextMenuStrip = this.mnuContextStrip;
            this.txtRaw.Dock = DockStyle.Fill;
            this.txtRaw.Font = new Font("Lucida Console", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.txtRaw.HideSelection = false;
            this.txtRaw.Location = new Point(0, 0);
            this.txtRaw.MaxLength = 1000000;
            this.txtRaw.Name = "txtRaw";
            this.txtRaw.ReadOnly = true;
            this.txtRaw.Size = new Size(380, 206);
            this.txtRaw.TabIndex = 0;
            this.txtRaw.Text = "";
            this.txtRaw.WordWrap = false;
            this.txtRaw.DragDrop += new DragEventHandler(this.txtRaw_DragDrop);
            this.txtRaw.DragEnter += new DragEventHandler(this.txtRaw_DragEnter);
            this.txtRaw.DragLeave += new EventHandler(this.txtRaw_DragLeave);
            this.txtRaw.DragOver += new DragEventHandler(this.txtRaw_DragOver);
            this.txtRaw.LinkClicked += new LinkClickedEventHandler(this.txtRaw_LinkClicked);
            this.txtRaw.ReadOnlyChanged += new EventHandler(this.txtRaw_ReadOnlyChanged);
            this.txtRaw.KeyDown += new KeyEventHandler(this.txtRaw_KeyDown);
            ToolStripItem[] toolStripItemArray = new ToolStripItem[7];
            toolStripItemArray[0] = this.miContextCopy;
            toolStripItemArray[1] = this.miContextCut;
            toolStripItemArray[2] = this.miContextPaste;
            toolStripItemArray[3] = this.miTextWizard;
            toolStripItemArray[4] = this.toolStripMenuItem1;
            toolStripItemArray[5] = this.miContextAutoTruncate;
            toolStripItemArray[6] = this.miContextWordWrap;
            this.mnuContextStrip.Items.AddRange(toolStripItemArray);
            this.mnuContextStrip.Name = "mnuContextStrip";
            this.mnuContextStrip.ShowCheckMargin = true;
            this.mnuContextStrip.ShowImageMargin = false;
            this.mnuContextStrip.Size = new Size(185, 164);
            this.mnuContextStrip.Opening += new CancelEventHandler(this.mnuContextStrip_Opening);
            this.miContextCopy.Name = "miContextCopy";
            this.miContextCopy.Size = new Size(184, 22);
            this.miContextCopy.Text = "&Copy";
            this.miContextCopy.Click += new EventHandler(this.miContextCopy_Click);
            this.miContextCut.Name = "miContextCut";
            this.miContextCut.Size = new Size(184, 22);
            this.miContextCut.Text = "Cu&t";
            this.miContextCut.Click += new EventHandler(this.miContextCut_Click);
            this.miContextPaste.Name = "miContextPaste";
            this.miContextPaste.Size = new Size(184, 22);
            this.miContextPaste.Text = "&Paste";
            this.miContextPaste.Click += new EventHandler(this.miContextPaste_Click);
            this.miTextWizard.Name = "miTextWizard";
            this.miTextWizard.Size = new Size(184, 22);
            this.miTextWizard.Text = "Send to T&extWizard...";
            this.miTextWizard.Click += new EventHandler(this.miTextWizard_Click);
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new Size(181, 6);
            this.miContextAutoTruncate.Checked = true;
            this.miContextAutoTruncate.CheckOnClick = true;
            this.miContextAutoTruncate.CheckState = CheckState.Checked;
            this.miContextAutoTruncate.Name = "miContextAutoTruncate";
            this.miContextAutoTruncate.Size = new Size(184, 22);
            this.miContextAutoTruncate.Text = "&AutoTruncate";
            this.miContextWordWrap.Name = "miContextWordWrap";
            this.miContextWordWrap.Size = new Size(184, 22);
            this.miContextWordWrap.Text = "&Word Wrap";
            this.miContextWordWrap.Click += new EventHandler(this.miContextWordWrap_Click);
            this.pnlActions.BackColor = SystemColors.AppWorkspace;
            this.pnlActions.Controls.Add(this.txtSearchFor);
            this.pnlActions.Controls.Add(this.btnSpawnTextEditor);
            this.pnlActions.Dock = DockStyle.Bottom;
            this.pnlActions.Location = new Point(0, 206);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new Size(380, 22);
            this.pnlActions.TabIndex = 4;
            this.txtSearchFor.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.txtSearchFor.BackColor = SystemColors.Window;
            this.txtSearchFor.BorderStyle = BorderStyle.FixedSingle;
            this.txtSearchFor.Font = new Font("Tahoma", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.txtSearchFor.ForeColor = SystemColors.ControlText;
            this.txtSearchFor.Location = new Point(3, 0);
            this.txtSearchFor.Name = "txtSearchFor";
            this.txtSearchFor.Size = new Size(275, 21);
            this.txtSearchFor.TabIndex = 10;
            this.txtSearchFor.TextChanged += new EventHandler(this.txtSearchFor_TextChanged);
            this.txtSearchFor.Enter += new EventHandler(this.txtSearchFor_Enter);
            this.txtSearchFor.KeyDown += new KeyEventHandler(this.txtSearchFor_KeyDown);
            this.btnSpawnTextEditor.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnSpawnTextEditor.BackColor = SystemColors.Control;
            this.btnSpawnTextEditor.Enabled = false;
            this.btnSpawnTextEditor.FlatStyle = FlatStyle.Flat;
            this.btnSpawnTextEditor.Font = new Font("Tahoma", 8f);
            this.btnSpawnTextEditor.Location = new Point(280, 0);
            this.btnSpawnTextEditor.Name = "btnSpawnTextEditor";
            this.btnSpawnTextEditor.Size = new Size(98, 20);
            this.btnSpawnTextEditor.TabIndex = 11;
            this.btnSpawnTextEditor.Text = "View in Notepad";
            this.btnSpawnTextEditor.UseVisualStyleBackColor = false;
            this.btnSpawnTextEditor.Click += new EventHandler(this.btnSpawnTextEditor_Click);
            base.Controls.Add(this.txtRaw);
            base.Controls.Add(this.pnlActions);
            this.Font = new Font("Tahoma", 8f);
            base.Name = "RawView";
            base.Size = new Size(380, 228);
            this.mnuContextStrip.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.pnlActions.PerformLayout();
            base.ResumeLayout(false);
        }

        private void miContextCopy_Click(object sender, EventArgs e)
        {
            this.txtRaw.Copy();
        }

        private void miContextCut_Click(object sender, EventArgs e)
        {
            this.txtRaw.Cut();
        }

        private void miContextPaste_Click(object sender, EventArgs e)
        {
            this.DoPlainTextPaste();
        }

        private void miContextWordWrap_Click(object sender, EventArgs e)
        {
            this.miContextWordWrap.Checked = !this.miContextWordWrap.Checked;
            this.SetWordWrapState(this.miContextWordWrap.Checked);
            FiddlerApplication.Prefs.SetBoolPref(string.Concat(this.sPrefKey, "wordwrap"), this.txtRaw.WordWrap);
        }

        private void miTextWizard_Click(object sender, EventArgs e)
        {
            FiddlerApplication.UI.actShowTextWizard(this.txtRaw.SelectedText);
        }

        private void mnuContextStrip_Opening(object sender, CancelEventArgs e)
        {
            bool length;
            bool dataPresent;
            bool flag = this.txtRaw.SelectedText.Length > 0;
            bool flag1 = flag;
            this.miContextCopy.Enabled = flag;
            this.miTextWizard.Enabled = flag1;
            ToolStripMenuItem toolStripMenuItem = this.miContextCut;
            if (this.txtRaw.ReadOnly)
            {
                length = false;
            }
            else
            {
                length = this.txtRaw.SelectedText.Length > 0;
            }
            toolStripMenuItem.Enabled = length;
            ToolStripMenuItem toolStripMenuItem1 = this.miContextPaste;
            if (this.txtRaw.ReadOnly)
            {
                dataPresent = false;
            }
            else
            {
                if (Clipboard.GetDataObject() == null)
                {
                    dataPresent = false;
                }
                else
                {
                    dataPresent = Clipboard.GetDataObject().GetDataPresent("Text", true);
                }
            }
            toolStripMenuItem1.Enabled = dataPresent;
            this.miContextAutoTruncate.Enabled = this.txtRaw.ReadOnly;
            this.miContextWordWrap.Checked = this.txtRaw.WordWrap;
        }

        internal void Reset()
        {
            this.txtRaw.Clear();
            this.txtSearchFor.BackColor = Color.FromKnownColor(KnownColor.Window);
        }

        private void SetWordWrapState(bool bWordWrap)
        {
            bool modified = this.txtRaw.Modified;
            this.txtRaw.WordWrap = bWordWrap;
            if (this.txtRaw.Modified != modified)
            {
                this.txtRaw.Modified = modified;
            }
        }

        private void txtRaw_DragDrop(object sender, DragEventArgs e)
        {
            if (!this.txtRaw.ReadOnly)
            {
                if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    if (e.Data.GetDataPresent(DataFormats.Text))
                    {
                        this.txtRaw.SelectedText = (string)e.Data.GetData(DataFormats.Text);
                    }
                }
                else
                {
                    string[] data = (string[])e.Data.GetData("FileDrop", false);
                    if ((int)data.Length > 0)
                    {
                        return;
                    }
                }
                return;
            }
            else
            {
                return;
            }
        }

        private void txtRaw_DragEnter(object sender, DragEventArgs e)
        {
            if (this.txtRaw.ReadOnly || !e.Data.GetDataPresent("Fiddler.Session"))
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            else
            {
                e.Effect = DragDropEffects.Copy;
                this.txtRaw.BackColor = Color.Lime;
                return;
            }
        }

        private void txtRaw_DragLeave(object sender, EventArgs e)
        {
            if (!this.txtRaw.ReadOnly)
            {
                this.txtRaw.BackColor = Color.FromKnownColor(KnownColor.Window);
            }
        }

        private void txtRaw_DragOver(object sender, DragEventArgs e)
        {
            if (this.txtRaw.ReadOnly || !e.Data.GetDataPresent(DataFormats.Text) && !e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            else
            {
                e.Effect = DragDropEffects.Copy;
                return;
            }
        }

        private void txtRaw_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers != Keys.Control || e.KeyCode != Keys.V)
            {
                if (e.KeyCode == Keys.F3)
                {
                    this.txtSearchFor.Focus();
                    bool flag = true;
                    bool flag1 = flag;
                    e.Handled = flag;
                    e.SuppressKeyPress = flag1;
                }
                return;
            }
            else
            {
                this.DoPlainTextPaste();
                bool flag2 = true;
                bool flag3 = flag2;
                e.SuppressKeyPress = flag2;
                e.Handled = flag3;
                return;
            }
        }

        private void txtRaw_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Utilities.LaunchHyperlink(e.LinkText);
        }

        private void txtRaw_ReadOnlyChanged(object sender, EventArgs e)
        {
            Color color;
            RichTextBox richTextBox = this.txtRaw;
            if (this.txtRaw.ReadOnly)
            {
                color = CONFIG.colorDisabledEdit;
            }
            else
            {
                color = Color.FromKnownColor(KnownColor.Window);
            }
            richTextBox.BackColor = color;
        }

        private void txtSearchFor_Enter(object sender, EventArgs e)
        {
            this.txtSearchFor.SelectAll();
        }

        private void txtSearchFor_KeyDown(object sender, KeyEventArgs e)
        {
            Keys keyCode = e.KeyCode;
            if (keyCode > Keys.Escape)
            {
                if (keyCode == Keys.Up)
                {
                    bool flag = true;
                    bool flag1 = flag;
                    e.SuppressKeyPress = flag;
                    e.Handled = flag1;
                    CommonMethods.ScrollUp(this.txtRaw);
                    return;
                }
                else if (keyCode == Keys.Right)
                {
                    return;
                }
                else if (keyCode == Keys.Down)
                {
                    bool flag2 = true;
                    bool flag3 = flag2;
                    e.SuppressKeyPress = flag2;
                    e.Handled = flag3;
                    CommonMethods.ScrollDown(this.txtRaw);
                    return;
                }
                if (keyCode != Keys.F3)
                {
                    return;
                }
            }
            else
            {
                if (keyCode != Keys.Return)
                {
                    if (keyCode == Keys.Escape)
                    {
                        this.txtSearchFor.Clear();
                        bool flag4 = true;
                        bool flag5 = flag4;
                        e.SuppressKeyPress = flag4;
                        e.Handled = flag5;
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            bool flag6 = true;
            bool flag7 = flag6;
            e.SuppressKeyPress = flag6;
            e.Handled = flag7;
            CommonMethods.DoRichTextSearch(this.txtRaw, this.txtSearchFor.Text, Control.ModifierKeys == Keys.Control);
        }

        private void txtSearchFor_TextChanged(object sender, EventArgs e)
        {
            Color lightGreen;
            if (this.txtSearchFor.Text.Length <= 0)
            {
                this.txtSearchFor.BackColor = Color.FromKnownColor(KnownColor.Window);
                this.txtRaw.Select(0, 0);
                return;
            }
            else
            {
                TextBox textBox = this.txtSearchFor;
                if (this.txtRaw.Find(this.txtSearchFor.Text, 0, RichTextBoxFinds.None) > -1)
                {
                    lightGreen = Color.LightGreen;
                }
                else
                {
                    lightGreen = Color.OrangeRed;
                }
                textBox.BackColor = lightGreen;
                return;
            }
        }
    }
}
