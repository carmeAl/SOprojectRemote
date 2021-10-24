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
    public partial class Consultas : Form
    {
        public int id_usuario;
        public bool conectado = false;
        Socket server;
        public Consultas()
        {
            InitializeComponent();
        }

        public void recibirID(int idU)
        {
            id_usuario = idU;
        }

        private void button_con_Click(object sender, EventArgs e)
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
            catch (SocketException)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                return;
            }
        }

        private void button_descon_Click(object sender, EventArgs e)
        {
            if (conectado)
            {
                //Mensaje de desconexión
                string mensaje = "0/";

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                // Nos desconectamos
                this.BackColor = Color.Gray;
                server.Shutdown(SocketShutdown.Both);
                server.Close();
                conectado = false;
            }
            else
            {
                MessageBox.Show("Ya estas desconectado");
            }
        }

        private void button_sol_Click(object sender, EventArgs e)
        {
            if (conectado)
            {
                if (nombre.Text.Length > 1)
                {
                    if (Consulta_32.Checked)
                    {
                        // Quiere saber las partidas consecutivas ganadas del jugador introducido
                        string mensaje = "32/" + nombre.Text;
                        // Enviamos al servidor el nombre tecleado con un vector de bytes
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);

                        //Recibimos la respuesta del servidor
                        byte[] msg2 = new byte[80];
                        server.Receive(msg2);
                        //Converit de bytes a ASCII
                        mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                        if (mensaje == "-1")
                            MessageBox.Show("El jugador no existe o no ha jugado ninguna partida");
                        else
                            MessageBox.Show("El jugador ha ganado " + mensaje + " partidas consecutivas");
                    }
                    else if (Consulta_33.Checked)
                    {
                        // Quiere saber si el nombre es bonito
                        string mensaje = "33/" + nombre.Text;
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);

                        //Recibimos la respuesta del servidor
                        byte[] msg2 = new byte[80];
                        server.Receive(msg2);
                        mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                        if (mensaje == "-1")
                            MessageBox.Show("No hay informacion");
                        else
                            MessageBox.Show("El jugador " + nombre.Text + " tiene una suma de puntos = " + mensaje);
                    }
                    else if (Consulta_34.Checked)
                    {
                        // Quiere saber la longitud
                        string mensaje = "34/" + nombre.Text;
                        // Enviamos al servidor el nombre tecleado con un vector de bytes
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);

                        //Recibimos la respuesta del servidor
                        byte[] msg2 = new byte[100];
                        server.Receive(msg2);
                        //Converit de bytes a ASCII
                        mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                        if (mensaje == "-1")
                            MessageBox.Show("No hay informacion sobre jugadores que hayan ganado contra el jugador buscado");
                        else
                            MessageBox.Show(nombre.Text + " ha perdido contra: " + mensaje);
                    }
                    else if (Consulta_35.Checked)
                    {
                        // Quiere saber si el nombre es bonito
                        string mensaje = "35/" + nombre.Text + "/" + id_usuario;
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);

                        //Recibimos la respuesta del servidor
                        byte[] msg2 = new byte[80];
                        server.Receive(msg2);
                        mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                        if ((mensaje == "-1") || (mensaje==null))
                            MessageBox.Show("No hay informacion");
                        else
                            MessageBox.Show("Tus partidas jugadas contra " + nombre.Text + " son: " + mensaje);
                    }
                }
                else
                {
                    MessageBox.Show("Nombre mal introducido");
                }

            }
            else
            {
                MessageBox.Show("No estas conectado al servidor");
            }
        }

        private void Consultas_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (conectado)
            {
                string mensaje = "0/";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                this.BackColor = Color.Gray;
                server.Shutdown(SocketShutdown.Both);
                server.Close();
                conectado = false;
            }
            Inicio Inicio=new Inicio();
            Inicio.recibirConectado(conectado);
        }
    }
}
