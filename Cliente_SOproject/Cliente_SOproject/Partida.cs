using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Cliente_SOproject
{
    public partial class Partida : Form
    {
        int Nform;
        Socket server;
        public Partida(int Nform, Socket server)
        {
            InitializeComponent();
            this.Nform = Nform;
            this.server = server;
        }

        private void Partida_Load(object sender, EventArgs e)
        {
            //PictureBox pic = new PictureBox();
            //pic.Width = 20;
            //pic.Height = 20;
            //pic.ClientSize = new Size(20, 20);

            //pic.Location = new Point(50, 50);
            //pic.SizeMode = PictureBoxSizeMode.StretchImage;
            //Bitmap image = new Bitmap("p.png");
            pictureBox2.Image = Image.FromFile("C:\\Users\xavi\\Downloads\\SOprojectRemote-ClienteMenu (1)\\SOprojectRemote-ClienteMenu\\Cliente_SOproject\\Cliente_SOproject\\Resources");
            //pictureBox2.Image = Image.FromFile("p");
            //pictureBox2.Image = Resources.Online_lime_icon.ToBitmap()
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        public void MoverCarta(string mensaje)
        {

        }
    }
}
