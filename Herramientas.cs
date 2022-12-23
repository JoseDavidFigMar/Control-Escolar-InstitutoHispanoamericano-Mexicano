using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Windows.Forms;

namespace Control_Escolar_InstitutoHispanoamericanoMexicano
{
    public partial class Herramientas : Form
    {
        private string usuario;
        private string puesto;

        public Herramientas()
        {
            InitializeComponent();
        }

        //Recibe los datos que ya tenemos de la ventana pasada
        public Herramientas(string usuario, string puesto)
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

        //Nos permite regresar al menu
        private void Salida_Click(object sender, EventArgs e)
        {
            Menu log = new Menu(this.usuario, this.puesto);
            log.Show();
            this.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        //Nos redirecciona a la bitacora
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Bitacora log = new Bitacora(this.usuario, this.puesto);
            log.Show();
            this.Close();
        }

        private void respaldo_Click(object sender, EventArgs e)
        {
            Respaldo res = new Respaldo();
            res.Show();
        }

        private void recuperacion_Click(object sender, EventArgs e)
        {
            Recuperacion res = new Recuperacion();
            res.Show();
        }

        private void estadistica_Click(object sender, EventArgs e)
        {
            Estadistica es = new Estadistica();
            es.Show();
        }
    }
}
