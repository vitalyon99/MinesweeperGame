using System;
using System.Windows.Forms;

namespace Minesweeper_Game
{
    public partial class FormEspecialState : Form
    {
        private int numberTilesInRow = 30;
        private int numberTilesInColumn = 24;
        private int numberMines = 100;

        public int NumberTilesInRow
        {
            private set
            {
                if (value > 30)
                {
                    numberTilesInRow = 30;
                    return;
                }
                if (value < 10)
                {
                    numberTilesInRow = 10;
                    return;
                }
                numberTilesInRow = value;
            }
            get
            {
                return numberTilesInRow;
            }
        }

        public int NumberTilesInColumn
        {
            private set
            {
                if (value > 24)
                {
                    numberTilesInColumn = 24;
                    return;
                }
                if (value < 10)
                {
                    numberTilesInColumn = 10;
                    return;
                }
                numberTilesInColumn = value;
            }
            get
            {
                return numberTilesInColumn;
            }
        }

        public int NumberMines
        {
            private set
            {
                if (value > ((numberTilesInColumn * numberTilesInRow) / 2))
                {
                    numberMines = ((numberTilesInColumn * numberTilesInRow) / 2);
                    return;
                }
                if (numberMines < 10)
                {
                    numberMines = 10;
                    return;
                }
                numberMines = value;
            }
            get
            {
                return numberMines;
            }
        }

        public FormEspecialState()
        {
            InitializeComponent();
        }

        // Cancel.
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        // OK.
        private void button1_Click(object sender, EventArgs e)
        {
            
            try
            {
                NumberTilesInColumn = Convert.ToInt32(textBox1.Text);
                NumberTilesInRow = Convert.ToInt32(textBox2.Text);
                NumberMines = Convert.ToInt32(textBox3.Text);
            }
            catch(FormatException)
            {
                //MessageBox.Show($"{ex.ToString()}");

            }
            textBox1.Text = NumberTilesInColumn.ToString();
            textBox2.Text = NumberTilesInRow.ToString();
            textBox3.Text = NumberMines.ToString();
            Close();
        }
    }
}
