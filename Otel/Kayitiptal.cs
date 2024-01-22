using System;
using System.Windows.Forms;

namespace Otel
{
    public partial class Kayitiptal : Form
    {
        public Kayitiptal()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 2)
            {
                richTextBox1.Height = 180;
                label2.Visible = true;
                richTextBox1.Visible = true;
                this.Height = 400;
            }
            else
            {
                richTextBox1.Visible = false;
                this.Height = 178;
                label2.Visible = false;
            }
        }
    }
}