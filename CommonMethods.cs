using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Fiddler;

namespace JsonpViewer
{
    internal class CommonMethods
    {
        internal static void DoRichTextSearch(RichTextBox rtbSearch, string sSearchFor, bool bHighlightAll)
        {
            if (rtbSearch.TextLength >= 1)
            {
                if (!bHighlightAll)
                {
                    int num = Math.Min(rtbSearch.SelectionStart + 1, rtbSearch.TextLength);
                    rtbSearch.Find(sSearchFor, num, RichTextBoxFinds.None);
                }
                else
                {
                    try
                    {
                        CommonMethods.LockWindowUpdate(rtbSearch.Handle);
                        rtbSearch.SelectionStart = 0;
                        rtbSearch.SelectionLength = rtbSearch.TextLength;
                        rtbSearch.SelectionBackColor = rtbSearch.BackColor;
                        rtbSearch.SelectionLength = 0;
                        if (!string.IsNullOrEmpty(sSearchFor))
                        {
                            int num1 = rtbSearch.Find(sSearchFor, 0, RichTextBoxFinds.None);
                            int num2 = num1;
                            while (num1 > -1)
                            {
                                rtbSearch.SelectionBackColor = Color.Yellow;
                                num1++;
                                if (num1 >= rtbSearch.TextLength)
                                {
                                    break;
                                }
                                num1 = rtbSearch.Find(sSearchFor, num1, RichTextBoxFinds.None);
                            }
                            if (num2 > -1)
                            {
                                rtbSearch.SelectionLength = 0;
                                rtbSearch.SelectionStart = num2;
                            }
                        }
                    }
                    finally
                    {
                        CommonMethods.LockWindowUpdate(IntPtr.Zero);
                    }
                }
                return;
            }
            else
            {
                return;
            }
        }

        internal static int GetLineCountForTextBox(TextBoxBase txtBox)
        {
            return (int)CommonMethods.SendMessage(txtBox.Handle, 186, 0, 0);
        }

        internal static string GetTreeNodeFullText(TreeNode oTN)
        {
            if (oTN.Tag != null)
            {
                return oTN.Tag as string;
            }
            else
            {
                return oTN.Text;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.None)]
        private static extern bool LockWindowUpdate(IntPtr hWndLock);

        internal static void ScrollDown(RichTextBox rtb)
        {
            CommonMethods.SendMessage(rtb.Handle, 277, 1, 0);
        }

        internal static void ScrollUp(RichTextBox rtb)
        {
            CommonMethods.SendMessage(rtb.Handle, 277, 0, 0);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        internal static void SetTreeNodeLimitedText(TreeNode oTN, string sText)
        {
            string str = sText;
            if (str.Length > 259)
            {
                str = string.Concat(Utilities.TrimTo(str, 258), (char)8230);
            }
            oTN.Text = str;
            if (str != sText)
            {
                oTN.Tag = sText;
            }
        }
    }
}
