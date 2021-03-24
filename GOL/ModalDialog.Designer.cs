
namespace GOL
{
    partial class ModalOptions
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.numericUpDownTimerInterval = new System.Windows.Forms.NumericUpDown();
            this.labelTimerInterval = new System.Windows.Forms.Label();
            this.labelUniverseWidth = new System.Windows.Forms.Label();
            this.labelUniverseHeight = new System.Windows.Forms.Label();
            this.numericUpDownUniverseHeight = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownUniverseWidth = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimerInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownUniverseHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownUniverseWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(5, 195);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(86, 195);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // numericUpDownTimerInterval
            // 
            this.numericUpDownTimerInterval.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownTimerInterval.Location = new System.Drawing.Point(228, 68);
            this.numericUpDownTimerInterval.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.numericUpDownTimerInterval.Name = "numericUpDownTimerInterval";
            this.numericUpDownTimerInterval.Size = new System.Drawing.Size(75, 20);
            this.numericUpDownTimerInterval.TabIndex = 2;
            // 
            // labelTimerInterval
            // 
            this.labelTimerInterval.AutoSize = true;
            this.labelTimerInterval.Location = new System.Drawing.Point(83, 70);
            this.labelTimerInterval.Name = "labelTimerInterval";
            this.labelTimerInterval.Size = new System.Drawing.Size(140, 13);
            this.labelTimerInterval.TabIndex = 3;
            this.labelTimerInterval.Text = "Timer Interval in Miliseconds";
            // 
            // labelUniverseWidth
            // 
            this.labelUniverseWidth.AutoSize = true;
            this.labelUniverseWidth.Location = new System.Drawing.Point(83, 137);
            this.labelUniverseWidth.Name = "labelUniverseWidth";
            this.labelUniverseWidth.Size = new System.Drawing.Size(128, 13);
            this.labelUniverseWidth.TabIndex = 4;
            this.labelUniverseWidth.Text = "Width of Universe in Cells";
            // 
            // labelUniverseHeight
            // 
            this.labelUniverseHeight.AutoSize = true;
            this.labelUniverseHeight.Location = new System.Drawing.Point(83, 103);
            this.labelUniverseHeight.Name = "labelUniverseHeight";
            this.labelUniverseHeight.Size = new System.Drawing.Size(131, 13);
            this.labelUniverseHeight.TabIndex = 5;
            this.labelUniverseHeight.Text = "Height of Universe in Cells";
            // 
            // numericUpDownUniverseHeight
            // 
            this.numericUpDownUniverseHeight.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownUniverseHeight.Location = new System.Drawing.Point(228, 101);
            this.numericUpDownUniverseHeight.Name = "numericUpDownUniverseHeight";
            this.numericUpDownUniverseHeight.Size = new System.Drawing.Size(75, 20);
            this.numericUpDownUniverseHeight.TabIndex = 6;
            // 
            // numericUpDownUniverseWidth
            // 
            this.numericUpDownUniverseWidth.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownUniverseWidth.Location = new System.Drawing.Point(228, 135);
            this.numericUpDownUniverseWidth.Name = "numericUpDownUniverseWidth";
            this.numericUpDownUniverseWidth.Size = new System.Drawing.Size(75, 20);
            this.numericUpDownUniverseWidth.TabIndex = 7;
            // 
            // ModalOptions
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(381, 226);
            this.Controls.Add(this.numericUpDownUniverseWidth);
            this.Controls.Add(this.numericUpDownUniverseHeight);
            this.Controls.Add(this.labelUniverseHeight);
            this.Controls.Add(this.labelUniverseWidth);
            this.Controls.Add(this.labelTimerInterval);
            this.Controls.Add(this.numericUpDownTimerInterval);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModalOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimerInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownUniverseHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownUniverseWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.NumericUpDown numericUpDownTimerInterval;
        private System.Windows.Forms.Label labelTimerInterval;
        private System.Windows.Forms.Label labelUniverseWidth;
        private System.Windows.Forms.Label labelUniverseHeight;
        private System.Windows.Forms.NumericUpDown numericUpDownUniverseHeight;
        private System.Windows.Forms.NumericUpDown numericUpDownUniverseWidth;
    }
}