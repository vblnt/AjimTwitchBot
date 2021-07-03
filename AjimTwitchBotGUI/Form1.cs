using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;

namespace AjimTwitchBotGUI
{
    public partial class Form1 : Form
    {
        static IrcClientcs irc = new IrcClientcs();
        public Thread irc_thread = new Thread(new ThreadStart(irc_read));
        static bool irc_run = false;
        static string inMessage;
        string oldMessage;
        string song;
        string current;
        string name;
        string message;
        string start;
        DateTime Time_Now;
        int x;
        int ppl;
        int count;
        bool spot = false;
        bool irc_pick = false;



        private void cmd_update()
        {
            StreamReader rd = new StreamReader("cmd.txt", Encoding.GetEncoding("iso-8859-1"));
            while (!rd.EndOfStream)
            {
                string cmd = rd.ReadLine();
                string[] scmd = cmd.Split('\t');
                dataGridView1.Rows.Add(scmd[0], scmd[1]);
            }
            rd.Close();
        }
        private void irc_picker()
        {

            if (Convert.ToInt32(textBox3.Text) > 0)
            {
                if (DateTime.Now >= Time_Now)
                {
                    irc_pick = false;
                    
                }
            }

            if (get_irc_message() == textBox1.Text)
            {
                if (!listBox1.Items.Contains(name))
                {
                    listBox1.Items.Add(name);
                }
                
                if (ppl > 0)
                {
                    if (count<ppl)
                    {
                        count++;
                    }
                    else
                    {
                        irc_pick = false;
                    }
                    
                }

            }
        }

        private string get_irc_name()
        {
            x = 0;
            if (inMessage[0] == ':')
            {
                try
                {
                    while (x < inMessage.Length && inMessage[x] != '!')
                    {
                        x++;
                    }
                    name = inMessage.Substring(1, x - 1);
                }
                catch (Exception ex)
                {
                    name = "Error:" + ex;
                }
            }
            return name;
        }
        private string get_irc_message()
        {
            if (inMessage.Contains("#"))
            {
                x = 0;

                while (x < inMessage.Length && inMessage[x] != '#')
                {

                    x++;

                }
                message = inMessage.Substring(x + Properties.Settings.Default.channel.Count() + 3);
            }
            return message;
        }

        private void irc_messages()
        {
            if (inMessage != oldMessage && inMessage != "PING :tmi.twitch.tv")
            {
                
                richTextBox1.Text = richTextBox1.Text + get_irc_name() + ":" + get_irc_message() + "\n";
            }
            
            oldMessage = inMessage;
           
        }

        private void Irc()
        {

            irc.IrcClient(Properties.Settings.Default.IP, Properties.Settings.Default.Port, Properties.Settings.Default.Username, Properties.Settings.Default.Password);
            irc.joinRoom(Properties.Settings.Default.channel);
        }

        private string spotify()
        {
            try
            {
                var proc = Process.GetProcessesByName("Spotify").FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.MainWindowTitle));
                song = proc.MainWindowTitle;
            }
            catch (Exception e)
            {
                song = "Error! Something is wrong:" + e ;
            }
            return song;
        }
        public static void irc_read()
        {
            while (irc_run == true)
            {
                inMessage = irc.readMessage();
                if (inMessage== "PING :tmi.twitch.tv")
                {
                    irc.Pong();
                }
            }
                       
            
        }
        
        public Form1()
        {
            InitializeComponent();
            

    }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            
            
            try
            {
                cmd_update();
            }
            catch(Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex));
            }
        }
      
        private void iRCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            options frm = new options();
            frm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (checkBox1.Checked == true)
            {
                try
                {
                    Irc();
                    irc_run = true;
                    Thread irc_thread = new Thread(new ThreadStart(irc_read));
                    irc_thread.Start();
                    irc_thread.IsBackground = true;
                }
                catch(Exception ex)
                {
                    richTextBox1.Text = richTextBox1.Text + ex + "\n";
                }
                
            }
            if (checkBox2.Checked==true)
            {
                spot = true;
            }
            checkBox1.Enabled = false;
            checkBox2.Enabled = false;
            timer1.Start();
            start = DateTime.Now.ToString("HH:mm");
            label5.Text = "Started: " + start;
            button1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (irc_run == true)
                {
                    irc_messages();
                }
                if (spot == true)
                {
                    if (spotify() != current)
                    {

                        richTextBox1.Text = richTextBox1.Text + DateTime.Now.ToString("[HH:mm] ") + spotify() + "\n";
                        current = spotify();
                    }
                }
                if (irc_pick == true)
                {
                    irc_picker();
                    label4.Text = "On";
                }
                else
                {
                    label4.Text = "Off";
                }
                }
            catch(Exception)
            {
                timer1.Stop();
                MessageBox.Show("Error please check IRC!");
            }
            }
        
       private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            irc_thread.Abort();
            checkBox1.Enabled = true;
            checkBox2.Enabled = true;
            button1.Enabled = true;
            irc_run = false;
            irc_pick = false;
            spot = false;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            
            if(textBox1.Text == null)
            {
                MessageBox.Show("Error! Command is missing!");
            }
            else
            {
                count = 0;

                try
                {
                    ppl = Convert.ToInt32(textBox2.Text);
                    Time_Now = DateTime.Now;
                    Time_Now = Time_Now.AddSeconds(Convert.ToInt32(textBox3.Text));
                    
                    irc_pick = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Convert.ToString(ex));
                }
            }

            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            irc_pick = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
