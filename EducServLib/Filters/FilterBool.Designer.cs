namespace EducServLib
{
    partial class FilterBool
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
            this.rbNo = new System.Windows.Forms.RadioButton();
            this.rbYEs = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // rbNo
            // 
            this.rbNo.AutoSize = true;
            this.rbNo.Location = new System.Drawing.Point(82, 3);
            this.rbNo.Name = "rbNo";
            this.rbNo.Size = new System.Drawing.Size(44, 17);
            this.rbNo.TabIndex = 2;
            this.rbNo.Text = "Нет";
            this.rbNo.UseVisualStyleBackColor = true;
            // 
            // rbYEs
            // 
            this.rbYEs.AutoSize = true;
            this.rbYEs.Checked = true;
            this.rbYEs.Location = new System.Drawing.Point(3, 3);
            this.rbYEs.Name = "rbYEs";
            this.rbYEs.Size = new System.Drawing.Size(40, 17);
            this.rbYEs.TabIndex = 1;
            this.rbYEs.TabStop = true;
            this.rbYEs.Text = "Да";
            this.rbYEs.UseVisualStyleBackColor = true;
            // 
            // FilterBool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rbNo);
            this.Controls.Add(this.rbYEs);
            this.Name = "FilterBool";
            this.Size = new System.Drawing.Size(124, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbNo;
        private System.Windows.Forms.RadioButton rbYEs;
    }
}
