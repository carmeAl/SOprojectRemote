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
    }
}
