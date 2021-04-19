using System;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper_Game
{
    public partial class Form1 : Form
    {
        private Minesweeper.Game game;
        private FormEspecialState formEspecialState;

        public Form1()
        {
            InitializeComponent();
            //this.AutoSize = true;
            //this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            CreateGame(Minesweeper.DifficultyGame.Basic);
        }

        public void CreateGame(Minesweeper.DifficultyGame difficulty)
        {
            if (game != null)
            {
                game.StopGame();
                this.Controls.Remove(game);
            }
            game = new Minesweeper.Game(difficulty);
            game.Name = "Game";
            game.Location = new Point(2, 27);
            this.Width = (game.Width + 21);
            this.Height = (game.Height + 69);
            this.game.GameWin += Game_Win;
            this.game.GameOver += Game_Over;
            this.Controls.Add(game);
        }

        public void CreateGame(int NumberTilesInRow, int NumberTilesInColumn, int NumberMines)
        {
            if (game != null)
            {
                game.StopGame();
                this.Controls.Remove(game);
            }
            game = new Minesweeper.Game(NumberTilesInRow,NumberTilesInColumn,NumberMines);
            game.Name = "Game";
            game.Location = new Point(2, 27);
            this.Width = (game.Width + 21);
            this.Height = (game.Height + 69);
            this.game.GameWin += Game_Win;
            this.game.GameOver += Game_Over;
            this.Controls.Add(game);
        }

        // Обробник події GameWin.
        private void Game_Win(object sender, EventArgs e)
        {
            // Рекорди записуються тільки для Basic, Intermediate та Advanced. 
            WinForm winForm;
            if ((game.NumberTilesInColumn == 10) && (game.NumberTilesInRow == 10) && (game.NumberMines == 12))
            {
                if (game.Timing < Properties.Settings.Default.BasicTime1)
                {
                    winForm = new WinForm(Minesweeper.DifficultyGame.Basic, 1, game.Timing);
                    winForm.ShowDialog();
                    return;
                }
                if (game.Timing < Properties.Settings.Default.BasicTime2)
                {
                    winForm = new WinForm(Minesweeper.DifficultyGame.Basic, 2, game.Timing);
                    winForm.ShowDialog();
                    return;
                }
                if (game.Timing < Properties.Settings.Default.BasicTime3)
                {
                    winForm = new WinForm(Minesweeper.DifficultyGame.Basic, 3, game.Timing);
                    winForm.ShowDialog();
                    return;
                }
            }
            if ((game.NumberTilesInColumn == 16) && (game.NumberTilesInRow == 16) && (game.NumberMines == 40))
            {
                if (game.Timing < Properties.Settings.Default.IntermediateTime1)
                {
                    winForm = new WinForm(Minesweeper.DifficultyGame.Intermediate, 1, game.Timing);
                    winForm.ShowDialog();
                    return;
                }
                if (game.Timing < Properties.Settings.Default.IntermediateTime2)
                {
                    winForm = new WinForm(Minesweeper.DifficultyGame.Intermediate, 2, game.Timing);
                    winForm.ShowDialog();
                    return;
                }
                if (game.Timing < Properties.Settings.Default.IntermediateTime3)
                {
                    winForm = new WinForm(Minesweeper.DifficultyGame.Intermediate, 3, game.Timing);
                    winForm.ShowDialog();
                    return;
                }
            }
            if ((game.NumberTilesInColumn == 16) && (game.NumberTilesInRow == 30) && (game.NumberMines == 99))
            {
                if (game.Timing < Properties.Settings.Default.AdvancedTime1)
                {
                    winForm = new WinForm(Minesweeper.DifficultyGame.Advanced, 1, game.Timing);
                    winForm.ShowDialog();
                    return;
                }
                if (game.Timing < Properties.Settings.Default.AdvancedTime2)
                {
                    winForm = new WinForm(Minesweeper.DifficultyGame.Advanced, 2, game.Timing);
                    winForm.ShowDialog();
                    return;
                }
                if (game.Timing < Properties.Settings.Default.IntermediateTime3)
                {
                    winForm = new WinForm(Minesweeper.DifficultyGame.Advanced, 3, game.Timing);
                    winForm.ShowDialog();
                    return;
                }
            }
        }

        // Обробник події GameOver.
        private void Game_Over(object sender, EventArgs e)
        {
            //MessageBox.Show("Game Over");
        }

        // Exit.
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // New game.
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.game.ResetField();
        }

        // Basic.
        private void BasicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BasicToolStripMenuItem.Checked = true;
            this.intermediateToolStripMenuItem.Checked = false;
            this.advancedToolStripMenuItem.Checked = false;
            this.especialToolStripMenuItem.Checked = false;
            CreateGame(Minesweeper.DifficultyGame.Basic);
        }

        // Intermediate.
        private void intermediateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.intermediateToolStripMenuItem.Checked = true;
            this.BasicToolStripMenuItem.Checked = false;
            this.advancedToolStripMenuItem.Checked = false;
            this.especialToolStripMenuItem.Checked = false;
            CreateGame(Minesweeper.DifficultyGame.Intermediate);
        }

        // Advanced.
        private void advancedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.advancedToolStripMenuItem.Checked = true;
            this.BasicToolStripMenuItem.Checked = false;
            this.intermediateToolStripMenuItem.Checked = false;
            this.especialToolStripMenuItem.Checked = false;
            CreateGame(Minesweeper.DifficultyGame.Advanced);
        }

        // Especial.
        private void especialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formEspecialState == null)
            {
                formEspecialState = new FormEspecialState();
                formEspecialState.button1.Click += FormEspecialStateClickOK;
            }
            formEspecialState.ShowDialog();
        }

        // Обробник події натискання кнопки ОК у formEspecialState.
        private void FormEspecialStateClickOK(object sender, EventArgs e)
        {
            this.advancedToolStripMenuItem.Checked = false;
            this.BasicToolStripMenuItem.Checked = false;
            this.intermediateToolStripMenuItem.Checked = false;
            this.especialToolStripMenuItem.Checked = true;
            CreateGame(formEspecialState.NumberTilesInRow,formEspecialState.NumberTilesInColumn,
                formEspecialState.NumberMines);
        }

        // Champions.
        private void championsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormChampions();
            form.ShowDialog();
        }

        // About Game.
        private void aboutGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAboutGame formAboutGame = new FormAboutGame();
            formAboutGame.ShowDialog();
        }
    }
}
