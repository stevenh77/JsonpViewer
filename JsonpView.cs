using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Fiddler;
using Fiddler.WebFormats;

namespace JsonpViewer
{
    internal class JsonpView : UserControl
    {
        internal NoToolTipTreeView tvJSON;

        private Panel pnlFooter;

        private TextBox txtSearch;

        private Button btnCollapseAll;

        private Button btnExpandAll;

        private Label lblStatus;

        private ContextMenuStrip mnuJSONTree;

        private ToolStripMenuItem tsmiCopyNode;

        private ToolStripMenuItem tsmiSendNodeToTextWizard;

        private IContainer components;

        public JsonpView()
        {
            this.InitializeComponent();
            this.tvJSON.BackColor = CONFIG.colorDisabledEdit;
            Utilities.SetCueText(this.txtSearch, "Search...");
            if (CONFIG.flFontSize != this.tvJSON.Font.Size)
            {
                this.tvJSON.Font = new Font(this.tvJSON.Font.FontFamily, CONFIG.flFontSize);
            }
        }

        public void BeginUpdate()
        {
            this.tvJSON.BeginUpdate();
            this.tvJSON.SuspendLayout();
        }

        private void btnCollapseAll_Click(object sender, EventArgs e)
        {
            this.BeginUpdate();
            this.tvJSON.CollapseAll();
            if (1 == this.tvJSON.Nodes.Count)
            {
                this.tvJSON.Nodes[0].Expand();
            }
            this.EndUpdate();
        }

        private void btnExpandAll_Click(object sender, EventArgs e)
        {
            this.BeginUpdate();
            this.tvJSON.ExpandAll();
            if (this.tvJSON.SelectedNode == null)
            {
                if (this.tvJSON.Nodes.Count > 0)
                {
                    this.tvJSON.Nodes[0].EnsureVisible();
                }
            }
            else
            {
                this.tvJSON.SelectedNode.EnsureVisible();
            }
            this.EndUpdate();
        }

        private void ChainNodes(TreeNode inTreeNode, object o, bool bParentIsArray, ref uint iNodeCount)
        {
            string str;
            object obj;
            if (o as ArrayList == null)
            {
                if (o as Hashtable == null)
                {
                    if (o == null)
                    {
                        str = "(null)";
                    }
                    else
                    {
                        str = o.ToString();
                    }
                    string str1 = str;
                    if (!bParentIsArray)
                    {
                        string treeNodeFullText = CommonMethods.GetTreeNodeFullText(inTreeNode);
                        TreeNode treeNode = inTreeNode;
                        string str2 = "{0}{1}{2}";
                        string str3 = treeNodeFullText;
                        if (treeNodeFullText.Contains("="))
                        {
                            obj = ", ";
                        }
                        else
                        {
                            obj = "=";
                        }
                        CommonMethods.SetTreeNodeLimitedText(treeNode, string.Format(str2, str3, obj, str1));
                        return;
                    }
                    else
                    {
                        TreeNode treeNode1 = new TreeNode();
                        CommonMethods.SetTreeNodeLimitedText(treeNode1, str1);
                        inTreeNode.Nodes.Add(treeNode1);
                        iNodeCount = iNodeCount + 1;
                        return;
                    }
                }
                else
                {
                    Hashtable hashtables = o as Hashtable;
                    if (hashtables.Count > 0)
                    {
                        if (bParentIsArray)
                        {
                            inTreeNode = inTreeNode.Nodes.Add("{}");
                            iNodeCount = iNodeCount + 1;
                        }
                        SortedList sortedLists = new SortedList(hashtables);
                        foreach (DictionaryEntry sortedList in sortedLists)
                        {
                            string str4 = sortedList.Key.ToString();
                            TreeNode treeNode2 = new TreeNode();
                            CommonMethods.SetTreeNodeLimitedText(treeNode2, str4);
                            inTreeNode.Nodes.Add(treeNode2);
                            iNodeCount = iNodeCount + 1;
                            this.ChainNodes(treeNode2, sortedList.Value, false, ref iNodeCount);
                        }
                    }
                    return;
                }
            }
            else
            {
                ArrayList arrayLists = o as ArrayList;
                if (arrayLists.Count > 0)
                {
                    if (bParentIsArray)
                    {
                        inTreeNode = inTreeNode.Nodes.Add("[]");
                        iNodeCount = iNodeCount + 1;
                    }
                    foreach (object obj1 in arrayLists)
                    {
                        this.ChainNodes(inTreeNode, obj1, true, ref iNodeCount);
                    }
                }
                return;
            }
        }

        public void Clear()
        {
            this.BeginUpdate();
            this.tvJSON.Nodes.Clear();
            this.EndUpdate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void EndUpdate()
        {
            this.tvJSON.EndUpdate();
            this.tvJSON.ResumeLayout();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mnuJSONTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCopyNode = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSendNodeToTextWizard = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnCollapseAll = new System.Windows.Forms.Button();
            this.btnExpandAll = new System.Windows.Forms.Button();
            this.tvJSON = new NoToolTipTreeView();
            this.mnuJSONTree.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuJSONTree
            // 
            this.mnuJSONTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCopyNode,
            this.tsmiSendNodeToTextWizard});
            this.mnuJSONTree.Name = "contextMenuStrip1";
            this.mnuJSONTree.Size = new System.Drawing.Size(185, 48);
            this.mnuJSONTree.Opening += new System.ComponentModel.CancelEventHandler(this.mnuJSONTree_Opening);
            // 
            // tsmiCopyNode
            // 
            this.tsmiCopyNode.Name = "tsmiCopyNode";
            this.tsmiCopyNode.Size = new System.Drawing.Size(184, 22);
            this.tsmiCopyNode.Text = "&Copy";
            this.tsmiCopyNode.Click += new System.EventHandler(this.tsmiCopyNode_Click);
            // 
            // tsmiSendNodeToTextWizard
            // 
            this.tsmiSendNodeToTextWizard.Name = "tsmiSendNodeToTextWizard";
            this.tsmiSendNodeToTextWizard.Size = new System.Drawing.Size(184, 22);
            this.tsmiSendNodeToTextWizard.Text = "S&end to TextWizard...";
            this.tsmiSendNodeToTextWizard.Click += new System.EventHandler(this.tsmiSendNodeToTextWizard_Click);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.lblStatus);
            this.pnlFooter.Controls.Add(this.txtSearch);
            this.pnlFooter.Controls.Add(this.btnCollapseAll);
            this.pnlFooter.Controls.Add(this.btnExpandAll);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 272);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(489, 25);
            this.pnlFooter.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.Location = new System.Drawing.Point(162, 3);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(314, 20);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(421, 2);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(65, 21);
            this.txtSearch.TabIndex = 2;
            this.txtSearch.Visible = false;
            // 
            // btnCollapseAll
            // 
            this.btnCollapseAll.Location = new System.Drawing.Point(81, 2);
            this.btnCollapseAll.Name = "btnCollapseAll";
            this.btnCollapseAll.Size = new System.Drawing.Size(75, 23);
            this.btnCollapseAll.TabIndex = 1;
            this.btnCollapseAll.Text = "Collapse";
            this.btnCollapseAll.UseVisualStyleBackColor = true;
            this.btnCollapseAll.Click += new System.EventHandler(this.btnCollapseAll_Click);
            // 
            // btnExpandAll
            // 
            this.btnExpandAll.Location = new System.Drawing.Point(0, 2);
            this.btnExpandAll.Name = "btnExpandAll";
            this.btnExpandAll.Size = new System.Drawing.Size(75, 23);
            this.btnExpandAll.TabIndex = 0;
            this.btnExpandAll.Text = "Expand All";
            this.btnExpandAll.UseVisualStyleBackColor = true;
            this.btnExpandAll.Click += new System.EventHandler(this.btnExpandAll_Click);
            // 
            // tvJSON
            // 
            this.tvJSON.BackColor = System.Drawing.Color.AliceBlue;
            this.tvJSON.ContextMenuStrip = this.mnuJSONTree;
            this.tvJSON.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvJSON.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.tvJSON.Location = new System.Drawing.Point(0, 0);
            this.tvJSON.Name = "tvJSON";
            this.tvJSON.Size = new System.Drawing.Size(489, 272);
            this.tvJSON.TabIndex = 0;
            this.tvJSON.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvXML_KeyDown);
            // 
            // JsonpView
            // 
            this.Controls.Add(this.tvJSON);
            this.Controls.Add(this.pnlFooter);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "JsonpView";
            this.Size = new System.Drawing.Size(489, 297);
            this.mnuJSONTree.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.ResumeLayout(false);

        }

        private void mnuJSONTree_Opening(object sender, CancelEventArgs e)
        {
            Point client = this.tvJSON.PointToClient(Cursor.Position);
            TreeNode nodeAt = this.tvJSON.GetNodeAt(client);
            if (nodeAt != null)
            {
                this.tvJSON.SelectedNode = nodeAt;
            }
            bool selectedNode = null != this.tvJSON.SelectedNode;
            bool flag = selectedNode;
            this.tsmiSendNodeToTextWizard.Enabled = selectedNode;
            this.tsmiCopyNode.Enabled = flag;
        }

        public void SetJSON(string sText)
        {
            JSON.JSONParseErrors jSONParseError = null;
            TreeNode treeNode = new TreeNode("JSON");
            uint num = 0;
            try
            {
                this.Clear();
                int num1 = sText.IndexOf('{');
                int num2 = sText.IndexOf("[");
                int num3 = sText.IndexOf(":");
                int length = sText.Length;
                char chr = '\0';
                if (num1 > -1 && num1 < length)
                {
                    chr = '}';
                    length = num1;
                }
                if (num2 > -1 && num2 < length)
                {
                    chr = ']';
                    length = num2;
                }
                if (num3 <= -1 || num3 >= length)
                {
                    int length1 = sText.Length;
                    if (chr == '}' || chr == ']')
                    {
                        length1 = sText.LastIndexOf(chr) + 1;
                        if (length1 < 1)
                        {
                            this.lblStatus.Text = "The response does not contain valid JSON text; matching end missing.";
                            return;
                        }
                    }
                    sText = sText.Substring(length, length1 - length);
                    object obj = JSON.JsonDecode(sText, out jSONParseError);
                    if (obj != null)
                    {
                        if (string.IsNullOrEmpty(jSONParseError.sWarningText))
                        {
                            this.lblStatus.Text = "JSON parsing completed.";
                        }
                        else
                        {
                            this.lblStatus.Text = string.Format("WARNING: {0}", jSONParseError.sWarningText);
                        }
                        this.ChainNodes(treeNode, obj, false, ref num);
                    }
                    else
                    {
                        this.lblStatus.Text = string.Format("Invalid text at position {0}.", jSONParseError.iErrorIndex);
                        return;
                    }
                }
                else
                {
                    this.lblStatus.Text = "The response does not contain valid JSON text.";
                    return;
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                this.lblStatus.Text = exception.Message;
            }
            try
            {
                this.BeginUpdate();
                this.tvJSON.Nodes.Add(treeNode);
                if (num >= 2000)
                {
                    this.tvJSON.Nodes[0].Expand();
                }
                else
                {
                    this.tvJSON.Nodes[0].ExpandAll();
                }
                this.tvJSON.Nodes[0].EnsureVisible();
                this.EndUpdate();
            }
            finally
            {
                this.EndUpdate();
            }
        }

        private void tsmiCopyNode_Click(object sender, EventArgs e)
        {
            if (this.tvJSON.SelectedNode != null)
            {
                Utilities.CopyToClipboard(CommonMethods.GetTreeNodeFullText(this.tvJSON.SelectedNode));
            }
        }

        private void tsmiSendNodeToTextWizard_Click(object sender, EventArgs e)
        {
            if (this.tvJSON.SelectedNode != null)
            {
                FiddlerApplication.UI.actShowTextWizard(CommonMethods.GetTreeNodeFullText(this.tvJSON.SelectedNode));
            }
        }

        private void tvXML_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.LButton | Keys.RButton | Keys.Cancel | Keys.A | Keys.B | Keys.C | Keys.Control) && this.tvJSON.SelectedNode != null)
            {
                Utilities.CopyToClipboard(CommonMethods.GetTreeNodeFullText(this.tvJSON.SelectedNode));
                bool flag = true;
                bool flag1 = flag;
                e.SuppressKeyPress = flag;
                e.Handled = flag1;
            }
        }
    }
}
