namespace SolitaireSolver
{
    partial class FrmSolitaireGUI
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
            this.btnNextMove = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnNextMove
            // 
            this.btnNextMove.Location = new System.Drawing.Point(12, 266);
            this.btnNextMove.Name = "btnNextMove";
            this.btnNextMove.Size = new System.Drawing.Size(129, 23);
            this.btnNextMove.TabIndex = 0;
            this.btnNextMove.Text = "Start";
            this.btnNextMove.UseVisualStyleBackColor = true;
            this.btnNextMove.Click += new System.EventHandler(this.btnNextMove_Click);
            // 
            // FrmSolitaireGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnNextMove);
            this.Name = "FrmSolitaireGUI";
            this.Text = "FrmSolitaireGUI";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnNextMove;
    }
}