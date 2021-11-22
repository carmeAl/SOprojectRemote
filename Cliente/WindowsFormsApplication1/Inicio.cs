using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace WindowsFormsApplication1
{
    public partial class Inicio : Form
    {
        Socket server;
        char respuesta;
        public int idUsuario;
        public bool conectado;
        public Inicio()
        {
            InitializeComponent();
        }

        public void recibirConectado(bool connect)
        {
            conectado = connect;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9121);

            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                this.BackColor = Color.Green;
                MessageBox.Show("Conectado");
                conectado = true;

            }
            catch (SocketException ex)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                Close();
                return;
            }

            if ((nombre.Text != "") && (contraseña.Text != ""))
            {
                string mensaje = "11/" + nombre.Text + "/" + contraseña.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                if (mensaje != "NO")
                {
                    idUsuario = Convert.ToInt32(mensaje);
                    MessageBox.Show("Bienvenido");
                    mensaje = "0/";
                    msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    this.BackColor = Color.Gray;
                    server.Shutdown(SocketShutdown.Both);
                    server.Close();
                    conectado = false;
                    Consultas consultas = new Consultas();
                    consultas.recibirID(idUsuario);
                    consultas.ShowDialog();
                    if (conectado) { 
                        mensaje = "0/";
                        msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);
                        this.BackColor = Color.Gray;
                        server.Shutdown(SocketShutdown.Both);
                        server.Close();
                        conectado = false;
                    }

                    
                    Close();
                }
                else
                {
                    MessageBox.Show("Usuario no encontrado, escriba bien el usuario y la contraseña, o " +
                        "registrese");
                    mensaje = "0/";
                    msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                    this.BackColor = Color.Gray;
                    server.Shutdown(SocketShutdown.Both);
                    server.Close();
                    conectado = false;
                }
            }
            else
            {
                MessageBox.Show("Los campos de nombre o contraseña estan vacios");

                string mensaje = "0/";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                this.BackColor = Color.Gray;
                server.Shutdown(SocketShutdown.Both);
                server.Close();
                conectado = false;
            }


        }
        private void button4_Click(object sender, EventArgs e)
        {
            //string mensaje = "0/";
            //byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            //server.Send(msg);
            //this.BackColor = Color.Gray;
            Registrarse registro = new Registrarse();
            registro.ShowDialog();
            //respuesta=registro.GetRespuesta();
        }
    }
}
