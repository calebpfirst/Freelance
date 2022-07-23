using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4
{
    public partial class GameGUI : Form
    {
        private const int GRID_WIDTH = 450;
        private const int BUTTON_WIDTH = 25;
        private const int CELLS = GRID_WIDTH / BUTTON_WIDTH;
        private GameEngine engine = null;

        public GameGUI()
        {
            InitializeComponent();

            engine = new GameEngine(CELLS, CELLS);
        }

        private void GameGUI_Load(object sender, EventArgs e)
        {
            for (var j = 0; j + BUTTON_WIDTH <= GRID_WIDTH; j += BUTTON_WIDTH)
                for (var i = 0; i + BUTTON_WIDTH <= GRID_WIDTH; i += BUTTON_WIDTH)
                {
                    Button newButton = new Button();
                    newButton.Size = new Size(BUTTON_WIDTH, BUTTON_WIDTH);
                    newButton.Location = new Point(i, j);
                    newButton.Click += newButton_Click;
                    mainGroupBox.Controls.Add(newButton);
                }
            UpdateColours();
        }

        private void UpdateColours()
        {
            for (var i = 0; i < mainGroupBox.Controls.Count; i++)
            {
                var y = i / engine.Width;
                var x = i % engine.Width;

                if (engine[y, x] == 0)
                    mainGroupBox.Controls[i].BackColor = Color.LightGray;
                else if (engine[y, x] == 1)
                    mainGroupBox.Controls[i].BackColor = Color.Blue;
            }
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            if (timer.Enabled == true)
                return;

            var i = mainGroupBox.Controls.IndexOf(sender as Button);
            var y = i / engine.Width;
            var x = i % engine.Width;

            if (engine[y, x] == 0)
            {
                engine[y, x] = 1;
                mainGroupBox.Controls[i].BackColor = Color.LightGray;
            }
            else if (engine[y, x] == 1)
            {
                engine[y, x] = 0;
                mainGroupBox.Controls[i].BackColor = Color.Blue;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            engine.Tick();
            UpdateColours();
            genTextBox.Text = engine.Ticks.ToString();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
            startButton.Enabled = false;
            pauseButton.Enabled = true;
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            pauseButton.Enabled = false;
            startButton.Enabled = true;
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            pauseButton.Enabled = true;
            startButton.Enabled = true;

            engine = new GameEngine(engine.Width, engine.Height);
            genTextBox.Text = engine.Ticks.ToString();

            UpdateColours();
        }
    }
}