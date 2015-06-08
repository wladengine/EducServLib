namespace EducServLib
{
    partial class FilterMultySelect
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
            this.btnRightAll = new System.Windows.Forms.Button();
            this.btnLeftAll = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.lblExclude = new System.Windows.Forms.Label();
            this.lblInclude = new System.Windows.Forms.Label();
            this.lbNo = new System.Windows.Forms.ListBox();
            this.lbYes = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnRightAll
            // 
            this.btnRightAll.Location = new System.Drawing.Point(207, 161);
            this.btnRightAll.Name = "btnRightAll";
            this.btnRightAll.Size = new System.Drawing.Size(33, 23);
            this.btnRightAll.TabIndex = 7;
            this.btnRightAll.Text = ">>";
            this.btnRightAll.UseVisualStyleBackColor = true;
            this.btnRightAll.Click += new System.EventHandler(this.btnRightAll_Click);
            // 
            // btnLeftAll
            // 
            this.btnLeftAll.Location = new System.Drawing.Point(172, 123);
            this.btnLeftAll.Name = "btnLeftAll";
            this.btnLeftAll.Size = new System.Drawing.Size(33, 23);
            this.btnLeftAll.TabIndex = 8;
            this.btnLeftAll.Text = "<<";
            this.btnLeftAll.UseVisualStyleBackColor = true;
            this.btnLeftAll.Click += new System.EventHandler(this.btnLeftAll_Click);
            // 
            // btnRight
            // 
            this.btnRight.Location = new System.Drawing.Point(207, 67);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(33, 23);
            this.btnRight.TabIndex = 9;
            this.btnRight.Text = ">";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.Location = new System.Drawing.Point(172, 29);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(33, 23);
            this.btnLeft.TabIndex = 10;
            this.btnLeft.Text = "<";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // lblExclude
            // 
            this.lblExclude.AutoSize = true;
            this.lblExclude.Location = new System.Drawing.Point(293, 2);
            this.lblExclude.Name = "lblExclude";
            this.lblExclude.Size = new System.Drawing.Size(71, 13);
            this.lblExclude.TabIndex = 6;
            this.lblExclude.Text = "Не выбрано:";
            // 
            // lblInclude
            // 
            this.lblInclude.AutoSize = true;
            this.lblInclude.Location = new System.Drawing.Point(45, 2);
            this.lblInclude.Name = "lblInclude";
            this.lblInclude.Size = new System.Drawing.Size(55, 13);
            this.lblInclude.TabIndex = 5;
            this.lblInclude.Text = "Выбрано:";
            // 
            // lbNo
            // 
            this.lbNo.FormattingEnabled = true;
            this.lbNo.HorizontalScrollbar = true;
            this.lbNo.Location = new System.Drawing.Point(246, 18);
            this.lbNo.Name = "lbNo";
            this.lbNo.Size = new System.Drawing.Size(163, 186);
            this.lbNo.Sorted = true;
            this.lbNo.TabIndex = 3;
            // 
            // lbYes
            // 
            this.lbYes.FormattingEnabled = true;
            this.lbYes.HorizontalScrollbar = true;
            this.lbYes.Location = new System.Drawing.Point(3, 18);
            this.lbYes.Name = "lbYes";
            this.lbYes.Size = new System.Drawing.Size(163, 186);
            this.lbYes.Sorted = true;
            this.lbYes.TabIndex = 4;
            // 
            // FilterMultySelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRightAll);
            this.Controls.Add(this.btnLeftAll);
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.lblExclude);
            this.Controls.Add(this.lblInclude);
            this.Controls.Add(this.lbNo);
            this.Controls.Add(this.lbYes);
            this.Name = "FilterMultySelect";
            this.Size = new System.Drawing.Size(412, 206);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRightAll;
        private System.Windows.Forms.Button btnLeftAll;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Label lblExclude;
        private System.Windows.Forms.Label lblInclude;
        private System.Windows.Forms.ListBox lbNo;
        private System.Windows.Forms.ListBox lbYes;
    }
}
