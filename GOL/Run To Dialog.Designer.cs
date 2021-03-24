
namespace GOL
{
    partial class Run_To_Dialog
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
            this.labelRunToGen = new System.Windows.Forms.Label();
            this.numericUpDownGen = new System.Windows.Forms.NumericUpDown();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGen)).BeginInit();
            this.SuspendLayout();
            // 
            // labelRunToGen
            // 
            this.labelRunToGen.AutoSize = true;
            this.labelRunToGen.Location = new System.Drawing.Point(41, 26);
            this.labelRunToGen.Name = "labelRunToGen";
            this.labelRunToGen.Size = new System.Drawing.Size(92, 13);
            this.labelRunToGen.TabIndex = 0;
            this.labelRunToGen.Text = "Run to generation";
            // 
            // numericUpDownGen
            // 
            this.numericUpDownGen.Location = new System.Drawing.Point(139, 24);
            this.numericUpDownGen.Name = "numericUpDownGen";
            this.numericUpDownGen.Size = new System.Drawing.Size(92, 20);
            this.numericUpDownGen.TabIndex = 1;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(57, 65);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(139, 65);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // Run_To_Dialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 100);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.numericUpDownGen);
            this.Controls.Add(this.labelRunToGen);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Run_To_Dialog";
            this.Text = "Run To";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelRunToGen;
        private System.Windows.Forms.NumericUpDown numericUpDownGen;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
    }
}