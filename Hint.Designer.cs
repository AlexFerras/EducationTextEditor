namespace Text_Editor
{
    partial class Hint
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
            label1 = new CustomLabel();
            SuspendLayout();
            // 
            // label1
            // 
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            label1.Location = new System.Drawing.Point(24, 54);
            label1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(125, 70);
            label1.TabIndex = 0;
            label1.Text = "AdA\r\nDADAD\r\nDADADA\r\n";
            label1.Click += label1_Click;
            // 
            // Hint
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.LightGray;
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            Controls.Add(label1);
            ForeColor = System.Drawing.Color.White;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "Hint";
            Size = new System.Drawing.Size(244, 171);
            ResumeLayout(false);
        }

        #endregion

        private CustomLabel label1;
    }
}
