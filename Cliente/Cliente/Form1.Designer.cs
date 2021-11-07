
namespace Cliente
{
    partial class Form1
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
            this.Iniciar = new System.Windows.Forms.GroupBox();
            this.button_registro = new System.Windows.Forms.Button();
            this.button_iniciar = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_contra_in = new System.Windows.Forms.TextBox();
            this.textBox_nombre_in = new System.Windows.Forms.TextBox();
            this.Registrarse = new System.Windows.Forms.GroupBox();
            this.button_volver = new System.Windows.Forms.Button();
            this.button_registrarse = new System.Windows.Forms.Button();
            this.textBox_contra_re = new System.Windows.Forms.TextBox();
            this.textBox_nombre_re = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Consultas = new System.Windows.Forms.GroupBox();
            this.button_sol = new System.Windows.Forms.Button();
            this.Consulta_35 = new System.Windows.Forms.RadioButton();
            this.Consulta_34 = new System.Windows.Forms.RadioButton();
            this.Consulta_33 = new System.Windows.Forms.RadioButton();
            this.Consulta_32 = new System.Windows.Forms.RadioButton();
            this.textBox_nombre_consultas = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button_con = new System.Windows.Forms.Button();
            this.button_descon = new System.Windows.Forms.Button();
            this.button_mostrar = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Iniciar.SuspendLayout();
            this.Registrarse.SuspendLayout();
            this.Consultas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // Iniciar
            // 
            this.Iniciar.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Iniciar.Controls.Add(this.button_registro);
            this.Iniciar.Controls.Add(this.button_iniciar);
            this.Iniciar.Controls.Add(this.label4);
            this.Iniciar.Controls.Add(this.label3);
            this.Iniciar.Controls.Add(this.textBox_contra_in);
            this.Iniciar.Controls.Add(this.textBox_nombre_in);
            this.Iniciar.Location = new System.Drawing.Point(502, 12);
            this.Iniciar.Name = "Iniciar";
            this.Iniciar.Size = new System.Drawing.Size(269, 134);
            this.Iniciar.TabIndex = 0;
            this.Iniciar.TabStop = false;
            this.Iniciar.Text = "Iniciar";
            // 
            // button_registro
            // 
            this.button_registro.Location = new System.Drawing.Point(24, 95);
            this.button_registro.Name = "button_registro";
            this.button_registro.Size = new System.Drawing.Size(75, 23);
            this.button_registro.TabIndex = 5;
            this.button_registro.Text = "Registrarse";
            this.button_registro.UseVisualStyleBackColor = true;
            this.button_registro.Click += new System.EventHandler(this.button_registro_Click);
            // 
            // button_iniciar
            // 
            this.button_iniciar.Location = new System.Drawing.Point(177, 95);
            this.button_iniciar.Name = "button_iniciar";
            this.button_iniciar.Size = new System.Drawing.Size(75, 23);
            this.button_iniciar.TabIndex = 4;
            this.button_iniciar.Text = "Iniciar";
            this.button_iniciar.UseVisualStyleBackColor = true;
            this.button_iniciar.Click += new System.EventHandler(this.button_iniciar_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Contraseña:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Nombre:";
            // 
            // textBox_contra_in
            // 
            this.textBox_contra_in.Location = new System.Drawing.Point(112, 60);
            this.textBox_contra_in.Name = "textBox_contra_in";
            this.textBox_contra_in.Size = new System.Drawing.Size(100, 20);
            this.textBox_contra_in.TabIndex = 1;
            // 
            // textBox_nombre_in
            // 
            this.textBox_nombre_in.Location = new System.Drawing.Point(112, 30);
            this.textBox_nombre_in.Name = "textBox_nombre_in";
            this.textBox_nombre_in.Size = new System.Drawing.Size(100, 20);
            this.textBox_nombre_in.TabIndex = 0;
            // 
            // Registrarse
            // 
            this.Registrarse.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Registrarse.Controls.Add(this.button_volver);
            this.Registrarse.Controls.Add(this.button_registrarse);
            this.Registrarse.Controls.Add(this.textBox_contra_re);
            this.Registrarse.Controls.Add(this.textBox_nombre_re);
            this.Registrarse.Controls.Add(this.label2);
            this.Registrarse.Controls.Add(this.label1);
            this.Registrarse.Location = new System.Drawing.Point(502, 152);
            this.Registrarse.Name = "Registrarse";
            this.Registrarse.Size = new System.Drawing.Size(269, 146);
            this.Registrarse.TabIndex = 1;
            this.Registrarse.TabStop = false;
            this.Registrarse.Text = "Registrarse";
            // 
            // button_volver
            // 
            this.button_volver.Location = new System.Drawing.Point(21, 108);
            this.button_volver.Name = "button_volver";
            this.button_volver.Size = new System.Drawing.Size(75, 23);
            this.button_volver.TabIndex = 5;
            this.button_volver.Text = "Volver";
            this.button_volver.UseVisualStyleBackColor = true;
            this.button_volver.Click += new System.EventHandler(this.button_volver_Click);
            // 
            // button_registrarse
            // 
            this.button_registrarse.Location = new System.Drawing.Point(177, 108);
            this.button_registrarse.Name = "button_registrarse";
            this.button_registrarse.Size = new System.Drawing.Size(75, 23);
            this.button_registrarse.TabIndex = 4;
            this.button_registrarse.Text = "Registrarse";
            this.button_registrarse.UseVisualStyleBackColor = true;
            this.button_registrarse.Click += new System.EventHandler(this.button_registrarse_Click);
            // 
            // textBox_contra_re
            // 
            this.textBox_contra_re.Location = new System.Drawing.Point(102, 62);
            this.textBox_contra_re.Name = "textBox_contra_re";
            this.textBox_contra_re.Size = new System.Drawing.Size(100, 20);
            this.textBox_contra_re.TabIndex = 3;
            // 
            // textBox_nombre_re
            // 
            this.textBox_nombre_re.Location = new System.Drawing.Point(102, 31);
            this.textBox_nombre_re.Name = "textBox_nombre_re";
            this.textBox_nombre_re.Size = new System.Drawing.Size(100, 20);
            this.textBox_nombre_re.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Contraseña:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nombre:";
            // 
            // Consultas
            // 
            this.Consultas.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Consultas.Controls.Add(this.button_sol);
            this.Consultas.Controls.Add(this.Consulta_35);
            this.Consultas.Controls.Add(this.Consulta_34);
            this.Consultas.Controls.Add(this.Consulta_33);
            this.Consultas.Controls.Add(this.Consulta_32);
            this.Consultas.Controls.Add(this.textBox_nombre_consultas);
            this.Consultas.Controls.Add(this.label5);
            this.Consultas.Location = new System.Drawing.Point(12, 75);
            this.Consultas.Name = "Consultas";
            this.Consultas.Size = new System.Drawing.Size(477, 295);
            this.Consultas.TabIndex = 2;
            this.Consultas.TabStop = false;
            this.Consultas.Text = "Consultas";
            // 
            // button_sol
            // 
            this.button_sol.Location = new System.Drawing.Point(194, 232);
            this.button_sol.Name = "button_sol";
            this.button_sol.Size = new System.Drawing.Size(75, 23);
            this.button_sol.TabIndex = 6;
            this.button_sol.Text = "Solicitar";
            this.button_sol.UseVisualStyleBackColor = true;
            this.button_sol.Click += new System.EventHandler(this.button_sol_Click);
            // 
            // Consulta_35
            // 
            this.Consulta_35.AutoSize = true;
            this.Consulta_35.Location = new System.Drawing.Point(41, 190);
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
            this.Consulta_34.Location = new System.Drawing.Point(41, 159);
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
            this.Consulta_33.Location = new System.Drawing.Point(41, 124);
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
            this.Consulta_32.Location = new System.Drawing.Point(41, 90);
            this.Consulta_32.Name = "Consulta_32";
            this.Consulta_32.Size = new System.Drawing.Size(283, 17);
            this.Consulta_32.TabIndex = 2;
            this.Consulta_32.TabStop = true;
            this.Consulta_32.Text = "Partidas consecutivas ganadas del jugador introducido";
            this.Consulta_32.UseVisualStyleBackColor = true;
            // 
            // textBox_nombre_consultas
            // 
            this.textBox_nombre_consultas.Location = new System.Drawing.Point(208, 34);
            this.textBox_nombre_consultas.Name = "textBox_nombre_consultas";
            this.textBox_nombre_consultas.Size = new System.Drawing.Size(183, 20);
            this.textBox_nombre_consultas.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(37, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(144, 23);
            this.label5.TabIndex = 0;
            this.label5.Text = "Nombre jugador:";
            // 
            // button_con
            // 
            this.button_con.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_con.Location = new System.Drawing.Point(108, 29);
            this.button_con.Name = "button_con";
            this.button_con.Size = new System.Drawing.Size(120, 40);
            this.button_con.TabIndex = 3;
            this.button_con.Text = "Conectar";
            this.button_con.UseVisualStyleBackColor = true;
            this.button_con.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_descon
            // 
            this.button_descon.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_descon.Location = new System.Drawing.Point(247, 29);
            this.button_descon.Name = "button_descon";
            this.button_descon.Size = new System.Drawing.Size(120, 40);
            this.button_descon.TabIndex = 4;
            this.button_descon.Text = "Desconectar";
            this.button_descon.UseVisualStyleBackColor = true;
            this.button_descon.Click += new System.EventHandler(this.button_descon_Click);
            // 
            // button_mostrar
            // 
            this.button_mostrar.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_mostrar.Location = new System.Drawing.Point(117, 376);
            this.button_mostrar.Name = "button_mostrar";
            this.button_mostrar.Size = new System.Drawing.Size(259, 37);
            this.button_mostrar.TabIndex = 5;
            this.button_mostrar.Text = "Mostrar lista de conectados";
            this.button_mostrar.UseVisualStyleBackColor = true;
            this.button_mostrar.Click += new System.EventHandler(this.button_mostrar_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Nombre});
            this.dataGridView1.Location = new System.Drawing.Point(523, 336);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(145, 266);
            this.dataGridView1.TabIndex = 6;
            // 
            // Nombre
            // 
            this.Nombre.HeaderText = "Personas conectadas";
            this.Nombre.Name = "Nombre";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 614);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button_mostrar);
            this.Controls.Add(this.button_descon);
            this.Controls.Add(this.button_con);
            this.Controls.Add(this.Consultas);
            this.Controls.Add(this.Registrarse);
            this.Controls.Add(this.Iniciar);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Iniciar.ResumeLayout(false);
            this.Iniciar.PerformLayout();
            this.Registrarse.ResumeLayout(false);
            this.Registrarse.PerformLayout();
            this.Consultas.ResumeLayout(false);
            this.Consultas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox Iniciar;
        private System.Windows.Forms.GroupBox Registrarse;
        private System.Windows.Forms.GroupBox Consultas;
        private System.Windows.Forms.Button button_volver;
        private System.Windows.Forms.Button button_registrarse;
        private System.Windows.Forms.TextBox textBox_contra_re;
        private System.Windows.Forms.TextBox textBox_nombre_re;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_con;
        private System.Windows.Forms.Button button_descon;
        private System.Windows.Forms.Button button_registro;
        private System.Windows.Forms.Button button_iniciar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_contra_in;
        private System.Windows.Forms.TextBox textBox_nombre_in;
        private System.Windows.Forms.Button button_sol;
        private System.Windows.Forms.RadioButton Consulta_35;
        private System.Windows.Forms.RadioButton Consulta_34;
        private System.Windows.Forms.RadioButton Consulta_33;
        private System.Windows.Forms.RadioButton Consulta_32;
        private System.Windows.Forms.TextBox textBox_nombre_consultas;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_mostrar;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nombre;
    }
}

