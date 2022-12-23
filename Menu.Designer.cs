
namespace Control_Escolar_InstitutoHispanoamericanoMexicano
{
    partial class Menu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Menu));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.calificacion = new System.Windows.Forms.PictureBox();
            this.lista = new System.Windows.Forms.PictureBox();
            this.Herramientas = new System.Windows.Forms.PictureBox();
            this.alumno = new System.Windows.Forms.PictureBox();
            this.Salida = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.calificacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lista)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Herramientas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.alumno)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Salida)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(60, 243);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 31);
            this.label1.TabIndex = 30;
            this.label1.Text = "Alumnos";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(700, 243);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 31);
            this.label2.TabIndex = 32;
            this.label2.Text = "Herramientas";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(308, 243);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 31);
            this.label3.TabIndex = 34;
            this.label3.Text = "Listas";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Arial Narrow", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(475, 243);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(149, 31);
            this.label4.TabIndex = 36;
            this.label4.Text = "Calificaciones";
            // 
            // calificacion
            // 
            this.calificacion.BackColor = System.Drawing.Color.Transparent;
            this.calificacion.BackgroundImage = global::Control_Escolar_InstitutoHispanoamericanoMexicano.Properties.Resources.calificaciones;
            this.calificacion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.calificacion.Location = new System.Drawing.Point(481, 277);
            this.calificacion.Name = "calificacion";
            this.calificacion.Size = new System.Drawing.Size(124, 89);
            this.calificacion.TabIndex = 35;
            this.calificacion.TabStop = false;
            this.calificacion.Click += new System.EventHandler(this.calificacion_Click);
            // 
            // lista
            // 
            this.lista.BackColor = System.Drawing.Color.Transparent;
            this.lista.BackgroundImage = global::Control_Escolar_InstitutoHispanoamericanoMexicano.Properties.Resources.listas;
            this.lista.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.lista.Location = new System.Drawing.Point(278, 277);
            this.lista.Name = "lista";
            this.lista.Size = new System.Drawing.Size(124, 89);
            this.lista.TabIndex = 33;
            this.lista.TabStop = false;
            this.lista.Click += new System.EventHandler(this.lista_Click);
            // 
            // Herramientas
            // 
            this.Herramientas.BackColor = System.Drawing.Color.Transparent;
            this.Herramientas.BackgroundImage = global::Control_Escolar_InstitutoHispanoamericanoMexicano.Properties.Resources.Herramientas;
            this.Herramientas.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Herramientas.Location = new System.Drawing.Point(715, 277);
            this.Herramientas.Name = "Herramientas";
            this.Herramientas.Size = new System.Drawing.Size(124, 89);
            this.Herramientas.TabIndex = 31;
            this.Herramientas.TabStop = false;
            this.Herramientas.Click += new System.EventHandler(this.Herramienta_Click);
            // 
            // alumno
            // 
            this.alumno.BackColor = System.Drawing.Color.Transparent;
            this.alumno.BackgroundImage = global::Control_Escolar_InstitutoHispanoamericanoMexicano.Properties.Resources.alumno;
            this.alumno.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.alumno.Location = new System.Drawing.Point(49, 277);
            this.alumno.Name = "alumno";
            this.alumno.Size = new System.Drawing.Size(124, 89);
            this.alumno.TabIndex = 29;
            this.alumno.TabStop = false;
            this.alumno.Click += new System.EventHandler(this.alumno_Click);
            // 
            // Salida
            // 
            this.Salida.BackColor = System.Drawing.Color.Transparent;
            this.Salida.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Salida.BackgroundImage")));
            this.Salida.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Salida.Location = new System.Drawing.Point(418, 465);
            this.Salida.Name = "Salida";
            this.Salida.Size = new System.Drawing.Size(100, 71);
            this.Salida.TabIndex = 28;
            this.Salida.TabStop = false;
            this.Salida.Click += new System.EventHandler(this.Salida_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.BackgroundImage = global::Control_Escolar_InstitutoHispanoamericanoMexicano.Properties.Resources.Logo_Sin_Fondo;
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox3.Location = new System.Drawing.Point(85, 3);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(248, 182);
            this.pictureBox3.TabIndex = 4;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(445, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(375, 182);
            this.pictureBox1.TabIndex = 37;
            this.pictureBox1.TabStop = false;
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(875, 548);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.calificacion);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lista);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Herramientas);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.alumno);
            this.Controls.Add(this.Salida);
            this.Controls.Add(this.pictureBox3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Menu";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Menu_FormClosed);
            this.Load += new System.EventHandler(this.Menu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.calificacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lista)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Herramientas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.alumno)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Salida)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox Salida;
        private System.Windows.Forms.PictureBox alumno;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox Herramientas;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox lista;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox calificacion;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}