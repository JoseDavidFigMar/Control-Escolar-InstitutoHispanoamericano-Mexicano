using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Control_Escolar_InstitutoHispanoamericanoMexicano
{
    public partial class Recuperacion : Form
    {
        public Recuperacion()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Text = DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToString("HH:mm:ss");
        }

        private void Salida_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            OpenFileDialog folderBrowser = new OpenFileDialog();
            // Set validate names and check file exists to false otherwise windows will
            // not let you select "Folder Selection."
            folderBrowser.ValidateNames = false;
            folderBrowser.CheckFileExists = false;
            folderBrowser.CheckPathExists = true;
            // Always default to Folder Selection.
            folderBrowser.FileName = "Seleccionar Carpeta";
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                string folderPath = Path.GetDirectoryName(folderBrowser.FileName);
                txtRuta.Text = folderPath;
                button1.Enabled = true;
                txtRuta.Enabled = true;
                // ...
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists(@"" + txtRuta.Text + " sql5415399.sql"))
            {

                string cadenaConexion = "server = 192.168.0.9; database = preescolar; Uid = ControlEscolar; pwd=123456789";
                string archivo = @"" + txtRuta.Text + " sql5415399.sql";
                using (MySqlConnection conexionDB = new MySqlConnection(cadenaConexion))
                {
                    using (MySqlCommand comando = new MySqlCommand())
                    {
                        using (MySqlBackup respaldo = new MySqlBackup(comando))
                        {
                            comando.Connection = conexionDB;
                            conexionDB.Open();
                            respaldo.ImportFromFile(archivo);
                            conexionDB.Close();
                        }
                    }
                }
                MessageBox.Show("Recuperacion Exitosa");
            }
            else
            {
                MessageBox.Show("Archivo no encontrado para Recuperacion");
            }
        }
    }
}
