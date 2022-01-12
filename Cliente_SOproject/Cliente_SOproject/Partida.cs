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
        public string rival;
        public string nombreInvitado;
        string texto;
        int girar;
        int bloqueo = 0;
        int pesao;
        int conteo = 0;
        bool turno=false;
        bool Star_Start;
        int Star_Stop=0;
        bool tiempo = false;
        bool inicio_partida = false;
        int id_carta;
        //Parametros partida
        string sugerirPreguntas;
        string mapa;
        string limitePreguntas;
        string limiteTiempo;
        string creador_partida;
        string[] ListaRandom;
        int carta_inicial=0;
        bool[] UPCCarta2 = { false,false,false,false,false,false,false,false,false,
        false,false,false};
        public string[] ListaImagenes = { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a" };
        string mensaje_not;
        int id_carta_rival;
        int carta_seleccionada=-1;
        int vidas = 3;
        int seguridad=1;

        static readonly object _object = new object();

        delegate void DelegadoParaEscribir(string rival, string texto);
        delegate void DelegadoParaEscribirLabel(string msn, Label nameLabel);
        delegate void DelegadoPicureBox(PictureBox namePictureBox);
        delegate void DelegadoParaCambiarTab();
        delegate void DelegadoParaEscribir2(string nombre);
        delegate void DelegadoParaVisibleLabel(Label nameLabel, bool SINO);

        public Partida(int Nform, Socket server, string nombreUsuario,
            int id_partida, string nivel, string sugerirPreguntas, string mapa,
            string limitePreguntas, string limiteTiempo, string creador_partida)
        {
            InitializeComponent();
            this.Nform = Nform;
            this.server = server;
            this.nombreUsuario = nombreUsuario;
            this.id_partida = id_partida;
            this.sugerirPreguntas = sugerirPreguntas;
            this.mapa = mapa;
            this.limitePreguntas = limitePreguntas;
            this.limiteTiempo = limiteTiempo;
            this.creador_partida = creador_partida;
            this.girar = 0;

        }

        public void EscribirLabel(string msn, Label nameLabel)
        {
            nameLabel.Text = msn;
        }
        public void PonNoVisiblePictureBox(PictureBox namePictureBox)
        {
            namePictureBox.Visible = false;
        }
        public void PonVisibleONOLabel (Label nameLabel, bool SINO)
        {
            nameLabel.Visible = SINO;
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
                poner_nombre_fotos(0, label_Imagen1, "animal");
                pictureBoxImage2.Image = GetImageByName(ListaImagenes[1]);
                poner_nombre_fotos(1, label_Imagen2, "animal");
                pictureBoxImage3.Image = GetImageByName(ListaImagenes[2]);
                poner_nombre_fotos(2, label_Imagen3, "animal");
                pictureBoxImage4.Image = GetImageByName(ListaImagenes[3]);
                poner_nombre_fotos(3, label_Imagen4, "animal");
                pictureBoxImage5.Image = GetImageByName(ListaImagenes[4]);
                poner_nombre_fotos(4, label_Imagen5, "animal");
                pictureBoxImage6.Image = GetImageByName(ListaImagenes[5]);
                poner_nombre_fotos(5, label_Imagen6, "animal");
                pictureBoxImage7.Image = GetImageByName(ListaImagenes[6]);
                poner_nombre_fotos(6, label_Imagen7, "animal");
                pictureBoxImage8.Image = GetImageByName(ListaImagenes[7]);
                poner_nombre_fotos(7, label_Imagen8, "animal");
                pictureBoxImage9.Image = GetImageByName(ListaImagenes[8]);
                poner_nombre_fotos(8, label_Imagen9, "animal");
                pictureBoxImage10.Image = GetImageByName(ListaImagenes[9]);
                poner_nombre_fotos(9, label_Imagen10, "animal");
                pictureBoxImage11.Image = GetImageByName(ListaImagenes[10]);
                poner_nombre_fotos(10, label_Imagen11, "animal");
                pictureBoxImage12.Image = GetImageByName(ListaImagenes[11]);
                poner_nombre_fotos(11, label_Imagen12, "animal");
                if (sugerirPreguntas == "SI")
                {
                    comboBoxPTChat.Visible = true;

                    comboBoxPTChat.Items.Add("El animal es mamifero?");
                    comboBoxPTChat.Items.Add("El animal es carnivoro?");
                    comboBoxPTChat.Items.Add("El animal es vertebrado?");
                    comboBoxPTChat.Items.Add("El animal es acuatico?");
                    comboBoxPTChat.Items.Add("El animal es domestico?");
                    comboBoxPTChat.Items.Add("El animal es Oviparo?");
                    comboBoxPTChat.Items.Add("El animal es de sangre fria?");
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
                poner_nombre_fotos(0, label_Imagen1, "clase");
                pictureBoxImage2.Image = GetImageByName(ListaImagenes[1]);
                poner_nombre_fotos(1, label_Imagen2, "clase");
                pictureBoxImage3.Image = GetImageByName(ListaImagenes[2]);
                poner_nombre_fotos(2, label_Imagen3, "clase");
                pictureBoxImage4.Image = GetImageByName(ListaImagenes[3]);
                poner_nombre_fotos(3, label_Imagen4, "clase");
                pictureBoxImage5.Image = GetImageByName(ListaImagenes[4]);
                poner_nombre_fotos(4, label_Imagen5, "clase");
                pictureBoxImage6.Image = GetImageByName(ListaImagenes[5]);
                poner_nombre_fotos(5, label_Imagen6, "clase");
                pictureBoxImage7.Image = GetImageByName(ListaImagenes[6]);
                poner_nombre_fotos(6, label_Imagen7, "clase");
                pictureBoxImage8.Image = GetImageByName(ListaImagenes[7]);
                poner_nombre_fotos(7, label_Imagen8, "clase");
                pictureBoxImage9.Image = GetImageByName(ListaImagenes[8]);
                poner_nombre_fotos(8, label_Imagen9, "clase");
                pictureBoxImage10.Image = GetImageByName(ListaImagenes[9]);
                poner_nombre_fotos(9, label_Imagen10, "clase");
                pictureBoxImage11.Image = GetImageByName(ListaImagenes[10]);
                poner_nombre_fotos(10, label_Imagen11, "clase");
                pictureBoxImage12.Image = GetImageByName(ListaImagenes[11]);
                poner_nombre_fotos(11, label_Imagen12, "clase");
                if (sugerirPreguntas == "SI")
                {
                    comboBoxPTChat.Visible = true;
                    comboBoxPTChat.Items.Add("El nombre de la persona empieza por la letra A?");
                    comboBoxPTChat.Items.Add("La persona es rubia?");
                    comboBoxPTChat.Items.Add("La persona lleva gafas?");
                    comboBoxPTChat.Items.Add("La persona es una chica?");
                    comboBoxPTChat.Items.Add("La persona tiene barba?");
                    comboBoxPTChat.Items.Add("La persona tiene el pelo largo?");
                    comboBoxPTChat.Items.Add("La persona tiene la piel morena?");
                    comboBoxPTChat.Items.Add("La persona tiene pircing/s?");
                }
            }
            else
            {
                for (int i = 1; i < 13; i++)
                {
                    ListaImagenes[i - 1] = "pais_" + ListaRandom[i];
                }
                pictureBoxImage1.Image = GetImageByName(ListaImagenes[0]);
                poner_nombre_fotos(0, label_Imagen1, "pais");
                pictureBoxImage2.Image = GetImageByName(ListaImagenes[1]);
                poner_nombre_fotos(1, label_Imagen2, "pais");
                pictureBoxImage3.Image = GetImageByName(ListaImagenes[2]);
                poner_nombre_fotos(2, label_Imagen3, "pais");
                pictureBoxImage4.Image = GetImageByName(ListaImagenes[3]);
                poner_nombre_fotos(3, label_Imagen4, "pais");
                pictureBoxImage5.Image = GetImageByName(ListaImagenes[4]);
                poner_nombre_fotos(4, label_Imagen5, "pais");
                pictureBoxImage6.Image = GetImageByName(ListaImagenes[5]);
                poner_nombre_fotos(5, label_Imagen6, "pais");
                pictureBoxImage7.Image = GetImageByName(ListaImagenes[6]);
                poner_nombre_fotos(6, label_Imagen7, "pais");
                pictureBoxImage8.Image = GetImageByName(ListaImagenes[7]);
                poner_nombre_fotos(7, label_Imagen8, "pais");
                pictureBoxImage9.Image = GetImageByName(ListaImagenes[8]);
                poner_nombre_fotos(8, label_Imagen9, "pais");
                pictureBoxImage10.Image = GetImageByName(ListaImagenes[9]);
                poner_nombre_fotos(9, label_Imagen10, "pais");
                pictureBoxImage11.Image = GetImageByName(ListaImagenes[10]);
                poner_nombre_fotos(10, label_Imagen11, "pais");
                pictureBoxImage12.Image = GetImageByName(ListaImagenes[11]);
                poner_nombre_fotos(11, label_Imagen12, "pais");
                if (sugerirPreguntas == "SI")
                {
                    comboBoxPTChat.Visible = true;
                    comboBoxPTChat.Items.Add("El pais esta en Europa?");
                    comboBoxPTChat.Items.Add("En el pais se habla Español?");
                    comboBoxPTChat.Items.Add("El pais es frio?");
                    comboBoxPTChat.Items.Add("El pais es desertico?");
                    comboBoxPTChat.Items.Add("El pais se rige por una monarquia parlamntaria?");
                    comboBoxPTChat.Items.Add("La moneda del pais es el Euro?");
                    comboBoxPTChat.Items.Add("Es un pais subdesarrollado?");
                    comboBoxPTChat.Items.Add("La bandera del pais contiene el color rojo?");
                }
            }
            groupBoxPTTableroContrincante.Text = "Tablero de " + rival;
            //MessageBox.Show("Escoge una carta");
            Star_Stop = 1;
            timerTurno.Start();

         

        }


        //////////
        //FUNCIONES
        //////////

        public void RespuestaInvitacion(string nombreInvitado, string SiNo)
        {
            if (SiNo == "Si")
            {
                tabControlPartida.Invoke(new DelegadoParaCambiarTab(CambiarTab), new object[] { });

            }
            else
            {
                labelPE.Invoke(new DelegadoParaEscribirLabel(EscribirLabel), new object[] { nombreInvitado + " ha rechazado tu invitación", labelPE });
                pictureBoxPEGif.Image = Properties.Resources.Cross;
                pictureBoxPEBoton.Invoke(new DelegadoPicureBox(PonNoVisiblePictureBox), new object[] { pictureBoxPEBoton });
            }
        }
        public void PrepararTiempo_Turno ()
        {
            
            //label_turno.Invoke(new DelegadoParaVisibleLabel (PonVisibleONOLabel), new object[] { label_turno, true });
            //label_tiempo.Invoke(new DelegadoParaVisibleLabel(PonVisibleONOLabel), new object[] { label_tiempo, true });
            //label_turno.Invoke(new DelegadoParaEscribirLabel(EscribirLabel), new object[] { creador_partida, label_turno});
            //label_tiempo.Invoke(new DelegadoParaEscribirLabel(EscribirLabel), new object[] { limiteTiempo, label_tiempo});
            //label2.Invoke(new DelegadoParaVisibleLabel(PonVisibleONOLabel), new object[] { label2, true });
            //label3.Invoke(new DelegadoParaVisibleLabel(PonVisibleONOLabel), new object[] { label3, true });
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
            conversacion.Items.Insert(0, rival + ": " + mensaje);
        }
        private void button_enviar_Click(object sender, EventArgs e)
        {
            if (inicio_partida == true)
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
                    CambiarTurno();
                }
                else
                {
                    //Poner aqui que aparezca arriba un label que diga que escriba algo
                }
                
            }
            else
            {
                MessageBox.Show("Espera a que empiece la partida");
            }
        }
        public void poner_nombre_fotos(int i, Label label, string tipo)
        {
            if (tipo == "animal")
            {
                if (ListaImagenes[i] == "animal_0")
                {
                    EscribirLabel("Mariposa", label);
                }
                else if (ListaImagenes[i] == "animal_1")
                {
                    EscribirLabel("Murcielago", label);
                }
                else if (ListaImagenes[i] == "animal_2")
                {
                    EscribirLabel("Oso", label);
                }
                else if (ListaImagenes[i] == "animal_3")
                {
                    EscribirLabel("Camello", label);
                }
                else if (ListaImagenes[i] == "animal_4")
                {
                    EscribirLabel("Camaleon", label);
                }
                else if (ListaImagenes[i] == "animal_5")
                {
                    EscribirLabel("Cangrejo", label);
                }
                else if (ListaImagenes[i] == "animal_6")
                {
                    EscribirLabel("Elefante", label);
                }
                else if (ListaImagenes[i] == "animal_7")
                {
                    EscribirLabel("Flamenco", label);
                }
                else if (ListaImagenes[i] == "animal_8")
                {
                    EscribirLabel("Zorro", label);
                }
                else if (ListaImagenes[i] == "animal_9")
                {
                    EscribirLabel("Rana", label);
                }
                else if (ListaImagenes[i] == "animal_10")
                {
                    EscribirLabel("Girafa", label);
                }
                else if (ListaImagenes[i] == "animal_11")
                {
                    EscribirLabel("Erizo", label);
                }
                else if (ListaImagenes[i] == "animal_12")
                {
                    EscribirLabel("Canguro", label);
                }
                else if (ListaImagenes[i] == "animal_13")
                {
                    EscribirLabel("Leon", label);
                }
                else if (ListaImagenes[i] == "animal_14")
                {
                    EscribirLabel("Mono", label);
                }
                else if (ListaImagenes[i] == "animal_15")
                {
                    EscribirLabel("Raton", label);
                }
                else if (ListaImagenes[i] == "animal_16")
                {
                    EscribirLabel("Pulpo", label);
                }
                else if (ListaImagenes[i] == "animal_17")
                {
                    EscribirLabel("Pingüino", label);
                }
                else if (ListaImagenes[i] == "animal_18")
                {
                    EscribirLabel("Cerdo", label);
                }
                else if (ListaImagenes[i] == "animal_19")
                {
                    EscribirLabel("Conejo", label);
                }
                else if (ListaImagenes[i] == "animal_20")
                {
                    EscribirLabel("Rinoceronte", label);
                }
                else if (ListaImagenes[i] == "animal_21")
                {
                    EscribirLabel("Tiburon", label);
                }
                else if (ListaImagenes[i] == "animal_22")
                {
                    EscribirLabel("Perezoso", label);
                }
                else if (ListaImagenes[i] == "animal_23")
                {
                    EscribirLabel("Caracol", label);
                }
                else if (ListaImagenes[i] == "animal_24")
                {
                    EscribirLabel("Serpiente", label);
                }
                else if (ListaImagenes[i] == "animal_25")
                {
                    EscribirLabel("Ballena", label);
                }
            }
            if (tipo == "pais")
            {
                if (ListaImagenes[i] == "pais_0")
                {
                    EscribirLabel("Grecia", label);
                }
                else if (ListaImagenes[i] == "pais_1")
                {
                    EscribirLabel("Afghanistan", label);
                }
                else if (ListaImagenes[i] == "pais_2")
                {
                    EscribirLabel("Andorra", label);
                }
                else if (ListaImagenes[i] == "pais_3")
                {
                    EscribirLabel("Argentina", label);
                }
                else if (ListaImagenes[i] == "pais_4")
                {
                    EscribirLabel("Australia", label);
                }
                else if (ListaImagenes[i] == "pais_5")
                {
                    EscribirLabel("Belgica", label);
                }
                else if (ListaImagenes[i] == "pais_6")
                {
                    EscribirLabel("Brasil", label);
                }
                else if (ListaImagenes[i] == "pais_7")
                {
                    EscribirLabel("Canada", label);
                }
                else if (ListaImagenes[i] == "pais_8")
                {
                    EscribirLabel("Chile", label);
                }
                else if (ListaImagenes[i] == "pais_9")
                {
                    EscribirLabel("China", label);
                }
                else if (ListaImagenes[i] == "pais_10")
                {
                    EscribirLabel("Costa Rica", label);
                }
                else if (ListaImagenes[i] == "pais_11")
                {
                    EscribirLabel("Egipcio", label);
                }
                else if (ListaImagenes[i] == "pais_12")
                {
                    EscribirLabel("Inglaterra", label);
                }
                else if (ListaImagenes[i] == "pais_13")
                {
                    EscribirLabel("Francia", label);
                }
                else if (ListaImagenes[i] == "pais_14")
                {
                    EscribirLabel("Alemania", label);
                }
                else if (ListaImagenes[i] == "pais_15")
                {
                    EscribirLabel("India", label);
                }
                else if (ListaImagenes[i] == "pais_16")
                {
                    EscribirLabel("Italia", label);
                }
                else if (ListaImagenes[i] == "pais_17")
                {
                    EscribirLabel("Japon", label);
                }
                else if (ListaImagenes[i] == "pais_18")
                {
                    EscribirLabel("Mexico", label);
                }
                else if (ListaImagenes[i] == "pais_19")
                {
                    EscribirLabel("Conejo", label);
                }
                else if (ListaImagenes[i] == "pais_20")
                {
                    EscribirLabel("Rinoceronte", label);
                }
                else if (ListaImagenes[i] == "pais_21")
                {
                    EscribirLabel("Tiburon", label);
                }
                else if (ListaImagenes[i] == "pais_22")
                {
                    EscribirLabel("Perezoso", label);
                }
                else if (ListaImagenes[i] == "pais_23")
                {
                    EscribirLabel("Caracol", label);
                }
                else if (ListaImagenes[i] == "pais_24")
                {
                    EscribirLabel("Serpiente", label);
                }
                else if (ListaImagenes[i] == "pais_25")
                {
                    EscribirLabel("Ballena", label);
                }
            }
            if (tipo == "clase")
            {
                if (ListaImagenes[i] == "clase_0")
                {
                    EscribirLabel("Genís", label);
                }
                else if (ListaImagenes[i] == "clase_1")
                {
                    EscribirLabel("Guillem", label);
                }
                else if (ListaImagenes[i] == "clase_2")
                {
                    EscribirLabel("Alba", label);
                }
                else if (ListaImagenes[i] == "clase_3")
                {
                    EscribirLabel("Alex", label);
                }
                else if (ListaImagenes[i] == "clase_4")
                {
                    EscribirLabel("Angela", label);
                }
                else if (ListaImagenes[i] == "clase_5")
                {
                    EscribirLabel("Arnau", label);
                }
                else if (ListaImagenes[i] == "clase_6")
                {
                    EscribirLabel("Biel", label);
                }
                else if (ListaImagenes[i] == "clase_7")
                {
                    EscribirLabel("Carmen", label);
                }
                else if (ListaImagenes[i] == "clase_8")
                {
                    EscribirLabel("Ismael", label);
                }
                else if (ListaImagenes[i] == "clase_9")
                {
                    EscribirLabel("Itziar", label);
                }
                else if (ListaImagenes[i] == "clase_10")
                {
                    EscribirLabel("Jonathan", label);
                }
                else if (ListaImagenes[i] == "clase_11")
                {
                    EscribirLabel("Miguel", label);
                }
                else if (ListaImagenes[i] == "clase_12")
                {
                    EscribirLabel("Paula C.", label);
                }
                else if (ListaImagenes[i] == "clase_13")
                {
                    EscribirLabel("Júlia", label);
                }
                else if (ListaImagenes[i] == "clase_14")
                {
                    EscribirLabel("Maria", label);
                }
                else if (ListaImagenes[i] == "clase_15")
                {
                    EscribirLabel("Paula S.", label);
                }
                else if (ListaImagenes[i] == "clase_16")
                {
                    EscribirLabel("Laia", label);
                }
                else if (ListaImagenes[i] == "clase_17")
                {
                    EscribirLabel("Raúl", label);
                }
                else if (ListaImagenes[i] == "clase_18")
                {
                    EscribirLabel("Pau", label);
                }
                else if (ListaImagenes[i] == "clase_19")
                {
                    EscribirLabel("Victor F.", label);
                }
                else if (ListaImagenes[i] == "clase_20")
                {
                    EscribirLabel("Marc", label);
                }
                else if (ListaImagenes[i] == "clase_21")
                {
                    EscribirLabel("Xavier", label);
                }
                else if (ListaImagenes[i] == "clase_22")
                {
                    EscribirLabel("Borja", label);
                }
                else if (ListaImagenes[i] == "clase_23")
                {
                    EscribirLabel("Victòria", label);
                }
            }
        }

        internal void CambiarColorTableroContrincante(string num)
        {
            string textBoxName = "pictureBoxPTTableroContrincante" + num;
            b = Controls.Find(textBoxName, true);
            if (b[0].BackColor == Color.FromArgb(127, 64, 230))
            {
                b[0].BackColor = Color.FromArgb(0, 0, 129);
            }
            else
            {
                b[0].BackColor = Color.FromArgb(127, 64, 230);
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

        private void EnviarNumCartaServer(int numeroCarta)
        {
            // Envias el mensaje
            int numerCarta = numeroCarta + 1;
            string mensaje = "46/" + id_partida + "/" + nombreUsuario + "/" + numerCarta;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        PictureBox CartasName;
        int numCarta;
        int Opacidad=500;
        int siguiente;
        Image ImageCarta;
        private Control[] b;
        double  a = 1;
        double segundos = 0;


        private void timerFlip_Tick_1(object sender, EventArgs e)
        {
            

            if ((Opacidad > 0) && (siguiente == 0))
            {
                CartasName.Image = SetAlpha((Bitmap)CartasName.Image, Opacidad);
                Opacidad -= 50;
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
                Opacidad += 50;
            }
            if (Opacidad>=500)
            {
                timerFlip.Stop();
            }
            

        }

        private void pictureBoxImage1_Click(object sender, EventArgs e)
        {
            if (carta_inicial != 3)
            {
                if (carta_inicial == 0)
                {
                    id_carta = 0;
                    DialogResult r = MessageBox.Show("Estas seguro", ":", MessageBoxButtons.YesNo);
                    if (r == DialogResult.Yes)
                    {
                        mensaje_not = "48/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + id_carta;
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                        server.Send(msg);
                        carta_inicial = 2;

                    }
                    else
                    {
                        MessageBox.Show("Escoge otra carta, porfi UwU");
                    }
                }
                if (Opacidad >= 500 && carta_inicial == 1)
                {
                    numCarta = 0;
                    if (!UPCCarta2[numCarta])
                        UPCCarta2[numCarta] = true;
                    else
                        UPCCarta2[numCarta] = false;
                    Opacidad = 500;
                    siguiente = 0;
                    CartasName = pictureBoxImage1;
                    ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                    EnviarNumCartaServer(numCarta);
                    timerFlip.Start();
                }

                if (carta_inicial == 2)
                {
                    carta_inicial = 1;
                }

            }
            else
            {
                carta_seleccionada = id_carta;
                if (0 == id_carta_rival)
                {
                    MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                    carta_seleccionada = -1;
                    carta_inicial = 1;
                }
                else
                {
                    vidas = vidas - 1;
                    MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + "vidas");
                    if (vidas == 0)
                        MessageBox.Show("Has perdido");
                    carta_seleccionada = -1;
                    carta_inicial = 1;
                    groupBoxChat.Visible = true;
                }
            }
        }

        private void pictureBoxImage2_Click(object sender, EventArgs e)
        {
            if (carta_inicial != 3)
            {
                if (carta_inicial == 0)
                {
                    id_carta = 1;
                    DialogResult r = MessageBox.Show("Estas seguro", ":", MessageBoxButtons.YesNo);
                    if (r == DialogResult.Yes)
                    {
                        mensaje_not = "48/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + id_carta;
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                        server.Send(msg);
                        carta_inicial = 2;

                    }
                    else
                    {
                        MessageBox.Show("Escoge otra carta, porfi UwU");
                    }
                }

                if (Opacidad >= 500 && carta_inicial == 1)
                {
                    numCarta = 1;
                    if (!UPCCarta2[numCarta])
                        UPCCarta2[numCarta] = true;
                    else
                        UPCCarta2[numCarta] = false;
                    Opacidad = 500;
                    siguiente = 0;
                    CartasName = pictureBoxImage2;
                    ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                    EnviarNumCartaServer(numCarta);
                    timerFlip.Start();
                }
                if (carta_inicial == 2)
                {
                    carta_inicial = 1;
                }
            }
            else
            {
               
                if (1 == id_carta_rival)
                {
                    MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                    carta_seleccionada = -1;
                }
                else
                {
                    vidas = vidas - 1;
                    MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + "vidas");
                    if (vidas == 0)
                        MessageBox.Show("Has perdido");
                    carta_seleccionada = -1;
                    groupBoxChat.Visible = true;
                }
            }
        }

        private void pictureBoxImage3_Click(object sender, EventArgs e)
        {
            if (carta_inicial != 3)
            {
                if (carta_inicial == 0)
                {
                    id_carta = 2;
                    DialogResult r = MessageBox.Show("Estas seguro", ":", MessageBoxButtons.YesNo);
                    if (r == DialogResult.Yes)
                    {
                        mensaje_not = "48/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + id_carta;
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                        server.Send(msg);
                        carta_inicial = 2;

                    }
                    else
                    {
                        MessageBox.Show("Escoge otra carta, porfi UwU");
                    }
                }
                if (Opacidad >= 500 && carta_inicial == 1)
                {
                    numCarta = 2;
                    if (!UPCCarta2[numCarta])
                        UPCCarta2[numCarta] = true;
                    else
                        UPCCarta2[numCarta] = false;
                    Opacidad = 500;
                    siguiente = 0;
                    CartasName = pictureBoxImage3;
                    ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                    EnviarNumCartaServer(numCarta);
                    timerFlip.Start();
                }
                if (carta_inicial == 2)
                {
                    carta_inicial = 1;
                }
            }
            else
            {
                carta_seleccionada = id_carta;
                if (2 == id_carta_rival)
                {
                    MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                    carta_seleccionada = -1;
                }
                else
                {
                    vidas = vidas - 1;
                    MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + "vidas");
                    if (vidas == 0)
                        MessageBox.Show("Has perdido");
                    carta_seleccionada = -1;
                    groupBoxChat.Visible = true;
                }
            }
        }

        private void pictureBoxImage4_Click(object sender, EventArgs e)
        {
            if (carta_inicial != 3)
            {


                if (carta_inicial == 0)
                {
                    id_carta = 3;
                    DialogResult r = MessageBox.Show("Estas seguro", ":", MessageBoxButtons.YesNo);
                    if (r == DialogResult.Yes)
                    {
                        mensaje_not = "48/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + id_carta;
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                        server.Send(msg);
                        carta_inicial = 2;

                    }
                    else
                    {
                        MessageBox.Show("Escoge otra carta, porfi UwU");
                    }
                }
                if (Opacidad >= 500 && carta_inicial == 1)
                {
                    segundos = a;
                    numCarta = 3;
                    if (!UPCCarta2[numCarta])
                        UPCCarta2[numCarta] = true;
                    else
                        UPCCarta2[numCarta] = false;
                    Opacidad = 500;
                    siguiente = 0;
                    CartasName = pictureBoxImage4;
                    ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                    EnviarNumCartaServer(numCarta);
                    timerFlip.Start();
                }
                if (carta_inicial == 2)
                {
                    carta_inicial = 1;
                }
            }
            else
            {
                carta_seleccionada = id_carta;
                if (3 == id_carta_rival)
                {
                    MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                    carta_seleccionada = -1;
                }
                else
                {
                    vidas = vidas - 1;
                    MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + "vidas");
                    if (vidas == 0)
                        MessageBox.Show("Has perdido");
                    carta_seleccionada = -1;
                    groupBoxChat.Visible = true;
                }
            }
        }

        private void pictureBoxImage5_Click(object sender, EventArgs e)
        {
            if (carta_inicial != 3)
            {
                if (carta_inicial == 0)
                {
                    id_carta = 4;
                    DialogResult r = MessageBox.Show("Estas seguro", ":", MessageBoxButtons.YesNo);
                    if (r == DialogResult.Yes)
                    {
                        mensaje_not = "48/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + id_carta;
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                        server.Send(msg);
                        carta_inicial = 2;

                    }
                    else
                    {
                        MessageBox.Show("Escoge otra carta, porfi UwU");
                    }
                }
                if (Opacidad >= 500 && carta_inicial == 1)
                {
                    segundos = a;
                    numCarta = 4;
                    if (!UPCCarta2[numCarta])
                        UPCCarta2[numCarta] = true;
                    else
                        UPCCarta2[numCarta] = false;
                    Opacidad = 500;
                    siguiente = 0;
                    CartasName = pictureBoxImage5;
                    ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                    EnviarNumCartaServer(numCarta);
                    timerFlip.Start();
                }
                if (carta_inicial == 2)
                {
                    carta_inicial = 1;
                }
            }
            else
            {
                carta_seleccionada = id_carta;
                if (4== id_carta_rival)
                {
                    MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                    carta_seleccionada = -1;
                }
                else
                {
                    vidas = vidas - 1;
                    MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + "vidas");
                    if (vidas == 0)
                        MessageBox.Show("Has perdido");
                    carta_seleccionada = -1;
                    groupBoxChat.Visible = true;
                }
            }
        }

        private void pictureBoxImage6_Click(object sender, EventArgs e)
        {
            if (carta_inicial != 3)
            {


                if (carta_inicial == 0)
                {
                    id_carta = 5;
                    DialogResult r = MessageBox.Show("Estas seguro", ":", MessageBoxButtons.YesNo);
                    if (r == DialogResult.Yes)
                    {
                        mensaje_not = "48/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + id_carta;
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                        server.Send(msg);
                        carta_inicial = 2;

                    }
                    else
                    {
                        MessageBox.Show("Escoge otra carta, porfi UwU");
                    }
                }

                if (Opacidad >= 500 && carta_inicial == 1)
                {
                    numCarta = 5;
                    if (!UPCCarta2[numCarta])
                        UPCCarta2[numCarta] = true;
                    else
                        UPCCarta2[numCarta] = false;
                    Opacidad = 500;
                    siguiente = 0;
                    CartasName = pictureBoxImage6;
                    ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                    EnviarNumCartaServer(numCarta);
                    timerFlip.Start();
                }
                if (carta_inicial == 2)
                {
                    carta_inicial = 1;
                }
            }
            else
            {
                carta_seleccionada = id_carta;
                if (5 == id_carta_rival)
                {
                    MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                    carta_seleccionada = -1;
                }
                else
                {
                    vidas = vidas - 1;
                    MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + "vidas");
                    if (vidas == 0)
                        MessageBox.Show("Has perdido");
                    carta_seleccionada = -1;
                    groupBoxChat.Visible = true;
                }
            }
        }

        private void pictureBoxImage7_Click(object sender, EventArgs e)
        {
            if (carta_inicial != 3)
            {


                if (carta_inicial == 0)
                {
                    id_carta = 6;
                    DialogResult r = MessageBox.Show("Estas seguro", ":", MessageBoxButtons.YesNo);
                    if (r == DialogResult.Yes)
                    {
                        mensaje_not = "48/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + id_carta;
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                        server.Send(msg);
                        carta_inicial = 2;

                    }
                    else
                    {
                        MessageBox.Show("Escoge otra carta, porfi UwU");
                    }
                }
                if (Opacidad >= 500 && carta_inicial == 1)
                {
                    numCarta = 6;
                    if (!UPCCarta2[numCarta])
                        UPCCarta2[numCarta] = true;
                    else
                        UPCCarta2[numCarta] = false;
                    Opacidad = 500;
                    siguiente = 0;
                    CartasName = pictureBoxImage7;
                    ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                    EnviarNumCartaServer(numCarta);
                    timerFlip.Start();
                }
                if (carta_inicial == 2)
                {
                    carta_inicial = 1;
                }
            }
            else
            {
                carta_seleccionada = id_carta;
                if (6 == id_carta_rival)
                {
                    MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                    carta_seleccionada = -1;
                }
                else
                {
                    vidas = vidas - 1;
                    MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + "vidas");
                    if (vidas == 0)
                        MessageBox.Show("Has perdido");
                    carta_seleccionada = -1;
                    groupBoxChat.Visible = true;
                }
            }
        }

        private void pictureBoxImage8_Click(object sender, EventArgs e)
        {
            if (carta_inicial != 3)
            {
                if (carta_inicial == 0)
                {
                    id_carta = 7;
                    DialogResult r = MessageBox.Show("Estas seguro", ":", MessageBoxButtons.YesNo);
                    if (r == DialogResult.Yes)
                    {
                        mensaje_not = "48/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + id_carta;
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                        server.Send(msg);
                        carta_inicial = 2;

                    }
                    else
                    {
                        MessageBox.Show("Escoge otra carta, porfi UwU");
                    }
                }
                if (Opacidad >= 500 && carta_inicial == 1)
                {
                    numCarta = 7;
                    if (!UPCCarta2[numCarta])
                        UPCCarta2[numCarta] = true;
                    else
                        UPCCarta2[numCarta] = false;
                    Opacidad = 500;
                    siguiente = 0;
                    CartasName = pictureBoxImage8;
                    ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                    EnviarNumCartaServer(numCarta);
                    timerFlip.Start();
                }
                if (carta_inicial == 2)
                {
                    carta_inicial = 1;
                }
            }
            else
            {
                carta_seleccionada = id_carta;
                if (7 == id_carta_rival)
                {
                    MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                    carta_seleccionada = -1;
                }
                else
                {
                    vidas = vidas - 1;
                    MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + "vidas");
                    if (vidas == 0)
                        MessageBox.Show("Has perdido");
                    carta_seleccionada = -1;
                    groupBoxChat.Visible = true;
                }
            }
        }

        private void pictureBoxImage9_Click(object sender, EventArgs e)
        {
            if (carta_inicial != 3)
            {
                if (carta_inicial == 0)
                {
                    id_carta = 8;
                    DialogResult r = MessageBox.Show("Estas seguro", ":", MessageBoxButtons.YesNo);
                    if (r == DialogResult.Yes)
                    {
                        mensaje_not = "48/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + id_carta;
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                        server.Send(msg);
                        carta_inicial = 2;

                    }
                    else
                    {
                        MessageBox.Show("Escoge otra carta, porfi UwU");
                    }
                }
                if (Opacidad >= 500 && carta_inicial == 1)
                {
                    numCarta = 8;
                    if (!UPCCarta2[numCarta])
                        UPCCarta2[numCarta] = true;
                    else
                        UPCCarta2[numCarta] = false;
                    Opacidad = 500;
                    siguiente = 0;
                    CartasName = pictureBoxImage9;
                    ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                    EnviarNumCartaServer(numCarta);
                    timerFlip.Start();
                }
                if (carta_inicial == 2)
                {
                    carta_inicial = 1;
                }
            }
            else
            {
                carta_seleccionada = id_carta;
                if (8 == id_carta_rival)
                {
                    MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                    carta_seleccionada = -1;
                }
                else
                {
                    vidas = vidas - 1;
                    MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + "vidas");
                    if (vidas == 0)
                        MessageBox.Show("Has perdido");
                    carta_seleccionada = -1;
                    groupBoxChat.Visible = true;
                }
            }
        }

        private void pictureBoxImage10_Click(object sender, EventArgs e)
        {
            if (carta_inicial != 3)
            {
                if (carta_inicial == 0)
                {
                    id_carta = 9;
                    DialogResult r = MessageBox.Show("Estas seguro", ":", MessageBoxButtons.YesNo);
                    if (r == DialogResult.Yes)
                    {
                        mensaje_not = "48/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + id_carta;
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                        server.Send(msg);
                        carta_inicial = 2;

                    }
                    else
                    {
                        MessageBox.Show("Escoge otra carta, porfi UwU");
                    }
                }
                if (Opacidad >= 500 && carta_inicial == 1)
                {
                    numCarta = 9;
                    if (!UPCCarta2[numCarta])
                        UPCCarta2[numCarta] = true;
                    else
                        UPCCarta2[numCarta] = false;
                    Opacidad = 500;
                    siguiente = 0;
                    CartasName = pictureBoxImage10;
                    ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                    EnviarNumCartaServer(numCarta);
                    timerFlip.Start();
                }
                if (carta_inicial == 2)
                {
                    carta_inicial = 1;
                }
            }
            else
            {
                carta_seleccionada = id_carta;
                if (9 == id_carta_rival)
                {
                    MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                    carta_seleccionada = -1;
                }
                else
                {
                    vidas = vidas - 1;
                    MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + "vidas");
                    if (vidas == 0)
                        MessageBox.Show("Has perdido");
                    carta_seleccionada = -1;
                    groupBoxChat.Visible = true;
                }
            }
        }

        private void pictureBoxImage11_Click(object sender, EventArgs e)
        {
            if (carta_inicial != 3)
            {


                if (carta_inicial == 0)
                {
                    id_carta = 10;
                    DialogResult r = MessageBox.Show("Estas seguro", ":", MessageBoxButtons.YesNo);
                    if (r == DialogResult.Yes)
                    {
                        mensaje_not = "48/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + id_carta;
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                        server.Send(msg);
                        carta_inicial = 2;

                    }
                    else
                    {
                        MessageBox.Show("Escoge otra carta, porfi UwU");
                    }
                }
                if (Opacidad >= 500 && carta_inicial == 1)
                {
                    numCarta = 10;
                    if (!UPCCarta2[numCarta])
                        UPCCarta2[numCarta] = true;
                    else
                        UPCCarta2[numCarta] = false;
                    Opacidad = 500;
                    siguiente = 0;
                    CartasName = pictureBoxImage11;
                    ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                    EnviarNumCartaServer(numCarta);
                    timerFlip.Start();
                }
                if (carta_inicial == 2)
                {
                    carta_inicial = 1;
                }
            }
            else
            {
                carta_seleccionada = id_carta;
                if (10 == id_carta_rival)
                {
                    MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                    carta_seleccionada = -1;
                }
                else
                {
                    vidas = vidas - 1;
                    MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + "vidas");
                    if (vidas == 0)
                        MessageBox.Show("Has perdido");
                    carta_seleccionada = -1;
                    groupBoxChat.Visible = true;
                }
            }
        }

        private void pictureBoxImage12_Click(object sender, EventArgs e)
        {
            if (carta_inicial != 3)
            {


                if (carta_inicial == 0)
                {
                    id_carta = 11;
                    DialogResult r = MessageBox.Show("Estas seguro", ":", MessageBoxButtons.YesNo);
                    if (r == DialogResult.Yes)
                    {
                        mensaje_not = "48/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + id_carta;
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                        server.Send(msg);
                        carta_inicial = 2;

                    }
                    else
                    {
                        MessageBox.Show("Escoge otra carta, porfi UwU");
                    }
                }
                if (Opacidad >= 500 && carta_inicial == 1)
                {
                    numCarta = 11;
                    if (!UPCCarta2[numCarta])
                        UPCCarta2[numCarta] = true;
                    else
                        UPCCarta2[numCarta] = false;
                    Opacidad = 500;
                    siguiente = 0;
                    CartasName = pictureBoxImage12;
                    ImageCarta = GetImageByName(ListaImagenes[numCarta]);
                    EnviarNumCartaServer(numCarta);
                    timerFlip.Start();
                }
                if (carta_inicial == 2)
                {
                    carta_inicial = 1;
                }
            }
            else
            {
                carta_seleccionada = id_carta;
                if (11 == id_carta_rival)
                {
                    MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                    //mensaje el otro tiene que probar suerte
                    carta_seleccionada = -1;
                }
                else
                {
                    vidas = vidas - 1;
                    MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + "vidas");
                    if (vidas == 0)
                        MessageBox.Show("Has perdido");
                    //mensaje ha ganado el otro
                    carta_seleccionada = -1;
                    groupBoxChat.Visible = true;
                }
            }
        }


        private void Partida_Load(object sender, EventArgs e)
        {
            Stop.Start();
            conteo = Convert.ToInt32(limiteTiempo);
            groupBoxChat.Visible = false;
            label_turno.Visible = true;
            label_tiempo.Visible = true;
            label_turno.Text = creador_partida;
            label_tiempo.Text = limiteTiempo;
            label2.Visible = true;
            label3.Visible = true;
        }

        private void comboBoxPTChat_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_con.Text = comboBoxPTChat.Text;
        }

        private void timerTurno_Tick(object sender, EventArgs e)
        {
            int x = 0;
            conteo = conteo - 1;
            label_tiempo.Text = conteo.ToString();

            if (carta_inicial != 1 && conteo == 0)
            {
                x = x + 1;
                if (x == 2)
                {
                    //Ponerle carta random
                }
                else
                {
                    MessageBox.Show("Escoge una carta,tienes 10 segundos");
                    conteo = 10;
                    tiempo = true;
                }
            }
            if (carta_inicial == 1 && conteo == 0 && inicio_partida==false)
            {
                MessageBox.Show("Esperando al rival");
                conteo = Convert.ToInt32(limiteTiempo);
                tiempo = true;
            }
            if (conteo <= 0)
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
            if (inicio_partida)
            {
                if (turno==true)
                {
                    groupBoxChat.Visible = true;
                    conversacion.Visible = true;

                    comboBoxPTChat.Visible = false;
                    button_Nose.Visible = true;
                    button_Si.Visible = true;
                    button_No.Visible = true;
                    textBox_con.Visible = false;
                    button_enviar.Visible =false;
                }
                else
                {
                    groupBoxChat.Visible = true;
                    conversacion.Visible = true;

                    comboBoxPTChat.Visible = true;
                    button_Nose.Visible = false;
                    button_No.Visible = false;
                    button_Si.Visible = false;
                    textBox_con.Visible = true;
                    button_enviar.Visible = true;
                }
            }
           
        }

        private void button_Si_Click(object sender, EventArgs e)
        {
            if (inicio_partida == true)
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
            CambiarTurno();
        }

        private void button_No_Click(object sender, EventArgs e)
        {
            if (inicio_partida == true)
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
            CambiarTurno();
        }

        private void button_Nose_Click(object sender, EventArgs e)
        {
            if (inicio_partida == true)
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
            CambiarTurno();
        }

        private void pictureBoxPEBoton_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxPEBoton.Image = SetAlpha((Bitmap)pictureBoxPEBoton.Image, 150);
        }

        private void pictureBoxPEBoton_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxPEBoton.Image = SetAlpha((Bitmap)pictureBoxPEBoton.Image, 1000);
        }
        public void IniciarPartida(string carta)
        {
            
            this.id_carta_rival= Convert.ToInt32(carta);
            inicio_partida = true;
            MessageBox.Show("Carta rival :" + id_carta_rival);
            if (nombreUsuario == creador_partida)
            {
                turno = true;
            }
            else
            {
                turno = false;
            }
        }
        public void CambiarTurno()
        {
            if (turno==true)
            {
                turno = false;
            }
            else
            {
                turno = true;
            }
            // Envias el mensaje
            string mensaje = "47/" + Nform + "/" + id_partida + "/YA";
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            PrepararTiempo_Turno();
        }

        private void groupBoxChat_Enter(object sender, EventArgs e)
        {

        }

        private void Boton_respuesta_Click(object sender, EventArgs e)
        {
            groupBoxChat.Visible = false;
            carta_inicial = 3;
            MessageBox.Show("Escoge la carta del rival");

        }
    }
}


