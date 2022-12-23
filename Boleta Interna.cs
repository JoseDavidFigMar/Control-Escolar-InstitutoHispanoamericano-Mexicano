using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Control_Escolar_InstitutoHispanoamericanoMexicano
{
    public partial class Boleta_Interna : Form
    {

        public string usuario;
        public string puesto;
        public string trimestres;
        ConexionInsertar cnx = new ConexionInsertar();
        public Boleta_Interna()
        {
            InitializeComponent();
        }
        public Boleta_Interna(string usuario, string puesto)
        {
            InitializeComponent();
            this.usuario = usuario;
            this.puesto = puesto;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Text = DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToString("HH:mm:ss");
        }

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Menu log = new Menu(this.usuario, this.puesto);
            log.Show();
            this.Close();
        }

        private void verAlumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VerAlumno alu = new VerAlumno(this.usuario, this.puesto);
            alu.Show();
            this.Close();
        }

        private void agregarAlumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string accion = "agregar";
            DataAlumno log = new DataAlumno(this.usuario, this.puesto, accion);
            log.Show();
            this.Close();
        }

        private void Boleta_Interna_Load(object sender, EventArgs e)
        {
            //Oculta las ventanas que no puede usar la secretaria
            if (this.puesto == "Secretaria")
            {
                modificarAlumnosToolStripMenuItem.Visible = false;
                eliminarAlumnosToolStripMenuItem.Visible = false;
                modi.Visible = false;
                label17.Visible = false;
            }
            calificacionesToolStripMenuItem.Visible = false;
        }

        private void modificarAlumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string accion = "modificar";
            DataAlumno log = new DataAlumno(this.usuario, this.puesto, accion);
            log.Show();
            this.Close();
        }

        private void eliminarAlumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string accion = "eliminar";
            DataAlumno log = new DataAlumno(this.usuario, this.puesto, accion);
            log.Show();
            this.Close();
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string usuario = this.usuario;
            string hora = DateTime.Now.ToString("HH:mm:ss");
            string Fecha = DateTime.Now.ToString("yyyy-MM-dd");
            string accion = "Cerrar sesion";
            cnx.RegistrarBitacora(usuario, Fecha, hora, accion);
            Login log = new Login();
            log.Show();
            this.Close();
        }

        private void listaPorGradoYGrupoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Listas lis = new Listas(this.usuario, this.puesto);
            lis.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
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
                txtRuta.Enabled = true;
                button1.Enabled = true;
                // ...
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            MySqlDataReader reader = null;
            string sql = "Select ApellidoPaterno, ApellidoMaterno, Nombre, Grado, Grupo, CorreoTutor from alumno where Curp LIKE '" + txtCurp.Text + "' Limit 1";
            MySqlConnection conexionBD = Conector.Conexiones();
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        textAPA.Text = reader.GetString(0);
                        textAma.Text = reader.GetString(1);
                        textNom.Text = reader.GetString(2);
                        textBox1.Text = reader.GetString(3);
                        textBox2.Text = reader.GetString(4);
                        textCorreo.Text = reader.GetString(5);
                        comboBox2.Enabled = true;
                        string hora = DateTime.Now.ToString("HH:mm:ss");
                        string Fecha = DateTime.Now.ToString("yyyy-MM-dd");
                        string accion = "Busco a alumno: " + textNom.Text;
                        if (cnx.RegistrarBitacora(this.usuario, Fecha, hora, accion)) ;
                    }
                }
                else
                    MessageBox.Show("Alumno no registrado, registrelo y intente nuevamente");
            }
            catch (MySqlException ex)
            {
                comboBox2.Enabled = false;
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionBD.Close();
            comboBox1.Items.Clear();
            string b = "select Curp from alumno where Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "';";
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
            string a = "select count(Curp) from alumno where Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "';";
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

        private void button1_Click(object sender, EventArgs e)
        {
            string lec = "LENGUAJE Y COMUNICACION";
            string pm = "PENSAMIENTO MATEMATICO";
            string exp = "EXPLORACION Y COMPRENSION";
            string ar = "ARTES";
            string ef = "EDUCACION FISICA";
            string ing = "INGLES";
            string edu = "EDU. SOCIOEMOCIONAL";
            string iro = "INFORMATICA Y ROBOTICA";
            string Dia = "Diagnostico";
            string Sep = "Septiembre";
            string Octu = "Octubre";
            string Nov = "Nov/Dic";
            string Ene = "Enero";
            string Feb = "Febrero";
            string Mar = "Marzo";
            string Abr = "Abril ";
            string May = "Mayo";
            string Jun = "Junio";
            string cali;
            string grado = textBox1.Text;
            string pathPDF;
            string pathPDF2;
            string curp = txtCurp.Text;
            string Grado = textBox1.Text;
            string Grupo = textBox2.Text;
            string trimestre1 = "1";
            string trimestre2 = "2";
            string trimestre3 = "3";
            MySqlDataReader reader = null;
            MySqlConnection conexionB = Conector.Conexiones();
            int valor = Convert.ToInt32(label3.Text);
            comboBox1.Items.Clear();
            string ok = "select Curp from alumno where Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "';";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(ok, conexionB);
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
            conexionB.Close();
            string st = "select count(Curp) from alumno where Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "';";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(st, conexionB);
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
            conexionB.Close();
            if (label3.Text != "0")
            {
                label9.Text = "0";
                int prueba = Convert.ToInt32(label9.Text);
                label10.Text = comboBox1.Items[prueba].ToString();
            }
            if (textBox1.Text == "1")
            {
                string apa = textAPA.Text;
                string ama = textAma.Text;
                string nom = textNom.Text;
                string grup = textBox2.Text;
                System.IO.File.Delete(@"" + txtRuta.Text + "Calificacion del alumno " + txtCurp.Text + " del grado " + textBox1.Text + ".pdf");
                pathPDF = @"D:\Pdfs mias\Boleta Interna\1° Grado\" + grado + "°.pdf";
                pathPDF2 = @""+txtRuta.Text + "Calificacion del alumno " + txtCurp.Text + " del grado " + textBox1.Text + ".pdf";
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
                int prueba = Convert.ToInt32(label9.Text);
                oPDF.SetFontAndSize(zz, 10);
                if (prueba < valor)
                {
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "1";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "2";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "3";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "4";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "5";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "6";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "7";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "8";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "9";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "10";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "11";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "12";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "13";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "14";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "15";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "16";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "17";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "18";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "19";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "20";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "21";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "22";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "23";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "24";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "25";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                string s = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(s, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string a = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(a, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string b = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(b, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string c = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(c, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string d = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(d, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string f = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(f, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string g = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(g, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height -371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string h = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(h, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string i = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(i, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string j = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(j, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string k = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(k, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string l = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(l, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string m = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(m, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string n = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(n, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ñ = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ñ, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string o = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(o, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string p = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(p, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string q = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(q, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string r = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(r, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string t = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(t, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string u = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(u, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string v = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(v, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string w = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(w, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string x = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(x, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string y = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" +Nov + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(y, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string z = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(z, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string aa = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(aa, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ab = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ab, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ac = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ac, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ad = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ad, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string af = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(af, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ag = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ag, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ah = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ah, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ai = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ai, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string aj = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(aj, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ak = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ak, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string al = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(al, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string am = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(am, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string an = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(an, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string añ = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(añ, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ao = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ao, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ap = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ap, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string aq = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(aq, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string at = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(at, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string au = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(au, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string av = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(av, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string aw = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(aw, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ax = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ax, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ay = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ay, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string az = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(az, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ba = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ba, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bb = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bb, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bc = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bc, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bd = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bd, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string be = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(be, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bf = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bf, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bg = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bg, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 333, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bi = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bi, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 333, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bj = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bj, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 333, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bk = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bk, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 333, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bl = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bl, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 333, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bm = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bm, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 333, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bn = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bn, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 333, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bñ = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bñ, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 333, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bo = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bo, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bp = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bp, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bq = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bq, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string br = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(br, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bs = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bs, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bt = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bt, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bu = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bu, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bv = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bv, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bw = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bw, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bx = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bx, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string by = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(by, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bz = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bz, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ca = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ca, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string cb = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cb, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string cc = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cc, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string cd = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cd, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string cf = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cf, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 231, 0);
                                oPDF.EndText();                                                               
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string cg = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cg, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string ch = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ch, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string ci = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ci, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cj = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cj, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string ck = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ck, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cl = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cl, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cm = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cm, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cn = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cn, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string cñ = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cñ, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string co = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(co, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cp = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cp, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cq = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cq, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string cr = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cr, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cs = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cs, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string ct = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ct, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cu = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cu, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string cv = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cv, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cw = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cw, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cx = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cx, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cy = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cy, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string cz = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cz, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string da = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(da, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height -311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height -311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string db = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(db, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dc = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dc, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string dd = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dd, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string de = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(de, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string df = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(df, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dg = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dg, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string dh = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dh, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string di = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(di, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dj = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dj, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dk = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" +Dia+"'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dk, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dl = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Sep + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dl, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dm = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Octu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dm, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dn = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Nov + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dn, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dñ = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Ene + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dñ, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dp = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Feb + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dp, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dq = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Mar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dq, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dr = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Abr + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dr, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6) 
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string ds = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + May + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ds, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 363, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 364, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 363, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 363, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 363, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dt = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Jun + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dt, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string du = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(du, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string dv = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dv, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dw = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dw, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dx = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dx, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dy = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dy, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string dz = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dz, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string ea = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ea, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string eb = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(eb, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string ec = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ec, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string ed = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ed, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string eh = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(eh, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string ei = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ei, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string ej = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ej, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 363, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string ek = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ek, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string el = "select SUM(Inasistencias) from asistencias where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = 1";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(el, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            cali = reader.GetString(0);
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 411, 0);
                            oPDF.EndText();                            
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string em = "select SUM(Inasistencias) from asistencias where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = 2";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(em, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            cali = reader.GetString(0);
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string ep = "select SUM(Inasistencias) from asistencias where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = 3";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ep, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            cali = reader.GetString(0);
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            oPDF.ShowTextAligned(0, cali, 501, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string eñ = "select SUM(Inasistencias) from asistencias where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >=1";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(eñ, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            cali = reader.GetString(0);
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                PdfImportedPage page1 = oWriter.GetImportedPage(oReader, 1);
                oPDF.AddTemplate(page1, 0, 0);
                oDocument.Close();
                oFS.Close();
                oWriter.Close();
                oReader.Close();
                oDocument.Dispose();
                oFS.Dispose();
                oWriter.Dispose();
                oReader.Dispose();
                MessageBox.Show("PDF Generado con exito en: " + txtRuta.Text);
            }
            if (textBox1.Text == "2")
            {
                string apa = textAPA.Text;
                string ama = textAma.Text;
                string nom = textNom.Text;
                string grup = textBox2.Text;
                System.IO.File.Delete(@"" + txtRuta.Text + "Calificacion del alumno " + txtCurp.Text + " del grado " + textBox1.Text + ".pdf");
                pathPDF = @"D:\Pdfs mias\Boleta Interna\2° Grado\" + grado + "°.pdf";
                pathPDF2 = @"" + txtRuta.Text + "Calificacion del alumno " + txtCurp.Text + " del grado " + textBox1.Text + ".pdf";
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
                int prueba = Convert.ToInt32(label9.Text);
                oPDF.SetFontAndSize(zz, 10);
                if (prueba < valor)
                {
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "1";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "2";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "3";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "4";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "5";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "6";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "7";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "8";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "9";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "10";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "11";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "12";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "13";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "14";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "15";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "16";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "17";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "18";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "19";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "20";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "21";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "22";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "23";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "24";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "25";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }

                string s = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(s, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string a = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(a, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string b = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(b, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string c = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(c, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string d = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(d, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string f = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(f, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string g = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(g, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string h = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(h, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string i = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(i, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string j = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(j, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string k = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(k, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string l = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(l, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string m = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(m, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string n = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(n, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ñ = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ñ, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string o = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(o, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string p = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(p, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string q = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(q, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string r = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(r, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string t = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(t, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string u = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(u, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string v = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(v, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string w = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(w, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string x = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(x, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string y = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(y, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string z = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(z, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string aa = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(aa, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ab = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ab, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ac = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ac, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ad = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ad, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string af = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(af, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ag = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ag, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ah = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ah, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ai = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ai, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string aj = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(aj, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ak = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ak, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string al = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(al, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string am = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(am, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string an = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(an, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string añ = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(añ, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ao = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ao, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ap = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ap, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string aq = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(aq, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string at = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(at, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string au = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(au, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string av = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(av, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string aw = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(aw, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ax = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ax, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ay = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ay, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string az = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(az, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ba = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ba, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bb = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bb, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bc = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bc, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bd = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bd, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string be = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(be, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bf = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bf, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bg = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bg, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bi = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bi, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bj = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bj, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bk = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bk, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bl = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bl, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bm = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bm, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bn = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bn, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bñ = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bñ, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bo = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bo, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bp = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bp, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bq = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bq, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string br = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(br, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bs = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bs, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bt = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bt, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bu = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bu, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bv = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bv, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bw = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bw, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bx = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bx, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string by = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(by, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bz = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bz, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ca = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ca, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string cb = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cb, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string cc = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cc, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string cd = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cd, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string cf = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cf, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string cg = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cg, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string ch = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ch, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string ci = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ci, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cj = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cj, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string ck = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ck, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cl = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cl, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cm = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cm, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cn = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cn, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string cñ = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cñ, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string co = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(co, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cp = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cp, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cq = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cq, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string cr = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cr, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cs = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cs, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string ct = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ct, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cu = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cu, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string cv = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cv, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cw = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cw, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cx = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cx, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cy = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cy, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string cz = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cz, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string da = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(da, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string db = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(db, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dc = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dc, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string dd = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dd, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string de = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(de, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string df = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(df, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dg = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dg, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string dh = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dh, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string di = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(di, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dj = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dj, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dk = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Dia + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dk, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dl = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Sep + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dl, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dm = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Octu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dm, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dn = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Nov + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dn, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dñ = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Ene + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dñ, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dp = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Feb + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dp, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dq = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Mar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dq, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dr = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Abr + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dr, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string ds = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + May + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ds, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 363, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 364, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 363, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 363, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 363, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dt = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Jun + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dt, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string du = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(du, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string dv = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dv, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dw = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dw, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dx = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dx, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dy = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dy, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string dz = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dz, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string ea = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ea, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string eb = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(eb, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string ec = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ec, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string ed = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ed, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string eh = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(eh, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string ei = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ei, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string ej = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ej, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 363, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string ek = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ek, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string el = "select  SUM(Inasistencias) from asistencias where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(el, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            cali = reader.GetString(0);
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string em = "select SUM(Inasistencias) from asistencias where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(em, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            cali = reader.GetString(0);
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string ep = "select SUM(Inasistencias) from asistencias where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ep, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            cali = reader.GetString(0);
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            oPDF.ShowTextAligned(0, cali, 501, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string eñ = "select SUM(Inasistencias) from asistencias where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >=1";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(eñ, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            cali = reader.GetString(0);
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                PdfImportedPage page1 = oWriter.GetImportedPage(oReader, 1);
                oPDF.AddTemplate(page1, 0, 0);
                oDocument.Close();
                oFS.Close();
                oWriter.Close();
                oReader.Close();
                oDocument.Dispose();
                oFS.Dispose();
                oWriter.Dispose();
                oReader.Dispose();
                MessageBox.Show("PDF Generado con exito en: " + txtRuta.Text);
            }
            if (textBox1.Text == "3")
            {
                string apa = textAPA.Text;
                string ama = textAma.Text;
                string nom = textNom.Text;
                string grup = textBox2.Text;
                System.IO.File.Delete(@"" + txtRuta.Text + "Calificacion del alumno " + txtCurp.Text + " del grado " + textBox1.Text + ".pdf");
                pathPDF = @"D:\Pdfs mias\Boleta Interna\3° Grado\" + grado + "°.pdf";
                pathPDF2 = @"" + txtRuta.Text + "Calificacion del alumno " + txtCurp.Text + " del grado " + textBox1.Text + ".pdf";
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
                int prueba = Convert.ToInt32(label9.Text);
                oPDF.SetFontAndSize(zz, 10);
                if (prueba < valor)
                {
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "1";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "2";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "3";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "4";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "5";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "6";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "7";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "8";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "9";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "10";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "11";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "12";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "13";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "14";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "15";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "16";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "17";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "18";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "19";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "20";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "21";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "22";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "23";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "24";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }
                label9.Text = "25";
                prueba = prueba + 1;
                if (prueba < valor)
                {
                    label10.Text = comboBox1.Items[prueba].ToString();
                    if (txtCurp.Text == label10.Text)
                    {
                        oPDF.SetColorFill(BaseColor.BLACK);
                        oPDF.SetFontAndSize(zz, 12);
                        string espacios = " ";
                        oPDF.ShowTextAligned(0, apa + espacios + ama + espacios + nom, 270, oSize.Height - 145, 0);
                        oPDF.EndText();
                        oPDF.SetFontAndSize(zz, 11);
                        oPDF.BeginText();
                        oPDF.ShowTextAligned(0, Convert.ToString(prueba + 1), 252, oSize.Height - 112, 0);
                        oPDF.ShowTextAligned(0, grup, 440, oSize.Height - 101, 0);
                        oPDF.EndText();
                    }
                }

                string s = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(s, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string a = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(a, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string b = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(b, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string c = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(c, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string d = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(d, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string f = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(f, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string g = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(g, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string h = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(h, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string i = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(i, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string j = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(j, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string k = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(k, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string l = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(l, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string m = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(m, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string n = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(n, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ñ = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ñ, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string o = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(o, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string p = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(p, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string q = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(q, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string r = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(r, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string t = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(t, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string u = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(u, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string v = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(v, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string w = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(w, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string x = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(x, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string y = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(y, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string z = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(z, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string aa = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(aa, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ab = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ab, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ac = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ac, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ad = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ad, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string af = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(af, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ag = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ag, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ah = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ah, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ai = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ai, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string aj = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(aj, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ak = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ak, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string al = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(al, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string am = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(am, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string an = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(an, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string añ = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(añ, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ao = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ao, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ap = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ap, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string aq = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(aq, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string at = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(at, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string au = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(au, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string av = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(av, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string aw = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(aw, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ax = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ax, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ay = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ay, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string az = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(az, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ba = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ba, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bb = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bb, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bc = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bc, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bd = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bd, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string be = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(be, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bf = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bf, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bg = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bg, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bi = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bi, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bj = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bj, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bk = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bk, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bl = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bl, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bm = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bm, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bn = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bn, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bñ = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bñ, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bo = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bo, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bp = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bp, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bq = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bq, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string br = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(br, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bs = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bs, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bt = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bt, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bu = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bu, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bv = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bv, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 360, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bw = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bw, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 231, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bx = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bx, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 245, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string by = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(by, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 264, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string bz = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(bz, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 283, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string ca = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ca, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 297, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string cb = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cb, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 311, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();
                string cc = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cc, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 371, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string cd = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cd, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 384, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string cf = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cf, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string cg = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cg, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string ch = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ch, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string ci = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + lec + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ci, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 231, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cj = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cj, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string ck = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ck, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cl = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cl, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cm = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + pm + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cm, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 245, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cn = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cn, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string cñ = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cñ, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string co = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(co, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cp = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + exp + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cp, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 264, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cq = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cq, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string cr = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cr, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cs = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cs, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string ct = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + ar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ct, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 283, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cu = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cu, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string cv = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cv, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cw = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cw, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cx = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + ef + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cx, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 297, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string cy = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cy, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string cz = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(cz, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string da = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(da, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string db = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + ing + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(db, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 311, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dc = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dc, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string dd = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dd, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string de = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(de, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string df = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + edu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(df, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 371, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dg = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dg, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string dh = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dh, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string di = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(di, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dj = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1 and Materia = '" + iro + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dj, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 384, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dk = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Dia + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dk, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dl = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Sep + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dl, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dm = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Octu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dm, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dn = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Nov + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dn, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dñ = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Ene + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dñ, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dp = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Feb + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dp, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dq = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Mar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dq, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dr = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Abr + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dr, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string ds = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + May + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ds, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 363, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 364, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 363, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 363, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 363, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dt = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Mes = '" + Jun + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dt, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string du = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(du, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string dv = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dv, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dw = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dw, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 507, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dx = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >= 1";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dx, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string val = reader.GetString(0);
                            int datos = Convert.ToInt32(val);
                            if (datos <= 6)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "NA";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 7)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "R";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 8)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "B";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 9)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "MB";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                            if (datos == 10)
                            {
                                oPDF.SetFontAndSize(zz, 10);
                                oPDF.BeginText();
                                cali = "E";
                                oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 325, 0);
                                oPDF.EndText();
                            }
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                conexionB.Close();

                string dy = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Dia + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dy, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 188, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string dz = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Sep + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(dz, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 208, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string ea = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Octu + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ea, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 228, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string eb = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Nov + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(eb, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 248, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string ec = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Ene + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ec, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 268, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string ed = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Feb + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ed, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 288, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string eh = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Mar + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(eh, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 313, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string ei = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Abr + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ei, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 338, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string ej = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + May + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ej, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 363, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string ek = "Select Inasistencias from asistencias where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + Jun + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ek, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            cali = reader.GetString(0);
                            oPDF.ShowTextAligned(0, cali, 383, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }

                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionB.Close();

                string el = "select  SUM(Inasistencias) from asistencias where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(el, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            cali = reader.GetString(0);
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            oPDF.ShowTextAligned(0, cali, 433, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string em = "select SUM(Inasistencias) from asistencias where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(em, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            cali = reader.GetString(0);
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            oPDF.ShowTextAligned(0, cali, 467, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string ep = "select SUM(Inasistencias) from asistencias where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "'";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(ep, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            cali = reader.GetString(0);
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            oPDF.ShowTextAligned(0, cali, 501, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();

                string eñ = "select SUM(Inasistencias) from asistencias where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre >=1";
                conexionB.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(eñ, conexionB);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            cali = reader.GetString(0);
                            oPDF.SetFontAndSize(zz, 10);
                            oPDF.BeginText();
                            oPDF.ShowTextAligned(0, cali, 545, oSize.Height - 411, 0);
                            oPDF.EndText();
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                conexionB.Close();
                PdfImportedPage page1 = oWriter.GetImportedPage(oReader, 1);
                oPDF.AddTemplate(page1, 0, 0);
                oDocument.Close();
                oFS.Close();
                oWriter.Close();
                oReader.Close();
                oDocument.Dispose();
                oFS.Dispose();
                oWriter.Dispose();
                oReader.Dispose();
                MessageBox.Show("PDF Generado con exito en: " + txtRuta.Text);
            }
            Process.Start(@"" + txtRuta.Text + "Calificacion del alumno " + txtCurp.Text + " del grado " + textBox1.Text + ".pdf");
            button3.Enabled = true;
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string lec = "LENGUAJE Y COMUNICACION";
            string pm = "PENSAMIENTO MATEMATICO";
            string exp = "EXPLORACION Y COMPRENSION";
            string ar = "ARTES";
            string ef = "EDUCACION FISICA";
            string ing = "INGLES";
            string edu = "EDU. SOCIOEMOCIONAL";
            string iro = "INFORMATICA Y ROBOTICA";
            MySqlDataReader reader = null;
            if (comboBox2.Text == "Diagnostico")
            {
                txt1.Enabled = true;
                txt5.Enabled = true;
                txt2.Enabled = true;
                txt6.Enabled = true;
                txt3.Enabled = true;
                txt7.Enabled = true;
                txt4.Enabled = true;
                txt8.Enabled = true;
                this.trimestres = "0";
                guar.Visible = true;
                label8.Visible = true;
                button2.Enabled = false;
            }
            if (comboBox2.Text == "Septiembre")
            {
                string curp = txtCurp.Text;
                string grado = textBox1.Text;
                string grupo = textBox2.Text;
                string mes = "Diagnostico";
                string sql = "Select curp, grado, grupo from calificacionmensual where Curp ='" + curp + "' and  Grado = '" + grado + "' and Grupo = '" + grupo + "' and Mes = '" + mes + "'";
                MySqlConnection conexionBD = Conector.Conexiones();
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            txt1.Enabled = true;
                            txt5.Enabled = true;
                            txt2.Enabled = true;
                            txt6.Enabled = true;
                            txt3.Enabled = true;
                            txt7.Enabled = true;
                            txt4.Enabled = true;
                            txt8.Enabled = true;
                            this.trimestres = "1";
                            guar.Visible = true;
                            label8.Visible = true;
                            button2.Enabled = false;
                        }
                    }
                    else
                    {
                        txt1.Enabled = false;
                        txt5.Enabled = false;
                        txt2.Enabled = false;
                        txt6.Enabled = false;
                        txt3.Enabled = false;
                        txt7.Enabled = false;
                        txt4.Enabled = false;
                        txt8.Enabled = false;
                        button2.Enabled = false;
                        MessageBox.Show("Mes no corresponde falta valores");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            if (comboBox2.Text == "Octubre")
            {
                string curp = txtCurp.Text;
                string grado = textBox1.Text;
                string grupo = textBox2.Text;
                string mes = "Septiembre";
                string sql = "Select curp, grado, grupo from calificacionmensual where Curp ='" + curp + "' and  Grado = '" + grado + "' and Grupo = '" + grupo + "' and Mes = '" + mes + "'";
                MySqlConnection conexionBD = Conector.Conexiones();
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            txt1.Enabled = true;
                            txt5.Enabled = true;
                            txt2.Enabled = true;
                            txt6.Enabled = true;
                            txt3.Enabled = true;
                            txt7.Enabled = true;
                            txt4.Enabled = true;
                            txt8.Enabled = true;
                            this.trimestres = "1";
                            guar.Visible = true;
                            label8.Visible = true;
                            button2.Enabled = false;
                        }
                    }
                    else
                    {
                        txt1.Enabled = false;
                        txt5.Enabled = false;
                        txt2.Enabled = false;
                        txt6.Enabled = false;
                        txt3.Enabled = false;
                        txt7.Enabled = false;
                        txt4.Enabled = false;
                        txt8.Enabled = false;
                        button2.Enabled = false;
                        MessageBox.Show("Mes no corresponde falta valores");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            if (comboBox2.Text == "Nov/Dic")
            {
                string curp = txtCurp.Text;
                string grado = textBox1.Text;
                string grupo = textBox2.Text;
                string mes = "Octubre";
                string sql = "Select curp, grado, grupo from calificacionmensual where Curp ='" + curp + "' and  Grado = '" + grado + "' and Grupo = '" + grupo + "' and Mes = '" + mes + "'";
                MySqlConnection conexionBD = Conector.Conexiones();
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            txt1.Enabled = true;
                            txt5.Enabled = true;
                            txt2.Enabled = true;
                            txt6.Enabled = true;
                            txt3.Enabled = true;
                            txt7.Enabled = true;
                            txt4.Enabled = true;
                            txt8.Enabled = true;
                            this.trimestres = "1";
                            guar.Visible = true;
                            label8.Visible = true;
                            button2.Enabled = false;
                        }
                    }
                    else
                    {
                        txt1.Enabled = false;
                        txt5.Enabled = false;
                        txt2.Enabled = false;
                        txt6.Enabled = false;
                        txt3.Enabled = false;
                        txt7.Enabled = false;
                        txt4.Enabled = false;
                        txt8.Enabled = false;
                        button2.Enabled = false;
                        MessageBox.Show("Mes no corresponde falta valores");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            if (comboBox2.Text == "Enero")
            {
                string curp = txtCurp.Text;
                string grado = textBox1.Text;
                string grupo = textBox2.Text;
                string mes = "Nov/Dic";
                string sql = "Select curp, grado, grupo from calificacionmensual where Curp ='" + curp + "' and  Grado = '" + grado + "' and Grupo = '" + grupo + "' and Mes = '" + mes + "'";
                MySqlConnection conexionBD = Conector.Conexiones();
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            txt1.Enabled = true;
                            txt5.Enabled = true;
                            txt2.Enabled = true;
                            txt6.Enabled = true;
                            txt3.Enabled = true;
                            txt7.Enabled = true;
                            txt4.Enabled = true;
                            txt8.Enabled = true;
                            this.trimestres = "2";
                            guar.Visible = true;
                            label8.Visible = true;
                            button2.Enabled = false;
                        }
                    }
                    else
                    {
                        txt1.Enabled = false;
                        txt5.Enabled = false;
                        txt2.Enabled = false;
                        txt6.Enabled = false;
                        txt3.Enabled = false;
                        txt7.Enabled = false;
                        txt4.Enabled = false;
                        txt8.Enabled = false;
                        button2.Enabled = false;
                        MessageBox.Show("Mes no corresponde falta valores");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            if (comboBox2.Text == "Febrero")
            {
                string curp = txtCurp.Text;
                string grado = textBox1.Text;
                string grupo = textBox2.Text;
                string mes = "Enero";
                string sql = "Select curp, grado, grupo from calificacionmensual where Curp ='" + curp + "' and  Grado = '" + grado + "' and Grupo = '" + grupo + "' and Mes = '" + mes + "'";
                MySqlConnection conexionBD = Conector.Conexiones();
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            txt1.Enabled = true;
                            txt5.Enabled = true;
                            txt2.Enabled = true;
                            txt6.Enabled = true;
                            txt3.Enabled = true;
                            txt7.Enabled = true;
                            txt4.Enabled = true;
                            txt8.Enabled = true;
                            this.trimestres = "2";
                            guar.Visible = true;
                            label8.Visible = true;
                            button2.Enabled = false;
                        }
                    }
                    else
                    {
                        txt1.Enabled = false;
                        txt5.Enabled = false;
                        txt2.Enabled = false;
                        txt6.Enabled = false;
                        txt3.Enabled = false;
                        txt7.Enabled = false;
                        txt4.Enabled = false;
                        txt8.Enabled = false;
                        button2.Enabled = false;
                        MessageBox.Show("Mes no corresponde falta valores");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            if (comboBox2.Text == "Marzo")
            {
                string curp = txtCurp.Text;
                string grado = textBox1.Text;
                string grupo = textBox2.Text;
                string mes = "Febrero";
                string sql = "Select curp, grado, grupo from calificacionmensual where Curp ='" + curp + "' and  Grado = '" + grado + "' and Grupo = '" + grupo + "' and Mes = '" + mes + "'";
                MySqlConnection conexionBD = Conector.Conexiones();
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            txt1.Enabled = true;
                            txt5.Enabled = true;
                            txt2.Enabled = true;
                            txt6.Enabled = true;
                            txt3.Enabled = true;
                            txt7.Enabled = true;
                            txt4.Enabled = true;
                            txt8.Enabled = true;
                            this.trimestres = "2";
                            guar.Visible = true;
                            label8.Visible = true;
                            button2.Enabled = false;
                        }
                    }
                    else
                    {
                        txt1.Enabled = false;
                        txt5.Enabled = false;
                        txt2.Enabled = false;
                        txt6.Enabled = false;
                        txt3.Enabled = false;
                        txt7.Enabled = false;
                        txt4.Enabled = false;
                        txt8.Enabled = false;
                        button2.Enabled = false;
                        MessageBox.Show("Mes no corresponde falta valores");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            if (comboBox2.Text == "Abril ")
            {
                string curp = txtCurp.Text;
                string grado = textBox1.Text;
                string grupo = textBox2.Text;
                string mes = "Marzo";
                string sql = "Select curp, grado, grupo from calificacionmensual where Curp ='" + curp + "' and  Grado = '" + grado + "' and Grupo = '" + grupo + "' and Mes = '" + mes + "'";
                MySqlConnection conexionBD = Conector.Conexiones();
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            txt1.Enabled = true;
                            txt5.Enabled = true;
                            txt2.Enabled = true;
                            txt6.Enabled = true;
                            txt3.Enabled = true;
                            txt7.Enabled = true;
                            txt4.Enabled = true;
                            txt8.Enabled = true;
                            this.trimestres = "3";
                            guar.Visible = true;
                            label8.Visible = true;
                            button2.Enabled = false;
                        }
                    }
                    else
                    {
                        txt1.Enabled = false;
                        txt5.Enabled = false;
                        txt2.Enabled = false;
                        txt6.Enabled = false;
                        txt3.Enabled = false;
                        txt7.Enabled = false;
                        txt4.Enabled = false;
                        txt8.Enabled = false;
                        button2.Enabled = false;
                        MessageBox.Show("Mes no corresponde falta valores");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            if (comboBox2.Text == "Mayo")
            {
                string curp = txtCurp.Text;
                string grado = textBox1.Text;
                string grupo = textBox2.Text;
                string mes = "Abril ";
                string sql = "Select curp, grado, grupo from calificacionmensual where Curp ='" + curp + "' and  Grado = '" + grado + "' and Grupo = '" + grupo + "' and Mes = '" + mes + "'";
                MySqlConnection conexionBD = Conector.Conexiones();
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            txt1.Enabled = true;
                            txt5.Enabled = true;
                            txt2.Enabled = true;
                            txt6.Enabled = true;
                            txt3.Enabled = true;
                            txt7.Enabled = true;
                            txt4.Enabled = true;
                            txt8.Enabled = true;
                            this.trimestres = "3";
                            guar.Visible = true;
                            label8.Visible = true;
                            button2.Enabled = false;
                        }
                    }
                    else
                    {
                        txt1.Enabled = false;
                        txt5.Enabled = false;
                        txt2.Enabled = false;
                        txt6.Enabled = false;
                        txt3.Enabled = false;
                        txt7.Enabled = false;
                        txt4.Enabled = false;
                        txt8.Enabled = false;
                        button2.Enabled = false;
                        MessageBox.Show("Mes no corresponde falta valores");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            if (comboBox2.Text == "Junio")
            {
                string curp = txtCurp.Text;
                string grado = textBox1.Text;
                string grupo = textBox2.Text;
                string mes = "Mayo";
                string sql = "Select curp, grado, grupo from calificacionmensual where Curp ='" + curp + "' and  Grado = '" + grado + "' and Grupo = '" + grupo + "' and Mes = '" + mes + "'";
                MySqlConnection conexionBD = Conector.Conexiones();
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            txt1.Enabled = true;
                            txt5.Enabled = true;
                            txt2.Enabled = true;
                            txt6.Enabled = true;
                            txt3.Enabled = true;
                            txt7.Enabled = true;
                            txt4.Enabled = true;
                            txt8.Enabled = true;
                            this.trimestres = "3";
                            guar.Visible = true;
                            label8.Visible = true;
                            button2.Enabled = false;
                        }
                    }
                    else
                    {
                        txt1.Enabled = false;
                        txt5.Enabled = false;
                        txt2.Enabled = false;
                        txt6.Enabled = false;
                        txt3.Enabled = false;
                        txt7.Enabled = false;
                        txt4.Enabled = false;
                        txt8.Enabled = false;
                        button2.Enabled = false;
                        MessageBox.Show("Mes no corresponde falta valores");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            txt1.Clear(); txt5.Clear(); txt2.Clear(); txt6.Clear(); txt3.Clear(); txt7.Clear();
            txt4.Clear(); txt8.Clear();
            MySqlConnection conexionB = Conector.Conexiones();
            string s = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '"+textBox1.Text+"' and Grupo = '"+textBox2.Text+"' and Mes = '"+comboBox2.Text+"' and Materia = '"+lec+"'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(s, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        txt1.Text = reader.GetString(0);                       
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionB.Close();
            string a = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + comboBox2.Text + "' and Materia = '" +pm + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(a, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        txt5.Text = reader.GetString(0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionB.Close();
            string b = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + comboBox2.Text + "' and Materia = '" + exp + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(b, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        txt2.Text = reader.GetString(0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionB.Close();
            string c = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + comboBox2.Text + "' and Materia = '" + ar + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(c, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        txt6.Text = reader.GetString(0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionB.Close();
            string d = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + comboBox2.Text + "' and Materia = '" + ef + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(d, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        txt3.Text = reader.GetString(0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionB.Close();
            string f = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + comboBox2.Text + "' and Materia = '" + ing + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(f, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        txt7.Text = reader.GetString(0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionB.Close();
            string g = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + comboBox2.Text + "' and Materia = '" + edu + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(g, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        txt4.Text = reader.GetString(0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionB.Close();
            string h = "Select Calificacion from calificacionmensual where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + comboBox2.Text + "' and Materia = '" + iro+ "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(h, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        txt8.Text = reader.GetString(0);
                        guar.Visible = false;
                        label8.Visible = false;
                        button2.Enabled = true;
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionB.Close();
        }

        private void guar_Click(object sender, EventArgs e)
        {
            if (label18.Text != " ")
            {
                if (label19.Text != " ")
                {
                    if (label20.Text != " ")
                    {
                        if (label21.Text != " ")
                        {
                            if (label22.Text != " ")
                            {
                                if (label23.Text != " ")
                                {
                                    if (label24.Text != " ")
                                    {
                                        if (label25.Text != " ")
                                        {
                                            string Asistencia;
                                            string Inasistencia;
                                            Asistencia = Microsoft.VisualBasic.Interaction.InputBox("Ingrese Asistencias", "Registro de Asistencias", "Asistencias");
                                            Inasistencia = Microsoft.VisualBasic.Interaction.InputBox("Ingrese Inasistencias", "Registro de Inasistencias", "Inasistencias");
                                            int total = Convert.ToInt32(Asistencia) + Convert.ToInt32(Inasistencia);
                                            if (total >= 15)
                                            {
                                                if (total <= 26)
                                                {
                                                    cnx.RegistrarAsistencia(txtCurp.Text, textBox1.Text, textBox2.Text, comboBox2.Text, this.trimestres, Asistencia, Inasistencia);
                                                    cnx.RegistrarBoletaInterna(txtCurp.Text, textBox1.Text, textBox2.Text, comboBox2.Text, this.trimestres, len.Text, txt1.Text, label18.Text);
                                                    cnx.RegistrarBoletaInterna(txtCurp.Text, textBox1.Text, textBox2.Text, comboBox2.Text, this.trimestres, PEN.Text, txt5.Text, label19.Text);
                                                    cnx.RegistrarBoletaInterna(txtCurp.Text, textBox1.Text, textBox2.Text, comboBox2.Text, this.trimestres, EXP.Text, txt2.Text, label20.Text);
                                                    cnx.RegistrarBoletaInterna(txtCurp.Text, textBox1.Text, textBox2.Text, comboBox2.Text, this.trimestres, label12.Text, txt6.Text, label21.Text);
                                                    cnx.RegistrarBoletaInterna(txtCurp.Text, textBox1.Text, textBox2.Text, comboBox2.Text, this.trimestres, label13.Text, txt3.Text, label22.Text);
                                                    cnx.RegistrarBoletaInterna(txtCurp.Text, textBox1.Text, textBox2.Text, comboBox2.Text, this.trimestres, label14.Text, txt7.Text, label23.Text);
                                                    cnx.RegistrarBoletaInterna(txtCurp.Text, textBox1.Text, textBox2.Text, comboBox2.Text, this.trimestres, label15.Text, txt4.Text, label24.Text);
                                                    cnx.RegistrarBoletaInterna(txtCurp.Text, textBox1.Text, textBox2.Text, comboBox2.Text, this.trimestres, label16.Text, txt8.Text, label25.Text);
                                                    button2.Enabled = true;
                                                    MessageBox.Show("Calificaciones del mes " + comboBox2.Text + " del alumno " + txtCurp.Text + " agregadas");                                                   
                                                    if(comboBox2.Text == "Nov/Dic")
                                                    {
                                                        Boleta_Externa exte = new Boleta_Externa(this.usuario,this.puesto,txtCurp.Text, comboBox2.Text);
                                                        exte.Show();
                                                    }
                                                    if (comboBox2.Text == "Marzo")
                                                    {
                                                        Boleta_Externa exte = new Boleta_Externa(this.usuario, this.puesto, txtCurp.Text, comboBox2.Text);
                                                        exte.Show();
                                                    }
                                                    if (comboBox2.Text == "Junio")
                                                    {
                                                        Boleta_Externa exte = new Boleta_Externa(this.usuario, this.puesto, txtCurp.Text, comboBox2.Text);
                                                        exte.Show();
                                                    }
                                                }
                                                else
                                                    MessageBox.Show("Asistencias y Inasistencias no coinciden");
                                            }
                                            else
                                                MessageBox.Show("Asistencias y Inasistencias no coinciden");
                                        }
                                        else
                                            MessageBox.Show("VALORES INCORRECTOS, SOLO SE PERMITE: E, MB, B, R, NA");
                                    }
                                    else
                                        MessageBox.Show("VALORES INCORRECTOS, SOLO SE PERMITE: E, MB, B, R, NA");
                                }
                                else
                                    MessageBox.Show("VALORES INCORRECTOS, SOLO SE PERMITE: E, MB, B, R, NA");
                            }
                            else
                                MessageBox.Show("VALORES INCORRECTOS, SOLO SE PERMITE: E, MB, B, R, NA");
                        }
                        else
                            MessageBox.Show("VALORES INCORRECTOS, SOLO SE PERMITE: E, MB, B, R, NA");
                    }
                    else
                        MessageBox.Show("VALORES INCORRECTOS, SOLO SE PERMITE: E, MB, B, R, NA");
                }
                else
                    MessageBox.Show("VALORES INCORRECTOS, SOLO SE PERMITE: E, MB, B, R, NA");
            }
            else
                MessageBox.Show("VALORES INCORRECTOS, SOLO SE PERMITE: E, MB, B, R, NA");
        }

        private void TextoCalificacion(object sender, EventArgs e)
        {
            TextBox texto = (TextBox)sender;
            switch(texto.Text)
            {
                case "E":
                    if (texto.Name == "txt1")
                        label18.Text = "10";
                    if (texto.Name == "txt2")
                        label20.Text = "10";
                    if (texto.Name == "txt3")
                        label22.Text = "10";
                    if (texto.Name == "txt4")
                        label24.Text = "10";
                    if (texto.Name == "txt5")
                        label19.Text = "10";
                    if (texto.Name == "txt6")
                        label21.Text = "10";
                    if (texto.Name == "txt7")
                        label23.Text = "10";
                    if (texto.Name == "txt8")
                        label25.Text = "10";
                    break;
                case "MB":
                    if (texto.Name == "txt1")
                        label18.Text = "9";
                    if (texto.Name == "txt2")
                        label20.Text = "9";
                    if (texto.Name == "txt3")
                        label22.Text = "9";
                    if (texto.Name == "txt4")
                        label24.Text = "9";
                    if (texto.Name == "txt5")
                        label19.Text = "9";
                    if (texto.Name == "txt6")
                        label21.Text = "9";
                    if (texto.Name == "txt7")
                        label23.Text = "9";
                    if (texto.Name == "txt8")
                        label25.Text = "9";
                    break;
                case "B":
                    if (texto.Name == "txt1")
                        label18.Text = "8";
                    if (texto.Name == "txt2")
                        label20.Text = "8";
                    if (texto.Name == "txt3")
                        label22.Text = "8";
                    if (texto.Name == "txt4")
                        label24.Text = "8";
                    if (texto.Name == "txt5")
                        label19.Text = "8";
                    if (texto.Name == "txt6")
                        label21.Text = "8";
                    if (texto.Name == "txt7")
                        label23.Text = "8";
                    if (texto.Name == "txt8")
                        label25.Text = "8";
                    break;
                case "R":
                    if (texto.Name == "txt1")
                        label18.Text = "7";
                    if (texto.Name == "txt2")
                        label20.Text = "7";
                    if (texto.Name == "txt3")
                        label22.Text = "7";
                    if (texto.Name == "txt4")
                        label24.Text = "7";
                    if (texto.Name == "txt5")
                        label19.Text = "7";
                    if (texto.Name == "txt6")
                        label21.Text = "7";
                    if (texto.Name == "txt7")
                        label23.Text = "7";
                    if (texto.Name == "txt8")
                        label25.Text = "7";
                    break;
                case "NA":
                    if (texto.Name == "txt1")
                        label18.Text = "5";
                    if (texto.Name == "txt2")
                        label20.Text = "5";
                    if (texto.Name == "txt3")
                        label22.Text = "5";
                    if (texto.Name == "txt4")
                        label24.Text = "5";
                    if (texto.Name == "txt5")
                        label19.Text = "5";
                    if (texto.Name == "txt6")
                        label21.Text = "5";
                    if (texto.Name == "txt7")
                        label23.Text = "5";
                    if (texto.Name == "txt8")
                        label25.Text = "5";
                    break;
                default:
                    if (texto.Name == "txt1")
                        label18.Text = " ";
                    if (texto.Name == "txt2")
                        label20.Text = " ";
                    if (texto.Name == "txt3")
                        label22.Text = " ";
                    if (texto.Name == "txt4")
                        label24.Text = " ";
                    if (texto.Name == "txt5")
                        label19.Text = " ";
                    if (texto.Name == "txt6")
                        label21.Text = " ";
                    if (texto.Name == "txt7")
                        label23.Text = " ";
                    if (texto.Name == "txt8")
                        label25.Text = " ";
                    break;
            }
        }

        private void modi_Click(object sender, EventArgs e)
        {
            if (label18.Text != " ")
            {
                if (label19.Text != " ")
                {
                    if (label20.Text != " ")
                    {
                        if (label21.Text != " ")
                        {
                            if (label22.Text != " ")
                            {
                                if (label23.Text != " ")
                                {
                                    if (label24.Text != " ")
                                    {
                                        if (label25.Text != " ")
                                        {
                                            MySqlConnection conexionBD = Conector.Conexiones();
                                            conexionBD.Open();
                                            string sql = "UPDATE calificacionmensual SET Calificacion ='" + txt1.Text+"', CalificacionDigito = '"+label18.Text+"' WHERE Curp ='" + txtCurp.Text + "' and Grado = '"+textBox1.Text+"' and Grupo = '"+textBox2.Text+"' and Mes = '"+comboBox2.Text+"' and Trimestre = '"+this.trimestres+"' and Materia ='"+len.Text+"' ";
                                            try
                                            {
                                                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                                                comando.ExecuteNonQuery();
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show("No se pudo actualizar registro  " + ex);
                                            }
                                            conexionBD.Close();
                                            conexionBD.Open();
                                            string a = "UPDATE calificacionmensual SET Calificacion ='" + txt5.Text + "', CalificacionDigito = '" + label19.Text + "' WHERE Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + comboBox2.Text + "' and Trimestre = '" + this.trimestres + "' and Materia ='" + PEN.Text + "' ";
                                            try
                                            {
                                                MySqlCommand comando = new MySqlCommand(a, conexionBD);
                                                comando.ExecuteNonQuery();
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show("No se pudo actualizar registro  " + ex);
                                            }
                                            conexionBD.Close();
                                            conexionBD.Open();
                                            string b = "UPDATE calificacionmensual SET Calificacion ='" + txt2.Text + "', CalificacionDigito = '" + label20.Text + "' WHERE Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + comboBox2.Text + "' and Trimestre = '" + this.trimestres + "' and Materia ='" + EXP.Text + "' ";
                                            try
                                            {
                                                MySqlCommand comando = new MySqlCommand(b, conexionBD);
                                                comando.ExecuteNonQuery();
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show("No se pudo actualizar registro  " + ex);
                                            }
                                            conexionBD.Close();
                                            conexionBD.Open();
                                            string c = "UPDATE calificacionmensual SET Calificacion ='" + txt6.Text + "', CalificacionDigito = '" + label21.Text + "' WHERE Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + comboBox2.Text + "' and Trimestre = '" + this.trimestres + "' and Materia ='" + label12.Text + "' ";
                                            try
                                            {
                                                MySqlCommand comando = new MySqlCommand(c, conexionBD);
                                                comando.ExecuteNonQuery();
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show("No se pudo actualizar registro  " + ex);
                                            }
                                            conexionBD.Close();
                                            conexionBD.Open();
                                            string d = "UPDATE calificacionmensual SET Calificacion ='" + txt3.Text + "', CalificacionDigito = '" + label22.Text + "' WHERE Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + comboBox2.Text + "' and Trimestre = '" + this.trimestres + "' and Materia ='" + label13.Text + "' ";
                                            try
                                            {
                                                MySqlCommand comando = new MySqlCommand(d, conexionBD);
                                                comando.ExecuteNonQuery();
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show("No se pudo actualizar registro  " + ex);
                                            }
                                            conexionBD.Close();
                                            conexionBD.Open();
                                            string f = "UPDATE calificacionmensual SET Calificacion ='" + txt7.Text + "', CalificacionDigito = '" + label23.Text + "' WHERE Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + comboBox2.Text + "' and Trimestre = '" + this.trimestres + "' and Materia ='" + label14.Text + "' ";
                                            try
                                            {
                                                MySqlCommand comando = new MySqlCommand(f, conexionBD);
                                                comando.ExecuteNonQuery();
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show("No se pudo actualizar registro  " + ex);
                                            }
                                            conexionBD.Close();
                                            conexionBD.Open();
                                            string g = "UPDATE calificacionmensual SET Calificacion ='" + txt4.Text + "', CalificacionDigito = '" + label24.Text + "' WHERE Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + comboBox2.Text + "' and Trimestre = '" + this.trimestres + "' and Materia ='" + label15.Text + "' ";
                                            try
                                            {
                                                MySqlCommand comando = new MySqlCommand(g, conexionBD);
                                                comando.ExecuteNonQuery();
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show("No se pudo actualizar registro  " + ex);
                                            }
                                            conexionBD.Close();
                                            conexionBD.Open();
                                            string h = "UPDATE calificacionmensual SET Calificacion ='" + txt8.Text + "', CalificacionDigito = '" + label25.Text + "' WHERE Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Mes = '" + comboBox2.Text + "' and Trimestre = '" + this.trimestres + "' and Materia ='" + label16.Text + "' ";
                                            try
                                            {
                                                MySqlCommand comando = new MySqlCommand(h, conexionBD);
                                                comando.ExecuteNonQuery();
                                                MessageBox.Show("Calificaciones actualizadas con exito");
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show("No se pudo actualizar registro  " + ex);
                                            }
                                            conexionBD.Close();
                                        }
                                        else
                                            MessageBox.Show("VALORES INCORRECTOS, SOLO SE PERMITE: E, MB, B, R, NA");
                                    }
                                    else
                                        MessageBox.Show("VALORES INCORRECTOS, SOLO SE PERMITE: E, MB, B, R, NA");
                                }
                                else
                                    MessageBox.Show("VALORES INCORRECTOS, SOLO SE PERMITE: E, MB, B, R, NA");
                            }
                            else
                                MessageBox.Show("VALORES INCORRECTOS, SOLO SE PERMITE: E, MB, B, R, NA");
                        }
                        else
                            MessageBox.Show("VALORES INCORRECTOS, SOLO SE PERMITE: E, MB, B, R, NA");
                    }
                    else
                        MessageBox.Show("VALORES INCORRECTOS, SOLO SE PERMITE: E, MB, B, R, NA");
                }
                else
                    MessageBox.Show("VALORES INCORRECTOS, SOLO SE PERMITE: E, MB, B, R, NA");
            }
            else
                MessageBox.Show("VALORES INCORRECTOS, SOLO SE PERMITE: E, MB, B, R, NA");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Boleta_Externa exte = new Boleta_Externa(this.usuario, this.puesto, txtCurp.Text, comboBox2.Text);
            exte.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Promedios pro = new Promedios(this.usuario, this.puesto, txtCurp.Text);
            pro.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string correoelec = textCorreo.Text;
            string espacio = " ";
            MailMessage correo = new MailMessage();
            correo.From = new MailAddress("tobilegendario@gmail.com", "Boleta", System.Text.Encoding.UTF8);//Correo de salida
            if (textBox1.Text == "1")
            {
                correo.To.Add(correoelec); //Correo destino?
                correo.Subject = "Boleta de calificaciones"; //Asunto
                correo.Body = "Boleta de calificaciones del alumno: " + textAPA.Text + espacio + textAma.Text + espacio + textNom.Text + " "; //Mensaje del correo
                string ruta = @"" + txtRuta.Text + "Calificacion del alumno " + txtCurp.Text + " del grado " + textBox1.Text + ".pdf"; //aqui va la ruta del pdf con @""
                System.Net.Mail.Attachment Archivo = new System.Net.Mail.Attachment(ruta);
                correo.Attachments.Add(Archivo); //Archivo
            }
            if (textBox1.Text == "2")
            {
                correo.To.Add(correoelec); //Correo destino?
                correo.Subject = "Boleta de calificaciones"; //Asunto
                correo.Body = "Boleta de calificaciones del alumno: " + textAPA.Text + espacio + textAma.Text + espacio + textNom.Text + " "; //Mensaje del correo
                string ruta = @"" + txtRuta.Text + "Calificacion del alumno " + txtCurp.Text + " del grado " + textBox1.Text + ".pdf"; //aqui va la ruta del pdf con @""
                System.Net.Mail.Attachment Archivo = new System.Net.Mail.Attachment(ruta);
                correo.Attachments.Add(Archivo); //Archivo
            }
            if (textBox1.Text == "3")
            {
                correo.To.Add(correoelec); //Correo destino?
                correo.Subject = "Boleta de calificaciones"; //Asunto
                correo.Body = "Boleta de calificaciones del alumno: " + textAPA.Text + espacio + textAma.Text + espacio + textNom.Text + " "; //Mensaje del correo
                string ruta = @"" + txtRuta.Text + "Calificacion del alumno " + txtCurp.Text + " del grado " + textBox1.Text + ".pdf"; //aqui va la ruta del pdf con @""
                System.Net.Mail.Attachment Archivo = new System.Net.Mail.Attachment(ruta);
                correo.Attachments.Add(Archivo); //Archivo
            }
            correo.IsBodyHtml = true;
            correo.Priority = MailPriority.Normal;
            SmtpClient smtp = new SmtpClient();
            smtp.UseDefaultCredentials = false;
            smtp.Host = "smtp.gmail.com"; //Host del servidor de correo
            smtp.Port = 25; //Puerto de salida
            smtp.Credentials = new System.Net.NetworkCredential("tobilegendario@gmail.com", "putatumadre12");//Cuenta de correo
            ServicePointManager.ServerCertificateValidationCallback = delegate (object sz, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            smtp.EnableSsl = true;//True si el servidor de correo permite ssl
            smtp.Send(correo);
            smtp.Dispose();
            correo.Dispose();
            MessageBox.Show("Correo Enviado con Exito");
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

