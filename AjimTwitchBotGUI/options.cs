using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AjimTwitchBotGUI
{
    public partial class options : Form
    {
        public options()
        {
            InitializeComponent();
        }

        private void options_Load(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.IP;
            textBox2.Text = Convert.ToString( Properties.Settings.Default.Port);
            textBox3.Text = Properties.Settings.Default.Username;
            textBox4.Text = Properties.Settings.Default.Password;
            textBox5.Text = Properties.Settings.Default.channel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.IP = textBox1.Text;
            Properties.Settings.Default.Port = Convert.ToInt32( textBox2.Text);
            Properties.Settings.Default.Username = textBox3.Text;
            Properties.Settings.Default.Password = textBox4.Text;
            Properties.Settings.Default.channel = textBox5.Text;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
