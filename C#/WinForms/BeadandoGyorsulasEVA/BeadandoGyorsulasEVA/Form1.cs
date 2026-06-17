using System.Reflection;
using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using Gyorsulás.Model; //Import the stuff from model
using System.Timers;
using Gyorsulás.Persistence;

namespace BeadandoGyorsulasEVA
{
    public partial class Form1 : Form
    {

        private Game mainGame; //This contains all the data needed for the logic of the game


        public Form1()
        {
            InitializeComponent();
            this.ClientSize = new Size(500, 1000);
            this.DoubleBuffered = true; //Better drawing quality
            this.KeyPreview = true; //For checking the keys


            labelTime.BringToFront();
            labelFuel.BringToFront();
            labelScore.BringToFront();


            mainGame = new Game(); //Make a new game




            mainGame.GameUpdated += OnGameUpdated; //When Game updates the game, thiss will run and the form will react to it
            mainGame.mainTimer.Start(); //Start clock

            pictureBox.Paint += MiddleLinePaint; //bg paint
            pictureBox.Paint += FuelPaint; //fuel box (the pink one)
            pictureBox.Paint += MotorPaint; //motor boc (purple)

            this.KeyUp += ButtonReleased; //Key released duh
            this.KeyDown += ButtonPressedDown; //Key pressed down duh
            

        }

        
        private void OnGameUpdated(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object?, EventArgs>(OnGameUpdated), sender, e);
                return;
            }

            pictureBox.Invalidate();
            labelFuel.Text = $"Fuel:\n{mainGame.mainMotor._fuel,4:0.0}";
            labelScore.Text = $"Score:\n{mainGame.score}";
            labelTime.Text = $"Time:\n{mainGame.elapsedTime:F1} seconds";
        }
        private void ButtonPressedDown(object? sender, KeyEventArgs e) //KeyEventArgs bcs we are interested in the actions of keys
        {
            //Escape menu check
            if (e.KeyCode == Keys.Escape && mainGame.isPaused == false)
            {
                mainGame.Pause();
                mainGame.mainTimer.Stop();

                //Menu visibility

                buttonExit.Visible = true;
                buttonLoad.Visible = true;
                buttonNewGame.Visible = true;
                buttonSave.Visible = true;
                boxMenu.Visible = true;
            }
            else if (e.KeyCode == Keys.Escape && mainGame.isPaused == true)
            {
                mainGame.Resume();

                //Restart the timer pls
                mainGame.mainTimer.Start();
                //Menu visibility
                boxMenu.Visible = false;
                buttonExit.Visible = false;
                buttonLoad.Visible = false;
                buttonNewGame.Visible = false;
                buttonSave.Visible = false;
            }
            //Moving switch case
            switch (e.KeyCode)
            {
                case Keys.Left:
                    mainGame.mainDirection = WhereToMove.Left;
                    break;
                case Keys.Right:
                    mainGame.mainDirection = WhereToMove.Right;
                    break;
                default:
                    mainGame.mainDirection = WhereToMove.Stay;
                    break;
            }
        }

        private void ButtonReleased(object? sender, EventArgs e)
        {
            mainGame.mainDirection = WhereToMove.Stay;
        }
        private void MotorPaint(object? sender, PaintEventArgs e)//PaintEventArgs bcs we are Bob Ross hellyea
        {
            Graphics g = e.Graphics;


            float x = (float)(mainGame.mainMotor._posX * pictureBox.Width);
            float y = (float)(mainGame.mainMotor._posY * pictureBox.Height);
            float width = (float)(mainGame.mainMotor._width * pictureBox.Width);
            float height = (float)(mainGame.mainMotor._height * pictureBox.Height);

            g.FillRectangle(Brushes.Purple, x, y, width, height);
        }

        private void MiddleLinePaint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            float x = (float)(mainGame.mainMap._posX * pictureBox.Width);
            float y = (float)(mainGame.mainMap._posY * pictureBox.Height);
            float width = (float)(mainGame.mainMap._width * pictureBox.Width);
            float height = (float)(mainGame.mainMap._height * pictureBox.Height);

            g.FillRectangle(Brushes.White, x, y, width, height);
        }

        private void FuelPaint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            float x = (float)(mainGame.mainRoadFuel._posX * pictureBox.Width);
            float y = (float)(mainGame.mainRoadFuel._posY * pictureBox.Height);
            float width = (float)(mainGame.mainRoadFuel._width * pictureBox.Width);
            float height = (float)(mainGame.mainRoadFuel._height * pictureBox.Height);


            g.FillRectangle(Brushes.Goldenrod, x, y, width, height);
        }

        private void buttonNewGame_Click(object sender, EventArgs e)
        {
            mainGame.Newgame();
            this.Invalidate();
            //Menu visibility
            boxMenu.Visible = false;
            buttonExit.Visible = false;
            buttonLoad.Visible = false;
            buttonNewGame.Visible = false;
            buttonSave.Visible = false;
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void buttonSave_Click_1(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = "C:\\";
                saveFileDialog.Filter = "Text files (*.txt)|*.txt";
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.Title = "Saving";
                saveFileDialog.FileName = "Save.txt";
                saveFileDialog.DefaultExt = "txt";
                saveFileDialog.OverwritePrompt = true;
                saveFileDialog.AddExtension = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    IFileManager fileManager = new FileManager(saveFileDialog.FileName);
                    try
                    {
                        mainGame.Save(fileManager);
                    }
                    catch (FileManagerException ex)
                    {
                        MessageBox.Show("There was a problem during saving >~<" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

            }


        }
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog loadSearch = new OpenFileDialog())
            {
                loadSearch.InitialDirectory = "C:\\";
                loadSearch.Filter = "Text files (*.txt)|*.txt";
                loadSearch.RestoreDirectory = true;
                loadSearch.Title = "Load window";
                loadSearch.CheckFileExists = true;
                loadSearch.CheckPathExists = true;
                
                if (loadSearch.ShowDialog() == DialogResult.OK)
                {
                    IFileManager fileManager = new FileManager(loadSearch.FileName);
                    try
                    {
                        mainGame.LoadGame(fileManager);
                    }
                    catch (FileManagerException ex)
                    {
                      MessageBox.Show("There was a problem during loading >~<" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                      return;
                    }
                }

            }
        }
    }
}

