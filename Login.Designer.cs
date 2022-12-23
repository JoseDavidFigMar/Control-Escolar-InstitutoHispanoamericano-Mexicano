
namespace Control_Escolar_InstitutoHispanoamericanoMexicano
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textusuario = new System.Windows.Forms.TextBox();
            this.textcontraseña = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Conexion = new System.Windows.Forms.PictureBox();
            this.Salida = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Conexion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Salida)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.BackgroundImage = global::Control_Escolar_InstitutoHispanoamericanoMexicano.Properties.Resources.Logo_Sin_Fondo;
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox3.Location = new System.Drawing.Point(133, 66);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(201, 205);
            this.pictureBox3.TabIndex = 3;
            this.pictureBox3.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(80, 274);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 29);
            this.label1.TabIndex = 18;
            this.label1.Text = "Usuario:";
            // 
            // textusuario
            // 
            this.textusuario.Location = new System.Drawing.Point(237, 283);
            this.textusuario.Name = "textusuario";
            this.textusuario.Size = new System.Drawing.Size(140, 20);
            this.textusuario.TabIndex = 21;
            // 
            // textcontraseña
            // 
            this.textcontraseña.Location = new System.Drawing.Point(237, 333);
            this.textcontraseña.Name = "textcontraseña";
            this.textcontraseña.PasswordChar = '*';
            this.textcontraseña.Size = new System.Drawing.Size(140, 20);
            this.textcontraseña.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(80, 324);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 29);
            this.label2.TabIndex = 22;
            this.label2.Text = "Contraseña:";
            // 
            // Conexion
            // 
            this.Conexion.BackColor = System.Drawing.Color.Transparent;
            this.Conexion.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Conexion.BackgroundImage")));
            this.Conexion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Conexion.Location = new System.Drawing.Point(86, 378);
            this.Conexion.Name = "Conexion";
            this.Conexion.Size = new System.Drawing.Size(100, 71);
            this.Conexion.TabIndex = 26;
            this.Conexion.TabStop = false;
            this.Conexion.Click += new System.EventHandler(this.Conexion_Click);
            // 
            // Salida
            // 
            this.Salida.BackColor = System.Drawing.Color.Transparent;
            this.Salida.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Salida.BackgroundImage")));
            this.Salida.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Salida.Location = new System.Drawing.Point(278, 378);
            this.Salida.Name = "Salida";
            this.Salida.Size = new System.Drawing.Size(100, 71);
            this.Salida.TabIndex = 27;
            this.Salida.TabStop = false;
            this.Salida.Click += new System.EventHandler(this.Salida_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(159, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(161, 29);
            this.label3.TabIndex = 28;
            this.label3.Text = "BIENVENIDO";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(183, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 19);
            this.label4.TabIndex = 29;
            this.label4.Text = "Inicia Sesión";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(482, 483);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Salida);
            this.Controls.Add(this.Conexion);
            this.Controls.Add(this.textcontraseña);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textusuario);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Login_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Conexion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Salida)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textusuario;
        private System.Windows.Forms.TextBox textcontraseña;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox Conexion;
        private System.Windows.Forms.PictureBox Salida;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}