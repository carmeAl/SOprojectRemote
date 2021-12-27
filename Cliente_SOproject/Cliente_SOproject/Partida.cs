using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Cliente_SOproject
{
    public partial class Partida : Form
    {
        int Nform;
        Socket server;
        string nombreUsuario;
        int id_partida;
        string rival;
        string texto;
        delegate void DelegadoParaEscribir(string rival, string texto);

        public Partida(int Nform, Socket server, string nombreUsuario, int id_partida)
        {
            InitializeComponent();
            this.Nform = Nform;
            this.server = server;
            this.nombreUsuario = nombreUsuario;
            this.id_partida = id_partida;
        }

        private void Partida_Load(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.p;
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
        public void EnviarTexto(string mensaje, string rival)
        {
            this.rival = rival;
            this.texto = mensaje;
            DelegadoParaEscribir delegado441 = new DelegadoParaEscribir(PonMSN);
            conversacion.Invoke(delegado441, new object[] { rival,texto });
        }
        public void PonMSN(string rival,string mensaje)
        {
            conversacion.Items.Add(rival + ": " + mensaje);
        }

        private void button_enviar_Click(object sender, EventArgs e)
        {
            conversacion.Items.Add(nombreUsuario + ": " + textBox_con.Text);
            // Envias el mensaje
            string mensaje = "44/" +  id_partida + "/"+ Nform + "/" + nombreUsuario + "/" + textBox_con.Text;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            textBox_con.Clear();
        }

        static Bitmap SetAlpha(Bitmap bmpIn, int alpha)
        {
            Bitmap bmpOut = new Bitmap(bmpIn.Width, bmpIn.Height);
            float a = alpha / 255f;
            Rectangle r = new Rectangle(0, 0, bmpIn.Width, bmpIn.Height);

            float[][] matrixItems = {
            new float[] {1, 0, 0, 0, 0},
            new float[] {0, 1, 0, 0, 0},
            new float[] {0, 0, 1, 0, 0},
            new float[] {0, 0, 0, a, 0},
            new float[] {0, 0, 0, 0, 1}};

            ColorMatrix colorMatrix = new ColorMatrix(matrixItems);

            ImageAttributes imageAtt = new ImageAttributes();
            imageAtt.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            using (Graphics g = Graphics.FromImage(bmpOut))
                g.DrawImage(bmpIn, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel, imageAtt);

            return bmpOut;
        }
        PictureBox CartasName;
        int Opacidad;
        int siguiente;
        Image ImageCarta;
        bool UPCCarta2=false;
        
        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            if (!UPCCarta2)
                UPCCarta2 = true;
            else
                UPCCarta2 = false;
            Opacidad = 500;
            siguiente = 0;
            CartasName = pictureBox2;
            ImageCarta = Properties.Resources.p;
            timerFlip.Start();
        }

        private void timerFlip_Tick_1(object sender, EventArgs e)
        {
            if ((Opacidad > 0) && (siguiente==0))
            {
                CartasName.Image = SetAlpha((Bitmap)CartasName.Image, Opacidad);
                Opacidad -= 50;
            }
            else
            {
                
                if (UPCCarta2)
                {
                    CartasName.Image = Properties.Resources.UPClogo;
                    siguiente = 1;
                }   
                else if (!UPCCarta2)
                {
                    CartasName.Image = ImageCarta;
                    siguiente = 1;
                }
                CartasName.Image = SetAlpha((Bitmap)CartasName.Image, Opacidad);
                Opacidad += 50;
            }
        }
    }
}
