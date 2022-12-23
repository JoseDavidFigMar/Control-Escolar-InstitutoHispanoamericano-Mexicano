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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Control_Escolar_InstitutoHispanoamericanoMexicano
{
    public partial class Estadistica : Form
    {
        public Estadistica()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Text = DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToString("HH:mm:ss");
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
            MySqlConnection conexionBD = Conector.Conexiones();
            MySqlDataReader reader = null;
            DateTime Today = DateTime.Now;
            string año = DateTime.Now.ToString("yyyy");
            string mes = DateTime.Now.ToString("MM");
            string dia = DateTime.Now.ToString("dd");
            string meses = DateTime.Now.ToString("MMMM");
            string A = "K";
            string B = "P";
            string M = "F";
            string H = "M";
            string pathPDF3;
            string pathPDF = @"D:\Pdfs mias\Estadisticas\Estadistica.pdf";
            string pathPDF2 = @"D:\Pdfs mias\Estadisticas\EstadisticaA.pdf";
            PdfReader oReader = new PdfReader(pathPDF);
            Rectangle oSize = oReader.GetPageSizeWithRotation(1);
            Document oDocument = new Document(oSize);
            FileStream oFS = new FileStream(pathPDF2, FileMode.Create);
            PdfWriter oWriter = PdfWriter.GetInstance(oDocument, oFS);
            oDocument.Open();
            PdfContentByte oPDF = oWriter.DirectContent;
            BaseFont zz = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            oPDF.SetColorFill(BaseColor.BLACK);
            oPDF.SetFontAndSize(zz, 7);
            oPDF.BeginText();
            oPDF.ShowTextAligned(0, mes + "/" + dia + "/" + año, 512, oSize.Height - 94, 0);
            oPDF.EndText();
            oPDF.SetFontAndSize(zz, 9);
            oPDF.BeginText();
            oPDF.ShowTextAligned(0, A, 235, oSize.Height - 212, 0);
            oPDF.ShowTextAligned(0, A, 235, oSize.Height - 246, 0);
            oPDF.ShowTextAligned(0, A, 235, oSize.Height - 280, 0);
            oPDF.ShowTextAligned(0, A, 235, oSize.Height - 314, 0);

            string a = "select count(Curp) from alumno where Grado = 1 and Grupo = '" + A + "' and Sexo = '" + H + "';";
            string Contadores;            
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(a, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF.ShowTextAligned(0, Contadores, 360, oSize.Height - 212, 0);
                        oPDF.ShowTextAligned(0, Contadores, 360, oSize.Height - 226, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF.ShowTextAligned(0, Contadores, 360, oSize.Height - 212, 0);
                    oPDF.ShowTextAligned(0, Contadores, 360, oSize.Height - 226, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);
            }
            conexionBD.Close();

            string b = "select count(Curp) from alumno where Grado = 1 and Grupo = '" + A + "' and Sexo = '" + M + "';";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(b, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF.ShowTextAligned(0, Contadores, 460, oSize.Height - 212, 0);
                        oPDF.ShowTextAligned(0, Contadores, 460, oSize.Height - 226, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF.ShowTextAligned(0, Contadores, 460, oSize.Height - 212, 0);
                    oPDF.ShowTextAligned(0, Contadores, 460, oSize.Height - 226, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionBD.Close();

            string c = "select count(Curp) from alumno where Grado = 1 and Grupo = '" + A + "';";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(c, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF.ShowTextAligned(0, Contadores, 550, oSize.Height - 212, 0);
                        oPDF.ShowTextAligned(0, Contadores, 550, oSize.Height - 226, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF.ShowTextAligned(0, Contadores, 550, oSize.Height - 212, 0);
                    oPDF.ShowTextAligned(0, Contadores, 550, oSize.Height - 226, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionBD.Close();

            string d = "select count(Curp) from alumno where Grado = 2 and Grupo = '" + A + "' and Sexo = '" + H + "';";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(d, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF.ShowTextAligned(0, Contadores, 360, oSize.Height - 246, 0);
                        oPDF.ShowTextAligned(0, Contadores, 360, oSize.Height - 260, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF.ShowTextAligned(0, Contadores, 360, oSize.Height - 246, 0);
                    oPDF.ShowTextAligned(0, Contadores, 360, oSize.Height - 260, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);
            }
            conexionBD.Close();

            string ee = "select count(Curp) from alumno where Grado = 2 and Grupo = '" + A + "' and Sexo = '" + M + "';";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(ee, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF.ShowTextAligned(0, Contadores, 460, oSize.Height - 246, 0);
                        oPDF.ShowTextAligned(0, Contadores, 460, oSize.Height - 260, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF.ShowTextAligned(0, Contadores, 460, oSize.Height - 246, 0);
                    oPDF.ShowTextAligned(0, Contadores, 460, oSize.Height - 260, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionBD.Close();

            string f = "select count(Curp) from alumno where Grado = 2 and Grupo = '" + A + "';";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(f, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF.ShowTextAligned(0, Contadores, 550, oSize.Height - 246, 0);
                        oPDF.ShowTextAligned(0, Contadores, 550, oSize.Height - 260, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF.ShowTextAligned(0, Contadores, 550, oSize.Height - 246, 0);
                    oPDF.ShowTextAligned(0, Contadores, 550, oSize.Height - 260, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionBD.Close();

            string g = "select count(Curp) from alumno where Grado = 3 and Grupo = '" + A + "' and Sexo = '" + H + "';";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(g, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF.ShowTextAligned(0, Contadores, 360, oSize.Height - 280, 0);
                        oPDF.ShowTextAligned(0, Contadores, 360, oSize.Height - 294, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF.ShowTextAligned(0, Contadores, 360, oSize.Height - 280, 0);
                    oPDF.ShowTextAligned(0, Contadores, 360, oSize.Height - 294, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);
            }
            conexionBD.Close();

            string h = "select count(Curp) from alumno where Grado = 3 and Grupo = '" + A + "' and Sexo = '" + M + "';";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(h, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF.ShowTextAligned(0, Contadores, 460, oSize.Height - 280, 0);
                        oPDF.ShowTextAligned(0, Contadores, 460, oSize.Height - 294, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF.ShowTextAligned(0, Contadores, 460, oSize.Height - 280, 0);
                    oPDF.ShowTextAligned(0, Contadores, 460, oSize.Height - 294, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionBD.Close();

            string i = "select count(Curp) from alumno where Grado = 3 and Grupo = '" + A + "';";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(i, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF.ShowTextAligned(0, Contadores, 550, oSize.Height - 280, 0);
                        oPDF.ShowTextAligned(0, Contadores, 550, oSize.Height - 294, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF.ShowTextAligned(0, Contadores, 550, oSize.Height - 280, 0);
                    oPDF.ShowTextAligned(0, Contadores, 550, oSize.Height - 294, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionBD.Close();

            string t = "select count(Curp) from alumno where Grupo = '" + A + "' and Sexo = '" + H + "';";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(t, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF.ShowTextAligned(0, Contadores, 360, oSize.Height - 314, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF.ShowTextAligned(0, Contadores, 360, oSize.Height - 314, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionBD.Close();

            string u = "select count(Curp) from alumno where Grupo = '" + A + "' and Sexo = '" + M + "';";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(u, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF.ShowTextAligned(0, Contadores, 460, oSize.Height - 314, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF.ShowTextAligned(0, Contadores, 460, oSize.Height - 314, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionBD.Close();

            string v = "select count(Curp) from alumno where Grupo = '" + A + "';";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(v, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF.ShowTextAligned(0, Contadores, 550, oSize.Height - 314, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF.ShowTextAligned(0, Contadores, 550, oSize.Height - 314, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionBD.Close();

            oPDF.EndText();
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
            pathPDF = @"D:\Pdfs mias\Estadisticas\Estadistica.pdf";
            pathPDF3 = @"D:\Pdfs mias\Estadisticas\EstadisticaB.pdf";
            PdfReader oReader1 = new PdfReader(pathPDF);
            Rectangle oSize1 = oReader1.GetPageSizeWithRotation(1);
            Document oDocument1 = new Document(oSize1);
            FileStream oFS1 = new FileStream(pathPDF3, FileMode.Create);
            PdfWriter oWriter1 = PdfWriter.GetInstance(oDocument1, oFS1);
            oDocument1.Open();
            PdfContentByte oPDF1 = oWriter1.DirectContent;
            BaseFont aa = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            oPDF1.SetColorFill(BaseColor.BLACK);
            oPDF1.SetFontAndSize(aa, 7);
            oPDF1.BeginText();
            oPDF1.ShowTextAligned(0, mes + "/" + dia + "/" + año, 512, oSize1.Height - 94, 0);
            oPDF1.EndText();
            oPDF1.SetFontAndSize(aa, 9);
            oPDF1.BeginText();
            oPDF1.ShowTextAligned(0, B, 235, oSize1.Height - 212, 0);
            oPDF1.ShowTextAligned(0, B, 235, oSize1.Height - 246, 0);
            oPDF1.ShowTextAligned(0, B, 235, oSize1.Height - 280, 0);
            oPDF1.ShowTextAligned(0, B, 235, oSize1.Height - 314, 0);
            string ab = "select count(Curp) from alumno where Grado = 1 and Grupo = '" + B + "' and Sexo = '" + H + "';";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(ab, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF1.ShowTextAligned(0, Contadores, 360, oSize1.Height - 212, 0);
                        oPDF1.ShowTextAligned(0, Contadores, 360, oSize.Height - 226, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF1.ShowTextAligned(0, Contadores, 360, oSize1.Height - 212, 0);
                    oPDF1.ShowTextAligned(0, Contadores, 360, oSize1.Height - 226, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);
            }
            conexionBD.Close();

            string ba = "select count(Curp) from alumno where Grado = 1 and Grupo = '" + B + "' and Sexo = '" + M + "';";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(ba, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF1.ShowTextAligned(0, Contadores, 460, oSize1.Height - 212, 0);
                        oPDF1.ShowTextAligned(0, Contadores, 460, oSize1.Height - 226, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF1.ShowTextAligned(0, Contadores, 460, oSize1.Height - 212, 0);
                    oPDF1.ShowTextAligned(0, Contadores, 460, oSize1.Height - 226, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionBD.Close();

            string ca = "select count(Curp) from alumno where Grado = 1 and Grupo = '" + B + "';";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(ca, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF1.ShowTextAligned(0, Contadores, 550, oSize1.Height - 212, 0);
                        oPDF1.ShowTextAligned(0, Contadores, 550, oSize1.Height - 226, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF1.ShowTextAligned(0, Contadores, 550, oSize1.Height - 212, 0);
                    oPDF1.ShowTextAligned(0, Contadores, 550, oSize1.Height - 226, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionBD.Close();

            string da = "select count(Curp) from alumno where Grado = 2 and Grupo = '" + B + "' and Sexo = '" + H + "';";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(da, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF1.ShowTextAligned(0, Contadores, 360, oSize1.Height - 246, 0);
                        oPDF1.ShowTextAligned(0, Contadores, 360, oSize1.Height - 260, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF1.ShowTextAligned(0, Contadores, 360, oSize1.Height - 246, 0);
                    oPDF1.ShowTextAligned(0, Contadores, 360, oSize1.Height - 260, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);
            }
            conexionBD.Close();

            string eea = "select count(Curp) from alumno where Grado = 2 and Grupo = '" + B + "' and Sexo = '" + M + "';";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(eea, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF1.ShowTextAligned(0, Contadores, 460, oSize1.Height - 246, 0);
                        oPDF1.ShowTextAligned(0, Contadores, 460, oSize1.Height - 260, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF1.ShowTextAligned(0, Contadores, 460, oSize1.Height - 246, 0);
                    oPDF1.ShowTextAligned(0, Contadores, 460, oSize1.Height - 260, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionBD.Close();

            string fa = "select count(Curp) from alumno where Grado = 2 and Grupo = '" + B + "';";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(fa, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF1.ShowTextAligned(0, Contadores, 550, oSize1.Height - 246, 0);
                        oPDF1.ShowTextAligned(0, Contadores, 550, oSize1.Height - 260, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF1.ShowTextAligned(0, Contadores, 550, oSize1.Height - 246, 0);
                    oPDF1.ShowTextAligned(0, Contadores, 550, oSize1.Height - 260, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionBD.Close();

            string ga = "select count(Curp) from alumno where Grado = 3 and Grupo = '" + B + "' and Sexo = '" + H + "';";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(ga, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF1.ShowTextAligned(0, Contadores, 360, oSize1.Height - 280, 0);
                        oPDF1.ShowTextAligned(0, Contadores, 360, oSize1.Height - 294, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF1.ShowTextAligned(0, Contadores, 360, oSize1.Height - 280, 0);
                    oPDF1.ShowTextAligned(0, Contadores, 360, oSize1.Height - 294, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);
            }
            conexionBD.Close();

            string ha = "select count(Curp) from alumno where Grado = 3 and Grupo = '" + B + "' and Sexo = '" + M + "';";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(ha, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF1.ShowTextAligned(0, Contadores, 460, oSize1.Height - 280, 0);
                        oPDF1.ShowTextAligned(0, Contadores, 460, oSize1.Height - 294, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF1.ShowTextAligned(0, Contadores, 460, oSize1.Height - 280, 0);
                    oPDF1.ShowTextAligned(0, Contadores, 460, oSize1.Height - 294, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionBD.Close();

            string ia = "select count(Curp) from alumno where Grado = 3 and Grupo = '" + B + "';";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(ia, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF1.ShowTextAligned(0, Contadores, 550, oSize1.Height - 280, 0);
                        oPDF1.ShowTextAligned(0, Contadores, 550, oSize1.Height - 294, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF1.ShowTextAligned(0, Contadores, 550, oSize1.Height - 280, 0);
                    oPDF1.ShowTextAligned(0, Contadores, 550, oSize1.Height - 294, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionBD.Close();

            string ta = "select count(Curp) from alumno where Grupo = '" + B + "' and Sexo = '" + H + "';";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(ta, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF1.ShowTextAligned(0, Contadores, 360, oSize1.Height - 314, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF1.ShowTextAligned(0, Contadores, 360, oSize1.Height - 314, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionBD.Close();

            string au = "select count(Curp) from alumno where Grupo = '" + B + "' and Sexo = '" + M + "';";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(au, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF1.ShowTextAligned(0, Contadores, 460, oSize1.Height - 314, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF1.ShowTextAligned(0, Contadores, 460, oSize1.Height - 314, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionBD.Close();

            string va = "select count(Curp) from alumno where Grupo = '" + B + "';";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(va, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contadores = reader.GetString(0);
                        oPDF1.ShowTextAligned(0, Contadores, 550, oSize1.Height - 314, 0);
                    }
                }
                else
                {
                    Contadores = "0";
                    oPDF1.ShowTextAligned(0, Contadores, 550, oSize1.Height - 314, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionBD.Close();

            oPDF1.EndText();
            PdfImportedPage page2 = oWriter1.GetImportedPage(oReader1, 1);
            oPDF1.AddTemplate(page2, 0, 0);
            oDocument1.Close();
            oFS1.Close();
            oWriter1.Close();
            oReader1.Close();
            oDocument1.Dispose();
            oFS1.Dispose();
            oWriter1.Dispose();
            oReader1.Dispose();
            string[] lstFiles = new string[3];
            lstFiles[0] = @"D:\Pdfs mias\Estadisticas\EstadisticaA.pdf";
            lstFiles[1] = @"D:\Pdfs mias\Estadisticas\EstadisticaB.pdf";
            PdfReader reader2 = null;
            Document sourceDocument = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage;
            string outputPdfPath = @"" + txtRuta.Text + "Estadistica general del " + dia + " de " + meses + " del " + año + ".pdf";
            sourceDocument = new Document();
            pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));
            //Open the output file
            sourceDocument.Open();
            try
            {
                //Loop through the files list
                for (int sd = 0; sd < lstFiles.Length - 1; sd++)
                {
                    int pages = get_pageCcount(lstFiles[sd]);

                    reader2 = new PdfReader(lstFiles[sd]);
                    //Add pages of current file
                    for (int ii = 1; ii <= pages; ii++)
                    {
                        importedPage = pdfCopyProvider.GetImportedPage(reader2, ii);
                        pdfCopyProvider.AddPage(importedPage);
                    }

                    reader2.Close();
                }
                //At the end save the output file
                sourceDocument.Close();
                System.IO.File.Delete(@"D:\Pdfs mias\Boleta Externa\1° pruebaA.pdf");
                System.IO.File.Delete(@"D:\Pdfs mias\Boleta Externa\1° pruebaB.pdf");
                Process.Start(@"" + txtRuta.Text + "Estadistica general del " + dia + " de " + meses + " del " + año + ".pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static int get_pageCcount(string file)
        {
            using (StreamReader sr = new StreamReader(File.OpenRead(file)))
            {
                Regex regex = new Regex(@"/Type\s*/Page[^s]");
                MatchCollection matches = regex.Matches(sr.ReadToEnd());

                return matches.Count;
            }

        }

        private void Salida_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

