using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Control_Escolar_InstitutoHispanoamericanoMexicano
{
    public partial class VerAlumno : Form
    {
        ConexionInsertar cnx = new ConexionInsertar();
        Conexionbuscar cn = new Conexionbuscar();
        public string usuarios;
        public string puesto;
        public string opcion;
        public VerAlumno()
        {
            InitializeComponent();
        }
        public VerAlumno(string usuarios, string puesto)
        {
            this.usuarios = usuarios;
            this.puesto = puesto;
            InitializeComponent();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Text = DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToString("HH:mm:ss");
        }

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Menu men = new Menu(this.usuarios, this.puesto);
            men.Show();
            this.Close();
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string usuario = this.usuarios;
            string hora = DateTime.Now.ToString("HH:mm:ss");
            string Fecha = DateTime.Now.ToString("yyyy-MM-dd");
            string accion = "Cerrar sesion";
            if (cnx.RegistrarBitacora(usuario, Fecha, hora, accion)) ;
            Login log = new Login();
            log.Show();
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string accion = "agregar";
            DataAlumno log = new DataAlumno(this.usuarios, this.puesto, accion);
            log.Show();
            this.Close();
        }

        private void bitacoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitacora men = new Bitacora();
            men.Show();
            this.Close();
        }

        private void VerAlumno_Load(object sender, EventArgs e)
        {
            dataAlumno.DataSource = Conexionbuscar.MostrarAlumno();
            if (this.puesto == "Director")
            {
            }
            //Oculta las ventanas que no puede usar la secretaria
            if (this.puesto == "Secretaria")
            {
                agregar.Visible = true;
                modificar.Visible = false;
                eliminar.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label3.Visible = true;
                modificarAlumnosToolStripMenuItem.Visible = false;
                eliminarAlumnosToolStripMenuItem.Visible = false;
            }
        }

        private void modificar_Click(object sender, EventArgs e)
        {
            string accion = "modificar";
            DataAlumno log = new DataAlumno(this.usuarios, this.puesto, accion);
            log.Show();
            this.Close();
        }

        private void eliminar_Click(object sender, EventArgs e)
        {
            string accion = "eliminar";
            DataAlumno log = new DataAlumno(this.usuarios, this.puesto, accion);
            log.Show();
            this.Close();
        }

        private void agregarAlumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string accion = "agregar";
            DataAlumno log = new DataAlumno(this.usuarios, this.puesto, accion);
            log.Show();
            this.Close();
        }

        private void listaPorGradoYGrupoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Listas men = new Listas(this.usuarios, this.puesto);
            men.Show();
            this.Close();
        }

        private void textBC_TextChanged(object sender, EventArgs e)
        {
            if (textBC.Text != "")
            {
                dataAlumno.DataSource = cn.BuscarCurp(textBC.Text);
            }
            else
                dataAlumno.DataSource = Conexionbuscar.MostrarAlumno();
        }

        private void textPa_TextChanged(object sender, EventArgs e)
        {
            if (textPa.Text != "")
            {
                dataAlumno.DataSource = cn.BuscarApellido(textPa.Text);
            }
            else
                dataAlumno.DataSource = Conexionbuscar.MostrarAlumno();
        }

        private void calificacionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Boleta_Interna lis = new Boleta_Interna(this.usuarios, this.puesto);
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

        private void dataAlumno_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
