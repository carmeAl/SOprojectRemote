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

namespace Cliente_SOproject
{
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            InitializeComponent();
            //Image img = Image.FromFile("C:/Users/calca/Downloads/giphy.gif");

            //pictureBox1.Image = img;
        }

        private void pictureBox7_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxMDesconectar.Image = SetAlpha((Bitmap)pictureBoxMDesconectar.Image, 150);
        }
        private void pictureBox7_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxMDesconectar.Image = SetAlpha((Bitmap)pictureBoxMDesconectar.Image, 1000);
        }

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

        //NAVIEGACION
        private void labelLRegistrar_Click(object sender, EventArgs e) => tabControl1.SelectedTab = tabPageRegister;

        private void pictureBoxRVolver_Click(object sender, EventArgs e) => tabControl1.SelectedTab = tabPageLogin;

        private void pictureBoxMCrearPartida_Click(object sender, EventArgs e) => tabControl1.SelectedTab = tabPageCrearPartida;

        private void pictureBoxCVolver_Click(object sender, EventArgs e) => tabControl1.SelectedTab = tabPageMenu;



        //DISEÑO
        
        private void textBoxLNombre_MouseDown(object sender, MouseEventArgs e)
        {
            if (textBoxLNombre.Text == "USUARIO")
            {
                textBoxLNombre.Text = "";
                textBoxLNombre.ForeColor = Color.Black;
            }
        }

       
    }
}

