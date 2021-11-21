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
    public partial class FormMenu : Form
    {
        public int id_usuario;
        public bool conectado = false;
        Socket server;
        Thread atender;

        public FormMenu()
        {
            InitializeComponent();
            tabControl1.SelectedTab = tabPageLogin;
        }



        //FUNCIONAMIENTO

        delegate void DelegadoDGV(DataGridView mensaje);
        delegate void DelegadoParaEscribir(string mensaje);
        public void PonDataGridView(string mensaje)
        {
            if (mensaje != null && mensaje != "")
            {

                dataGridViewListaCon.Rows.Clear();
                string[] partes = mensaje.Split(',');
                int num = Convert.ToInt32(partes[0]);
                int i = 0;
                while (i < num)
                {
                    int n = dataGridViewListaCon.Rows.Add();
                    dataGridViewListaCon.Rows[n].Cells[0].Value = partes[i + 1];
                    i++;
                }
            }
            else
            {
                MessageBox.Show("Ha ocurrido un error");
            }

        }
        //THREAD
        private void AtenderServidor()
        {
            while (true)
            {
                //Recibimos mensaje del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                string[] trozos = Encoding.ASCII.GetString(msg2).Split('/');
                int codigo = Convert.ToInt32(trozos[0]);
                string mensaje = trozos[1].Split('\0')[0];
                switch (codigo)
                {

                    case 11: // respuesta a iniciar
                        if (mensaje != "NO")
                        {
                            id_usuario = Convert.ToInt32(mensaje);
                            MessageBox.Show("Bienvenido");
                            tabControl1.SelectedTab = tabPageMenu;
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
                        break;

                    case 36: //respuesta lista de conectados
                        DelegadoParaEscribir delegado36 = new DelegadoParaEscribir(PonDataGridView);
                        dataGridViewListaCon.Invoke(delegado36, new object[] { mensaje });
                        break;
                }
            }
        }

        //NAVIEGACION
        private void pictureBoxLIniciar_Click(object sender, EventArgs e)
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

            if (conectado)
            {
                if ((textBoxLNombre.ForeColor != Color.FromArgb(173, 188, 236) ) && (textBoxLContraseña.ForeColor != Color.FromArgb(173, 188, 236)))
                {
                    string mensaje = "11/" + textBoxLNombre.Text + "/" + textBoxLContraseña.Text;
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
        private void labelLRegistrar_Click(object sender, EventArgs e) => tabControl1.SelectedTab = tabPageRegister;

        private void pictureBoxRVolver_Click(object sender, EventArgs e) => tabControl1.SelectedTab = tabPageLogin;

        private void pictureBoxMCrearPartida_Click(object sender, EventArgs e) => tabControl1.SelectedTab = tabPageCrearPartida;

        private void pictureBoxCVolver_Click(object sender, EventArgs e) => tabControl1.SelectedTab = tabPageMenu;



        //DISEÑO
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
        private void pictureBox7_MouseEnter(object sender, EventArgs e)=>pictureBoxMDesconectar.Image = SetAlpha((Bitmap)pictureBoxMDesconectar.Image, 150);
        private void pictureBox7_MouseLeave(object sender, EventArgs e) => pictureBoxMDesconectar.Image = SetAlpha((Bitmap)pictureBoxMDesconectar.Image, 1000);
        private void pictureBoxLIniciar_MouseEnter(object sender, EventArgs e) => pictureBoxLIniciar.Image = SetAlpha((Bitmap)pictureBoxLIniciar.Image, 150);
        private void pictureBoxLIniciar_MouseLeave(object sender, EventArgs e) => pictureBoxLIniciar.Image = SetAlpha((Bitmap)pictureBoxLIniciar.Image, 1000);
        private void labelLRegistrar_MouseEnter(object sender, EventArgs e) => labelLRegistrar.BackColor = Color.FromArgb(69, 69, 69); 
        private void labelLRegistrar_MouseLeave(object sender, EventArgs e) => labelLRegistrar.BackColor = Color.Black;
        private void pictureBoxRVolver_MouseEnter(object sender, EventArgs e) => pictureBoxRVolver.Image = SetAlpha((Bitmap)pictureBoxRVolver.Image, 150);
        private void pictureBoxRVolver_MouseLeave(object sender, EventArgs e) => pictureBoxRVolver.Image = SetAlpha((Bitmap)pictureBoxRVolver.Image, 1000);
        private void pictureBoxRRegistrarse_MouseEnter(object sender, EventArgs e) => pictureBoxRRegistrarse.Image = SetAlpha((Bitmap)pictureBoxRRegistrarse.Image, 150);
        private void pictureBoxRRegistrarse_MouseLeave(object sender, EventArgs e) => pictureBoxRRegistrarse.Image = SetAlpha((Bitmap)pictureBoxRRegistrarse.Image, 1000);
        private void pictureBoxMCrearPartida_MouseEnter(object sender, EventArgs e) => pictureBoxMCrearPartida.Image = SetAlpha((Bitmap)pictureBoxMCrearPartida.Image, 150);
        private void pictureBoxMCrearPartida_MouseLeave(object sender, EventArgs e) => pictureBoxMCrearPartida.Image = SetAlpha((Bitmap)pictureBoxMCrearPartida.Image, 1000);
        private void pictureBoxMSocial_MouseEnter(object sender, EventArgs e) => pictureBoxMSocial.Image = SetAlpha((Bitmap)pictureBoxMSocial.Image, 150);
        private void pictureBoxMSocial_MouseLeave(object sender, EventArgs e) => pictureBoxMSocial.Image = SetAlpha((Bitmap)pictureBoxMSocial.Image, 1000);
        private void pictureBoxMPerfil_MouseEnter(object sender, EventArgs e) => pictureBoxMPerfil.Image = SetAlpha((Bitmap)pictureBoxMPerfil.Image, 150);
        private void pictureBoxMPerfil_MouseLeave(object sender, EventArgs e) => pictureBoxMPerfil.Image = SetAlpha((Bitmap)pictureBoxMPerfil.Image, 1000);
        private void pictureBoxMConfiguracion_MouseEnter(object sender, EventArgs e) => pictureBoxMConfiguracion.Image = SetAlpha((Bitmap)pictureBoxMConfiguracion.Image, 150);
        private void pictureBoxMConfiguracion_MouseLeave(object sender, EventArgs e) => pictureBoxMConfiguracion.Image = SetAlpha((Bitmap)pictureBoxMConfiguracion.Image, 1000);
        private void pictureBoxCVolver_MouseEnter(object sender, EventArgs e) => pictureBoxCVolver.Image = SetAlpha((Bitmap)pictureBoxCVolver.Image, 150);
        private void pictureBoxCVolver_MouseLeave(object sender, EventArgs e) => pictureBoxCVolver.Image = SetAlpha((Bitmap)pictureBoxCVolver.Image, 1000);
        private void pictureBoxCInvitar_MouseEnter(object sender, EventArgs e) => pictureBoxCInvitar.Image = SetAlpha((Bitmap)pictureBoxCInvitar.Image, 150);
        private void pictureBoxCInvitar_MouseLeave(object sender, EventArgs e) => pictureBoxCInvitar.Image = SetAlpha((Bitmap)pictureBoxCInvitar.Image, 1000);

        private void textBoxLNombre_Enter(object sender, EventArgs e)
        {
            if (textBoxLNombre.Text == "USUARIO")
            {
                textBoxLNombre.Text = "";
                textBoxLNombre.ForeColor = Color.Black;
            }
        }

        private void textBoxLNombre_Leave(object sender, EventArgs e)
        {
            if (textBoxLNombre.Text == "")
            {
                textBoxLNombre.Text = "USUARIO";
                textBoxLNombre.ForeColor = Color.FromArgb(173, 188, 236);
            }
        }

        private void textBoxLContraseña_Enter(object sender, EventArgs e)
        {
            if (textBoxLContraseña.Text == "CONTRASEÑA")
            {
                textBoxLContraseña.Text = "";
                textBoxLContraseña.ForeColor = Color.Black;
            }
        }

        private void textBoxLContraseña_Leave(object sender, EventArgs e)
        {
            if (textBoxLContraseña.Text == "")
            {
                textBoxLContraseña.Text = "CONTRASEÑA";
                textBoxLContraseña.ForeColor = Color.FromArgb(173, 188, 236);
            }
        }

        private void textBoxRUsuario_Enter(object sender, EventArgs e)
        {
            if (textBoxRUsuario.Text == "USUARIO")
            {
                textBoxRUsuario.Text = "";
                textBoxRUsuario.ForeColor = Color.Black;
            }
        }

        private void textBoxRUsuario_Leave(object sender, EventArgs e)
        {
            if (textBoxRUsuario.Text == "")
            {
                textBoxRUsuario.Text = "USUARIO";
                textBoxRUsuario.ForeColor = Color.FromArgb(173, 188, 236);
            }
        }

        private void textBoxRContraseña_Enter(object sender, EventArgs e)
        {
            if (textBoxRContraseña.Text == "CONTRASEÑA")
            {
                textBoxRContraseña.Text = "";
                textBoxRContraseña.ForeColor = Color.Black;
            }
        }

        private void textBoxRContraseña_Leave(object sender, EventArgs e)
        {
            if (textBoxRContraseña.Text == "")
            {
                textBoxRContraseña.Text = "CONTRASEÑA";
                textBoxRContraseña.ForeColor = Color.FromArgb(173, 188, 236);
            }
        }

        private void textBoxCNumPreg_Enter(object sender, EventArgs e)
        {
            if (textBoxCNumPreg.Text == "10")
            {
                textBoxCNumPreg.Text = "";
                textBoxCNumPreg.ForeColor = Color.Black;
            }
        }

        private void textBoxCNumPreg_Leave(object sender, EventArgs e)
        {
            if (textBoxCNumPreg.Text == "")
            {
                textBoxCNumPreg.Text = "10";
                textBoxCNumPreg.ForeColor = Color.FromArgb(173, 188, 236);
            }
        }

        private void textBoxCLimTiempo_Enter(object sender, EventArgs e)
        {
            if (textBoxCLimTiempo.Text == "60")
            {
                textBoxCLimTiempo.Text = "";
                textBoxCLimTiempo.ForeColor = Color.Black;
            }
        }

        private void textBoxCLimTiempo_Leave(object sender, EventArgs e)
        {
            if (textBoxCLimTiempo.Text == "")
            {
                textBoxCLimTiempo.Text = "60";
                textBoxCLimTiempo.ForeColor = Color.FromArgb(173, 188, 236);
            }
        }

        
    }
}

