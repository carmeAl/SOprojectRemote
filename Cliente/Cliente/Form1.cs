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
using System.Threading;


namespace Cliente
{
    public partial class Form1 : Form
    {
        public int id_usuario;
        public bool conectado = false;
        string invitado;
        string contrincante;
        string credaor_partida;
        int id_partida;
        Socket server;
        Thread atender;

        delegate void DelegadoParaEscribir(string mensaje);
        delegate void DelegadoGB(GroupBox mensaje);
        delegate void DelegadoGBR(ListBox mensaje);
        delegate void DelegadoGBT(TextBox mensaje);
        delegate void DelegadoDGV(DataGridView mensaje);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Consultas.Visible = false;
            Registrarse.Visible = false;
            dataGridView1.Visible = false;
            groupBoxListaCon.Visible = false;
            groupBoxChat.Visible = false;
        }

        public void PonDataGridView(string mensaje )
        {
            if (mensaje != null && mensaje != "")
            {

                dataGridView1.Rows.Clear();
                string[] partes = mensaje.Split(',');
                int num = Convert.ToInt32(partes[0]);
                int i = 0;
                while (i < num)
                {
                    int n = dataGridView1.Rows.Add();
                    dataGridView1.Rows[n].Cells[0].Value = partes[i + 1];
                    i++;
                }
            }
            else
            {
                MessageBox.Show("Ha ocurrido un error");
            }
            
        }
        public void PonVisibleGB(GroupBox nombre)
        {
            nombre.Visible = true;

        }
        public void PonVisibleDGV(DataGridView nombre)
        {
            nombre.Visible = true;
        }
        public void PonNoVisibleGB(GroupBox nombre)
        {
            nombre.Visible = false;
        }
        public void Refresca(ListBox nombre)
        {
            nombre.Items.Clear(); 
        }
        public void BorraText(TextBox nombre)
        {
            nombre.Clear();
        }
        public void PonNoVisibleDGV(DataGridView nombre)
        {
            nombre.Visible = false;
        }
        public void PonMSN(string mensaje)
        {
            conversacion.Items.Add(contrincante + ": " + mensaje);
        }


        private void AtenderServidor()
        {
            while (true)
            {
                //Recibimos mensaje del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                string[] trozos = Encoding.ASCII.GetString(msg2).Split('\0');
                string[] trozos1 = trozos[0].Split('/');
                int codigo = Convert.ToInt32(trozos1[0]);
                
                //string mensaje = trozos[trozos.Length-1].Split('\0')[0];
                
                string mensaje = trozos1[1];
                switch (codigo)
                {
              
                    case 11: // respuesta a iniciar
                        if (mensaje != "NO")
                        {
                            id_usuario = Convert.ToInt32(mensaje);
                            MessageBox.Show("Bienvenido");
                            DelegadoGB delegado1111 = new DelegadoGB(PonNoVisibleGB);
                            Iniciar.Invoke(delegado1111, new object[] { Iniciar });
                            DelegadoGB delegado1112 = new DelegadoGB(PonVisibleGB);
                            Consultas.Invoke(delegado1112, new object[] { Consultas });
                            DelegadoGB delegado1113 = new DelegadoGB(PonVisibleGB);
                            Consultas.Invoke(delegado1113, new object[] { groupBoxListaCon });
                            DelegadoDGV delegado112 = new DelegadoDGV(PonVisibleDGV);
                            dataGridView1.Invoke(delegado112, new object[] { dataGridView1 });
                            

                        }
                        else
                        {
                            MessageBox.Show("Usuario no encontrado, escriba bien el usuario y la contraseña, o " +
                                "registrese");
                        }
                        break;

                    case 21: //respuesta a registrarse
                        if (mensaje == "SI")
                            MessageBox.Show("Registrado");
                        else
                            MessageBox.Show("Usuario ya esta registrado, escriba otro usuario");
                        DelegadoGB delegado211 = new DelegadoGB(PonNoVisibleGB);
                        Registrarse.Invoke(delegado211, new object[] { Registrarse });
                        DelegadoGB delegado212 = new DelegadoGB(PonVisibleGB);
                        Iniciar.Invoke(delegado212, new object[] { Iniciar });
                        break;

                    case 32: //respuesta consulta partidas consecutivas
                        if (mensaje == "-1")
                            MessageBox.Show("El jugador no existe o no ha jugado ninguna partida");
                        else
                            MessageBox.Show("El jugador ha ganado " + mensaje + " partidas consecutivas");
                        break;

                    case 33: //respuesta consulta suma de los puntos del jugador introducido
                        if (mensaje == "-1")
                            MessageBox.Show("No hay informacion");
                        else
                            MessageBox.Show("El jugador " + textBox_nombre_consultas.Text + " tiene una suma de puntos = " + mensaje);
                        break;

                    case 34: //respuesta consulta lista de jugadores que han ganado contra el jugador introducido
                        if (mensaje == "-1")
                            MessageBox.Show("No hay informacion sobre jugadores que hayan ganado contra el jugador buscado");
                        else
                            MessageBox.Show(textBox_nombre_consultas.Text + " ha perdido contra: " + mensaje);
                        break;

                    case 35: //respuesta consulta lista de IDs de partidas que el usuario ha tenido contra el jugador introducido
                        if ((mensaje == "-1") || (mensaje == null))
                            MessageBox.Show("No hay informacion");
                        else
                            MessageBox.Show("Tus partidas jugadas contra " + textBox_nombre_consultas.Text + " son: " + mensaje);
                        break;

                    case 36: //respuesta lista de conectados
                        DelegadoParaEscribir delegado36 = new DelegadoParaEscribir(PonDataGridView);
                        dataGridView1.Invoke(delegado36, new object[] { mensaje });
                        break;
                    case 41: //Recive invitacion
                        string nombre = mensaje;
                        string mensaje_not;
                        DialogResult r = MessageBox.Show(nombre + " te ha invitado a un juego.\n ¿Quieres aceptar?", "Invitacion", MessageBoxButtons.YesNo);
                        if (r == DialogResult.Yes)
                        {
                            DelegadoGBR delegado1114 = new DelegadoGBR(Refresca);
                            Consultas.Invoke(delegado1114, new object[] { conversacion });
                            DelegadoGBT delegado1115 = new DelegadoGBT(BorraText);
                            Consultas.Invoke(delegado1115, new object[] { textBox_con });
                            mensaje_not = "42/" + nombre + "/" + textBox_nombre_in.Text + "/Si";
                            DelegadoGB delegado411 = new DelegadoGB(PonVisibleGB);
                            groupBoxChat.Invoke(delegado411, new object[] { groupBoxChat });
                        }
                        else
                        {
                            mensaje_not = "42/" + nombre + "/" + textBox_nombre_in.Text + "/No";
                        }
                        // Enviamos al servidor el nombre tecleado con un vector de bytes
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                        server.Send(msg);
                        break;
                    case 42: //respuesta a la invitacion enviada
                        string mensaje2 = trozos1[2];
                        if (mensaje2 == "Si")
                        {
                            DelegadoGB delegado421 = new DelegadoGB(PonVisibleGB);
                            groupBoxChat.Invoke(delegado421, new object[] { groupBoxChat });
                            
                        }
                        else
                        {
                            MessageBox.Show(trozos1[1] + " ha rechazado tu invitación");
                        }
                        break;
                    case 43: //recivir el id de la partida
                        
                        credaor_partida = trozos1[1];
                        if (credaor_partida == textBox_nombre_in.Text)
                        {
                            contrincante = trozos1[2];
                        }
                        else
                        {
                            contrincante = trozos1[1];
                        }
                        
                        id_partida = Convert.ToInt32(trozos1[3]);
                        break;
                    case 44: //recive conversación
                        id_partida = Convert.ToInt32(trozos1[1]);
                        DelegadoParaEscribir delegado441=new DelegadoParaEscribir(PonMSN);
                        conversacion.Invoke(delegado441, new object[] { trozos1[2] });
                        //conversacion.Items.Add(contrincante + ": " + trozos1[2]);
                        break;
                    case 45: //recive cancelacion
                        DelegadoGB delegado451 = new DelegadoGB(PonNoVisibleGB);
                        groupBoxChat.Invoke(delegado451, new object[] { groupBoxChat });
                        break;
                }
            }
        }
        private void button_registrarse_Click(object sender, EventArgs e)
        {

            if (conectado)
            {
                if (((textBox_nombre_re.Text.Length > 1) && (textBox_contra_re.Text.Length > 1)) && ((textBox_nombre_re.Text != "") && (textBox_contra_re.Text != "")))
                {
                    string mensaje = "21/" + textBox_nombre_re.Text + "/" + textBox_contra_re.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else
                {
                    MessageBox.Show("El nombre debe tener mas de un caracter");
                }
            }
            else
            {
                MessageBox.Show("No esta conectado con el servidor");
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
            IPAddress direc = IPAddress.Parse("147.83.117.22");
            IPEndPoint ipep = new IPEndPoint(direc, 50060);


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
            //pongo en marcha el thread que atenderà los mensajes del servidor 
            ThreadStart ts = delegate { AtenderServidor(); };
            atender = new Thread(ts);
            atender.Start();
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
                atender.Abort();
                this.BackColor = Color.Gray;
                server.Shutdown(SocketShutdown.Both);
                server.Close();
                conectado = false;
                groupBoxChat.Visible = false;
                groupBoxListaCon.Visible = false;
                Consultas.Visible = false;
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
                    }
                    else if (Consulta_33.Checked)
                    {
                        // Suma de los puntos del jugador introducido
                        string mensaje = "33/" + textBox_nombre_consultas.Text;
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);
                    }
                    else if (Consulta_34.Checked)
                    {
                        // Lista de los jugadores que han ganado contra el introducido
                        string mensaje = "34/" + textBox_nombre_consultas.Text;
                        // Enviamos al servidor el nombre tecleado con un vector de bytes
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);
                    }
                    else if (Consulta_35.Checked)
                    {
                        // Lista de IDs de partidas que el usuario ha tenido con el jugador introducido
                        string mensaje = "35/" + textBox_nombre_consultas.Text + "/" + id_usuario;
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);
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

    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (conectado)
            {
                //Mensaje de desconexión
                string mensaje = "0/" + textBox_nombre_in.Text;

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                // Nos desconectamos
                atender.Abort();
                this.BackColor = Color.Gray;
                server.Shutdown(SocketShutdown.Both);
                server.Close();
                conectado = false;
            }
        }
        


        private void button_salircon_Click(object sender, EventArgs e)
        {
            
        }

        private void button_invitar_Click_1(object sender, EventArgs e)
        {
            DelegadoGBT delegado1115 = new DelegadoGBT(BorraText);
            Consultas.Invoke(delegado1115, new object[] { textBox_con });
            DelegadoGBR delegado1114 = new DelegadoGBR(Refresca);
            Consultas.Invoke(delegado1114, new object[] { conversacion });
            // Lista de IDs de partidas que el usuario ha tenido con el jugador introducido
            try 
            {
                
                if (dataGridView1.CurrentRow.Cells[0].Value==null)//.ToString()
                {
                    MessageBox.Show("Selecciona a alguien");
                }

                else
                {
                    invitado = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    string creador = textBox_nombre_in.Text;
                    if (invitado == creador)
                    {
                        MessageBox.Show("No te puedes invitar a ti mismo");

                    }
                    else
                    {
                        string mensaje = "41/" + textBox_nombre_in.Text + "/" + invitado;
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);

                    }

                }
                
               
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Selecciona a una persona");
            }
            
            
            
        }

        private void button_enviar_Click_1(object sender, EventArgs e)
        {
            conversacion.Items.Add(textBox_nombre_in.Text + ": " + textBox_con.Text);
            // Envias el mensaje
            string mensaje = "44/" + id_partida + "/" + textBox_nombre_in.Text + "/" + textBox_con.Text;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            DelegadoGBT delegado1115 = new DelegadoGBT(BorraText);
            Consultas.Invoke(delegado1115, new object[] { textBox_con });
        }

        private void button_salircon_Click_1(object sender, EventArgs e)
        {
            // Envias que te desconectas de la conversación
            string mensaje = "45/" + id_partida+"/"+ textBox_nombre_in.Text;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            groupBoxChat.Visible = false;
        }
    }
}
