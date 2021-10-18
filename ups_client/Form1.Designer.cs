
namespace ups_client
{
    partial class Form1
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
            this.playerNameLabel = new System.Windows.Forms.Label();
            this.playerStonePanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.opponentStonePanel = new System.Windows.Forms.Panel();
            this.opponentNameLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.clearSelectionBtn = new System.Windows.Forms.Button();
            this.playingNameLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.winnerNameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // playerNameLabel
            // 
            this.playerNameLabel.AutoSize = true;
            this.playerNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.playerNameLabel.Location = new System.Drawing.Point(688, 28);
            this.playerNameLabel.Name = "playerNameLabel";
            this.playerNameLabel.Size = new System.Drawing.Size(26, 33);
            this.playerNameLabel.TabIndex = 1;
            this.playerNameLabel.Text = "-";
            // 
            // playerStonePanel
            // 
            this.playerStonePanel.BackColor = System.Drawing.SystemColors.Control;
            this.playerStonePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.playerStonePanel.Location = new System.Drawing.Point(612, 12);
            this.playerStonePanel.Name = "playerStonePanel";
            this.playerStonePanel.Size = new System.Drawing.Size(70, 70);
            this.playerStonePanel.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(704, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 73);
            this.label1.TabIndex = 3;
            this.label1.Text = "VS";
            // 
            // opponentStonePanel
            // 
            this.opponentStonePanel.BackColor = System.Drawing.SystemColors.Control;
            this.opponentStonePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.opponentStonePanel.Location = new System.Drawing.Point(612, 183);
            this.opponentStonePanel.Name = "opponentStonePanel";
            this.opponentStonePanel.Size = new System.Drawing.Size(70, 70);
            this.opponentStonePanel.TabIndex = 4;
            // 
            // opponentNameLabel
            // 
            this.opponentNameLabel.AutoSize = true;
            this.opponentNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.opponentNameLabel.Location = new System.Drawing.Point(688, 204);
            this.opponentNameLabel.Name = "opponentNameLabel";
            this.opponentNameLabel.Size = new System.Drawing.Size(26, 33);
            this.opponentNameLabel.TabIndex = 5;
            this.opponentNameLabel.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(606, 290);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 33);
            this.label2.TabIndex = 6;
            this.label2.Text = "Hraje:";
            // 
            // clearSelectionBtn
            // 
            this.clearSelectionBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.clearSelectionBtn.Location = new System.Drawing.Point(612, 487);
            this.clearSelectionBtn.Name = "clearSelectionBtn";
            this.clearSelectionBtn.Size = new System.Drawing.Size(299, 85);
            this.clearSelectionBtn.TabIndex = 7;
            this.clearSelectionBtn.Text = "Zrušit výběr";
            this.clearSelectionBtn.UseVisualStyleBackColor = true;
            this.clearSelectionBtn.Click += new System.EventHandler(this.clearSelectionBtn_Click);
            // 
            // playingNameLabel
            // 
            this.playingNameLabel.AutoSize = true;
            this.playingNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.playingNameLabel.Location = new System.Drawing.Point(711, 290);
            this.playingNameLabel.Name = "playingNameLabel";
            this.playingNameLabel.Size = new System.Drawing.Size(26, 33);
            this.playingNameLabel.TabIndex = 8;
            this.playingNameLabel.Text = "-";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(606, 349);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 33);
            this.label3.TabIndex = 9;
            this.label3.Text = "Vítěz:";
            // 
            // winnerNameLabel
            // 
            this.winnerNameLabel.AutoSize = true;
            this.winnerNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.winnerNameLabel.Location = new System.Drawing.Point(711, 349);
            this.winnerNameLabel.Name = "winnerNameLabel";
            this.winnerNameLabel.Size = new System.Drawing.Size(26, 33);
            this.winnerNameLabel.TabIndex = 10;
            this.winnerNameLabel.Text = "-";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 592);
            this.Controls.Add(this.winnerNameLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.playingNameLabel);
            this.Controls.Add(this.clearSelectionBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.opponentNameLabel);
            this.Controls.Add(this.opponentStonePanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.playerStonePanel);
            this.Controls.Add(this.playerNameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dáma";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label playerNameLabel;
        private System.Windows.Forms.Panel playerStonePanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel opponentStonePanel;
        private System.Windows.Forms.Label opponentNameLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button clearSelectionBtn;
        private System.Windows.Forms.Label playingNameLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label winnerNameLabel;
    }
}

