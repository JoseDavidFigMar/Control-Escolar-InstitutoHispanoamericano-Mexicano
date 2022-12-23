
namespace Control_Escolar_InstitutoHispanoamericanoMexicano
{
    partial class VerAlumno
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VerAlumno));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alumnosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.agregarAlumnosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modificarAlumnosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eliminarAlumnosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listaPorGradoYGrupoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calificacionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.herramientasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.respaldoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recuperacionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.estadisticaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bitacoraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cerrarSesiónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textPa = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBC = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataAlumno = new System.Windows.Forms.DataGridView();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.agregar = new System.Windows.Forms.PictureBox();
            this.modificar = new System.Windows.Forms.PictureBox();
            this.eliminar = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataAlumno)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.agregar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.modificar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eliminar)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem,
            this.alumnosToolStripMenuItem,
            this.listaPorGradoYGrupoToolStripMenuItem,
            this.calificacionesToolStripMenuItem,
            this.herramientasToolStripMenuItem,
            this.cerrarSesiónToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(875, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.Image = global::Control_Escolar_InstitutoHispanoamericanoMexicano.Properties.Resources.Menu;
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.menuToolStripMenuItem.Text = "Menu";
            this.menuToolStripMenuItem.Click += new System.EventHandler(this.menuToolStripMenuItem_Click);
            // 
            // alumnosToolStripMenuItem
            // 
            this.alumnosToolStripMenuItem.BackgroundImage = global::Control_Escolar_InstitutoHispanoamericanoMexicano.Properties.Resources.alumno;
            this.alumnosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.agregarAlumnosToolStripMenuItem,
            this.modificarAlumnosToolStripMenuItem,
            this.eliminarAlumnosToolStripMenuItem});
            this.alumnosToolStripMenuItem.Image = global::Control_Escolar_InstitutoHispanoamericanoMexicano.Properties.Resources.alumno;
            this.alumnosToolStripMenuItem.Name = "alumnosToolStripMenuItem";
            this.alumnosToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.alumnosToolStripMenuItem.Text = "Alumnos";
            // 
            // agregarAlumnosToolStripMenuItem
            // 
            this.agregarAlumnosToolStripMenuItem.BackgroundImage = global::Control_Escolar_InstitutoHispanoamericanoMexicano.Properties.Resources.agregar;
            this.agregarAlumnosToolStripMenuItem.Image = global::Control_Escolar_InstitutoHispanoamericanoMexicano.Properties.Resources.agregar;
            this.agregarAlumnosToolStripMenuItem.Name = "agregarAlumnosToolStripMenuItem";
            this.agregarAlumnosToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.agregarAlumnosToolStripMenuItem.Text = "Agregar Alumnos";
            this.agregarAlumnosToolStripMenuItem.Click += new System.EventHandler(this.agregarAlumnosToolStripMenuItem_Click);
            // 
            // modificarAlumnosToolStripMenuItem
            // 
            this.modificarAlumnosToolStripMenuItem.Image = global::Control_Escolar_InstitutoHispanoamericanoMexicano.Properties.Resources.modificar;
            this.modificarAlumnosToolStripMenuItem.Name = "modificarAlumnosToolStripMenuItem";
            this.modificarAlumnosToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.modificarAlumnosToolStripMenuItem.Text = "Modificar Alumnos ";
            // 
            // eliminarAlumnosToolStripMenuItem
            // 
            this.eliminarAlumnosToolStripMenuItem.Image = global::Control_Escolar_InstitutoHispanoamericanoMexicano.Properties.Resources.eliminar;
            this.eliminarAlumnosToolStripMenuItem.Name = "eliminarAlumnosToolStripMenuItem";
            this.eliminarAlumnosToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.eliminarAlumnosToolStripMenuItem.Text = "Eliminar Alumnos";
            // 
            // listaPorGradoYGrupoToolStripMenuItem
            // 
            this.listaPorGradoYGrupoToolStripMenuItem.Image = global::Control_Escolar_InstitutoHispanoamericanoMexicano.Properties.Resources.listas;
            this.listaPorGradoYGrupoToolStripMenuItem.Name = "listaPorGradoYGrupoToolStripMenuItem";
            this.listaPorGradoYGrupoToolStripMenuItem.Size = new System.Drawing.Size(160, 20);
            this.listaPorGradoYGrupoToolStripMenuItem.Text = "Lista por Grado y Grupo";
            this.listaPorGradoYGrupoToolStripMenuItem.Click += new System.EventHandler(this.listaPorGradoYGrupoToolStripMenuItem_Click);
            // 
            // calificacionesToolStripMenuItem
            // 
            this.calificacionesToolStripMenuItem.Image = global::Control_Escolar_InstitutoHispanoamericanoMexicano.Properties.Resources.calificaciones;
            this.calificacionesToolStripMenuItem.Name = "calificacionesToolStripMenuItem";
            this.calificacionesToolStripMenuItem.Size = new System.Drawing.Size(108, 20);
            this.calificacionesToolStripMenuItem.Text = "Calificaciones";
            this.calificacionesToolStripMenuItem.Click += new System.EventHandler(this.calificacionesToolStripMenuItem_Click);
            // 
            // herramientasToolStripMenuItem
            // 
            this.herramientasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.respaldoToolStripMenuItem,
            this.recuperacionToolStripMenuItem,
            this.estadisticaToolStripMenuItem,
            this.bitacoraToolStripMenuItem});
            this.herramientasToolStripMenuItem.Image = global::Control_Escolar_InstitutoHispanoamericanoMexicano.Properties.Resources.Herramientas;
            this.herramientasToolStripMenuItem.Name = "herramientasToolStripMenuItem";
            this.herramientasToolStripMenuItem.Size = new System.Drawing.Size(106, 20);
            this.herramientasToolStripMenuItem.Text = "Herramientas";
            // 
            // respaldoToolStripMenuItem
            // 
            this.respaldoToolStripMenuItem.Name = "respaldoToolStripMenuItem";
            this.respaldoToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.respaldoToolStripMenuItem.Text = "Respaldo";
            this.respaldoToolStripMenuItem.Click += new System.EventHandler(this.respaldoToolStripMenuItem_Click);
            // 
            // recuperacionToolStripMenuItem
            // 
            this.recuperacionToolStripMenuItem.Name = "recuperacionToolStripMenuItem";
            this.recuperacionToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.recuperacionToolStripMenuItem.Text = "Recuperacion";
            this.recuperacionToolStripMenuItem.Click += new System.EventHandler(this.recuperacionToolStripMenuItem_Click);
            // 
            // estadisticaToolStripMenuItem
            // 
            this.estadisticaToolStripMenuItem.Name = "estadisticaToolStripMenuItem";
            this.estadisticaToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.estadisticaToolStripMenuItem.Text = "Estadistica";
            this.estadisticaToolStripMenuItem.Click += new System.EventHandler(this.estadisticaToolStripMenuItem_Click);
            // 
            // bitacoraToolStripMenuItem
            // 
            this.bitacoraToolStripMenuItem.Name = "bitacoraToolStripMenuItem";
            this.bitacoraToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.bitacoraToolStripMenuItem.Text = "Bitacora";
            this.bitacoraToolStripMenuItem.Click += new System.EventHandler(this.bitacoraToolStripMenuItem_Click);
            // 
            // cerrarSesiónToolStripMenuItem
            // 
            this.cerrarSesiónToolStripMenuItem.Image = global::Control_Escolar_InstitutoHispanoamericanoMexicano.Properties.Resources._161430963049185471;
            this.cerrarSesiónToolStripMenuItem.Name = "cerrarSesiónToolStripMenuItem";
            this.cerrarSesiónToolStripMenuItem.Size = new System.Drawing.Size(104, 20);
            this.cerrarSesiónToolStripMenuItem.Text = "Cerrar Sesión";
            this.cerrarSesiónToolStripMenuItem.Click += new System.EventHandler(this.cerrarSesiónToolStripMenuItem_Click);
            // 
            // textPa
            // 
            this.textPa.Location = new System.Drawing.Point(19, 329);
            this.textPa.Name = "textPa";
            this.textPa.Size = new System.Drawing.Size(100, 20);
            this.textPa.TabIndex = 101;
            this.textPa.TextChanged += new System.EventHandler(this.textPa_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F);
            this.label2.Location = new System.Drawing.Point(15, 308);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 18);
            this.label2.TabIndex = 100;
            this.label2.Text = "Apellido Paterno:";
            // 
            // textBC
            // 
            this.textBC.Location = new System.Drawing.Point(19, 211);
            this.textBC.Name = "textBC";
            this.textBC.Size = new System.Drawing.Size(100, 20);
            this.textBC.TabIndex = 98;
            this.textBC.TextChanged += new System.EventHandler(this.textBC_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F);
            this.label1.Location = new System.Drawing.Point(15, 188);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 18);
            this.label1.TabIndex = 97;
            this.label1.Text = " CURP:";
            // 
            // dataAlumno
            // 
            this.dataAlumno.AllowUserToAddRows = false;
            this.dataAlumno.AllowUserToDeleteRows = false;
            this.dataAlumno.BackgroundColor = System.Drawing.Color.White;
            this.dataAlumno.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataAlumno.Location = new System.Drawing.Point(192, 188);
            this.dataAlumno.Name = "dataAlumno";
            this.dataAlumno.ReadOnly = true;
            this.dataAlumno.Size = new System.Drawing.Size(652, 247);
            this.dataAlumno.TabIndex = 103;
            this.dataAlumno.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataAlumno_CellContentClick);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImage = global::Control_Escolar_InstitutoHispanoamericanoMexicano.Properties.Resources.Logo_Sin_Fondo;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox2.Location = new System.Drawing.Point(12, 27);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(169, 129);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 172;
            this.pictureBox2.TabStop = false;
            // 
            // agregar
            // 
            this.agregar.BackColor = System.Drawing.Color.Transparent;
            this.agregar.BackgroundImage = global::Control_Escolar_InstitutoHispanoamericanoMexicano.Properties.Resources.agregar;
            this.agregar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.agregar.Location = new System.Drawing.Point(230, 69);
            this.agregar.Name = "agregar";
            this.agregar.Size = new System.Drawing.Size(89, 62);
            this.agregar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.agregar.TabIndex = 173;
            this.agregar.TabStop = false;
            this.agregar.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // modificar
            // 
            this.modificar.BackColor = System.Drawing.Color.Transparent;
            this.modificar.BackgroundImage = global::Control_Escolar_InstitutoHispanoamericanoMexicano.Properties.Resources.modificar;
            this.modificar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.modificar.Location = new System.Drawing.Point(517, 69);
            this.modificar.Name = "modificar";
            this.modificar.Size = new System.Drawing.Size(94, 62);
            this.modificar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.modificar.TabIndex = 174;
            this.modificar.TabStop = false;
            this.modificar.Click += new System.EventHandler(this.modificar_Click);
            // 
            // eliminar
            // 
            this.eliminar.BackColor = System.Drawing.Color.Transparent;
            this.eliminar.BackgroundImage = global::Control_Escolar_InstitutoHispanoamericanoMexicano.Properties.Resources.eliminar;
            this.eliminar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.eliminar.Location = new System.Drawing.Point(754, 69);
            this.eliminar.Name = "eliminar";
            this.eliminar.Size = new System.Drawing.Size(90, 62);
            this.eliminar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.eliminar.TabIndex = 175;
            this.eliminar.TabStop = false;
            this.eliminar.Click += new System.EventHandler(this.eliminar_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F);
            this.label3.Location = new System.Drawing.Point(211, 48);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 18);
            this.label3.TabIndex = 176;
            this.label3.Text = "Agregar Alumno";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F);
            this.label4.Location = new System.Drawing.Point(496, 48);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(145, 18);
            this.label4.TabIndex = 177;
            this.label4.Text = "Modificar Alumno";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F);
            this.label5.Location = new System.Drawing.Point(728, 48);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 18);
            this.label5.TabIndex = 178;
            this.label5.Text = "Eliminar Alumno";
            // 
            // VerAlumno
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(875, 497);
            this.ControlBox = false;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.eliminar);
            this.Controls.Add(this.modificar);
            this.Controls.Add(this.agregar);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.dataAlumno);
            this.Controls.Add(this.textPa);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBC);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VerAlumno";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alumno";
            this.Load += new System.EventHandler(this.VerAlumno_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataAlumno)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.agregar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.modificar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eliminar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alumnosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem agregarAlumnosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modificarAlumnosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eliminarAlumnosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listaPorGradoYGrupoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem calificacionesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem herramientasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem respaldoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recuperacionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem estadisticaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cerrarSesiónToolStripMenuItem;
        private System.Windows.Forms.TextBox textPa;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataAlumno;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox agregar;
        private System.Windows.Forms.PictureBox modificar;
        private System.Windows.Forms.PictureBox eliminar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripMenuItem bitacoraToolStripMenuItem;
    }
}