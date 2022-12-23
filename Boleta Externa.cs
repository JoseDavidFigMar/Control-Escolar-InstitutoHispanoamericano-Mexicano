using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class Boleta_Externa : Form
    {
        public string user;
        public string puesto;
        public string curp;
        public string trimestres;
        public string mes;
        public string foler;
        ConexionInsertar cnx = new ConexionInsertar();

        public Boleta_Externa()
        {
            InitializeComponent();
        }
        public Boleta_Externa(string usuario, string puesto, string Curp, string mes)
        {
            this.user = usuario;
            this.puesto = puesto;
            this.curp = Curp;
            this.mes = mes;
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Text = DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToString("HH:mm:ss");
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtCurp_TextChanged(object sender, EventArgs e)
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
                    }
                }
                else
                {
                    MessageBox.Show("Alumno no registrado, registrelo y intente nuevamente");
                    this.Close();
                }
            }
            catch (MySqlException ex)
            {
                comboBox2.Enabled = false;
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionBD.Close();
        }

        private void Salida_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt1_TextChanged(object sender, EventArgs e)
        {
            txt1.MaxLength = 120;
            label4.Text = Convert.ToString(txt1.Text.Length);
            if (label4.Text == "120")
            {
                MessageBox.Show("Maximo de caracteres alcanzados");
            }
        }

        private void txt5_TextChanged(object sender, EventArgs e)
        {
            txt5.MaxLength = 120;
            label7.Text = Convert.ToString(txt5.Text.Length);
            if (label7.Text == "120")
            {
                MessageBox.Show("Maximo de caracteres alcanzados");
            }
        }

        private void txt2_TextChanged(object sender, EventArgs e)
        {
            txt2.MaxLength = 120;
            label10.Text = Convert.ToString(txt2.Text.Length);
            if (label10.Text == "120")
            {
                MessageBox.Show("Maximo de caracteres alcanzados");
            }
        }

        private void txt6_TextChanged(object sender, EventArgs e)
        {
            txt6.MaxLength = 120;
            label19.Text = Convert.ToString(txt6.Text.Length);
            if (label19.Text == "120")
            {
                MessageBox.Show("Maximo de caracteres alcanzados");
            }
        }

        private void txt3_TextChanged(object sender, EventArgs e)
        {
            txt3.MaxLength = 120;
            label21.Text = Convert.ToString(txt3.Text.Length);
            if (label21.Text == "120")
            {
                MessageBox.Show("Maximo de caracteres alcanzados");
            }
        }

        private void txt7_TextChanged(object sender, EventArgs e)
        {
            txt7.MaxLength = 120;
            label23.Text = Convert.ToString(txt7.Text.Length);
            if (label23.Text == "120")
            {
                MessageBox.Show("Maximo de caracteres alcanzados");
            }
        }
           
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string lec = "LENGUAJE Y COMUNICACION";
            string pm = "PENSAMIENTO MATEMATICO";
            string exp = "EXPLORACION Y COMPRENSION";
            string ar = "ARTES";
            string ef = "EDUCACION FISICA";
            string ing = "INGLES";
            MySqlDataReader reader = null;
            if (comboBox2.Text == "1")
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
                            this.trimestres = "1";
                            guar.Visible = true;
                            label8.Visible = true;
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
                        MessageBox.Show("Mes no corresponde falta valores");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            if (comboBox2.Text == "2")
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
                            this.trimestres = "2";
                            guar.Visible = true;
                            label8.Visible = true;
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
                        MessageBox.Show("Mes no corresponde falta valores");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
                conexionBD.Close();
            }
            if (comboBox2.Text == "3")
            {
                string curp = txtCurp.Text;
                string grado = textBox1.Text;
                string grupo = textBox2.Text;
                string mes = "Junio";
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
                            this.trimestres = "3";
                            guar.Visible = true;
                            label8.Visible = true;
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
            MySqlConnection conexionB = Conector.Conexiones();
            string s = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = '" + comboBox2.Text + "' and Materia = '" + lec + "'";
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
            string a = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = '" + comboBox2.Text + "' and Materia = '" + pm + "'";
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
            string b = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = '" + comboBox2.Text + "' and Materia = '" + exp + "'";
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
            string c = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = '" + comboBox2.Text + "' and Materia = '" + ar + "'";
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
            string d = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = '" + comboBox2.Text + "' and Materia = '" + ef + "'";
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
            string f = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = '" + comboBox2.Text + "' and Materia = '" + ing + "'";
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
                        guar.Visible = false;
                        label8.Visible = false;
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
            if (txt1.Enabled != false)
            {
                if (txt2.Enabled != false)
                {
                    if (txt3.Enabled != false)
                    {
                        if (txt5.Enabled != false)
                        {
                            if (txt6.Enabled != false)
                            {
                                if (txt7.Enabled != false)
                                {

                                    cnx.RegistrarBoletaExterna(txtCurp.Text, textBox1.Text, textBox2.Text, comboBox2.Text, len.Text, txt1.Text);
                                    cnx.RegistrarBoletaExterna(txtCurp.Text, textBox1.Text, textBox2.Text, comboBox2.Text, PEN.Text, txt5.Text);
                                    cnx.RegistrarBoletaExterna(txtCurp.Text, textBox1.Text, textBox2.Text, comboBox2.Text, EXP.Text, txt2.Text);
                                    cnx.RegistrarBoletaExterna(txtCurp.Text, textBox1.Text, textBox2.Text, comboBox2.Text, label12.Text, txt6.Text);
                                    cnx.RegistrarBoletaExterna(txtCurp.Text, textBox1.Text, textBox2.Text, comboBox2.Text, label13.Text, txt3.Text);
                                    cnx.RegistrarBoletaExterna(txtCurp.Text, textBox1.Text, textBox2.Text, comboBox2.Text, label14.Text, txt7.Text); ;
                                    MessageBox.Show("Se guardaron las observaciones correctamente");
                                }
                                else
                                    MessageBox.Show("No se han introducido observaciones");
                            }
                            else
                                MessageBox.Show("No se han introducido observaciones");
                        }
                        else
                            MessageBox.Show("No se han introducido observaciones");
                    }
                    else
                        MessageBox.Show("No se han introducido observaciones");
                }
                else
                    MessageBox.Show("No se han introducido observaciones");
            }
            else
                MessageBox.Show("No se han introducido observaciones");
        }

        private void Boleta_Externa_Load(object sender, EventArgs e)
        {
            txtCurp.Text = this.curp;
            MessageBox.Show("Ingrese Observaciones");
            if (this.puesto == "Secretaria")
            {
                modi.Visible = false;
                label17.Visible = false;
            }
            if(this.mes == "Nov/Dic")
            {
                comboBox2.Text = "1";
            }
            if (this.mes == "Marzo")
            {
                comboBox2.Text = "2";
            }
            if (this.mes == "Junio")
            {
                comboBox2.Text = "3";
                Random r = new Random();
                this.foler = Convert.ToString(r.Next(1,1000));

            }
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

        private void modi_Click(object sender, EventArgs e)
        {
            if (txt1.Enabled != false)
            {
                if (txt2.Enabled != false)
                {
                    if (txt3.Enabled != false)
                    {
                        if (txt5.Enabled != false)
                        {
                            if (txt6.Enabled != false)
                            {
                                if (txt7.Enabled != false)
                                {

                                    MySqlConnection conexionBD = Conector.Conexiones();
                                    conexionBD.Open();
                                    string sql = "UPDATE calificaciontrimestre SET Observacion ='" + txt1.Text + "' WHERE Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = '" + comboBox2.Text + "'  and Materia ='" + len.Text + "' ";
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
                                    string a = "UPDATE calificaciontrimestre SET Observacion ='" + txt5.Text + "' WHERE Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = '" + comboBox2.Text + "' and Materia ='" + PEN.Text + "' ";
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
                                    string b = "UPDATE calificaciontrimestre SET Observacion ='" + txt2.Text + "' WHERE Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = '" + comboBox2.Text + "' and Materia ='" + EXP.Text + "' ";
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
                                    string c = "UPDATE calificaciontrimestre SET Observacion ='" + txt6.Text + "' WHERE Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = '" + comboBox2.Text + "' and Materia ='" + label12.Text + "' ";
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
                                    string d = "UPDATE calificaciontrimestre SET Observacion ='" + txt3.Text + "' WHERE Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = '" + comboBox2.Text + "' and Materia ='" + label13.Text + "' ";
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
                                    string f = "UPDATE calificaciontrimestre SET Observacion ='" + txt7.Text + "' WHERE Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = '" + comboBox2.Text + "' and Materia ='" + label14.Text + "' ";
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
                                }
                                else
                                    MessageBox.Show("No se han introducido observaciones");
                            }
                            else
                                MessageBox.Show("No se han introducido observaciones");
                        }
                        else
                            MessageBox.Show("No se han introducido observaciones");
                    }
                    else
                        MessageBox.Show("No se han introducido observaciones");
                }
                else
                    MessageBox.Show("No se han introducido observaciones");
            }
            else
                MessageBox.Show("No se han introducido observaciones");        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            this.foler = Convert.ToString(r.Next(1, 1000));

            MySqlConnection conexionB = Conector.Conexiones();
            MySqlDataReader reader = null;
            string lec = "LENGUAJE Y COMUNICACION";
            string pm = "PENSAMIENTO MATEMATICO";
            string exp = "EXPLORACION Y COMPRENSION";
            string ar = "ARTES";
            string ef = "EDUCACION FISICA";
            string ing = "INGLES";
            string pathPDF;
            string pathPDF2;
            string pathPDF3;
            string obser;
            pathPDF = @"D:\Pdfs mias\Boleta Externa\" + textBox1.Text + "°.pdf";
            pathPDF2 = @"" + txtRuta.Text + " Calificaciones " +textBox1.Text + "° A.pdf";
            string folio = this.foler;
            string folio1 = "Sin validar";
            DateTime Today = DateTime.Now;
            string año = DateTime.Now.ToString("yyyy");
            string mes = DateTime.Now.ToString("MM");
            string dia = DateTime.Now.ToString("dd");
            PdfReader oReader = new PdfReader(pathPDF);
            Rectangle oSize = oReader.GetPageSizeWithRotation(1);
            Document oDocument = new Document(oSize);
            FileStream oFS = new FileStream(pathPDF2, FileMode.Create);
            PdfWriter oWriter = PdfWriter.GetInstance(oDocument, oFS);
            oDocument.Open();
            PdfContentByte oPDF = oWriter.DirectContent;
            BaseFont zz = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            oPDF.SetColorFill(BaseColor.BLACK);
            oPDF.BeginText();
            oPDF.SetFontAndSize(zz, 10);
            oPDF.ShowTextAligned(0, textAPA.Text, 55, oSize.Height - 89, 0);
            oPDF.ShowTextAligned(0, textAma.Text, 195, oSize.Height - 89, 0);
            oPDF.ShowTextAligned(0, textNom.Text, 360, oSize.Height - 89, 0);
            oPDF.ShowTextAligned(0, txtCurp.Text, 484, oSize.Height - 89, 0);
            oPDF.ShowTextAligned(0, textBox1.Text, 345, oSize.Height - 130, 0);
            oPDF.EndText();
            oPDF.SetFontAndSize(zz, 8);
            oPDF.BeginText();
            string s = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = 1 and Materia = '" + lec + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(s, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        obser = reader.GetString(0);
                        oPDF.ShowTextAligned(0, obser, 163, oSize.Height - 250, 0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionB.Close();
            oPDF.EndText();
            oPDF.BeginText();
            string a = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = 1 and Materia = '" + pm + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(a, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        obser = reader.GetString(0);
                        oPDF.ShowTextAligned(0, obser, 163, oSize.Height - 380, 0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            oPDF.EndText();
            conexionB.Close();
            oPDF.BeginText();
            string b = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = 1 and Materia = '" + exp + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(b, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        obser = reader.GetString(0);
                        oPDF.ShowTextAligned(0, obser, 163, oSize.Height - 505, 0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            oPDF.EndText();
            conexionB.Close();
            oPDF.BeginText();
            string f = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = 1 and Materia = '" + ing + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(f, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        obser = reader.GetString(0);
                        oPDF.ShowTextAligned(0, obser, 163, oSize.Height - 630, 0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            oPDF.EndText();
            conexionB.Close();
            oPDF.BeginText();
            string ss = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = 2 and Materia = '" + lec + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(ss, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        obser = reader.GetString(0);
                        oPDF.ShowTextAligned(0, obser, 163, oSize.Height - 290, 0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            oPDF.EndText();
            conexionB.Close();
            oPDF.BeginText();
            string aa = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = 2 and Materia = '" + pm + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(aa, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        obser = reader.GetString(0);
                        oPDF.ShowTextAligned(0, obser, 163, oSize.Height - 420, 0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            oPDF.EndText();
            conexionB.Close();
            oPDF.BeginText();
            string bb = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = 2 and Materia = '" + exp + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(bb, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        obser = reader.GetString(0);
                        oPDF.ShowTextAligned(0, obser, 163, oSize.Height - 545, 0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            oPDF.EndText();
            conexionB.Close();
            oPDF.BeginText();
            string ff = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = 2 and Materia = '" + ing + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(ff, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        obser = reader.GetString(0);
                        oPDF.ShowTextAligned(0, obser, 163, oSize.Height - 675, 0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            oPDF.EndText();
            conexionB.Close();
            oPDF.BeginText();
            string sss = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = 3 and Materia = '" + lec + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(sss, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        obser = reader.GetString(0);
                        oPDF.ShowTextAligned(0, obser, 163, oSize.Height - 335, 0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            oPDF.EndText();
            conexionB.Close();
            oPDF.BeginText();
            string aaa= "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = 3 and Materia = '" + pm + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(aaa, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        obser = reader.GetString(0);
                        oPDF.ShowTextAligned(0, obser, 163, oSize.Height - 460, 0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            oPDF.EndText();
            conexionB.Close();
            oPDF.BeginText();
            string bbb = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = 3 and Materia = '" + exp + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(bbb, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        obser = reader.GetString(0);
                        oPDF.ShowTextAligned(0, obser, 163, oSize.Height - 590, 0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            oPDF.EndText();
            conexionB.Close();
            oPDF.BeginText();
            string fff = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = 3 and Materia = '" + ing + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(fff, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        obser = reader.GetString(0);
                        oPDF.ShowTextAligned(0, obser, 163, oSize.Height - 715, 0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            oPDF.EndText();
            conexionB.Close();
            conexionB.Open();
            oPDF.BeginText();
            string sl = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = 3 and Materia = '"+ing+"'";
            try
            {
                MySqlCommand comando = new MySqlCommand(sl, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    string refe;
                    while (reader.Read())
                    {
                        refe = reader.GetString(0);
                        oPDF.SetFontAndSize(zz, 10);
                        oPDF.ShowTextAligned(0, folio, 490, oSize.Height - 748, 0);
                    }
                }
                else
                {
                    oPDF.SetFontAndSize(zz, 10);
                    oPDF.ShowTextAligned(0, folio1, 490, oSize.Height - 748, 0);
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            oPDF.EndText();
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
            pathPDF = @"D:\Pdfs mias\Boleta Externa\" + textBox1.Text + "°.pdf";
            pathPDF3 = @"" + txtRuta.Text + " Calificaciones " + textBox1.Text + "° B.pdf";
            PdfReader oReader1 = new PdfReader(pathPDF);
            Rectangle oSize1 = oReader1.GetPageSizeWithRotation(1);
            Document oDocument1 = new Document(oSize1);
            FileStream oFS1 = new FileStream(pathPDF3, FileMode.Create);
            PdfWriter oWriter1 = PdfWriter.GetInstance(oDocument1, oFS1);
            oDocument1.Open();
            PdfContentByte oPDF1 = oWriter1.DirectContent;
            BaseFont mn = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            oPDF1.SetColorFill(BaseColor.BLACK);
            oPDF1.SetFontAndSize(mn, 8);
            oPDF1.BeginText();
            string c = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = 1 and Materia = '" + ar + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(c, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        obser = reader.GetString(0);
                        oPDF1.ShowTextAligned(0, obser, 163, oSize.Height - 55, 0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            oPDF1.EndText();
            conexionB.Close();
            oPDF1.BeginText();
            string d = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = 1 and Materia = '" + ef + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(d, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        obser = reader.GetString(0);
                        oPDF1.ShowTextAligned(0, obser, 163, oSize.Height - 230, 0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            oPDF1.EndText();
            oPDF1.BeginText();
            conexionB.Close();
            string cc = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = 2 and Materia = '" + ar + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(cc, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        obser = reader.GetString(0);
                        oPDF1.ShowTextAligned(0, obser, 163, oSize.Height - 115, 0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            oPDF1.EndText();
            conexionB.Close();
            oPDF1.BeginText();
            string dd = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = 2 and Materia = '" + ef + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(dd, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        obser = reader.GetString(0);
                        oPDF1.ShowTextAligned(0, obser, 163, oSize.Height - 290, 0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            oPDF1.EndText();
            conexionB.Close();
            oPDF1.BeginText();
            string ccc = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = 3 and Materia = '" + ar + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(ccc, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        obser = reader.GetString(0);
                        oPDF1.ShowTextAligned(0, obser, 163, oSize.Height - 170, 0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            oPDF1.EndText();
            conexionB.Close();
            oPDF1.BeginText();
            string ddd = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = 3 and Materia = '" + ef + "'";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(ddd, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        obser = reader.GetString(0);
                        oPDF1.ShowTextAligned(0, obser, 163, oSize.Height -340, 0);
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            oPDF1.EndText();
            conexionB.Close();
            conexionB.Open();
            oPDF1.BeginText();
            string sql = "Select Observacion from calificaciontrimestre where Curp ='" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre = 3 and Materia = '"+ef+"'";
            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    string refe;
                    while (reader.Read())
                    {
                        refe = reader.GetString(0);
                        oPDF1.SetFontAndSize(mn, 12);
                        oPDF1.ShowTextAligned(0, folio, 55, oSize1.Height - 705, 0);
                    }
                }
                else
                {
                    oPDF1.SetFontAndSize(mn, 12);
                    oPDF1.ShowTextAligned(0, folio1, 55, oSize1.Height - 705, 0);
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            oPDF1.EndText();
            conexionB.Close();
            oPDF1.BeginText();
            oPDF1.SetFontAndSize(mn, 12);
            oPDF1.ShowTextAligned(0, año, 50, oSize1.Height - 673, 0);
            oPDF1.ShowTextAligned(0, mes, 105, oSize1.Height - 673, 0);
            oPDF1.ShowTextAligned(0, dia, 150, oSize1.Height - 673, 0);
            oPDF1.EndText();
            oPDF1.SetFontAndSize(mn, 6);
            oPDF1.BeginText();
            oPDF1.ShowTextAligned(0, txtCurp.Text, 55, oSize1.Height - 710, 0);
            oPDF1.EndText();
            oPDF1.SetFontAndSize(mn, 8);
            string eñ = "select SUM(Asistencias) from asistencias where Curp = '" + txtCurp.Text + "' and Grado = '" + textBox1.Text + "' and Grupo = '" + textBox2.Text + "' and Trimestre >=1";
            conexionB.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(eñ, conexionB);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string cali;
                        cali = reader.GetString(0);
                        oPDF1.BeginText();
                        oPDF1.ShowTextAligned(0, cali, 550, oSize1.Height - 420, 0);
                        oPDF1.EndText();
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
            PdfImportedPage page2 = oWriter1.GetImportedPage(oReader1, 2);
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
            lstFiles[0] = @"" + txtRuta.Text + " Calificaciones " + textBox1.Text + "° A.pdf";
            lstFiles[1] = @"" + txtRuta.Text + " Calificaciones " + textBox1.Text + "° B.pdf";
            PdfReader reader2 = null;
            Document sourceDocument = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage;
            string outputPdfPath = @"" + txtRuta.Text + "Calificaciones de "+textBox1.Text+"° Grado del alumno " +txtCurp.Text+".pdf";
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
                    for (int i = 1; i <= pages; i++)
                    {
                        importedPage = pdfCopyProvider.GetImportedPage(reader2, i);
                        pdfCopyProvider.AddPage(importedPage);
                    }

                    reader2.Close();
                }
                //At the end save the output file
                sourceDocument.Close();
                System.IO.File.Delete(@"" + txtRuta.Text + " Calificaciones " + textBox1.Text + "° A.pdf");
                System.IO.File.Delete(@"" + txtRuta.Text + " Calificaciones " + textBox1.Text + "° B.pdf");
                Process.Start(@"" + txtRuta.Text + "Calificaciones de "+textBox1.Text+"° Grado del alumno " + txtCurp.Text + ".pdf");
                if (comboBox2.Text == "3")
                {
                    button3.Enabled = true;
                }
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

        private void button3_Click(object sender, EventArgs e)
        {
                string correoelec = textCorreo.Text;
                string espacio = " ";
                MailMessage correo = new MailMessage();
                correo.From = new MailAddress("tobilegendario@gmail.com", "Boleta", System.Text.Encoding.UTF8);//Correo de salida
                    correo.To.Add(correoelec); //Correo destino?
                    correo.Subject = "Boleta de calificaciones"; //Asunto
                    correo.Body = "Boleta de calificaciones del alumno: " + textAPA.Text + espacio + textAma.Text + espacio + textNom.Text + " "; //Mensaje del correo
                    string ruta = @"" + txtRuta.Text + "Calificaciones de " + textBox1.Text + "° Grado del alumno " + txtCurp.Text + ".pdf"; //aqui va la ruta del pdf con @""
                    System.Net.Mail.Attachment Archivo = new System.Net.Mail.Attachment(ruta);
                    correo.Attachments.Add(Archivo); //Archivo
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
                button3.Enabled = false;
        }
    }
}
