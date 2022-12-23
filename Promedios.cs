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
    public partial class Promedios : Form
    {
        public string Usuario;
        public string Puesto;
        public string Curp;

        public Promedios()
        {
            InitializeComponent();
        }
        public Promedios(string Usuario, string Puesto, String Curp)
        {
            this.Usuario = Usuario;
            this.Puesto = Puesto;
            this.Curp = Curp;
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Text = DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToString("HH:mm:ss");
        }

        private void len_Click(object sender, EventArgs e)
        {

        }

        private void Salida_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Promedios_Load(object sender, EventArgs e)
        {
            txtCurp.Text = this.Curp;
        }

        private void txtCurp_TextChanged(object sender, EventArgs e)
        {
            MySqlDataReader reader = null;
            string sql = "Select ApellidoPaterno, ApellidoMaterno, Nombre, Grado, Grupo from alumno where Curp LIKE '" + txtCurp.Text + "' Limit 1";
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
                        txtgrado.Text = reader.GetString(3);
                        txtgrupo.Text = reader.GetString(4);
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
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionBD.Close();
            string curp = txtCurp.Text;
            string Grado = txtgrado.Text;
            string Grupo = txtgrupo.Text;
            string trimestre1 = "1";
            string trimestre2 = "2";
            string trimestre3 = "3";

            string a = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '"+Grupo+"' and Trimestre = '"+trimestre1+"' and Materia = '"+len.Text+"'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(a, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato <= 6)
                        {
                            textBox1.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox1.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox1.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox1.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox1.Text = "E";
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
            conexionBD.Close();

            string b = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + len.Text + "'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(b, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato <= 6)
                        {
                            textBox2.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox2.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox2.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox2.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox2.Text = "E";
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
            conexionBD.Close();

            string c = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + len.Text + "'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(c, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato <= 6)
                        {
                            textBox3.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox3.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox3.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox3.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox3.Text = "E";
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
            conexionBD.Close();

            string d = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + PEN.Text + "'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(d, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato <= 6)
                        {
                            textBox6.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox6.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox6.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox6.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox6.Text = "E";
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
            conexionBD.Close();

            string f = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + PEN.Text + "'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(f, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato <= 6)
                        {
                            textBox5.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox5.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox5.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox5.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox5.Text = "E";
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
            conexionBD.Close();

            string g = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + PEN.Text + "'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(g, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato <= 6)
                        {
                            textBox4.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox4.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox4.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox4.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox4.Text = "E";
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
            conexionBD.Close();

            string h = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + EXP.Text + "'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(h, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato <= 6)
                        {
                            textBox9.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox9.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox9.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox9.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox9.Text = "E";
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
            conexionBD.Close();

            string I = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + EXP.Text + "'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(I, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato <= 6)
                        {
                            textBox8.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox8.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox8.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox8.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox8.Text = "E";
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
            conexionBD.Close();

            string J = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + EXP.Text + "'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(J, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato <= 6)
                        {
                            textBox7.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox7.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox7.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox7.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox7.Text = "E";
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
            conexionBD.Close();

            string k = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + label12.Text + "'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(k, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato <= 6)
                        {
                            textBox12.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox12.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox12.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox12.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox12.Text = "E";
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
            conexionBD.Close();

            string l = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + label12.Text + "'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(l, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato <= 6)
                        {
                            textBox11.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox11.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox11.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox11.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox11.Text = "E";
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
            conexionBD.Close();

            string m = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + label12.Text + "'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(m, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato <= 6)
                        {
                            textBox10.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox10.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox10.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox10.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox10.Text = "E";
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
            conexionBD.Close();

            string n = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + label13.Text + "'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(n, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato <= 6)
                        {
                            textBox15.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox15.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox15.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox15.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox15.Text = "E";
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
            conexionBD.Close();

            string o = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + label13.Text + "'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(o, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato<= 6)
                        {
                            textBox14.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox14.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox14.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox14.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox14.Text = "E";
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
            conexionBD.Close();

            string p = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + label13.Text + "'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(p, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato <= 6)
                        {
                            textBox13.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox13.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox13.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox13.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox13.Text = "E";
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
            conexionBD.Close();

            string q = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + label14.Text + "'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(q, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato <= 6)
                        {
                            textBox18.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox18.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox18.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox18.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox18.Text = "E";
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
            conexionBD.Close();

            string r = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + label14.Text + "'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(r, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato <= 6)
                        {
                            textBox17.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox17.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox17.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox17.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox17.Text = "E";
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
            conexionBD.Close();

            string s = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + label14.Text + "'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(s, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato <= 6)
                        {
                            textBox16.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox16.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox16.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox16.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox16.Text = "E";
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
            conexionBD.Close();

            string t = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + label15.Text + "'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(t, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato <= 6)
                        {
                            textBox21.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox21.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox21.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox21.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox21.Text = "E";
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
            conexionBD.Close();

            string u = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + label15.Text + "'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(u, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato <= 6)
                        {
                            textBox20.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox20.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox20.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox20.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox20.Text = "E";
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
            conexionBD.Close();

            string v = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + label15.Text + "'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(v, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato <= 6)
                        {
                            textBox19.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox19.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox19.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox19.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox19.Text = "E";
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
            conexionBD.Close();

            string w = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre1 + "' and Materia = '" + label16.Text + "'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(w, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato <= 6)
                        {
                            textBox24.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox24.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox24.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox24.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox24.Text = "E";
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
            conexionBD.Close();

            string x = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre2 + "' and Materia = '" + label16.Text + "'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(x, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato <= 6)
                        {
                            textBox23.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox23.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox23.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox23.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox23.Text = "E";
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
            conexionBD.Close();

            string z = "select  Round(avg(CalificacionDigito)) from calificacionmensual where Curp = '" + curp + "' and Grado = '" + Grado + "' and Grupo = '" + Grupo + "' and Trimestre = '" + trimestre3 + "' and Materia = '" + label16.Text + "'";
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(z, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string valor = reader.GetString(0);
                        int dato = Convert.ToInt32(valor);
                        if (dato <= 6)
                        {
                            textBox22.Text = "NA";
                        }
                        if (dato == 7)
                        {
                            textBox22.Text = "R";
                        }
                        if (dato == 8)
                        {
                            textBox22.Text = "B";
                        }
                        if (dato == 9)
                        {
                            textBox22.Text = "MB";
                        }
                        if (dato == 10)
                        {
                            textBox22.Text = "E";
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
            conexionBD.Close();
        }
    }
}
