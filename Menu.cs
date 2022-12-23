using System;
using System.Windows.Forms;

namespace Control_Escolar_InstitutoHispanoamericanoMexicano
{
    public partial class Menu : Form
    {
        private string usuarios;
        private string puesto;
        ConexionInsertar cnx = new ConexionInsertar();
        public Menu()
        {
            InitializeComponent();
        }
        public Menu(string usuario,string puesto)
        {
            InitializeComponent();
            this.usuarios = usuario;
            this.puesto = puesto;
        }

        private void Menu_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        //Cerramos sesion
        private void Salida_Click(object sender, EventArgs e)
        {
            string usuario = this.usuarios;
            string hora = DateTime.Now.ToString("HH:mm:ss");
            string Fecha = DateTime.Now.ToString("yyyy-MM-dd");
            string accion = "Cerrar sesion";
            cnx.RegistrarBitacora(usuario, Fecha, hora, accion);
            Login log = new Login();
            log.Show();
            this.Close();
        }

        //Muestra hora y fecha en el programa
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Text = DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToString("HH:mm:ss");
        }

        //Nos conduce a las herramientas
        private void Herramienta_Click(object sender, EventArgs e)
        { 
            Herramientas log = new Herramientas(this.usuarios,this.puesto);
            log.Show();
            this.Close();
        }

        //Nos conduce al menu de alumnos
        private void alumno_Click(object sender, EventArgs e)
        {
            VerAlumno log = new VerAlumno(this.usuarios, this.puesto);
            log.Show();
            this.Close();
        }

        //Da la bienvenida al usuario y le dice los permisos que tiene
        private void Menu_Load(object sender, EventArgs e)
        {
           
        }

        private void lista_Click(object sender, EventArgs e)
        {
            Listas lis = new Listas(this.usuarios, this.puesto);
            lis.Show();
            this.Close();
        }

        private void calificacion_Click(object sender, EventArgs e)
        {
            Boleta_Interna lis = new Boleta_Interna(this.usuarios, this.puesto);
            lis.Show();
            this.Close();
        }
    }
}
