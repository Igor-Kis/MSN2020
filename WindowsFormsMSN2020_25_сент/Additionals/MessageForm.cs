using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace wfSAControlDLL
{
    public partial class MessageForm : Form
    {
        bool isSetTimer = false;
        public MessageForm(string header, string message, int duration)
        {
            InitializeComponent();
            this.Text += header;
            label1.Text = message;
            timer1.Interval = 5000;
            if (duration != 0)
            {
                isSetTimer = true;
                timer1.Interval = duration * 1000;
            }

        }




        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            button1_Click(null, null);
        }

        private void MessageForm_Shown(object sender, EventArgs e)
        {
            if (isSetTimer)
                timer1.Start();
        }
    }
}
