using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace AjimTwitchBotGUI
{
    class Irc
    {
        private string userName;
        private string channel;

        private TcpClient tcpclient;
        private StreamReader inputstream;
        private StreamWriter outputstream;
        public IrcClient(string ip, int port, string userName, string password)
        {
            this.userName = userName;
            tcpclient = new TcpClient(ip, port);
            inputstream = new StreamReader(tcpclient.GetStream());
            outputstream = new StreamWriter(tcpclient.GetStream());
            outputstream.WriteLine("PASS " + password);
            outputstream.WriteLine("NICK " + userName);
            outputstream.WriteLine("USER " + userName + " 8 * :" + userName);
            outputstream.Flush();
        }
        public void joinRoom(string channel)
        {
            this.channel = channel;
            outputstream.WriteLine("JOIN #" + channel);
            outputstream.Flush();
        }
        public void sendIrcMessage(string message)
        {
            outputstream.WriteLine(message);
            outputstream.Flush();
        }

        public void sendChatMessage(string message)
        {
            sendIrcMessage(":" + userName + "!" + userName + "@" + userName + ".tmi.twitch.tv PRIVMSG #" + channel + " :" + message);

        }
        public string readMessage()
        {
            string message = inputstream.ReadLine();
            return message;
        }
    }
}
