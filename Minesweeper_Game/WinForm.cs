using System;
using System.Windows.Forms;

namespace Minesweeper_Game
{
    public partial class WinForm : Form
    {
        private int Timing;
        private Minesweeper.DifficultyGame Difficulty;
        private int Place;

        public WinForm(Minesweeper.DifficultyGame difficulty,int place, int timing)
        {
            InitializeComponent();
            Difficulty = difficulty;
            Timing = timing;
            Place = place;
        }

        // Ok.
        private void button2_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            switch (Difficulty)
            {
                case Minesweeper.DifficultyGame.Basic:
                    switch (Place)
                    {
                        case 1:
                            Properties.Settings.Default.BasicName3 = Properties.Settings.Default.BasicName2;
                            Properties.Settings.Default.BasicTime3 = Properties.Settings.Default.BasicTime2;
                            Properties.Settings.Default.BasicName2 = Properties.Settings.Default.BasicName1;
                            Properties.Settings.Default.BasicTime2 = Properties.Settings.Default.BasicTime1;
                            Properties.Settings.Default.BasicName1 = name;
                            Properties.Settings.Default.BasicTime1 = Timing;
                            Properties.Settings.Default.Save();
                            break;
                        case 2:
                            Properties.Settings.Default.BasicName3 = Properties.Settings.Default.BasicName2;
                            Properties.Settings.Default.BasicTime3 = Properties.Settings.Default.BasicTime2;
                            Properties.Settings.Default.BasicName2 = name;
                            Properties.Settings.Default.BasicTime2 = Timing;
                            Properties.Settings.Default.Save();
                            break;
                        case 3:
                            Properties.Settings.Default.BasicName3 = name;
                            Properties.Settings.Default.BasicTime3 = Timing;
                            Properties.Settings.Default.Save();
                            break;
                        default:
                            break;
                    }
                    break;
                case Minesweeper.DifficultyGame.Intermediate:
                    switch (Place)
                    {
                        case 1:
                            Properties.Settings.Default.IntermediateName3 = Properties.Settings.Default.IntermediateName2;
                            Properties.Settings.Default.IntermediateTime3 = Properties.Settings.Default.IntermediateTime2;
                            Properties.Settings.Default.IntermediateName2 = Properties.Settings.Default.IntermediateName1;
                            Properties.Settings.Default.IntermediateTime2 = Properties.Settings.Default.IntermediateTime1;
                            Properties.Settings.Default.IntermediateName1 = name;
                            Properties.Settings.Default.IntermediateTime1 = Timing;
                            Properties.Settings.Default.Save();
                            break;
                        case 2:
                            Properties.Settings.Default.IntermediateName3 = Properties.Settings.Default.IntermediateName2;
                            Properties.Settings.Default.IntermediateTime3 = Properties.Settings.Default.IntermediateTime2;
                            Properties.Settings.Default.IntermediateName2 = name;
                            Properties.Settings.Default.IntermediateTime2 = Timing;
                            Properties.Settings.Default.Save();
                            break;
                        case 3:
                            Properties.Settings.Default.IntermediateName3 = name;
                            Properties.Settings.Default.IntermediateTime3 = Timing;
                            Properties.Settings.Default.Save();
                            break;
                        default:
                            break;
                    }
                    break;
                case Minesweeper.DifficultyGame.Advanced:
                    switch (Place)
                    {
                        case 1:
                            Properties.Settings.Default.AdvancedName3 = Properties.Settings.Default.AdvancedName2;
                            Properties.Settings.Default.AdvancedTime3 = Properties.Settings.Default.AdvancedTime2;
                            Properties.Settings.Default.AdvancedName2 = Properties.Settings.Default.AdvancedName1;
                            Properties.Settings.Default.AdvancedTime2 = Properties.Settings.Default.AdvancedTime1;
                            Properties.Settings.Default.AdvancedName1 = name;
                            Properties.Settings.Default.AdvancedTime1 = Timing;
                            Properties.Settings.Default.Save();
                            break;
                        case 2:
                            Properties.Settings.Default.AdvancedName3 = Properties.Settings.Default.AdvancedName2;
                            Properties.Settings.Default.AdvancedTime3 = Properties.Settings.Default.AdvancedTime2;
                            Properties.Settings.Default.AdvancedName2 = name;
                            Properties.Settings.Default.AdvancedTime2 = Timing;
                            Properties.Settings.Default.Save();
                            break;
                        case 3:
                            Properties.Settings.Default.AdvancedName3 = name;
                            Properties.Settings.Default.AdvancedTime3 = Timing;
                            Properties.Settings.Default.Save();
                            break;
                        default:
                            break;
                    }
                    break;
            }
            Close();
        }

        // Cancel.
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
