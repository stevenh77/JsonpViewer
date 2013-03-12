using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Fiddler;

namespace JsonpViewer
{
    internal static class InspectorUtils
    {
        internal static void OpenTextEditor(string sFilename, byte[] arrData)
        {
            InspectorUtils.OpenTextEditor(sFilename, null, arrData);
        }

        internal static void OpenTextEditor(string sFilename, HTTPHeaders oH, byte[] arrData)
        {
            try
            {
                string path = CONFIG.GetPath("TextEditor");
                InspectorUtils.WriteFile(sFilename, oH, arrData);
                Process process = Process.Start(path, string.Concat((char)34, sFilename, (char)34));
                using (process)
                {
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                MessageBox.Show(string.Concat(exception.Message, "\n", sFilename), "TextEditor Failed");
            }
        }

        internal static void OpenWith(string sFilename, byte[] arrData)
        {
            try
            {
                InspectorUtils.WriteFile(sFilename, null, arrData);
                Utilities.DoOpenFileWith(sFilename);
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                MessageBox.Show(string.Concat(exception.Message, "\n", sFilename), "Editor Failed");
            }
        }

        private static void WriteFile(string sFilename, HTTPHeaders oH, byte[] arrData)
        {
            FileStream fileStream = new FileStream(sFilename, FileMode.Create, FileAccess.Write);
            if (oH != null)
            {
                HTTPRequestHeaders hTTPRequestHeader = oH as HTTPRequestHeaders;
                if (hTTPRequestHeader == null)
                {
                    HTTPResponseHeaders hTTPResponseHeader = oH as HTTPResponseHeaders;
                    if (hTTPResponseHeader != null)
                    {
                        byte[] byteArray = hTTPResponseHeader.ToByteArray(true, true);
                        fileStream.Write(byteArray, 0, (int)byteArray.Length);
                    }
                }
                else
                {
                    byte[] numArray = hTTPRequestHeader.ToByteArray(true, true, true);
                    fileStream.Write(numArray, 0, (int)numArray.Length);
                }
            }
            if (arrData != null)
            {
                fileStream.Write(arrData, 0, (int)arrData.Length);
            }
            fileStream.Close();
        }
    }
}
