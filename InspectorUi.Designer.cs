namespace JsonpViewer
{
    partial class InspectorUi
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._text = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _text
            // 
            this._text.Location = new System.Drawing.Point(0, 0);
            this._text.Multiline = true;
            this._text.Name = "_text";
            this._text.ReadOnly = true;
            this._text.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._text.Size = new System.Drawing.Size(147, 147);
            this._text.TabIndex = 0;
            // 
            // InspectorUi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._text);
            this.Name = "InspectorUi";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _text;
    }
}
