using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace AjimTwitchBotGUI
{
    public partial class loading : Form
    {
        public loading()
        {
            InitializeComponent();
        }
        private void loading_Load(object sender, EventArgs e)
        {
            Form1 main = new Form1();
            loading startup = new loading();
            main.Show();
            startup.Close();

           
        }
    }
}
