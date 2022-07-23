
namespace Lab4
{
    partial class GameGUI
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
            this.components = new System.ComponentModel.Container();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.controlsGroupBox = new System.Windows.Forms.GroupBox();
            this.genTextBox = new System.Windows.Forms.TextBox();
            this.labelGeneration = new System.Windows.Forms.Label();
            this.resetButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.mainGroupBox = new System.Windows.Forms.GroupBox();
            this.controlsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Interval = 500;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // controlsGroupBox
            // 
            this.controlsGroupBox.Controls.Add(this.genTextBox);
            this.controlsGroupBox.Controls.Add(this.labelGeneration);
            this.controlsGroupBox.Controls.Add(this.resetButton);
            this.controlsGroupBox.Controls.Add(this.pauseButton);
            this.controlsGroupBox.Controls.Add(this.startButton);
            this.controlsGroupBox.Location = new System.Drawing.Point(14, 4);
            this.controlsGroupBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.controlsGroupBox.Name = "controlsGroupBox";
            this.controlsGroupBox.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.controlsGroupBox.Size = new System.Drawing.Size(506, 75);
            this.controlsGroupBox.TabIndex = 0;
            this.controlsGroupBox.TabStop = false;
            // 
            // genTextBox
            // 
            this.genTextBox.Enabled = false;
            this.genTextBox.Location = new System.Drawing.Point(336, 40);
            this.genTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.genTextBox.Name = "genTextBox";
            this.genTextBox.Size = new System.Drawing.Size(131, 26);
            this.genTextBox.TabIndex = 4;
            this.genTextBox.Text = "0";
            // 
            // labelGeneration
            // 
            this.labelGeneration.AutoSize = true;
            this.labelGeneration.Location = new System.Drawing.Point(335, 15);
            this.labelGeneration.Name = "labelGeneration";
            this.labelGeneration.Size = new System.Drawing.Size(136, 20);
            this.labelGeneration.TabIndex = 3;
            this.labelGeneration.Text = "Generation Label:";
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(202, 15);
            this.resetButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(91, 49);
            this.resetButton.TabIndex = 2;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // pauseButton
            // 
            this.pauseButton.Location = new System.Drawing.Point(105, 15);
            this.pauseButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(91, 49);
            this.pauseButton.TabIndex = 1;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(7, 15);
            this.startButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(91, 49);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // mainGroupBox
            // 
            this.mainGroupBox.Location = new System.Drawing.Point(14, 79);
            this.mainGroupBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mainGroupBox.Name = "mainGroupBox";
            this.mainGroupBox.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mainGroupBox.Size = new System.Drawing.Size(506, 562);
            this.mainGroupBox.TabIndex = 1;
            this.mainGroupBox.TabStop = false;
            // 
            // GameGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 636);
            this.Controls.Add(this.mainGroupBox);
            this.Controls.Add(this.controlsGroupBox);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(548, 692);
            this.MinimumSize = new System.Drawing.Size(548, 692);
            this.Name = "GameGUI";
            this.Text = "PRG455 - Game of Life";
            this.Load += new System.EventHandler(this.GameGUI_Load);
            this.controlsGroupBox.ResumeLayout(false);
            this.controlsGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.GroupBox controlsGroupBox;
        private System.Windows.Forms.TextBox genTextBox;
        private System.Windows.Forms.Label labelGeneration;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox mainGroupBox;
    }
}

