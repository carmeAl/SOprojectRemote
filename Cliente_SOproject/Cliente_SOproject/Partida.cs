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
using System.IO;

namespace Cliente_SOproject
{
    public partial class Partida : Form
    {
        int Nform;
        Socket server;
        string nombreUsuario;
        public int id_partida;
        string rival;
        public string nombreInvitado;
        string texto;
        int girar;
        int bloqueo=0;
        int pesao;
        int conteo = 0;
        bool turno;
        bool tiempo = false;
        //Parametros partida
        string nivel;
        string sugerirPreguntas;
        string mapa;
        string limitePreguntas;
        string limiteTiempo;
        string creador_partida;
        string[] ListaRandom;
        bool[] UPCCarta2 = { false,false,false,false,false,false,false,false,false,
        false,false,false};
        public string[] ListaImagenes = { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a" };

        static readonly object _object = new object();

        delegate void DelegadoParaEscribir(string rival, string texto);
        delegate void DelegadoParaEscribirLabel(string msn, Label nameLabel);
        delegate void DelegadoPicureBox(PictureBox namePictureBox);
        delegate void DelegadoParaCambiarTab();
        delegate void DelegadoParaEscribir2(string nombre);

        public Partida(int Nform, Socket server, string nombreUsuario,
            int id_partida, string nivel, string sugerirPreguntas, string mapa,
            string limitePreguntas, string limiteTiempo, string creador_partida)
        {
            InitializeComponent();
            this.Nform = Nform;
            this.server = server;
            this.nombreUsuario = nombreUsuario;
            this.id_partida = id_partida;
            this.nivel = nivel;
            this.sugerirPreguntas = sugerirPreguntas;
            this.mapa = mapa;
            this.limitePreguntas = limitePreguntas;
            this.limiteTiempo = limiteTiempo;
            this.creador_partida = creador_partida;
            this.girar = 0;

        }


        public void EscribirNombre(string nombre)
        {
            textBox1.Text = nombre;
        }
        public void EscribirLabel(string msn, Label nameLabel)
        {
            nameLabel.Text = msn;
        }
        public void PonNoVisiblePictureBox(PictureBox namePictureBox)
        {
            namePictureBox.Visible = false;
        }
        public static Bitmap GetImageByName(string imageName)
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            string resourceName = asm.GetName().Name + ".Properties.Resources";
            var rm = new System.Resources.ResourceManager(resourceName, asm);
            return (Bitmap)rm.GetObject(imageName);

        }
        public void CambiarTab()
        {

            comboBoxPTChat.Text = "Extienda la lista para seleccionar pregunta";
            tabControlPartida.SelectedTab = tabPageTablero;
            if (mapa == "ANIMALES")
            {
                
                for (int i = 1; i < 13; i++)
                {
                    ListaImagenes[i - 1] = "animal_" + ListaRandom[i];
                }
                pictureBoxImage1.Image = GetImageByName(ListaImagenes[0]);
                pictureBoxImage2.Image = GetImageByName(ListaImagenes[1]);
                pictureBoxImage3.Image = GetImageByName(ListaImagenes[2]);
                pictureBoxImage4.Image = GetImageByName(ListaImagenes[3]);
                pictureBoxImage5.Image = GetImageByName(ListaImagenes[4]);
                pictureBoxImage6.Image = GetImageByName(ListaImagenes[5]);
                pictureBoxImage7.Image = GetImageByName(ListaImagenes[6]);
                pictureBoxImage8.Image = GetImageByName(ListaImagenes[7]);
                pictureBoxImage9.Image = GetImageByName(ListaImagenes[8]);
                pictureBoxImage10.Image = GetImageByName(ListaImagenes[9]);
                pictureBoxImage11.Image = GetImageByName(ListaImagenes[10]);
                pictureBoxImage12.Image = GetImageByName(ListaImagenes[11]);
                if (sugerirPreguntas == "SI")
                {
                    comboBoxPTChat.Visible = true;
                    
                    comboBoxPTChat.Items.Add("¿El animal es mamifero?");
                    comboBoxPTChat.Items.Add("¿El animal es carnivoro?");
                    comboBoxPTChat.Items.Add("¿El animal es vertebrado?");
                    comboBoxPTChat.Items.Add("¿El animal es acuatico?");
                    comboBoxPTChat.Items.Add("¿El animal es domestico?");
                    comboBoxPTChat.Items.Add("¿El animal es Oviparo?");
                    comboBoxPTChat.Items.Add("¿El animal es de sangre fria?");
                    comboBoxPTChat.Items.Add("El animal es nomada?");
                }
            }
            else if (mapa == "COMPANEROS CLASE")
            {
                for (int i = 1; i < 13; i++)
                {
                    ListaImagenes[i - 1] = "clase_" + ListaRandom[i];
                }
                pictureBoxImage1.Image = GetImageByName(ListaImagenes[0]);
                pictureBoxImage2.Image = GetImageByName(ListaImagenes[1]);
                pictureBoxImage3.Image = GetImageByName(ListaImagenes[2]);
                pictureBoxImage4.Image = GetImageByName(ListaImagenes[3]);
                pictureBoxImage5.Image = GetImageByName(ListaImagenes[4]);
                pictureBoxImage6.Image = GetImageByName(ListaImagenes[5]);
                pictureBoxImage7.Image = GetImageByName(ListaImagenes[6]);
                pictureBoxImage8.Image = GetImageByName(ListaImagenes[7]);
                pictureBoxImage9.Image = GetImageByName(ListaImagenes[8]);
                pictureBoxImage10.Image = GetImageByName(ListaImagenes[9]);
                pictureBoxImage11.Image = GetImageByName(ListaImagenes[10]);
                pictureBoxImage12.Image = GetImageByName(ListaImagenes[11]);
                if (sugerirPreguntas == "SI")
                {
                    comboBoxPTChat.Visible = true;
                    comboBoxPTChat.Items.Add("¿El nombre de la persona empieza por la letra A?");
                    comboBoxPTChat.Items.Add("¿La persona es rubia?");
                    comboBoxPTChat.Items.Add("¿La persona lleva gafas?");
                    comboBoxPTChat.Items.Add("¿La persona es una chica?");
                    comboBoxPTChat.Items.Add("¿La persona tiene barba?");
                    comboBoxPTChat.Items.Add("¿La persona tiene el pelo largo?");
                    comboBoxPTChat.Items.Add("¿La persona tiene la piel morena?");
                    comboBoxPTChat.Items.Add("¿La persona tiene pircing/s?");
                }
            }
            else
            {
                for (int i = 1; i < 13; i++)
                {
                    ListaImagenes[i - 1] = "pais_" + ListaRandom[i];
                }
                pictureBoxImage1.Image = GetImageByName(ListaImagenes[0]);
                pictureBoxImage2.Image = GetImageByName(ListaImagenes[1]);
                pictureBoxImage3.Image = GetImageByName(ListaImagenes[2]);
                pictureBoxImage4.Image = GetImageByName(ListaImagenes[3]);
                pictureBoxImage5.Image = GetImageByName(ListaImagenes[4]);
                pictureBoxImage6.Image = GetImageByName(ListaImagenes[5]);
                pictureBoxImage7.Image = GetImageByName(ListaImagenes[6]);
                pictureBoxImage8.Image = GetImageByName(ListaImagenes[7]);
                pictureBoxImage9.Image = GetImageByName(ListaImagenes[8]);
                pictureBoxImage10.Image = GetImageByName(ListaImagenes[9]);
                pictureBoxImage11.Image = GetImageByName(ListaImagenes[10]);
                pictureBoxImage12.Image = GetImageByName(ListaImagenes[11]);
                if (sugerirPreguntas == "SI")
                {
                    comboBoxPTChat.Visible = true;
                    comboBoxPTChat.Items.Add("¿El pais esta en Europa?");
                    comboBoxPTChat.Items.Add("¿En el pais se habla Español?");
                    comboBoxPTChat.Items.Add("¿El pais es frio?");
                    comboBoxPTChat.Items.Add("¿El pais es desertico?");
                    comboBoxPTChat.Items.Add("¿El pais se rige por una monarquia parlamntaria?");
                    comboBoxPTChat.Items.Add("¿La moneda del pais es el Euro?");
                    comboBoxPTChat.Items.Add("¿Es un pais subdesarrollado?");
                    comboBoxPTChat.Items.Add("¿La bandera del pais contiene el color rojo?");
                }
            }
        }





        //////////
        //FUNCIONES
        //////////

        public void RespuestaInvitacion(string nombreInvitado, string SiNo)
        {
            if (SiNo == "Si")
            {
                tabControlPartida.Invoke(new DelegadoParaCambiarTab(CambiarTab), new object[] {});
                timerTurno.Enabled = true;
            }
            else
            {
                labelPE.Invoke(new DelegadoParaEscribirLabel(EscribirLabel), new object[] { nombreInvitado + " ha rechazado tu invitación", labelPE });
                pictureBoxPEGif.Image = Properties.Resources.Cross;
                pictureBoxPEBoton.Invoke(new DelegadoPicureBox(PonNoVisiblePictureBox), new object[] { pictureBoxPEBoton });
            }
        }
        public void PasarListaRandom(string lista)
        {
            ListaRandom = lista.Split(',');
        }
        public void EnviarTexto(string mensaje, string rival)
        {
            this.rival = rival;
            this.texto = mensaje;
            DelegadoParaEscribir delegado441 = new DelegadoParaEscribir(PonMSN);
            conversacion.Invoke(delegado441, new object[] { rival, texto });
        }
        public void PonMSN(string rival, string mensaje)
        {
            conversacion.Items.Insert(0,rival + ": " + mensaje);
        }
        private void button_enviar_Click(object sender, EventArgs e)
        {
            if (textBox_con.Text != "")
            {
                PonMSN(nombreUsuario, textBox_con.Text);
                // Envias el mensaje
                string mensaje = "44/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + textBox_con.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                textBox_con.Clear();
                button_enviar.Visible = false;
                textBox_con.Visible = false;
            }
            else
            {
                //Poner aqui que aparezca arriba un label que diga que escriba algo
            }
        }








        //////////
        //MOVIMIENTOS
        //////////
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
        int numCarta;
        int Opacidad;
        int siguiente;
        Image ImageCarta;


        private void timerFlip_Tick_1(object sender, EventArgs e)
        {

                if ((Opacidad > 0) && (siguiente == 0))
                {
                    CartasName.Image = SetAlpha((Bitmap)CartasName.Image, Opacidad);
                    Opacidad -= 100;
                }
                else
                {

                    if (UPCCarta2[numCarta])
                    {
                        CartasName.Image = Properties.Resources.UPClogo;
                        siguiente = 1;
                    }
                    else if (!UPCCarta2[numCarta])
                    {
                        CartasName.Image = ImageCarta;
                        siguiente = 1;
                    }
                    CartasName.Image = SetAlpha((Bitmap)CartasName.Image, Opacidad);
                    Opacidad += 100;
                }

        }

        private void pictureBoxImage1_Click(object sender, EventArgs e)
        {

            if (bloqueo == 0)
            {
                bloqueo = 1;
                numCarta = 0;
                if (!UPCCarta2[numCarta])
                    UPCCarta2[numCarta] = true;
                else
                    UPCCarta2[numCarta] = false;
                Opacidad = 500;
                siguiente = 0;
                CartasName = pictureBoxImage1;
                ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                timerFlip.Start();
            }
            else
            {
                pesao = pesao + 1;

            }

        }

        private void pictureBoxImage2_Click(object sender, EventArgs e)
        {
            if (bloqueo == 0)
            {

                bloqueo = 1;
                numCarta = 1;
                if (!UPCCarta2[numCarta])
                    UPCCarta2[numCarta] = true;
                else
                    UPCCarta2[numCarta] = false;
                Opacidad = 500;
                siguiente = 0;
                CartasName = pictureBoxImage2;
                ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                timerFlip.Start();
                girar = 0;
            }
            else
            {
                pesao = pesao + 1;
                
            }
        }

        private void pictureBoxImage3_Click(object sender, EventArgs e)
        {
            if (bloqueo == 0)
            {

                bloqueo = 1;
                numCarta = 2;
                if (!UPCCarta2[numCarta])
                    UPCCarta2[numCarta] = true;
                else
                    UPCCarta2[numCarta] = false;
                Opacidad = 500;
                siguiente = 0;
                CartasName = pictureBoxImage3;
                ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                timerFlip.Start();
                girar = 0;
            }
            else
            {
                pesao = pesao + 1;
                
            }
        }

        private void pictureBoxImage4_Click(object sender, EventArgs e)
        {
            if (bloqueo == 0)
            {
                bloqueo = 1;

                numCarta = 3;
                if (!UPCCarta2[numCarta])
                    UPCCarta2[numCarta] = true;
                else
                    UPCCarta2[numCarta] = false;
                Opacidad = 500;
                siguiente = 0;
                CartasName = pictureBoxImage4;
                ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                timerFlip.Start();
                girar = 0;
            }
            else
            {
                pesao = pesao + 1;
                
            }
        }

        private void pictureBoxImage5_Click(object sender, EventArgs e)
        {
            if (bloqueo == 0)
            {

                bloqueo = 1;
                numCarta = 4;
                if (!UPCCarta2[numCarta])
                    UPCCarta2[numCarta] = true;
                else
                    UPCCarta2[numCarta] = false;
                Opacidad = 500;
                siguiente = 0;
                CartasName = pictureBoxImage5;
                ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                timerFlip.Start();
                girar = 0;
            }
            else
            {
                pesao = pesao + 1;
                
            }
        }

        private void pictureBoxImage6_Click(object sender, EventArgs e)
        {
            if (bloqueo == 0)
            {

                bloqueo = 1;
                numCarta = 5;
                if (!UPCCarta2[numCarta])
                    UPCCarta2[numCarta] = true;
                else
                    UPCCarta2[numCarta] = false;
                Opacidad = 500;
                siguiente = 0;
                CartasName = pictureBoxImage6;
                ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                timerFlip.Start();
                girar = 0;
            }
            else
            {
                pesao = pesao + 1;
                
            }
        }

        private void pictureBoxImage7_Click(object sender, EventArgs e)
        {
            if (bloqueo == 0)
            {

                bloqueo = 1;
                numCarta = 6;
                if (!UPCCarta2[numCarta])
                    UPCCarta2[numCarta] = true;
                else
                    UPCCarta2[numCarta] = false;
                Opacidad = 500;
                siguiente = 0;
                CartasName = pictureBoxImage7;
                ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                timerFlip.Start();
                girar = 0;
            }
            else
            {
                pesao = pesao + 1;
                
            }
        }

        private void pictureBoxImage8_Click(object sender, EventArgs e)
        {
            if (bloqueo == 0)
            {

                bloqueo = 1;
                numCarta = 7;
                if (!UPCCarta2[numCarta])
                    UPCCarta2[numCarta] = true;
                else
                    UPCCarta2[numCarta] = false;
                Opacidad = 500;
                siguiente = 0;
                CartasName = pictureBoxImage8;
                ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                timerFlip.Start();
                girar = 0;
            }
            else
            {
                pesao = pesao + 1;
               
            }
        }

        private void pictureBoxImage9_Click(object sender, EventArgs e)
        {
            if (bloqueo == 0)
            {

                bloqueo = 1;
                numCarta = 8;
                if (!UPCCarta2[numCarta])
                    UPCCarta2[numCarta] = true;
                else
                    UPCCarta2[numCarta] = false;
                Opacidad = 500;
                siguiente = 0;
                CartasName = pictureBoxImage9;
                ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                timerFlip.Start();
                girar = 0;
            }
            else
            {
                pesao = pesao + 1;
                
            }
        }

        private void pictureBoxImage10_Click(object sender, EventArgs e)
        {
            if (bloqueo == 0)
            {

                bloqueo = 1;
                numCarta = 9;
                if (!UPCCarta2[numCarta])
                    UPCCarta2[numCarta] = true;
                else
                    UPCCarta2[numCarta] = false;
                Opacidad = 500;
                siguiente = 0;
                CartasName = pictureBoxImage10;
                ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                timerFlip.Start();
                girar = 0;
            }
            else
            {
                pesao = pesao + 1;
                
            }
        }

        private void pictureBoxImage11_Click(object sender, EventArgs e)
        {
            if (bloqueo == 0)
            {

                bloqueo = 1;
                numCarta = 10;
                if (!UPCCarta2[numCarta])
                    UPCCarta2[numCarta] = true;
                else
                    UPCCarta2[numCarta] = false;
                Opacidad = 500;
                siguiente = 0;
                CartasName = pictureBoxImage11;
                ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                timerFlip.Start();
                girar = 0;
            }
            else
            {
                pesao = pesao + 1;
                
            }
        }

        private void pictureBoxImage12_Click(object sender, EventArgs e)
        {
            if (bloqueo == 0)
            {
                bloqueo = 1;
                numCarta = 11;
                if (!UPCCarta2[numCarta])
                    UPCCarta2[numCarta] = true;
                else
                    UPCCarta2[numCarta] = false;
                Opacidad = 500;
                siguiente = 0;
                CartasName = pictureBoxImage12;
                ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                timerFlip.Start();
               
            }
            else
            {
                pesao = pesao + 1;

            }
        }

        


        private void Partida_Load(object sender, EventArgs e)
        {
            textBox1.Invoke(new DelegadoParaEscribir2(EscribirNombre), new object[] { nombreUsuario });
            Stop.Start();
            conteo = Convert.ToInt32(limiteTiempo);
            timerTurno.Enabled = true;
            if (creador_partida == nombreUsuario)
            {
                turno = true;
                textBox_con.Visible = true;
                button_enviar.Visible = true;
                button_Si.Visible = false;
                button_No.Visible = false;
                button_Nose.Visible = false;
            }
            else
            {
                turno = false;
                textBox_con.Visible = false;
                button_enviar.Visible = false;
                button_Si.Visible = true;
                button_No.Visible = true;
                button_Nose.Visible = true;
            }
        }

        private void Stop_Tick(object sender, EventArgs e)
        {
            bloqueo = 0;
        }

        private void comboBoxPTChat_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_con.Text = comboBoxPTChat.Text;
        }

        private void timerTurno_Tick(object sender, EventArgs e)
        {
            conteo = conteo - 1;
            label_tiempo.Text = conteo.ToString();
            if (conteo == 0)
            {
                conteo = Convert.ToInt32(limiteTiempo);
                tiempo = true;
            }
            else if (conteo <= 5)
            {
                label_tiempo.ForeColor = Color.Red;
            }
            else
            {
                label_tiempo.ForeColor = Color.Black;
            }
        }

        private void button_Si_Click(object sender, EventArgs e)
        {
            PonMSN(nombreUsuario, "Si");
            // Envias el mensaje
            string mensaje = "44/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/Si";
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            textBox_con.Clear();
            button_Si.Visible = false;
            button_No.Visible = false;
            button_Nose.Visible = false;
        }

        private void button_No_Click(object sender, EventArgs e)
        {
            PonMSN(nombreUsuario, "No");
            // Envias el mensaje
            string mensaje = "44/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/No";
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            textBox_con.Clear();
            button_Si.Visible = false;
            button_No.Visible = false;
            button_Nose.Visible = false;
        }

        private void button_Nose_Click(object sender, EventArgs e)
        {
            PonMSN(nombreUsuario, "No se");
            // Envias el mensaje
            string mensaje = "44/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/No se";
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            textBox_con.Clear();
            button_Si.Visible = false;
            button_No.Visible = false;
            button_Nose.Visible = false;
        }
    }
}


