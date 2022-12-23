using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Windows.Forms;

namespace Control_Escolar_InstitutoHispanoamericanoMexicano
{
    public partial class Bitacora : Form
    {
        private string usuario;
        private string puesto;
        ConexionInsertar cnx = new ConexionInsertar();
        Conexionbuscar cn = new Conexionbuscar();
        public Bitacora()
        {
            InitializeComponent();
        }

        //Recibe datos
        public Bitacora(string usuario, string puesto)
        {
            InitializeComponent();
            this.usuario = usuario;
            this.puesto = puesto;
        }

        //Muestra fecha y hora del programa
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Text = DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToString("HH:mm:ss");
        }

        //Cerramos sesion
        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string usuario = this.usuario;
            string hora = DateTime.Now.ToString("HH:mm:ss");
            string Fecha = DateTime.Now.ToString("yyyy-MM-dd");
            string accion = "Cerrar sesion";
            if (cnx.RegistrarBitacora(usuario, Fecha, hora, accion)) ;
            Login log = new Login();
            log.Show();
            this.Close();
        }

        //Nos regresa al menu
        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Menu men = new Menu(this.usuario,this.puesto);
            men.Show();
            this.Close();
        }

        //Nos direcciona a agregar usuarios
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string accion = "agregar";
            Usuario men = new Usuario(this.usuario,this.puesto,accion);
            men.Show();
            this.Close();
        }

        //Nos direcciona a modificar usuarios
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            string accion = "modificar";
            Usuario men = new Usuario(this.usuario, this.puesto, accion);
            men.Show();
            this.Close();
        }

        //Nos direcciona a eliminar usuarios
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            string accion = "eliminar";
            Usuario men = new Usuario(this.usuario, this.puesto, accion);
            men.Show();
            this.Close();
        }

        //Nos permite ver el registro ya guardado en la bitacora
        private void Bitacora_Load(object sender, EventArgs e)
        {
            dataBitacora.DataSource = Conexionbuscar.MostrarBitacora();
            if(this.puesto == "Director")
            {
            }
            //Oculta las ventanas que no puede usar la secretaria
            if(this.puesto == "Secretaria")
            {
                pictureBox1.Visible = false;
                pictureBox3.Visible = false;
                pictureBox4.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                modificarAlumnosToolStripMenuItem.Visible = false;
                eliminarAlumnosToolStripMenuItem.Visible = false;
            }
        }

        private void textBC_TextChanged(object sender, EventArgs e)
        {
            if (textusuario.Text != "")
            {
                dataBitacora.DataSource = cn.BuscarUsuario(textusuario.Text);
            }
            else
            {
                dataBitacora.DataSource = Conexionbuscar.MostrarBitacora();
            }
        }

        private void verAlumnoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VerAlumno alu = new VerAlumno(this.usuario, this.puesto);
            alu.Show();
            this.Close();
                
        }

        private void agregarAlumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string accion = "agregar";
            DataAlumno alu = new DataAlumno(this.usuario, this.puesto, accion);
            alu.Show();
            this.Close();
        }

        private void modificarAlumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string accion = "modificar";
            DataAlumno alu = new DataAlumno(this.usuario, this.puesto, accion);
            alu.Show();
            this.Close();
        }

        private void eliminarAlumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string accion = "eliminar";
            DataAlumno alu = new DataAlumno(this.usuario, this.puesto, accion);
            alu.Show();
            this.Close();
        }

        private void listaPorGradoYGrupoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Listas lis = new Listas(this.usuario, this.puesto);
            lis.Show();
            this.Close();
        }

        private void calificacionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Boleta_Interna lis = new Boleta_Interna(this.usuario, this.puesto);
            lis.Show();
            this.Close();
        }

        private void respaldoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Respaldo res = new Respaldo();
            res.Show();
        }

        private void recuperacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Recuperacion res = new Recuperacion();
            res.Show();
        }

        private void estadisticaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Estadistica es = new Estadistica();
            es.Show();
        }
    }
}