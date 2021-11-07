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

namespace Cliente
{
    public partial class Form1 : Form
    {
        public int id_usuario;
        public bool conectado = false;
        Socket server;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Consultas.Visible = false;
            Registrarse.Visible = false;
            dataGridView1.Visible = false;
            button_mostrar.Visible = false;
        }

        private void button_registrarse_Click(object sender, EventArgs e)
        {

            if (((textBox_nombre_re.Text.Length > 1) && (textBox_contra_re.Text.Length > 1)) && ((textBox_nombre_re.Text != "") && (textBox_contra_re.Text != "")))
            {
                string mensaje = "21/" + textBox_nombre_re.Text + "/" + textBox_contra_re.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                if (mensaje == "SI")
                    MessageBox.Show("Registrado");
                else
                    MessageBox.Show("Usuario ya esta registrado, escriba otro usuario");
                Registrarse.Visible = false;
                Iniciar.Visible = true;
            }
            else
            {
                MessageBox.Show("El nombre debe tener mas de un caracter");
            }
            
        }

        private void button_volver_Click(object sender, EventArgs e)
        {
            Registrarse.Visible = false;
            Iniciar.Visible = true;
            textBox_nombre_re.Clear();
            textBox_contra_re.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9211);


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
                string mensaje = "0/"+ textBox_nombre_in.Text;

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                // Nos desconectamos
                this.BackColor = Color.Gray;
                server.Shutdown(SocketShutdown.Both);
                server.Close();
                conectado = false;
                Consultas.Visible = false;
                dataGridView1.Visible = false;
                button_mostrar.Visible = false;
                Iniciar.Visible = true;
                Registrarse.Visible = false;
            }
            else
            {
                MessageBox.Show("Ya estas desconectado");
            }
        }

        private void button_registro_Click(object sender, EventArgs e)
        {
            Iniciar.Visible = false;
            Registrarse.Visible = true;
            textBox_nombre_in.Clear();
            textBox_contra_in.Clear();
        }

        private void button_iniciar_Click(object sender, EventArgs e)
        {
            if (conectado)
            {
                if ((textBox_nombre_in.Text != "") && (textBox_contra_in.Text != ""))
                {
                    string mensaje = "11/" + textBox_nombre_in.Text + "/" + textBox_contra_in.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Recibimos la respuesta del servidor
                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                    if (mensaje != "NO")
                    {
                        id_usuario = Convert.ToInt32(mensaje);
                        MessageBox.Show("Bienvenido");
                        Iniciar.Visible = false;
                        Consultas.Visible = true;
                        button_mostrar.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("Usuario no encontrado, escriba bien el usuario y la contraseña, o " +
                            "registrese");
                    }
                }
                else
                {
                    MessageBox.Show("Los campos de nombre o contraseña estan vacios");
                }
            }
            else
            {
                MessageBox.Show("No estas conectado al servidor");
            }
        }

        private void button_sol_Click(object sender, EventArgs e)
        {
            if (conectado)
            {
                if (textBox_nombre_consultas.Text.Length > 1)
                {
                    if (Consulta_32.Checked)
                    {
                        // Quiere saber las partidas consecutivas ganadas del jugador introducido
                        string mensaje = "32/" + textBox_nombre_consultas.Text;
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
                        string mensaje = "33/" + textBox_nombre_consultas.Text;
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
                            MessageBox.Show("El jugador " + textBox_nombre_consultas.Text + " tiene una suma de puntos = " + mensaje);
                    }
                    else if (Consulta_34.Checked)
                    {
                        // Quiere saber la longitud
                        string mensaje = "34/" + textBox_nombre_consultas.Text;
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
                            MessageBox.Show(textBox_nombre_consultas.Text + " ha perdido contra: " + mensaje);
                    }
                    else if (Consulta_35.Checked)
                    {
                        // Quiere saber si el nombre es bonito
                        string mensaje = "35/" + textBox_nombre_consultas.Text + "/" + id_usuario;
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);

                        //Recibimos la respuesta del servidor
                        byte[] msg2 = new byte[80];
                        server.Receive(msg2);
                        mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                        if ((mensaje == "-1") || (mensaje == null))
                            MessageBox.Show("No hay informacion");
                        else
                            MessageBox.Show("Tus partidas jugadas contra " + textBox_nombre_consultas.Text + " son: " + mensaje);
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

        private void button_mostrar_Click(object sender, EventArgs e)
        {
            if (conectado) { 
                dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            // Quiere saber la lista de conectados
            string mensaje = "36/";
            // Enviamos al servidor el nombre tecleado con un vector de bytes
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            //Recibimos la respuesta del servidor
            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            //Converit de bytes a ASCII
            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];


            if (mensaje != null && mensaje != "")
            {
                string[] partes = mensaje.Split(',');
                int num = Convert.ToInt32(partes[0]);
                int i = 0;
                while (i<num)
                {
                    int n= dataGridView1.Rows.Add();
                    dataGridView1.Rows[n].Cells[0].Value = partes[i+1];
                    i++;
                }
            }
            else
            {
             MessageBox.Show("Ha ocurrido un error");
            }
            }
            else
            {
                MessageBox.Show("No estas conectado al servidor");
            }
        }
        

    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (conectado)
            {
                //Mensaje de desconexión
                string mensaje = "0/" + textBox_nombre_in.Text;

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                // Nos desconectamos
                this.BackColor = Color.Gray;
                server.Shutdown(SocketShutdown.Both);
                server.Close();
                conectado = false;
            }
        }
    }
}
