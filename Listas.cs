using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Control_Escolar_InstitutoHispanoamericanoMexicano
{
    public partial class Listas : Form
    {
        ConexionInsertar cnx = new ConexionInsertar();
        Conexionbuscar cn = new Conexionbuscar();
        public string usuario;
        public string puesto;
        public int contador;
        public Listas()
        {
            InitializeComponent();
        }
        public Listas(string usuario,string puesto)
        {
            InitializeComponent();
            this.usuario = usuario;
            this.puesto = puesto;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Text = DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToString("HH:mm:ss");
        }

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

        private void Listas_Load(object sender, EventArgs e)
        {
            listaPorGradoYGrupoToolStripMenuItem.Visible = false;
            if (this.puesto == "Secretaria")
            {
                modificarAlumnosToolStripMenuItem.Visible = false;
                eliminarAlumnosToolStripMenuItem.Visible = false;
            }
        }

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Menu men = new Menu(this.usuario,this.puesto);
            men.Show();
            this.Close();
        }

        private void verAlumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VerAlumno men = new VerAlumno(this.usuario, this.puesto);
            men.Show();
            this.Close();
        }

        private void agregarAlumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string accion = "agregar";
            DataAlumno men = new DataAlumno(this.usuario, this.puesto,accion);
            men.Show();
            this.Close();
        }

        private void modificarAlumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string accion = "modificar";
            DataAlumno men = new DataAlumno(this.usuario, this.puesto, accion);
            men.Show();
            this.Close();
        }

        private void eliminarAlumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string accion = "eliminar";
            DataAlumno men = new DataAlumno(this.usuario, this.puesto, accion);
            men.Show();
            this.Close();
        }

        private void comboGrado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboGrado.Text != "")
            {
                if (comboGrupo.Text != "")
                {
                    btnBuscar.Enabled = true;
                    dataAlumno.DataSource = cn.BuscarAlumnoGradoGrupo(comboGrado.Text, comboGrupo.Text);
                    comboBox1.Items.Clear();
                    MySqlConnection conexionBD = Conector.Conexiones();
                    MySqlDataReader reader = null;
                    string b = "select Curp from alumno where Grado = '" + comboGrado.Text + "' and Grupo = '" + comboGrupo.Text + "';";
                    conexionBD.Open();
                    try
                    {
                        MySqlCommand comando = new MySqlCommand(b, conexionBD);
                        reader = comando.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                comboBox1.Items.Add(reader.GetString(0));
                            }
                        }
                        else
                        {
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al buscar" + ex.Message);

                    }
                    conexionBD.Close();
                    string a = "select count(Curp) from alumno where Grado = '" + comboGrado.Text + "' and Grupo = '" + comboGrupo.Text + "';";
                    conexionBD.Open();
                    try
                    {
                        MySqlCommand comando = new MySqlCommand(a, conexionBD);
                        reader = comando.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                label3.Text = reader.GetString(0);
                            }
                        }
                        else
                        {
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al buscar" + ex.Message);

                    }
                    conexionBD.Close();
                
                if (label3.Text != "0")
                    {
                        label9.Text = "0";
                        int prueba = Convert.ToInt32(label9.Text);
                        label10.Text = comboBox1.Items[prueba].ToString();
                    }
                }
            }
        }

        private void comboGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboGrado.Text != "")
            {
                if (comboGrupo.Text != "")
                {
                    btnBuscar.Enabled = true;
                    dataAlumno.DataSource = cn.BuscarAlumnoGradoGrupo(comboGrado.Text, comboGrupo.Text);
                    comboBox1.Items.Clear();
                    MySqlConnection conexionBD = Conector.Conexiones();
                    MySqlDataReader reader = null;
                    string b = "select Curp from alumno where Grado = '" + comboGrado.Text + "' and Grupo = '" + comboGrupo.Text + "';";
                    conexionBD.Open();
                    try
                    {
                        MySqlCommand comando = new MySqlCommand(b, conexionBD);
                        reader = comando.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                comboBox1.Items.Add(reader.GetString(0));
                            }
                        }
                        else
                        {
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al buscar" + ex.Message);

                    }
                    conexionBD.Close();
                    string a = "select count(Curp) from alumno where Grado = '" + comboGrado.Text + "' and Grupo = '" + comboGrupo.Text + "' order by ApellidoPaterno;";
                    conexionBD.Open();
                    try
                    {
                        MySqlCommand comando = new MySqlCommand(a, conexionBD);
                        reader = comando.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                label3.Text = reader.GetString(0);
                            }
                        }
                        else
                        {
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al buscar" + ex.Message);

                    }
                    conexionBD.Close();

                if (label3.Text != "0")
                    {
                        label9.Text = "0";
                        int prueba = Convert.ToInt32(label9.Text);
                        label10.Text = comboBox1.Items[prueba].ToString();
                    }
                }
            }
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
                // ...
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            DateTime Today = DateTime.Now;
            string año = DateTime.Now.ToString("yyyy");
            string meses = DateTime.Now.ToString("MMMM");
            int X = 270;
            int y = 70;
            string APA;
            string AMA;
            string NA;
            string Contadores;
            string grado = comboGrado.Text;
            int valor = Convert.ToInt32(label3.Text);
            string pathPDF = @"D:\Pdfs mias\" + grado + "° Grado.pdf";
            string pathPDF2 = @txtRuta.Text + grado + "° " + comboGrupo.Text + " del mes de " + meses + " del " + año + ".pdf";
            PdfReader oReader = new PdfReader(pathPDF);
            Rectangle oSize = oReader.GetPageSizeWithRotation(1);
            Document oDocument = new Document(oSize);
            FileStream oFS = new FileStream(pathPDF2, FileMode.Create);
            PdfWriter oWriter = PdfWriter.GetInstance(oDocument, oFS);
            oDocument.Open();
            PdfContentByte oPDF = oWriter.DirectContent;
            BaseFont zz = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            oPDF.SetColorFill(BaseColor.BLACK);
            oPDF.BeginText();
            MySqlConnection conexionBD = Conector.Conexiones();
            MySqlDataReader reader = null;
            int prueba = Convert.ToInt32(label9.Text);
            oPDF.SetFontAndSize(zz, 10);
            oPDF.ShowTextAligned(0, label6.Text + " " + label7.Text + " " + label8.Text, 125, oSize.Height - 80, 0);
            oPDF.SetFontAndSize(zz, 12);
            oPDF.ShowTextAligned(0, meses, 90, oSize.Height - 88, 0);
            oPDF.ShowTextAligned(0, año, 143, oSize.Height - 101, 0);
            oPDF.SetFontAndSize(zz, 10);
            if (prueba < valor)
            {
                string a = "select ApellidoPaterno,ApellidoMaterno,Nombre,Sexo from alumno where Curp = '" + label10.Text + "' and Grado = '" + comboGrado.Text + "' and Grupo = '" + comboGrupo.Text + "';";
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(a, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            APA = reader.GetString(0);
                            AMA = reader.GetString(1);
                            NA = reader.GetString(2);
                            Contadores = reader.GetString(3);
                            oPDF.ShowTextAligned(0, Contadores, 50, oSize.Height - X, 0);
                            oPDF.ShowTextAligned(0, APA + " " + AMA + " " + NA, y, oSize.Height - X, 0);
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            label9.Text = "1";
            prueba = prueba + 1;
            X = X + 13;
            if (prueba < valor)
            {
                label10.Text = comboBox1.Items[prueba].ToString();
                string b = "select ApellidoPaterno,ApellidoMaterno,Nombre,Sexo from alumno where Curp = '" + label10.Text + "' and Grado = '" + comboGrado.Text + "' and Grupo = '" + comboGrupo.Text + "';";
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(b, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            APA = reader.GetString(0);
                            AMA = reader.GetString(1);
                            NA = reader.GetString(2);
                            Contadores = reader.GetString(3);
                            oPDF.ShowTextAligned(0, Contadores, 50, oSize.Height - X, 0);
                            oPDF.ShowTextAligned(0, APA + " " + AMA + " " + NA, y, oSize.Height - X, 0);
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            label9.Text = "2";
            prueba = prueba + 1;
            X = X + 13;
            if (prueba < valor)
            {
                label10.Text = comboBox1.Items[prueba].ToString();
                string b = "select ApellidoPaterno,ApellidoMaterno,Nombre,Sexo from alumno where Curp = '" + label10.Text + "' and Grado = '" + comboGrado.Text + "' and Grupo = '" + comboGrupo.Text + "';";
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(b, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            APA = reader.GetString(0);
                            AMA = reader.GetString(1);
                            NA = reader.GetString(2);
                            Contadores = reader.GetString(3);
                            oPDF.ShowTextAligned(0, Contadores, 50, oSize.Height - X, 0);
                            oPDF.ShowTextAligned(0, APA + " " + AMA + " " + NA, y, oSize.Height - X, 0);
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            label9.Text = "3";
            prueba = prueba + 1;
            X = X + 13;
            if (prueba < valor)
            {
                label10.Text = comboBox1.Items[prueba].ToString();
                string b = "select ApellidoPaterno,ApellidoMaterno,Nombre,Sexo from alumno where Curp = '" + label10.Text + "' and Grado = '" + comboGrado.Text + "' and Grupo = '" + comboGrupo.Text + "';";
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(b, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            APA = reader.GetString(0);
                            AMA = reader.GetString(1);
                            NA = reader.GetString(2);
                            Contadores = reader.GetString(3);
                            oPDF.ShowTextAligned(0, Contadores, 50, oSize.Height - X, 0);
                            oPDF.ShowTextAligned(0, APA + " " + AMA + " " + NA, y, oSize.Height - X, 0);
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            label9.Text = "4";
            prueba = prueba + 1;
            X = X + 13;
            if (prueba < valor)
            {
                label10.Text = comboBox1.Items[prueba].ToString();
                string b = "select ApellidoPaterno,ApellidoMaterno,Nombre,Sexo from alumno where Curp = '" + label10.Text + "' and Grado = '" + comboGrado.Text + "' and Grupo = '" + comboGrupo.Text + "';";
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(b, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            APA = reader.GetString(0);
                            AMA = reader.GetString(1);
                            NA = reader.GetString(2);
                            Contadores = reader.GetString(3);
                            oPDF.ShowTextAligned(0, Contadores, 50, oSize.Height - X, 0);
                            oPDF.ShowTextAligned(0, APA + " " + AMA + " " + NA, y, oSize.Height - X, 0);
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            label9.Text = "5";
            prueba = prueba + 1;
            X = X + 13;
            if (prueba < valor)
            {
                label10.Text = comboBox1.Items[prueba].ToString();
                string b = "select ApellidoPaterno,ApellidoMaterno,Nombre,Sexo from alumno where Curp = '" + label10.Text + "' and Grado = '" + comboGrado.Text + "' and Grupo = '" + comboGrupo.Text + "';";
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(b, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            APA = reader.GetString(0);
                            AMA = reader.GetString(1);
                            NA = reader.GetString(2);
                            Contadores = reader.GetString(3);
                            oPDF.ShowTextAligned(0, Contadores, 50, oSize.Height - X, 0);
                            oPDF.ShowTextAligned(0, APA + " " + AMA + " " + NA, y, oSize.Height - X, 0);
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            label9.Text = "6";
            prueba = prueba + 1;
            X = X + 13;
            if (prueba < valor)
            {
                label10.Text = comboBox1.Items[prueba].ToString();
                string b = "select ApellidoPaterno,ApellidoMaterno,Nombre,Sexo from alumno where Curp = '" + label10.Text + "' and Grado = '" + comboGrado.Text + "' and Grupo = '" + comboGrupo.Text + "';";
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(b, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            APA = reader.GetString(0);
                            AMA = reader.GetString(1);
                            NA = reader.GetString(2);
                            Contadores = reader.GetString(3);
                            oPDF.ShowTextAligned(0, Contadores, 50, oSize.Height - X, 0);
                            oPDF.ShowTextAligned(0, APA + " " + AMA + " " + NA, y, oSize.Height - X, 0);
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            label9.Text = "7";
            prueba = prueba + 1;
            X = X + 13;
            if (prueba < valor)
            {
                label10.Text = comboBox1.Items[prueba].ToString();
                string b = "select ApellidoPaterno,ApellidoMaterno,Nombre,Sexo from alumno where Curp = '" + label10.Text + "' and Grado = '" + comboGrado.Text + "' and Grupo = '" + comboGrupo.Text + "';";
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(b, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            APA = reader.GetString(0);
                            AMA = reader.GetString(1);
                            NA = reader.GetString(2);
                            Contadores = reader.GetString(3);
                            oPDF.ShowTextAligned(0, Contadores, 50, oSize.Height - X, 0);
                            oPDF.ShowTextAligned(0, APA + " " + AMA + " " + NA, y, oSize.Height - X, 0);
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            label9.Text = "8";
            prueba = prueba + 1;
            X = X + 13;
            if (prueba < valor)
            {
                label10.Text = comboBox1.Items[prueba].ToString();
                string b = "select ApellidoPaterno,ApellidoMaterno,Nombre,Sexo from alumno where Curp = '" + label10.Text + "' and Grado = '" + comboGrado.Text + "' and Grupo = '" + comboGrupo.Text + "';";
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(b, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            APA = reader.GetString(0);
                            AMA = reader.GetString(1);
                            NA = reader.GetString(2);
                            Contadores = reader.GetString(3);
                            oPDF.ShowTextAligned(0, Contadores, 50, oSize.Height - X, 0);
                            oPDF.ShowTextAligned(0, APA + " " + AMA + " " + NA, y, oSize.Height - X, 0);
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            label9.Text = "9";
            prueba = prueba + 1;
            X = X + 13;
            if (prueba < valor)
            {
                label10.Text = comboBox1.Items[prueba].ToString();
                string b = "select ApellidoPaterno,ApellidoMaterno,Nombre,Sexo from alumno where Curp = '" + label10.Text + "' and Grado = '" + comboGrado.Text + "' and Grupo = '" + comboGrupo.Text + "';";
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(b, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            APA = reader.GetString(0);
                            AMA = reader.GetString(1);
                            NA = reader.GetString(2);
                            Contadores = reader.GetString(3);
                            oPDF.ShowTextAligned(0, Contadores, 50, oSize.Height - X, 0);
                            oPDF.ShowTextAligned(0, APA + " " + AMA + " " + NA, y, oSize.Height - X, 0);
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            label9.Text = "10";
            prueba = prueba + 1;
            X = X + 13;
            if (prueba < valor)
            {
                label10.Text = comboBox1.Items[prueba].ToString();
                string b = "select ApellidoPaterno,ApellidoMaterno,Nombre,Sexo from alumno where Curp = '" + label10.Text + "' and Grado = '" + comboGrado.Text + "' and Grupo = '" + comboGrupo.Text + "';";
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(b, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            APA = reader.GetString(0);
                            AMA = reader.GetString(1);
                            NA = reader.GetString(2);
                            Contadores = reader.GetString(3);
                            oPDF.ShowTextAligned(0, Contadores, 50, oSize.Height - X, 0);
                            oPDF.ShowTextAligned(0, APA + " " + AMA + " " + NA, y, oSize.Height - X, 0);
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            label9.Text = "11";
            prueba = prueba + 1;
            X = X + 13;
            if (prueba < valor)
            {
                label10.Text = comboBox1.Items[prueba].ToString();
                string b = "select ApellidoPaterno,ApellidoMaterno,Nombre,Sexo from alumno where Curp = '" + label10.Text + "' and Grado = '" + comboGrado.Text + "' and Grupo = '" + comboGrupo.Text + "';";
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(b, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            APA = reader.GetString(0);
                            AMA = reader.GetString(1);
                            NA = reader.GetString(2);
                            Contadores = reader.GetString(3);
                            oPDF.ShowTextAligned(0, Contadores, 50, oSize.Height - X, 0);
                            oPDF.ShowTextAligned(0, APA + " " + AMA + " " + NA, y, oSize.Height - X, 0);
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            label9.Text = "12";
            prueba = prueba + 1;
            X = X + 13;
            if (prueba < valor)
            {
                label10.Text = comboBox1.Items[prueba].ToString();
                string b = "select ApellidoPaterno,ApellidoMaterno,Nombre,Sexo from alumno where Curp = '" + label10.Text + "' and Grado = '" + comboGrado.Text + "' and Grupo = '" + comboGrupo.Text + "';";
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(b, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            APA = reader.GetString(0);
                            AMA = reader.GetString(1);
                            NA = reader.GetString(2);
                            Contadores = reader.GetString(3);
                            oPDF.ShowTextAligned(0, Contadores, 50, oSize.Height - X, 0);
                            oPDF.ShowTextAligned(0, APA + " " + AMA + " " + NA, y, oSize.Height - X, 0);
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            label9.Text = "13";
            prueba = prueba + 1;
            X = X + 13;
            if (prueba < valor)
            {
                label10.Text = comboBox1.Items[prueba].ToString();
                string b = "select ApellidoPaterno,ApellidoMaterno,Nombre,Sexo from alumno where Curp = '" + label10.Text + "' and Grado = '" + comboGrado.Text + "' and Grupo = '" + comboGrupo.Text + "';";
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(b, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            APA = reader.GetString(0);
                            AMA = reader.GetString(1);
                            NA = reader.GetString(2);
                            Contadores = reader.GetString(3);
                            oPDF.ShowTextAligned(0, Contadores, 50, oSize.Height - X, 0);
                            oPDF.ShowTextAligned(0, APA + " " + AMA + " " + NA, y, oSize.Height - X, 0);
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            label9.Text = "14";
            prueba = prueba + 1;
            X = X + 13;
            if (prueba < valor)
            {
                label10.Text = comboBox1.Items[prueba].ToString();
                string b = "select ApellidoPaterno,ApellidoMaterno,Nombre,Sexo from alumno where Curp = '" + label10.Text + "' and Grado = '" + comboGrado.Text + "' and Grupo = '" + comboGrupo.Text + "';";
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(b, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            APA = reader.GetString(0);
                            AMA = reader.GetString(1);
                            NA = reader.GetString(2);
                            Contadores = reader.GetString(3);
                            oPDF.ShowTextAligned(0, Contadores, 50, oSize.Height - X, 0);
                            oPDF.ShowTextAligned(0, APA + " " + AMA + " " + NA, y, oSize.Height - X, 0);
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            label9.Text = "15";
            prueba = prueba + 1;
            X = X + 13;
            if (prueba < valor)
            {
                label10.Text = comboBox1.Items[prueba].ToString();
                string b = "select ApellidoPaterno,ApellidoMaterno,Nombre,Sexo from alumno where Curp = '" + label10.Text + "' and Grado = '" + comboGrado.Text + "' and Grupo = '" + comboGrupo.Text + "';";
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(b, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            APA = reader.GetString(0);
                            AMA = reader.GetString(1);
                            NA = reader.GetString(2);
                            Contadores = reader.GetString(3);
                            oPDF.ShowTextAligned(0, Contadores, 50, oSize.Height - X, 0);
                            oPDF.ShowTextAligned(0, APA + " " + AMA + " " + NA, y, oSize.Height - X, 0);
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            oPDF.EndText();
            PdfImportedPage page = oWriter.GetImportedPage(oReader, 1);
            oPDF.AddTemplate(page, 0, 0);
            oDocument.Close();
            oFS.Close();
            oWriter.Close();
            oReader.Close();
            Process.Start(@txtRuta.Text + grado + "° " + comboGrupo.Text + " del mes de " + meses + " del " + año + ".pdf");
            MessageBox.Show("Lista Realizada");
            txtRuta.Enabled = false;
        }

        private void txtRuta_TextChanged(object sender, EventArgs e)
        {
            txtRuta.Enabled = true;
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
