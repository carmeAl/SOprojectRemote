
namespace WindowsFormsApplication1
{
    partial class Consultas
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_sol = new System.Windows.Forms.Button();
            this.Consulta_35 = new System.Windows.Forms.RadioButton();
            this.Consulta_34 = new System.Windows.Forms.RadioButton();
            this.Consulta_33 = new System.Windows.Forms.RadioButton();
            this.Consulta_32 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.nombre = new System.Windows.Forms.TextBox();
            this.button_con = new System.Windows.Forms.Button();
            this.button_descon = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.groupBox1.Controls.Add(this.button_sol);
            this.groupBox1.Controls.Add(this.Consulta_35);
            this.groupBox1.Controls.Add(this.Consulta_34);
            this.groupBox1.Controls.Add(this.Consulta_33);
            this.groupBox1.Controls.Add(this.Consulta_32);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.nombre);
            this.groupBox1.Location = new System.Drawing.Point(27, 87);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(424, 289);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Consultas";
            // 
            // button_sol
            // 
            this.button_sol.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_sol.Location = new System.Drawing.Point(147, 217);
            this.button_sol.Name = "button_sol";
            this.button_sol.Size = new System.Drawing.Size(120, 40);
            this.button_sol.TabIndex = 6;
            this.button_sol.Text = "Solicitar";
            this.button_sol.UseVisualStyleBackColor = true;
            this.button_sol.Click += new System.EventHandler(this.button_sol_Click);
            // 
            // Consulta_35
            // 
            this.Consulta_35.AutoSize = true;
            this.Consulta_35.Location = new System.Drawing.Point(39, 174);
            this.Consulta_35.Name = "Consulta_35";
            this.Consulta_35.Size = new System.Drawing.Size(377, 17);
            this.Consulta_35.TabIndex = 5;
            this.Consulta_35.TabStop = true;
            this.Consulta_35.Text = "Lista de IDs de partidas que el usuario ha tenido con el jugador introducido";
            this.Consulta_35.UseVisualStyleBackColor = true;
            // 
            // Consulta_34
            // 
            this.Consulta_34.AutoSize = true;
            this.Consulta_34.Location = new System.Drawing.Point(39, 150);
            this.Consulta_34.Name = "Consulta_34";
            this.Consulta_34.Size = new System.Drawing.Size(345, 17);
            this.Consulta_34.TabIndex = 4;
            this.Consulta_34.TabStop = true;
            this.Consulta_34.Text = "Lista de los jugadores que han ganado contra el jugador introducido";
            this.Consulta_34.UseVisualStyleBackColor = true;
            // 
            // Consulta_33
            // 
            this.Consulta_33.AutoSize = true;
            this.Consulta_33.Location = new System.Drawing.Point(39, 126);
            this.Consulta_33.Name = "Consulta_33";
            this.Consulta_33.Size = new System.Drawing.Size(228, 17);
            this.Consulta_33.TabIndex = 3;
            this.Consulta_33.TabStop = true;
            this.Consulta_33.Text = "Suma de los puntos del jugador introducido";
            this.Consulta_33.UseVisualStyleBackColor = true;
            // 
            // Consulta_32
            // 
            this.Consulta_32.AutoSize = true;
            this.Consulta_32.Location = new System.Drawing.Point(39, 102);
            this.Consulta_32.Name = "Consulta_32";
            this.Consulta_32.Size = new System.Drawing.Size(283, 17);
            this.Consulta_32.TabIndex = 2;
            this.Consulta_32.TabStop = true;
            this.Consulta_32.Text = "Partidas consecutivas ganadas del jugador introducido";
            this.Consulta_32.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(22, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Nombre jugador:";
            // 
            // nombre
            // 
            this.nombre.Location = new System.Drawing.Point(172, 46);
            this.nombre.Name = "nombre";
            this.nombre.Size = new System.Drawing.Size(129, 20);
            this.nombre.TabIndex = 0;
            // 
            // button_con
            // 
            this.button_con.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_con.Location = new System.Drawing.Point(543, 178);
            this.button_con.Name = "button_con";
            this.button_con.Size = new System.Drawing.Size(120, 40);
            this.button_con.TabIndex = 1;
            this.button_con.Text = "Conectar";
            this.button_con.UseVisualStyleBackColor = true;
            this.button_con.Click += new System.EventHandler(this.button_con_Click);
            // 
            // button_descon
            // 
            this.button_descon.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_descon.Location = new System.Drawing.Point(543, 246);
            this.button_descon.Name = "button_descon";
            this.button_descon.Size = new System.Drawing.Size(120, 40);
            this.button_descon.TabIndex = 2;
            this.button_descon.Text = "Desconectar";
            this.button_descon.UseVisualStyleBackColor = true;
            this.button_descon.Click += new System.EventHandler(this.button_descon_Click);
            // 
            // Consultas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_descon);
            this.Controls.Add(this.button_con);
            this.Controls.Add(this.groupBox1);
            this.Name = "Consultas";
            this.Text = "Consultas";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Consultas_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_con;
        private System.Windows.Forms.Button button_descon;
        private System.Windows.Forms.Button button_sol;
        private System.Windows.Forms.RadioButton Consulta_35;
        private System.Windows.Forms.RadioButton Consulta_34;
        private System.Windows.Forms.RadioButton Consulta_33;
        private System.Windows.Forms.RadioButton Consulta_32;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nombre;
    }
}

