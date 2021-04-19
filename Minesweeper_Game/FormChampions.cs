using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper_Game
{
    public partial class FormChampions : Form
    {
        public FormChampions()
        {
            InitializeComponent();
        }

        // Delete Results.
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            Close();
        }

        // Ok.
        private void buttonOk_Click(object sender, EventArgs e)
        {

            Close();
        }

        // Basic.
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                labelName1.Text = Properties.Settings.Default.BasicName1;
                labelTime1.Text = Properties.Settings.Default.BasicTime1.ToString() + "s.";
                labelName2.Text = Properties.Settings.Default.BasicName2;
                labelTime2.Text = Properties.Settings.Default.BasicTime2.ToString() + "s.";
                labelName3.Text = Properties.Settings.Default.BasicName3;
                labelTime3.Text = Properties.Settings.Default.BasicTime3.ToString() + "s.";
            }
        }

        // Intermediate.
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                labelName1.Text = Properties.Settings.Default.IntermediateName1;
                labelTime1.Text = Properties.Settings.Default.IntermediateTime1.ToString() + "s.";
                labelName2.Text = Properties.Settings.Default.IntermediateName2;
                labelTime2.Text = Properties.Settings.Default.IntermediateTime2.ToString() + "s.";
                labelName3.Text = Properties.Settings.Default.IntermediateName3;
                labelTime3.Text = Properties.Settings.Default.IntermediateTime3.ToString() + "s.";
            }
        }

        // Advanced.
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                labelName1.Text = Properties.Settings.Default.AdvancedName1;
                labelTime1.Text = Properties.Settings.Default.AdvancedTime1.ToString() + "s.";
                labelName2.Text = Properties.Settings.Default.AdvancedName2;
                labelTime2.Text = Properties.Settings.Default.AdvancedTime2.ToString() + "s.";
                labelName3.Text = Properties.Settings.Default.AdvancedName3;
                labelTime3.Text = Properties.Settings.Default.AdvancedTime3.ToString() + "s.";
            }
        }
    }
}
