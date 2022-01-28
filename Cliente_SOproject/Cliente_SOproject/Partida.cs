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
        //El numero de formularios que tendrá el cliente
        int Nform;
        // El soquet tendrá el nombre de server
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
        public string[] ListaNombres = { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a" };
        public string[] ListaImagenes = { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a" };
        string mensaje_not;
        int id_carta_rival;
        int carta_seleccionada=-1;
        int vidas = 3;
        int seguridad=1;
        string resultado;
        int final = 0;
        int intentos;
        int rondas=0;
        int x = 0;
        public int bloqueo_turno = 0;
        public int y=0;
        public int terminado=0;

        private Menu FormMenu;

        static readonly object _object = new object();

        delegate void DelegadoParaEscribir(string rival, string texto);
        delegate void DelegadoParaEscribirLabel(string msn, Label nameLabel);
        delegate void DelegadoPicureBox(PictureBox namePictureBox);
        delegate void DelegadoParaCambiarTab();
        delegate void DelegadoParaEscribir2(string nombre);
        delegate void DelegadoParaVisibleLabel(Label nameLabel, bool SINO);

        //Este es el contructor del form con el nombre "Partida" que tiene todos estos parametros puestos aquí
        public Partida(int Nform, Socket server, string nombreUsuario,
            int id_partida, string nivel, string sugerirPreguntas, string mapa,
            string limitePreguntas, string limiteTiempo, string creador_partida, string invitado, Menu FormMenu)
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
            this.nombreInvitado = invitado;
            this.girar = 0;
            this.FormMenu = FormMenu;


        }

        //Funciones de los delegados
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

        //Esta función se utiliza para cambiar al estado de partida iniciada del Form, cambia el Tab de la pagina y establece los datos de las fotos
        public void CambiarTab()
        {
            comboBoxPTChat.Text = "Extienda la lista para seleccionar pregunta";
            tabControlPartida.SelectedTab = tabPageTablero;
            if (mapa == "ANIMALES")
            {

                for (int i = 1; i < 13; i++)
                {
                    ListaImagenes[i - 1] = "animal_" + ListaRandom[i];
                    ListaNombres[i - 1] = "animal_nombre_" + ListaRandom[i];
                }

                pictureBoxImage1.Image = GetImageByName(ListaImagenes[0]);
                pictureBoxNombre1.Image = GetImageByName(ListaNombres[0]);

                pictureBoxImage2.Image = GetImageByName(ListaImagenes[1]);
                pictureBoxNombre2.Image = GetImageByName(ListaNombres[1]);

                pictureBoxImage3.Image = GetImageByName(ListaImagenes[2]);
                pictureBoxNombre3.Image = GetImageByName(ListaNombres[2]);

                pictureBoxImage4.Image = GetImageByName(ListaImagenes[3]);
                pictureBoxNombre4.Image = GetImageByName(ListaNombres[3]);

                pictureBoxImage5.Image = GetImageByName(ListaImagenes[4]);
                pictureBoxNombre5.Image = GetImageByName(ListaNombres[4]);

                pictureBoxImage6.Image = GetImageByName(ListaImagenes[5]);
                pictureBoxNombre6.Image = GetImageByName(ListaNombres[5]);

                pictureBoxImage7.Image = GetImageByName(ListaImagenes[6]);
                pictureBoxNombre7.Image = GetImageByName(ListaNombres[6]);

                pictureBoxImage8.Image = GetImageByName(ListaImagenes[7]);
                pictureBoxNombre8.Image = GetImageByName(ListaNombres[7]);

                pictureBoxImage9.Image = GetImageByName(ListaImagenes[8]);
                pictureBoxNombre9.Image = GetImageByName(ListaNombres[8]);

                pictureBoxImage10.Image = GetImageByName(ListaImagenes[9]);
                pictureBoxNombre10.Image = GetImageByName(ListaNombres[9]);

                pictureBoxImage11.Image = GetImageByName(ListaImagenes[10]);
                pictureBoxNombre11.Image = GetImageByName(ListaNombres[10]);

                pictureBoxImage12.Image = GetImageByName(ListaImagenes[11]);
                pictureBoxNombre12.Image = GetImageByName(ListaNombres[11]);

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
                    ListaNombres[i - 1] = "clase_nombre_" + ListaRandom[i];
                }

                pictureBoxImage1.Image = GetImageByName(ListaImagenes[0]);
                pictureBoxNombre1.Image = GetImageByName(ListaNombres[0]);
                
                pictureBoxImage2.Image = GetImageByName(ListaImagenes[1]);
                pictureBoxNombre2.Image = GetImageByName(ListaNombres[1]);
                
                pictureBoxImage3.Image = GetImageByName(ListaImagenes[2]);
                pictureBoxNombre3.Image = GetImageByName(ListaNombres[2]);
               
                pictureBoxImage4.Image = GetImageByName(ListaImagenes[3]);
                pictureBoxNombre4.Image = GetImageByName(ListaNombres[3]);
                
                pictureBoxImage5.Image = GetImageByName(ListaImagenes[4]);
                pictureBoxNombre5.Image = GetImageByName(ListaNombres[4]);
                
                pictureBoxImage6.Image = GetImageByName(ListaImagenes[5]);
                pictureBoxNombre6.Image = GetImageByName(ListaNombres[5]);
                
                pictureBoxImage7.Image = GetImageByName(ListaImagenes[6]);
                pictureBoxNombre7.Image = GetImageByName(ListaNombres[6]);
                
                pictureBoxImage8.Image = GetImageByName(ListaImagenes[7]);
                pictureBoxNombre8.Image = GetImageByName(ListaNombres[7]);
                
                pictureBoxImage9.Image = GetImageByName(ListaImagenes[8]);
                pictureBoxNombre9.Image = GetImageByName(ListaNombres[8]);
                
                pictureBoxImage10.Image = GetImageByName(ListaImagenes[9]);
                pictureBoxNombre10.Image = GetImageByName(ListaNombres[9]);
                
                pictureBoxImage11.Image = GetImageByName(ListaImagenes[10]);
                pictureBoxNombre11.Image = GetImageByName(ListaNombres[10]);
                
                pictureBoxImage12.Image = GetImageByName(ListaImagenes[11]);
                pictureBoxNombre12.Image = GetImageByName(ListaNombres[11]);

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
                    ListaNombres[i - 1] = "pais_nombre_" + ListaRandom[i];
                }
                pictureBoxImage1.Image = GetImageByName(ListaImagenes[0]);
                pictureBoxNombre1.Image = GetImageByName(ListaNombres[0]);
                
                pictureBoxImage2.Image = GetImageByName(ListaImagenes[1]);
                pictureBoxNombre2.Image = GetImageByName(ListaNombres[1]);
                
                pictureBoxImage3.Image = GetImageByName(ListaImagenes[2]);
                pictureBoxNombre3.Image = GetImageByName(ListaNombres[2]);
               
                pictureBoxImage4.Image = GetImageByName(ListaImagenes[3]);
                pictureBoxNombre4.Image = GetImageByName(ListaNombres[3]);
                
                pictureBoxImage5.Image = GetImageByName(ListaImagenes[4]);
                pictureBoxNombre5.Image = GetImageByName(ListaNombres[4]);
                
                pictureBoxImage6.Image = GetImageByName(ListaImagenes[5]);
                pictureBoxNombre6.Image = GetImageByName(ListaNombres[5]);
                
                pictureBoxImage7.Image = GetImageByName(ListaImagenes[6]);
                pictureBoxNombre7.Image = GetImageByName(ListaNombres[6]);
                
                pictureBoxImage8.Image = GetImageByName(ListaImagenes[7]);
                pictureBoxNombre8.Image = GetImageByName(ListaNombres[7]);
               
                pictureBoxImage9.Image = GetImageByName(ListaImagenes[8]);
                pictureBoxNombre9.Image = GetImageByName(ListaNombres[8]);
                
                pictureBoxImage10.Image = GetImageByName(ListaImagenes[9]);
                pictureBoxNombre10.Image = GetImageByName(ListaNombres[9]);
               
                pictureBoxImage11.Image = GetImageByName(ListaImagenes[10]);
                pictureBoxNombre11.Image = GetImageByName(ListaNombres[10]);
                
                pictureBoxImage12.Image = GetImageByName(ListaImagenes[11]);
                pictureBoxNombre12.Image = GetImageByName(ListaNombres[11]);
                

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
        public void PrepararTiempo_Turno (int bloqueo_turno)
        {
            this.bloqueo_turno = bloqueo_turno;
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
                   
                }
                else
                {
                    //Poner aqui que aparezca arriba un label que diga que escriba algo
                }
                
            }
            else
            {
                //MessageBox.Show("Espera a que empiece la partida");
                label_info.Text = "Espera a que empiece la partida";
            }
        }
        // Según el tipo de partida elegido y la lista randomizada de numeros, se ponen los nombres debajo de su pictureBox correspondiente. 
        public void poner_nombre_fotos(int i, PictureBox picturebox, string tipo)
        {
            if (tipo == "animal")
            {
                if (ListaImagenes[i] == "animal_0")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_0");
                }
                else if (ListaImagenes[i] == "animal_1")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_1");
                }
                else if (ListaImagenes[i] == "animal_2")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_2");
                }
                else if (ListaImagenes[i] == "animal_3")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_3");
                }
                else if (ListaImagenes[i] == "animal_4")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_4");
                }
                else if (ListaImagenes[i] == "animal_5")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_5");
                }
                else if (ListaImagenes[i] == "animal_6")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_6");
                }
                else if (ListaImagenes[i] == "animal_7")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_7");
                }
                else if (ListaImagenes[i] == "animal_8")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_8");
                }
                else if (ListaImagenes[i] == "animal_9")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_9");
                }
                else if (ListaImagenes[i] == "animal_10")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_10");
                }
                else if (ListaImagenes[i] == "animal_11")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_11");
                }
                else if (ListaImagenes[i] == "animal_12")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_12");
                }
                else if (ListaImagenes[i] == "animal_13")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_13");
                }
                else if (ListaImagenes[i] == "animal_14")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_14");
                }
                else if (ListaImagenes[i] == "animal_15")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_15");
                }
                else if (ListaImagenes[i] == "animal_16")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_16");
                }
                else if (ListaImagenes[i] == "animal_17")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_17");
                }
                else if (ListaImagenes[i] == "animal_18")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_18");
                }
                else if (ListaImagenes[i] == "animal_19")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_19");
                }
                else if (ListaImagenes[i] == "animal_20")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_20");
                }
                else if (ListaImagenes[i] == "animal_21")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_21");
                }
                else if (ListaImagenes[i] == "animal_22")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_22");
                }
                else if (ListaImagenes[i] == "animal_23")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_23");
                }
                else if (ListaImagenes[i] == "animal_24")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_24");
                }
                else if (ListaImagenes[i] == "animal_25")
                {
                    pictureBoxImage1.Image = GetImageByName("animal_nombre_25");
                }
            }
            if (tipo == "pais")
            {
                if (ListaImagenes[i] == "pais_0")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_0");
                }
                else if (ListaImagenes[i] == "pais_1")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_1");
                }
                else if (ListaImagenes[i] == "pais_2")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_2");
                }
                else if (ListaImagenes[i] == "pais_3")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_3");
                }
                else if (ListaImagenes[i] == "pais_4")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_4");
                }
                else if (ListaImagenes[i] == "pais_5")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_5");
                }
                else if (ListaImagenes[i] == "pais_6")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_6");
                }
                else if (ListaImagenes[i] == "pais_7")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_7");
                }
                else if (ListaImagenes[i] == "pais_8")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_8");
                }
                else if (ListaImagenes[i] == "pais_9")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_9");
                }
                else if (ListaImagenes[i] == "pais_10")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_10");
                }
                else if (ListaImagenes[i] == "pais_11")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_11");
                }
                else if (ListaImagenes[i] == "pais_12")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_12");
                }
                else if (ListaImagenes[i] == "pais_13")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_13");
                }
                else if (ListaImagenes[i] == "pais_14")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_14");
                }
                else if (ListaImagenes[i] == "pais_15")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_15");
                }
                else if (ListaImagenes[i] == "pais_16")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_16");
                }
                else if (ListaImagenes[i] == "pais_17")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_17");
                }
                else if (ListaImagenes[i] == "pais_18")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_18");
                }
                else if (ListaImagenes[i] == "pais_19")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_19");
                }
                else if (ListaImagenes[i] == "pais_20")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_20");
                }
                else if (ListaImagenes[i] == "pais_21")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_21");
                }
                else if (ListaImagenes[i] == "pais_22")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_22");
                }
                else if (ListaImagenes[i] == "pais_23")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_23");
                }
                else if (ListaImagenes[i] == "pais_24")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_24");
                }
                else if (ListaImagenes[i] == "pais_25")
                {
                    pictureBoxImage1.Image = GetImageByName("pais_nombre_25");
                }
            }
            if (tipo == "clase")
            {
                if (ListaImagenes[i] == "clase_0")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_0");
                }
                else if (ListaImagenes[i] == "clase_1")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_1");
                }
                else if (ListaImagenes[i] == "clase_2")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_2");
                }
                else if (ListaImagenes[i] == "clase_3")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_3");
                }
                else if (ListaImagenes[i] == "clase_4")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_4");
                }
                else if (ListaImagenes[i] == "clase_5")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_5");
                }
                else if (ListaImagenes[i] == "clase_6")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_6");
                }
                else if (ListaImagenes[i] == "clase_7")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_7");
                }
                else if (ListaImagenes[i] == "clase_8")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_8");
                }
                else if (ListaImagenes[i] == "clase_9")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_9");
                }
                else if (ListaImagenes[i] == "clase_10")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_10");
                }
                else if (ListaImagenes[i] == "clase_11")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_11");
                }
                else if (ListaImagenes[i] == "clase_12")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_12");
                }
                else if (ListaImagenes[i] == "clase_13")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_13");
                }
                else if (ListaImagenes[i] == "clase_14")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_14");
                }
                else if (ListaImagenes[i] == "clase_15")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_15");
                }
                else if (ListaImagenes[i] == "clase_16")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_16");
                }
                else if (ListaImagenes[i] == "clase_17")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_17");
                }
                else if (ListaImagenes[i] == "clase_18")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_18");
                }
                else if (ListaImagenes[i] == "clase_19")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_19");
                }
                else if (ListaImagenes[i] == "clase_20")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_20");
                }
                else if (ListaImagenes[i] == "clase_21")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_21");
                }
                else if (ListaImagenes[i] == "clase_22")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_22");
                }
                else if (ListaImagenes[i] == "clase_23")
                {
                    pictureBoxImage1.Image = GetImageByName("clase_nombre_23");
                }
            }
        }

        //Esta función cambia los cuadrados del tablero del contrincante mostrado a la derecha para conseguir que el usiario vea las cartas que ha girado el rival.
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
            if (terminado == 0)
            {
                if (carta_inicial != 3)
                {
                    /*
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
                    */

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
                    if (final == 0)
                    {
                        carta_seleccionada = id_carta;
                        if (0 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                            label_info.Text = "Enorabuena has acertado, ahora le toca al rival...";
                            mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/Si" + "/" + vidas;
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            carta_seleccionada = -1;
                            if (rondas < Convert.ToInt32(limitePreguntas))
                            {
                                carta_inicial = 1;
                            }
                            terminado = 1;
                            label1.Visible = false;
                        }
                        else
                        {
                            vidas = vidas - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + " vidas");
                            label_info.Text="Lo siento, has fallado, te quedan " + vidas + " vidas";
                            if (vidas == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text="Has perdido";
                                mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/No" + "/" + vidas;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;
                            }
                            carta_seleccionada = -1;
                            if (rondas < Convert.ToInt32(limitePreguntas))
                            {
                                carta_inicial = 1;
                            }
                            groupBoxChat.Visible = true;
                        }
                    }
                    else
                    {
                        if (0 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, habeis ganado los dos!!");
                            label_info.Text = "Enorabuena has acertado, habeis ganado los dos!!";
                            mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/dos";
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            terminado = 1;
                            label1.Visible = false;


                        }
                        else
                        {
                            intentos = intentos - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + intentos + " intentos");
                            label_info.Text = "Lo siento, has fallado, te quedan " + intentos + " intentos";
                            if (intentos == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text = "Has perdido";
                                mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/" + rival;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;

                            }

                        }
                    }
                }
            }
        }

        private void pictureBoxImage2_Click(object sender, EventArgs e)
        {
            if (terminado == 0)
            {
                if (carta_inicial != 3)
                {
                    /*
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
                    */

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
                    if (final == 0)
                    {
                        carta_seleccionada = id_carta;
                        if (1 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                            label_info.Text = "Enorabuena has acertado, ahora le toca al rival...";
                            mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/Si" + "/" + vidas;
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            carta_seleccionada = -1;
                            if (rondas < Convert.ToInt32(limitePreguntas))
                            {
                                carta_inicial = 1;
                            }
                            terminado = 1;
                            label1.Visible = false;
                        }
                        else
                        {
                            vidas = vidas - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + " vidas");
                            label_info.Text = "Lo siento, has fallado, te quedan " + vidas + " vidas";
                            if (vidas == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text = "Has perdido";
                                mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/No" + "/" + vidas;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;
                            }
                            carta_seleccionada = -1;
                            if (rondas < Convert.ToInt32(limitePreguntas))
                            {
                                carta_inicial = 1;
                            }
                            groupBoxChat.Visible = true;
                        }
                    }
                    else
                    {
                        if (1 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, habeis ganado los dos!!");
                            label_info.Text = "Enorabuena has acertado, habeis ganado los dos!!";
                            mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/dos";
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            terminado = 1;
                            label1.Visible = false;


                        }
                        else
                        {
                            intentos = intentos - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + intentos + " intentos");
                            label_info.Text = "Lo siento, has fallado, te quedan " + intentos + " intentos";
                            if (intentos == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text = "Has perdido";
                                mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/" + rival;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;

                            }
                        }
                    }
                }
            }
        }

        private void pictureBoxImage3_Click(object sender, EventArgs e)
        {
            if (terminado == 0)
            {
                if (carta_inicial != 3)
                {
                    /*
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
                    */
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
                    if (final == 0)
                    {


                        carta_seleccionada = id_carta;
                        if (2 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                            label_info.Text = "Enorabuena has acertado, ahora le toca al rival...";
                            mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/Si" + "/" + vidas;
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            carta_seleccionada = -1;
                            carta_inicial = 1;
                            terminado = 1;
                            label1.Visible = false;
                        }
                        else
                        {
                            vidas = vidas - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + " vidas");
                            label_info.Text = "Lo siento, has fallado, te quedan " + vidas + " vidas";
                            if (vidas == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text = "Has perdido";
                                mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/No" + "/" + vidas;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;
                            }
                            carta_seleccionada = -1;
                            if (rondas < Convert.ToInt32(limitePreguntas))
                            {
                                carta_inicial = 1;
                            }
                            groupBoxChat.Visible = true;
                        }
                    }
                    else
                    {
                        if (2 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, habeis ganado los dos!!");
                            label_info.Text = "Enorabuena has acertado, habeis ganado los dos!!";
                            mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/dos";
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            terminado = 1;
                            label1.Visible = false;

                        }
                        else
                        {
                            intentos = intentos - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + intentos + " intentos");
                            label_info.Text = "Lo siento, has fallado, te quedan " + intentos + " intentos";
                            if (intentos == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text = "Has perdido";
                                mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/" + rival;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;

                            }

                        }
                    }
                }
            }
        }

        private void pictureBoxImage4_Click(object sender, EventArgs e)
        {
            if (terminado == 0)
            {
                if (carta_inicial != 3)
                {

                    /*
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
                    */
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
                    if (final == 0)
                    {
                        carta_seleccionada = id_carta;
                        if (3 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                            label_info.Text = "Enorabuena has acertado, ahora le toca al rival...";
                            mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/Si" + "/" + vidas;
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            if (rondas < Convert.ToInt32(limitePreguntas))
                            {
                                carta_seleccionada = -1;
                            }
                            carta_inicial = 1;
                            terminado = 1;
                            label1.Visible = false;
                        }
                        else
                        {
                            vidas = vidas - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + " vidas");
                            label_info.Text = "Lo siento, has fallado, te quedan " + vidas + " vidas";
                            if (vidas == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text = "Has perdido";
                                mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/No" + "/" + vidas;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;
                            }
                            carta_seleccionada = -1;
                            if (rondas < Convert.ToInt32(limitePreguntas))
                            {
                                carta_inicial = 1;
                            }
                            groupBoxChat.Visible = true;
                        }
                    }
                    else
                    {
                        if (3 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, habeis ganado los dos!!");
                            label_info.Text = "Enorabuena has acertado, habeis ganado los dos!!";
                            mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/dos" ;
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            terminado = 1;
                            label1.Visible = false;

                        }
                        else
                        {
                            intentos = intentos - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + intentos + " intentos");
                            label_info.Text = "Lo siento, has fallado, te quedan " + intentos + " intentos";
                            if (intentos == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text = "Has perdido";
                                mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/" + rival;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;

                            }

                        }
                    }
                }
            }
        }

        private void pictureBoxImage5_Click(object sender, EventArgs e)
        {
            if (terminado == 0)
            {
                if (carta_inicial != 3)
                {
                    /*
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
                    */
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
                    if (final == 0)
                    {


                        carta_seleccionada = id_carta;
                        if (4 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                            label_info.Text = "Enorabuena has acertado, ahora le toca al rival...";
                            mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/Si" + "/" + vidas;
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            carta_seleccionada = -1;
                            carta_inicial = 1;
                            terminado = 1;
                            label1.Visible = false;
                        }
                        else
                        {
                            vidas = vidas - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + " vidas");
                            label_info.Text = "Lo siento, has fallado, te quedan " + vidas + " vidas";
                            if (vidas == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text = "Has perdido";
                                mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/No" + "/" + vidas;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;
                            }
                            carta_seleccionada = -1;
                            if (rondas < Convert.ToInt32(limitePreguntas))
                            {
                                carta_inicial = 1;
                            }
                            groupBoxChat.Visible = true;
                        }
                    }
                    else
                    {
                        if (4 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, habeis ganado los dos!!");
                            label_info.Text = "Enorabuena has acertado, habeis ganado los dos!!";
                            mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/dos";
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            terminado = 1;
                            label1.Visible = false;

                        }
                        else
                        {
                            intentos = intentos - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + intentos + " intentos");
                            label_info.Text = "Lo siento, has fallado, te quedan " + intentos + " intentos";
                            if (intentos == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text = "Has perdido";
                                mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/" + rival; ;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;

                            }

                        }
                    }
                }
            }
        }

        private void pictureBoxImage6_Click(object sender, EventArgs e)
        {
            if (terminado == 0)
            {
                if (carta_inicial != 3)
                {

                    /*
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
                    */

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
                    if (final == 0)
                    {


                        carta_seleccionada = id_carta;
                        if (5 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                            label_info.Text = "Enorabuena has acertado, ahora le toca al rival...";
                            mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/Si" + "/" + vidas;
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            carta_seleccionada = -1;
                            carta_inicial = 1;
                            terminado = 1;
                            label1.Visible = false;
                        }
                        else
                        {
                            vidas = vidas - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + " vidas");
                            label_info.Text = "Lo siento, has fallado, te quedan " + vidas + " vidas";
                            if (vidas == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text = "Has perdido";
                                mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/No" + "/" + vidas;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;
                            }
                            carta_seleccionada = -1;
                            if (rondas < Convert.ToInt32(limitePreguntas))
                            {
                                carta_inicial = 1;
                            }
                            groupBoxChat.Visible = true;
                        }
                    }
                    else
                    {
                        if (5 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, habeis ganado los dos!!");
                            label_info.Text = "Enorabuena has acertado, habeis ganado los dos!!";
                            mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/dos" ;
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            terminado = 1;
                            label1.Visible = false;

                        }
                        else
                        {
                            intentos = intentos - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + intentos + " intentos");
                            label_info.Text = "Lo siento, has fallado, te quedan " + intentos + " intentos";
                            if (intentos == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text = "Has perdido";
                                mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/" + rival;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;

                            }

                        }
                    }
                }
            }
        }

        private void pictureBoxImage7_Click(object sender, EventArgs e)
        {
            if (terminado == 0)
            {
                if (carta_inicial != 3)
                {

                    /*
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
                    */
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
                    if (final == 0)
                    {
                        carta_seleccionada = id_carta;
                        if (6 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                            label_info.Text = "Enorabuena has acertado, ahora le toca al rival...";
                            mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/Si" + "/" + vidas;
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            carta_seleccionada = -1;
                            carta_inicial = 1;
                            terminado = 1;
                            label1.Visible = false;
                        }
                        else
                        {
                            vidas = vidas - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + " vidas");
                            label_info.Text = "Lo siento, has fallado, te quedan " + vidas + " vidas";
                            if (vidas == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text = "Has perdido";
                                mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/No" + "/" + vidas;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;
                            }
                            carta_seleccionada = -1;
                            if (rondas < Convert.ToInt32(limitePreguntas))
                            {
                                carta_inicial = 1;
                            }
                            groupBoxChat.Visible = true;
                        }
                    }
                    else
                    {
                        if (6 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, habeis ganado los dos!!");
                            label_info.Text = "Enorabuena has acertado, habeis ganado los dos!!";
                            mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/dos";
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            terminado = 1;
                            label1.Visible = false;

                        }
                        else
                        {
                            intentos = intentos - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + intentos + " intentos");
                            label_info.Text = "Lo siento, has fallado, te quedan " + intentos + " intentos";
                            if (intentos == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text = "Has perdido";
                                mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/" + rival;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;

                            }

                        }
                    }
                }
            }
        }

        private void pictureBoxImage8_Click(object sender, EventArgs e)
        {
            if (terminado == 0)
            {
                if (carta_inicial != 3)
                {
                    /*
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
                    */
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
                    if (final == 0)
                    {


                        carta_seleccionada = id_carta;
                        if (7 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                            label_info.Text = "Enorabuena has acertado, ahora le toca al rival...";
                            mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/Si" + "/" + vidas;
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            carta_seleccionada = -1;
                            carta_inicial = 1;
                            terminado = 1;
                            label1.Visible = false;
                        }
                        else
                        {
                            vidas = vidas - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + " vidas");
                            label_info.Text = "Lo siento, has fallado, te quedan " + vidas + " vidas";
                            if (vidas == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text = "Has perdido";
                                mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/No" + "/" + vidas;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;
                            }
                            carta_seleccionada = -1;
                            if (rondas < Convert.ToInt32(limitePreguntas))
                            {
                                carta_inicial = 1;
                            }
                            groupBoxChat.Visible = true;
                        }
                    }
                    else
                    {
                        if (7 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, habeis ganado los dos!!");
                            label_info.Text = "Enorabuena has acertado, habeis ganado los dos!!";
                            mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/dos" ;
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            terminado = 1;
                            label1.Visible = false;

                        }
                        else
                        {
                            intentos = intentos - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + intentos + " intentos");
                            label_info.Text = "Lo siento, has fallado, te quedan " + intentos + " intentos";
                            if (intentos == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text = "Has perdido";
                                mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/" + rival;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;

                            }

                        }
                    }
                }
            }
        }

        private void pictureBoxImage9_Click(object sender, EventArgs e)
        {
            if (terminado == 0)
            {
                if (carta_inicial != 3)
                {
                    /*
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
                    */
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
                    if (final == 0)
                    {


                        carta_seleccionada = id_carta;
                        if (8 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                            label_info.Text = "Enorabuena has acertado, ahora le toca al rival...";
                            mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/Si" + "/" + vidas;
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            carta_seleccionada = -1;
                            carta_inicial = 1;
                            terminado = 1;
                            label1.Visible = false;
                        }
                        else
                        {
                            vidas = vidas - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + " vidas");
                            label_info.Text = "Lo siento, has fallado, te quedan " + vidas + " vidas";
                            if (vidas == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text = "Has perdido";
                                mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/No" + "/" + vidas;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;
                            }
                            carta_seleccionada = -1;
                            if (rondas < Convert.ToInt32(limitePreguntas))
                            {
                                carta_inicial = 1;
                            }
                            groupBoxChat.Visible = true;
                        }
                    }
                    else
                    {
                        if (8 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, habeis ganado los dos!!");
                            label_info.Text = "Enorabuena has acertado, habeis ganado los dos!!";
                            mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/dos" ;
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            terminado = 1;
                            label1.Visible = false;

                        }
                        else
                        {
                            intentos = intentos - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + intentos + " intentos");
                            label_info.Text = "Lo siento, has fallado, te quedan " + intentos + " intentos";
                            if (intentos == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text = "Has perdido";
                                mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/" + rival;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;

                            }

                        }
                    }
                }
            }
        }

        private void pictureBoxImage10_Click(object sender, EventArgs e)
        {
            if (terminado == 0)
            {
                if (carta_inicial != 3)
                {
                    /*
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
                    */
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
                    if (final == 0)
                    {


                        carta_seleccionada = id_carta;
                        if (9 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                            label_info.Text = "Enorabuena has acertado, ahora le toca al rival...";
                            mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/Si" + "/" + vidas;
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            carta_seleccionada = -1;
                            carta_inicial = 1;
                            terminado = 1;
                            label1.Visible = false;
                        }
                        else
                        {
                            vidas = vidas - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + " vidas");
                            label_info.Text = "Lo siento, has fallado, te quedan " + vidas + " vidas";
                            if (vidas == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text = "Has perdido";
                                mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/No" + "/" + vidas;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;
                            }
                            carta_seleccionada = -1;
                            if (rondas < Convert.ToInt32(limitePreguntas))
                            {
                                carta_inicial = 1;
                            }
                            groupBoxChat.Visible = true;
                        }
                    }
                    else
                    {
                        if (9 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, habeis ganado los dos!!");
                            label_info.Text = "Enorabuena has acertado, habeis ganado los dos!!";
                            mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/dos";
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            terminado = 1;
                            label1.Visible = false;

                        }
                        else
                        {
                            intentos = intentos - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + intentos + " intentos");
                            label_info.Text = "Lo siento, has fallado, te quedan " + intentos + " intentos";
                            if (intentos == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text = "Has perdido";
                                mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/" + rival;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;

                            }

                        }
                    }
                }
            }
        }

        private void pictureBoxImage11_Click(object sender, EventArgs e)
        {
            if (terminado == 0)
            {
                if (carta_inicial != 3)
                {
                    /*
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
                    */
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
                    if (final == 0)
                    {


                        carta_seleccionada = id_carta;
                        if (10 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                            label_info.Text = "Enorabuena has acertado, ahora le toca al rival...";
                            mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/Si" + "/" + vidas;
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            carta_seleccionada = -1;
                            carta_inicial = 1;
                            terminado = 1;
                            label1.Visible = false;
                        }
                        else
                        {
                            vidas = vidas - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + " vidas");
                            label_info.Text = "Lo siento, has fallado, te quedan " + vidas + " vidas";
                            if (vidas == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text = "Has perdido";
                                mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/No" + "/" + vidas;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;
                            }
                            carta_seleccionada = -1;
                            if (rondas < Convert.ToInt32(limitePreguntas))
                            {
                                carta_inicial = 1;
                            }
                            groupBoxChat.Visible = true;
                        }
                    }
                    else
                    {
                        if (10 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, habeis ganado los dos!!");
                            label_info.Text = "Enorabuena has acertado, habeis ganado los dos!!";
                            mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/dos";
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            terminado = 1;
                            label1.Visible = false;


                        }
                        else
                        {
                            intentos = intentos - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + intentos + " intentos");
                            label_info.Text = "Lo siento, has fallado, te quedan " + intentos + " intentos";
                            if (intentos == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text = "Has perdido";
                                mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/" + rival;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;

                            }

                        }
                    }
                }
            }
        }

        private void pictureBoxImage12_Click(object sender, EventArgs e)
        {
            if (terminado == 0)
            {
                if (carta_inicial != 3)
                {
                    /*
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
                    */

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
                    if (final == 0)
                    {


                        carta_seleccionada = id_carta;
                        if (11 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, ahora le toca al rival...");
                            label_info.Text = "Enorabuena has acertado, ahora le toca al rival...";
                            mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/Si" + "/" + vidas;
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            carta_seleccionada = -1;
                            carta_inicial = 1;
                            terminado = 1;
                            label1.Visible = false;
                        }
                        else
                        {
                            vidas = vidas - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + vidas + " vidas");
                            label_info.Text = "Lo siento, has fallado, te quedan " + vidas + " vidas";
                            if (vidas == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text = "Has perdido";
                                mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/No" + "/" + vidas;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;
                            }
                            carta_seleccionada = -1;
                            if (rondas < Convert.ToInt32(limitePreguntas))
                            {
                                carta_inicial = 1;
                            }
                            groupBoxChat.Visible = true;
                        }
                    }
                    else
                    {
                        if (11 == id_carta_rival)
                        {
                            //MessageBox.Show("Enorabuena has acertado, habeis ganado los dos!!");
                            label_info.Text = "Enorabuena has acertado, habeis ganado los dos!!";
                            mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/dos";
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                            server.Send(msg);
                            terminado = 1;
                            label1.Visible = false;

                        }
                        else
                        {
                            intentos = intentos - 1;
                            //MessageBox.Show("Lo siento, has fallado, te quedan " + intentos + " intentos");
                            label_info.Text = "Lo siento, has fallado, te quedan " + intentos + " intentos";
                            if (intentos == 0)
                            {
                                //MessageBox.Show("Has perdido");
                                label_info.Text = "Has perdido";
                                mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/" + rival;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                                server.Send(msg);
                                terminado = 1;
                                label1.Visible = false;

                            }

                        }
                    }
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
            if (terminado == 0) 
            { 
                conteo = conteo - 1;
            }
            else
            {
                label1.Visible = false;
            }
            label_tiempo.Text = conteo.ToString();

            if (carta_inicial == 0)
            {
                Random rnd = new Random();
                id_carta = rnd.Next(11);
                pictureBoxImagenElegida.Image = GetImageByName(ListaImagenes[id_carta]);
                pictureBoxNombreElegida.Image = GetImageByName(ListaNombres[id_carta]);
                // MessageBox.Show("Tu carta es: " + id_carta);
                mensaje_not = "48/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + id_carta;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                server.Send(msg);
                carta_inicial = 1;
                inicio_partida = true;
                /*
                if (y == 2)
                {
                    MessageBox.Show("Escoge una carta,tienes 10 segundos, sino se escogera de forma random");
                    conteo = 10;
                    tiempo = true;
                    y = 2;
                }
                else
                {
                    Random rnd = new Random();
                    id_carta = rnd.Next(11);
                    MessageBox.Show("Tu carta es: " + id_carta);
                    mensaje_not = "48/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + id_carta;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                    server.Send(msg);
                    carta_inicial = 1;
                }
                */
            }
            /*
            if (carta_inicial == 1 && conteo == 0 && inicio_partida==false)
            {
                conteo = 1000;
                MessageBox.Show("Esperando al rival");
                tiempo = true;
            }
            */
            if (conteo <= 0)
            {
                conteo = Convert.ToInt32(limiteTiempo);
                tiempo = true;
                if (final == 0)
                {
                    CambiarTurno();
                }
                
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

                    if (turno == true)
                    {
                        groupBoxChat.Visible = true;
                        conversacion.Visible = true;
                        if (nombreUsuario == creador_partida)
                        {
                            label_turno.Text = rival;
                        }
                        else
                        {
                        label_turno.Text = rival;
                        }   
                        comboBoxPTChat.Visible = false;
                        button_Nose.Visible = true;
                        button_Si.Visible = true;
                        button_No.Visible = true;
                        textBox_con.Visible = false;
                        button_enviar.Visible = false;
                       
                }
                    else
                        {
                        groupBoxChat.Visible = true;
                        conversacion.Visible = true;
                        if (nombreUsuario != creador_partida)
                        {
                            label_turno.Text = nombreUsuario;
                        }
                        else
                        {
                        label_turno.Text = creador_partida;
                        }
                       
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
                
            }
            
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
               
            }
            
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
               
            }
           
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
            //MessageBox.Show("Carta rival :" + id_carta_rival);
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
            if (rondas < Convert.ToInt32(limitePreguntas))
            {
                    if (turno == true)
                    {
                        turno = false;
                    }
                    else
                    {
                        turno = true;

                    }
                    string mensaje = "47/" + Nform + "/" + id_partida + "/YA";
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                    conteo = Convert.ToInt32(limiteTiempo);
                    x = x + 1;
                    if (x == 2)
                    {
                        rondas = rondas + 1;
                        x = 0;
                    }    
            }
            else
            {
                MessageBox.Show("Se acabaron los turnos, toca escoger la carta, tienes " + vidas + " intentos");
                intentos = vidas;
                carta_inicial = 3;
                final = 1;
                conteo = 1000;
    
            }
            // Envias el mensaje  
        }

        private void Boton_respuesta_Click(object sender, EventArgs e)
        {
            if (final == 0 && terminado!=1)
            {
                groupBoxChat.Visible = false;
                carta_inicial = 3;
                MessageBox.Show("Escoge la carta del rival");
            }

        }
        public void Fasefinal(string nombre,string resultado,int vidas_final)
        {
            carta_inicial = 3;
            if (nombre==rival && resultado=="No")
            {
                //MessageBox.Show("Enorabuena, has ganado a " + rival);
                label_info.Invoke(new DelegadoParaEscribirLabel(EscribirLabel), new object[] { "Enorabuena, has ganado a " + rival, label_info });
                // Partida_FormClosed(Partida,Close);
                terminado = 1;
                mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/" + nombreUsuario;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                server.Send(msg);


            }
            else if(nombre == rival && resultado == "Si")
            {
                if (vidas_final <= vidas)
                {
                    intentos = vidas - vidas_final + 1;
                    //MessageBox.Show("Tienes " + intentos+ " intentos para poder empatar. Escoge la carta, tienes 60 segundos");
                    label_info.Text = "Tienes " + intentos + " intentos para poder empatar. Escoge la carta, tienes 60 segundos";
                    conteo = 60;
                    final = 1;


                }
                else
                {
                    //MessageBox.Show("Has perdido ante "+ rival);
                    label_info.Text = "Has perdido ante " + rival;
                    mensaje_not = "60/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/" + rival + "/"+ rival;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                    server.Send(msg);
                    terminado = 1;


                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*DialogResult r = MessageBox.Show("Quieres pasar de turno?", "Cambio de turno", MessageBoxButtons.YesNo);
            if (r == DialogResult.Yes)
            {
                CambiarTurno();

            }*/
        }

        private void Partida_FormClosed(object sender, FormClosedEventArgs e)
        {
           
            if (terminado==0 && inicio_partida ==true)
            {
                mensaje_not = "50/" + Nform + "/" + id_partida + "/" + nombreUsuario + "/No" + "/" + vidas;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                server.Send(msg);
            }
            FormMenu.StopMusicGame(Nform);

        }
    }
}


