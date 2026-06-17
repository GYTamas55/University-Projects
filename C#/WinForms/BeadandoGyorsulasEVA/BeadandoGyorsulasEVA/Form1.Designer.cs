namespace BeadandoGyorsulasEVA
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox = new PictureBox();
            labelFuel = new Label();
            labelScore = new Label();
            labelTime = new Label();
            boxMenu = new PictureBox();
            buttonNewGame = new Button();
            buttonExit = new Button();
            buttonSave = new Button();
            buttonLoad = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)boxMenu).BeginInit();
            SuspendLayout();
            // 
            // pictureBox
            // 
            pictureBox.BackColor = SystemColors.AppWorkspace;
            pictureBox.BorderStyle = BorderStyle.Fixed3D;
            pictureBox.Dock = DockStyle.Bottom;
            pictureBox.Location = new Point(0, -100);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(864, 861);
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            // 
            // labelFuel
            // 
            labelFuel.Location = new Point(225, 45);
            labelFuel.Name = "labelFuel";
            labelFuel.Size = new Size(102, 39);
            labelFuel.TabIndex = 3;
            labelFuel.Text = "Fuel Label";
            // 
            // labelScore
            // 
            labelScore.Location = new Point(360, 45);
            labelScore.Name = "labelScore";
            labelScore.Size = new Size(96, 39);
            labelScore.TabIndex = 4;
            labelScore.Text = "Score Label";
            // 
            // labelTime
            // 
            labelTime.Location = new Point(65, 45);
            labelTime.Name = "labelTime";
            labelTime.Size = new Size(127, 39);
            labelTime.TabIndex = 5;
            labelTime.Text = "Time Label";
            // 
            // boxMenu
            // 
            boxMenu.Location = new Point(86, 334);
            boxMenu.Name = "boxMenu";
            boxMenu.Size = new Size(305, 318);
            boxMenu.TabIndex = 6;
            boxMenu.TabStop = false;
            boxMenu.Visible = false;
            // 
            // buttonNewGame
            // 
            buttonNewGame.Location = new Point(127, 355);
            buttonNewGame.Name = "buttonNewGame";
            buttonNewGame.Size = new Size(239, 67);
            buttonNewGame.TabIndex = 7;
            buttonNewGame.Text = "New Game";
            buttonNewGame.UseVisualStyleBackColor = true;
            buttonNewGame.Visible = false;
            buttonNewGame.Click += buttonNewGame_Click;
            // 
            // buttonExit
            // 
            buttonExit.Location = new Point(127, 428);
            buttonExit.Name = "buttonExit";
            buttonExit.Size = new Size(239, 67);
            buttonExit.TabIndex = 8;
            buttonExit.Text = "Exit";
            buttonExit.UseVisualStyleBackColor = true;
            buttonExit.Visible = false;
            buttonExit.Click += buttonExit_Click;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(127, 501);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(239, 67);
            buttonSave.TabIndex = 9;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Visible = false;
            buttonSave.Click += buttonSave_Click_1;
            // 
            // buttonLoad
            // 
            buttonLoad.Location = new Point(127, 574);
            buttonLoad.Name = "buttonLoad";
            buttonLoad.Size = new Size(239, 67);
            buttonLoad.TabIndex = 10;
            buttonLoad.Text = "Load";
            buttonLoad.UseVisualStyleBackColor = true;
            buttonLoad.Visible = false;
            buttonLoad.Click += buttonLoad_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(864, 761);
            Controls.Add(buttonLoad);
            Controls.Add(buttonSave);
            Controls.Add(buttonExit);
            Controls.Add(buttonNewGame);
            Controls.Add(boxMenu);
            Controls.Add(labelTime);
            Controls.Add(labelScore);
            Controls.Add(labelFuel);
            Controls.Add(pictureBox);
            Location = new Point(50, 50);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)boxMenu).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox;
        private Label labelFuel;
        private Label labelScore;
        private Label labelTime;
        private PictureBox boxMenu;
        private Button buttonNewGame;
        private Button buttonExit;
        private Button buttonSave;
        private Button buttonLoad;
    }
}
