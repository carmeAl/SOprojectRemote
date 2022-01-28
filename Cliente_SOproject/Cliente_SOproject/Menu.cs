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
using System.Media;
//using Xamarin.Forms;


namespace Cliente_SOproject
{
    public partial class Menu : Form
    {


        public int id_usuario;
        public string nombreUsuario;
        public string creador_partida;
        public string invitado;
        public bool conectado = false;
        public int id_partida;
        public string contrincante;
        public string nombreInvitado;
        public int Nform;
        public int contadorMusic = 0;
        public bool MusicMenu = true;
        public bool Sound = true;
        public bool Cancelado = false;

        //Parametros partida
        string nivel;
        string sugerirPreguntas;
        string mapa;
        string limitePreguntas;
        string limiteTiempo;

        Socket server;
        Thread atender;
        List<Partida> formularios = new List<Partida>();
        SoundPlayer soundM = new SoundPlayer(@".\Music_menu.wav");
        SoundPlayer soundG = new SoundPlayer(@".\Music_game.wav");

        public Menu()
        {
            InitializeComponent();
            tabControl1.SelectedTab = tabPageLogin;
            soundM.PlayLooping();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            labelLUsuarioNoEncontrado.Visible = false;
            labelRUsuarioError.Visible = false;
            labelCError.Visible = false;
        }

        //FUNCIONAMIENTO

        delegate void DelegadoDGV(DataGridView mensaje);
        delegate void DelegadoParaEscribir(string mensaje);
        delegate void DelegadoParaCambiarTab(TabPage nameTab);
        delegate void DelegadoLabel(Label nameLabel);
        delegate void DelegadoColorLabel(Label nameLabel, Color color);
        delegate void DelegadoParaEscribirLabel(string msn, Label nameLabel);
        delegate void DelegadoPartida(Partida partida);
        delegate void DelegadoIniciarPartida();
        delegate void DelegadoImageBox(string imagen, PictureBox PB);



        //public Font(System.Drawing.FontFamily family, float emSize, System.Drawing.FontStyle style, System.Drawing.GraphicsUnit unit, byte gdiCharSet);

        public void PonImgen(string imagen, PictureBox PB)
        {
            PB.Image = GetImageByName(imagen);
        }
        public void PonDataGridView(string mensaje)
        {
            if (mensaje != null && mensaje != "")
            {
                dataGridViewListaCon.RowHeadersVisible = false;
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
        public void PonDataGridViewRanking(string mensaje)
        {
            if (mensaje != null && mensaje != "")
            {
                dataGridViewRanquing.Rows.Clear();
                string[] partes = mensaje.Split(',');
                int i = 0;
                while (i < partes.Length)
                {
                    int n = dataGridViewRanquing.Rows.Add();
                    dataGridViewRanquing.Rows[n].Cells[0].Value = partes[i];
                    i++;
                    if (partes[i] == "-1")
                    {
                        dataGridViewRanquing.Rows[n].Cells[1].Value = "0";
                    }
                    else
                    {
                        dataGridViewRanquing.Rows[n].Cells[1].Value = partes[i];
                    }
                    i++;
                }
                this.dataGridViewRanquing.Sort(this.dataGridViewRanquing.Columns[1], ListSortDirection.Descending);
            }
            else
            {
                MessageBox.Show("Ha ocurrido un error");
            }
        }
        public void CambiarTab(TabPage nameTab)
        {
            tabControl1.SelectedTab = nameTab;
        }
        public void PonVisibleLabel(Label nameLabel)
        {
            nameLabel.Visible = true;
        }
        public void PonNoVisibleLabel(Label nameLabel)
        {
            nameLabel.Visible = false;
        }
        public void EscribirLabel(string msn, Label nameLabel)
        {
            nameLabel.Text = msn;
        }
        public void CambiarColorLabel(Label nameLabel, Color color)
        {
            nameLabel.ForeColor = color;
        }
        public void IniciarPartida(string lista, string nombreInv, string IdPartida, string rival)
        {
            int cont = formularios.Count;
            Partida FormPartida = new Partida(Cancelado,cont, server, nombreUsuario, id_partida,
                nivel, sugerirPreguntas, mapa, limitePreguntas, limiteTiempo, creador_partida,nombreInvitado, this);
            formularios.Add(FormPartida);
            Nform = formularios.Count-1;

            formularios[Nform].rival = rival;
            FormPartida.PasarListaRandom(lista);
            FormPartida.CambiarTab();
            formularios[Nform].nombreInvitado = nombreInv;
            formularios[Nform].id_partida = id_partida;
            if (MusicMenu)
            {
                if (Sound)
                {
                    soundM.Stop();
                    soundG.PlayLooping();
                }
                MusicMenu = false;
            }
            FormPartida.ShowDialog();

        }
        public void PonerEnMarchaForm()
        {
            int cont = formularios.Count;
            Partida FormPartida = new Partida(Cancelado,cont, server, nombreUsuario, id_partida,
                    nivel, sugerirPreguntas, mapa, limitePreguntas, limiteTiempo, creador_partida,nombreInvitado, this);
            formularios.Add(FormPartida);
            Nform = formularios.Count - 1;
            if (MusicMenu)
            {
                if (Sound)
                {
                    soundM.Stop();
                    soundG.PlayLooping();
                }
                MusicMenu = false;
            }
            FormPartida.ShowDialog();
        }
        public void StopMusicGame(int index)
        {
            contadorMusic++;
            if (formularios.Count == contadorMusic)
            {
                soundG.Stop();
                soundM.PlayLooping();
                MusicMenu = true;
            }
        }
        public void ConectarServidor()
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
                conectado = true;

            }
            catch (SocketException)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                return;
            }
            //pongo en marcha el thread que atenderà los mensajes del servidor 
            ThreadStart ts = delegate { AtenderServidor(); };
            atender = new Thread(ts);
            atender.Start();
        }
        public void DesconectarServidor()
        {
            if (conectado)
            {
                //Mensaje de desconexión
                string mensaje = "0/" + textBoxLNombre.Text;

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                // Nos desconectamos
                atender.Abort();

                server.Shutdown(SocketShutdown.Both);
                server.Close();
                conectado = false;
            }
        }

        //THREAD
        private void AtenderServidor()
        {
            while (true)
            {
                //Recibimos mensaje del servidor
                byte[] msg2 = new byte[300];
                server.Receive(msg2);
                string[] trozos = Encoding.ASCII.GetString(msg2).Split('\0');
                string[] trozos1 = trozos[0].Split('/');
                int codigo = Convert.ToInt32(trozos1[0]);
                //int Nform = Convert.ToInt32(trozos1[1]);
                string mensaje = trozos1[2];


                switch (codigo)
                {

                    case 11: // respuesta a iniciar

                        if (mensaje != "NO")
                        {
                            id_usuario = Convert.ToInt32(mensaje);
                            DelegadoParaCambiarTab delegado111 = new DelegadoParaCambiarTab(CambiarTab);
                            tabPageMenu.Invoke(delegado111, new object[] { tabPageMenu });
                            DelegadoLabel delegado112 = new DelegadoLabel(PonNoVisibleLabel);
                            labelLUsuarioNoEncontrado.Invoke(delegado112, new object[] { labelLUsuarioNoEncontrado });
                        }
                        else
                        {

                            DelegadoParaEscribirLabel delegado113 = new DelegadoParaEscribirLabel(EscribirLabel);
                            labelLUsuarioNoEncontrado.Invoke(delegado113, new object[] { "Usuario no encontrado, escriba bien el usuario y/o la contraseña, o registrase", labelLUsuarioNoEncontrado });
                            DelegadoLabel delegado114 = new DelegadoLabel(PonVisibleLabel);
                            labelLUsuarioNoEncontrado.Invoke(delegado114, new object[] { labelLUsuarioNoEncontrado });
                            DesconectarServidor();
                        }
                        break;

                    case 21: //respuesta a registrarse
                        if (mensaje == "SI")
                        {
                            DelegadoParaEscribirLabel delegado211 = new DelegadoParaEscribirLabel(EscribirLabel);
                            labelRUsuarioError.Invoke(delegado211, new object[] { "Registrado de manera exitosa, ya puedes iniciar sesion en la pantalla de INICIO", labelRUsuarioError });
                            DelegadoColorLabel delegado212 = new DelegadoColorLabel(CambiarColorLabel);
                            labelRUsuarioError.Invoke(delegado212, new object[] { labelRUsuarioError, Color.Green });
                            DelegadoLabel delegado213 = new DelegadoLabel(PonVisibleLabel);
                            labelRUsuarioError.Invoke(delegado213, new object[] { labelRUsuarioError });
                            DesconectarServidor();
                        }
                        else
                        {
                            DelegadoParaEscribirLabel delegado214 = new DelegadoParaEscribirLabel(EscribirLabel);
                            labelRUsuarioError.Invoke(delegado214, new object[] { "Usuario ya esta registrado, escriba otro usuario", labelRUsuarioError });
                            DelegadoColorLabel delegado215 = new DelegadoColorLabel(CambiarColorLabel);
                            labelRUsuarioError.Invoke(delegado215, new object[] { labelRUsuarioError, Color.Red });
                            DelegadoLabel delegado216 = new DelegadoLabel(PonVisibleLabel);
                            labelRUsuarioError.Invoke(delegado216, new object[] { labelRUsuarioError });
                            DesconectarServidor();
                        }
                        break;

                    case 36: //respuesta lista de conectados
                        DelegadoParaEscribir delegado36 = new DelegadoParaEscribir(PonDataGridView);
                        dataGridViewListaCon.Invoke(delegado36, new object[] { mensaje });
                        break;
                    case 37:
                        DelegadoParaEscribirLabel delegado371 = new DelegadoParaEscribirLabel(EscribirLabel);
                        if (trozos1[2] != "-1")
                        {
                            labelPPuntosActuales.Invoke(delegado371, new object[] { trozos1[2], labelPPuntosActuales });
                        }
                        if (trozos1[3] != "-1")
                        {
                            labelPMaximoPuntos.Invoke(delegado371, new object[] { trozos1[3], labelPMaximoPuntos });
                        }
                        if (trozos1[4] != "-1")
                        {
                            labelPPartidasGanadas.Invoke(delegado371, new object[] { trozos1[4], labelPPartidasGanadas });
                        }
                        if (trozos1[5] != "-1")
                        {
                            labelPPartidasPerdidas.Invoke(delegado371, new object[] { trozos1[5], labelPPartidasPerdidas });
                        }
                        if (trozos1[6] != "-1")
                        {
                            labelPPartidasJugadas.Invoke(delegado371, new object[] { trozos1[6], labelPPartidasJugadas });
                        }
                        if (trozos1[7] != "-1")
                        {
                            pictureBoxRPersonaje.Invoke(new DelegadoImageBox(PonImgen), new object[] { trozos1[7], pictureBoxRPersonaje });
                        }
                        DelegadoParaCambiarTab delegado372 = new DelegadoParaCambiarTab(CambiarTab);
                        tabPagePerfil.Invoke(delegado372, new object[] { tabPagePerfil });
                        break;
                    case 38:
                        DelegadoParaEscribir delegado381 = new DelegadoParaEscribir(PonDataGridViewRanking);
                        dataGridViewListaCon.Invoke(delegado381, new object[] { mensaje });
                        DelegadoParaCambiarTab delegado382 = new DelegadoParaCambiarTab(CambiarTab);
                        tabPageSocial.Invoke(delegado382, new object[] { tabPageSocial });
                        break;
                    case 39:
                        DelegadoParaEscribirLabel delegado391 = new DelegadoParaEscribirLabel(EscribirLabel);
                        labelPRNombre.Invoke(delegado391, new object[] { trozos1[2], labelPRNombre });
                        labelPRId.Invoke(delegado391, new object[] { trozos1[3], labelPRId });
                        labelPRPuntosActuales.Invoke(delegado391, new object[] { trozos1[4], labelPRPuntosActuales });
                        labelPRMaxPuntos.Invoke(delegado391, new object[] { trozos1[5], labelPRMaxPuntos });
                        labelPRPartidasGanadas.Invoke(delegado391, new object[] { trozos1[6], labelPRPartidasGanadas });
                        labelPRPartidasPerdidas.Invoke(delegado391, new object[] { trozos1[7], labelPRPartidasPerdidas });
                        labelPRPartidasJugadas.Invoke(delegado391, new object[] { trozos1[8], labelPRPartidasJugadas });
                        labelPRPartidasGanadasVs.Invoke(delegado391, new object[] { trozos1[9], labelPRPartidasGanadasVs });
                        labelPRPartGanVs.Invoke(delegado391, new object[] { "Partidas ganadas VS " + trozos1[2] + ":", labelPRPartGanVs });
                        labelPRPartidasPerdidasVs.Invoke(delegado391, new object[] { trozos1[10], labelPRPartidasPerdidasVs });
                        labelPRPartPerdVs.Invoke(delegado391, new object[] { "Partidas perdidas VS " + trozos1[2] + ":", labelPRPartPerdVs });
                        labelPRPartidasJugadasVs.Invoke(delegado391, new object[] { trozos1[11], labelPRPartidasJugadasVs });
                        labelPRPartJugVs.Invoke(delegado391, new object[] { "Partidas jugadas VS " + trozos1[2] + ":", labelPRPartJugVs });
                        pictureBoxPRPersonaje.Invoke(new DelegadoImageBox(PonImgen), new object[] { trozos1[7], pictureBoxPRPersonaje });
                        DelegadoParaCambiarTab delegado392 = new DelegadoParaCambiarTab(CambiarTab);
                        tabPagePerfilRival.Invoke(delegado392, new object[] { tabPagePerfilRival });
                        break;

                    case 41: //Recive invitacion
                        string nombre = mensaje;
                        string mensaje_not;
                        string[] trozos2 = trozos1[3].Split(',');
                        //id_partida = Convert.ToInt32(trozos1[4]);
                        DialogResult r = MessageBox.Show(nombre + " te ha invitado a un juego.\n " + "\n"
                            + "Mapa: " + trozos2[1] + "\n"
                            + "Sugerir preguntas? " + trozos2[0] + "\n"
                            + "Límite preguntas: " + trozos2[2] + "\n"
                            + "Límite tiempo turno: " + trozos2[3] + " segundos" + "\n" + "\n"
                            + "¿Quieres aceptar?", "Invitacion", MessageBoxButtons.YesNo);

                        sugerirPreguntas = trozos2[0];
                        mapa = trozos2[1];
                        limitePreguntas = trozos2[2];
                        limiteTiempo = trozos2[3];

                        if (r == DialogResult.Yes)
                        {

                            mensaje_not = "42/" + trozos1[1] + "/" + nombre + "/" + nombreUsuario + "/Si";

                            nombreInvitado = nombreUsuario;

                        }
                        else
                        {
                            mensaje_not = "42/" + trozos1[1] + "/" + nombre + "/" + nombreUsuario + "/No";
                        }
                        // Enviamos al servidor el nombre tecleado con un vector de bytes
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje_not);
                        server.Send(msg);
                        break;
                    case 42:
                        creador_partida = trozos1[2];
                        
                        if (trozos1[4] == "Si")
                        {
                            id_partida = Convert.ToInt32(trozos1[5]);
                            if (creador_partida == nombreUsuario)
                            {
                                nombreInvitado= trozos1[3];
                                formularios[Nform].PasarListaRandom(trozos1[6]);
                                formularios[Nform].rival = trozos1[3];
                                formularios[Nform].RespuestaInvitacion(trozos1[3], trozos1[4]);
                                formularios[Nform].id_partida = id_partida;
                            }
                            else
                            {
                                ThreadStart ts = delegate { IniciarPartida(trozos1[6], trozos1[3], trozos1[5], trozos1[2]); };
                                Thread T = new Thread(ts);
                                T.Start();
                                
                                //MessageBox.Show("No se porque va si pongo esto");
                            }
                        }
                        else
                        {
                            if (creador_partida == nombreUsuario)
                            {
                                formularios[Nform].RespuestaInvitacion(trozos1[3], trozos1[4]);

                            }
                        }

                        //else if (trozos1[4] == "Si")
                        //{

                        //}

                        break;
                    case 44:
                        mensaje = trozos1[3];
                        int IdPartida = Convert.ToInt32(trozos1[1]);
                        contrincante = trozos1[2];
                        int i = 0;
                        int encontrado = 0;
                        while ((i < formularios.Count) && (encontrado == 0))
                        {
                            if (formularios[i].id_partida == IdPartida)
                            {
                                encontrado = 1;
                            }
                            else
                            {
                                i++;
                            }

                        }
                        formularios[i].EnviarTexto(mensaje, contrincante);

                        break;
                    case 46:
                        IdPartida = Convert.ToInt32(trozos1[1]);
                        string num = trozos1[2];
                        i = 0;
                        encontrado = 0;
                        while ((i < formularios.Count) && (encontrado == 0))
                        {
                            if (formularios[i].id_partida == id_partida)
                            {
                                encontrado = 1;
                            }
                            else
                            {
                                i++;
                            }

                        }
                        formularios[i].CambiarColorTableroContrincante(num);
                        break;
                    case 47:
                        i = 0;
                        encontrado = 0;
                        while ((i < formularios.Count) && (encontrado == 0))
                        {
                            if (formularios[i].id_partida == Convert.ToInt32(trozos1[1]))
                            {
                                encontrado = 1;
                            }
                            else
                            {
                                i++;
                            }
                        }
                        int turno = 1;
                        formularios[i].PrepararTiempo_Turno(turno);
                        break;
                    case 48:
                        id_partida = Convert.ToInt32(trozos1[1]);
                        string nombre1 = trozos1[2];
                        string carta1 = trozos1[3];
                  
                        string nombre2 = trozos1[4];
                        string carta2 = trozos1[5];
                     
                        i = 0;
                        encontrado = 0;
                        while ((i < formularios.Count) && (encontrado == 0))
                        {
                            if (formularios[i].id_partida == id_partida)
                            {
                                encontrado = 1;
                            }
                            else
                            {
                                i++;
                            }
                        }
                        if (nombre1 == nombreUsuario)
                        {
                            formularios[i].IniciarPartida(carta2);
                        }
                        else
                        {
                            formularios[i].IniciarPartida(carta1);
                        }
                        break;
                    case 49:
                        Cancelado = true;
                        
                        MessageBox.Show("");
                        break;
                    case 50:
                        id_partida= Convert.ToInt32(trozos1[1]);
                        string persona = trozos1[2];
                        string resultado = trozos1[3];
                        int vidas = Convert.ToInt32(trozos1[4]);
                        i = 0;
                        encontrado = 0;
                        while ((i < formularios.Count) && (encontrado == 0))
                        {
                            if (formularios[i].id_partida == id_partida)
                            {
                                encontrado = 1;
                            }
                            else
                            {
                                i++;
                            }
                        }
                        formularios[i].Fasefinal(persona, resultado,vidas);
                        break;
                }
            }
        }


        private void pictureBoxLIniciar_Click(object sender, EventArgs e)
        {

            ConectarServidor();
            nombreUsuario = textBoxLNombre.Text;
            if (conectado)
            {
                if ((textBoxLNombre.ForeColor != Color.FromArgb(173, 188, 236)) && (textBoxLContraseña.ForeColor != Color.FromArgb(173, 188, 236)))
                {
                    string mensaje = "11/" + nombreUsuario + "/" + textBoxLContraseña.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else
                {
                    labelLUsuarioNoEncontrado.Text = "Los campos de nombre o contraseña estan vacios";
                    labelLUsuarioNoEncontrado.Visible = true;
                }
            }
            else
            {
                labelLUsuarioNoEncontrado.Text = "No se ha podido conectar al servidor";
                labelLUsuarioNoEncontrado.Visible = true;
            }
        }
        private void pictureBoxRRegistrarse_Click(object sender, EventArgs e)
        {
            ConectarServidor();
            if (conectado)
            {
                if ((textBoxRUsuario.ForeColor != Color.FromArgb(173, 188, 236)) && (textBoxRContraseña.ForeColor != Color.FromArgb(173, 188, 236)) && (textBoxRUsuario.TextLength > 1) && (textBoxRContraseña.TextLength > 1))
                {
                    //Puede que de error porque debe ser superior a un caracter
                    string mensaje = "21/" + textBoxRUsuario.Text + "/" + textBoxRContraseña.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else
                {
                    labelRUsuarioError.Text = "El nombre debe tener mas de un caracter";
                    labelRUsuarioError.Visible = true;

                }
            }
            else
            {
                labelLUsuarioNoEncontrado.Text = "Error al conectarse con el servidor";
            }
        }

        private void FormMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            DesconectarServidor();
        }


        private void labelLRegistrar_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageRegister;
            labelLUsuarioNoEncontrado.Visible = false;
        }
        private void pictureBoxRVolver_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageLogin;
            DesconectarServidor();
        }

        private void pictureBoxMPerfil_Click(object sender, EventArgs e)
        {
            if (conectado)
            {
                string mensaje = "37/" + nombreUsuario + "/" + id_usuario;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            else
            {
                labelMUsuarioNoEncontrado.Text = "Error al conectarse con el servidor";
                labelMUsuarioNoEncontrado.Visible = true;
            }
            labelPNombre.Text = textBoxLNombre.Text;
            labelPId.Text = Convert.ToString(id_usuario);

        }
        private void pictureBoxMDesconectar_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageLogin;
            DesconectarServidor();
        }
        private void pictureBoxMSocial_Click(object sender, EventArgs e)
        {
            if (conectado)
            {
                string mensaje = "38/" + nombreUsuario;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            else
            {
                labelMUsuarioNoEncontrado.Text = "Error al conectarse con el servidor";
                labelMUsuarioNoEncontrado.Visible = true;
            }
        }
        
        private void dataGridViewRanquing_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if ((conectado) &&
                (Convert.ToString(dataGridViewRanquing.Rows[e.RowIndex].Cells[0].Value) != nombreUsuario) &&
                (Convert.ToString(dataGridViewRanquing.Rows[e.RowIndex].Cells[0].Value) != "NOMBRE"))
                {
                    string mensaje = "39/" + nombreUsuario + "/" + id_usuario + "/" + dataGridViewRanquing.CurrentRow.Cells[0].Value;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else
                {
                    if (conectado)
                    {
                        string mensaje = "37/" + nombreUsuario + "/" + id_usuario;
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);
                    }

                }
            }
            catch
            {

            }
        }
        private void pictureBoxCInvitar_Click(object sender, EventArgs e)
        {
            try
            {

                if (dataGridViewListaCon.CurrentRow.Cells[0].Value == null)//.ToString()
                {
                    MessageBox.Show("Selecciona a alguien");
                }

                else
                {
                    invitado = dataGridViewListaCon.CurrentRow.Cells[0].Value.ToString();
                    if (invitado == nombreUsuario)
                    {
                        labelCError.Text = "No te puedes invitar a ti mismo";
                        labelCError.Visible = true;

                    }
                    else
                    {
                        labelCError.Visible = false;
                        string mensaje = "41/" + formularios.Count + "/" + nombreUsuario + "/" + invitado +
                        "/" + comboBoxCSugPreg.Text + "," + comboBoxCMapa.Text +
                        "," + textBoxCNumPreg.Text + "," + textBoxCLimTiempo.Text;
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);

                        sugerirPreguntas = comboBoxCSugPreg.Text;
                        mapa = comboBoxCMapa.Text;
                        limitePreguntas = textBoxCNumPreg.Text;
                        limiteTiempo = textBoxCLimTiempo.Text;

                        ThreadStart ts = delegate { PonerEnMarchaForm(); };
                        Thread T = new Thread(ts);
                        T.Start();
                        creador_partida = nombreUsuario;
                    }

                }
            }
            catch (NullReferenceException)
            {
                labelCError.Text = "Selecciona a una persona";
                labelCError.Visible = true;
            }
        }

        private void pictureBoxMPDarseBaja_Click(object sender, EventArgs e)
        {
            if (conectado)
            {
                string mensaje = "22/" + id_usuario;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                tabControl1.SelectedTab = tabPageLogin;
                DesconectarServidor();
            }
        }
        private void buttonMMusic_Click(object sender, EventArgs e)
        {
            if (Sound)
            {
                if (MusicMenu)
                {
                    soundM.Stop();
                }
                else
                {
                    soundG.Stop();
                }
                buttonMMusic.BackgroundImage = Properties.Resources.MusicNO;
                Sound = false;
            }
            else
            {
                if (MusicMenu)
                {
                    soundM.PlayLooping();
                }
                else
                {
                    soundG.PlayLooping();
                }
                buttonMMusic.BackgroundImage = Properties.Resources.MusicYES;
                Sound = true;
            }
        }


        //NAVIEGACION
        private void pictureBoxMCrearPartida_Click(object sender, EventArgs e) => tabControl1.SelectedTab = tabPageCrearPartida;
        private void pictureBoxCVolver_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageMenu;
            labelCError.Visible = false;
        }
        private void pictureBoxPVolver_Click(object sender, EventArgs e) => tabControl1.SelectedTab = tabPageMenu;
        private void pictureBoxSVolver_Click(object sender, EventArgs e) => tabControl1.SelectedTab = tabPageMenu;
        private void pictureBoxPRVolver_Click(object sender, EventArgs e) => tabControl1.SelectedTab = tabPageSocial;


        public static Bitmap GetImageByName(string imageName)
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            string resourceName = asm.GetName().Name + ".Properties.Resources";
            var rm = new System.Resources.ResourceManager(resourceName, asm);
            return (Bitmap)rm.GetObject(imageName);

        }

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
        private void pictureBox7_MouseEnter(object sender, EventArgs e) => pictureBoxMDesconectar.Image = SetAlpha((Bitmap)pictureBoxMDesconectar.Image, 150);
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
        private void pictureBoxSVolver_MouseEnter(object sender, EventArgs e) => pictureBoxSVolver.Image = SetAlpha((Bitmap)pictureBoxSVolver.Image, 150);
        private void pictureBoxSVolver_MouseLeave(object sender, EventArgs e) => pictureBoxSVolver.Image = SetAlpha((Bitmap)pictureBoxSVolver.Image, 1000);
        private void pictureBoxPRVolver_MouseEnter(object sender, EventArgs e) => pictureBoxPRVolver.Image = SetAlpha((Bitmap)pictureBoxPRVolver.Image, 150);
        private void pictureBoxPRVolver_MouseLeave(object sender, EventArgs e) => pictureBoxPRVolver.Image = SetAlpha((Bitmap)pictureBoxPRVolver.Image, 1000);
        private void pictureBoxPVolver_MouseEnter(object sender, EventArgs e) => pictureBoxPVolver.Image = SetAlpha((Bitmap)pictureBoxPRVolver.Image, 150);
        private void pictureBoxPVolver_MouseLeave(object sender, EventArgs e) => pictureBoxPVolver.Image = SetAlpha((Bitmap)pictureBoxPRVolver.Image, 1000);
        private void pictureBoxMPDarseBaja_MouseEnter(object sender, EventArgs e) => pictureBoxMPDarseBaja.Image = SetAlpha((Bitmap)pictureBoxMPDarseBaja.Image, 150);
        private void pictureBoxMPDarseBaja_MouseLeave(object sender, EventArgs e) => pictureBoxMPDarseBaja.Image = Properties.Resources.DarseDeBaja;

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
                textBoxLContraseña.UseSystemPasswordChar = true;
                textBoxLContraseña.ForeColor = Color.Black;
            }
        }

        private void textBoxLContraseña_Leave(object sender, EventArgs e)
        {
            if (textBoxLContraseña.Text == "")
            {
                textBoxLContraseña.UseSystemPasswordChar = false;
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
                textBoxRContraseña.UseSystemPasswordChar = true;
                textBoxRContraseña.ForeColor = Color.Black;
            }
        }

        private void textBoxRContraseña_Leave(object sender, EventArgs e)
        {
            if (textBoxRContraseña.Text == "")
            {
                textBoxRContraseña.UseSystemPasswordChar = false;
                textBoxRContraseña.Text = "CONTRASEÑA";
                textBoxRContraseña.ForeColor = Color.FromArgb(173, 188, 236);
            }
        }
        private void buttonLPassword_Click(object sender, EventArgs e)
        {
            if (textBoxLContraseña.Text != "CONTRASEÑA")
            {
                if(textBoxLContraseña.UseSystemPasswordChar == false)
                {
                    textBoxLContraseña.UseSystemPasswordChar = true;
                    buttonLPassword.BackgroundImage = Properties.Resources.OpenEye;
                }
                else
                {
                    textBoxLContraseña.UseSystemPasswordChar = false;
                    buttonLPassword.BackgroundImage = Properties.Resources.CloseEye;
                }
            }
        }
        private void buttonRPassword_Click(object sender, EventArgs e)
        {
            if (textBoxRContraseña.Text != "CONTRASEÑA")
            {
                if (textBoxRContraseña.UseSystemPasswordChar == false)
                {
                    textBoxRContraseña.UseSystemPasswordChar = true;
                    buttonRPassword.BackgroundImage = Properties.Resources.OpenEye;
                }
                else
                {
                    textBoxRContraseña.UseSystemPasswordChar = false;
                    buttonRPassword.BackgroundImage = Properties.Resources.CloseEye;
                }
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

        private void ConstructFontWithString(PaintEventArgs e)
        {
            Font font1 = new Font("Arial", 20);
            e.Graphics.DrawString("Arial Font", font1, Brushes.Red, new PointF(10, 10));
        }

        
    }
}






