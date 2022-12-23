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
    public partial class DataAlumno : Form
    {
        ConexionInsertar cnx = new ConexionInsertar();
        ConexionEliminar cn = new ConexionEliminar();
        public string puesto;
        public string usuario;
        public string accion;
        public string antiguo;
        string[,] estados;

        public DataAlumno()
        {
            InitializeComponent();
        }

        public DataAlumno(string usuario, string puesto, string accion)
        {
            InitializeComponent();
            this.usuario = usuario;
            this.puesto = puesto;
            this.accion = accion;
        }

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Menu log = new Menu(this.usuario, this.puesto);
            log.Show();
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Text = DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToString("HH:mm:ss");
        }

        private void verAlumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VerAlumno alu = new VerAlumno(this.usuario, this.puesto);
            alu.Show();
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if(this.accion == "agregar")
            {
                if (txtCurp.Text != "")
                {
                    string sql = "Select  Curp  from alumno where Curp ='" + txtCurp.Text + "'";
                    MySqlConnection conexionBD = Conector.Conexiones();
                    MySqlDataReader reader = null;
                    conexionBD.Open();
                    try
                    {
                        MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                        reader = comando.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                txtCurp.Text = reader.GetString(0);
                                MessageBox.Show("Alumno ya existe");
                            }
                        }
                        else
                        {
                            conexionBD.Close();
                            string sl = "Select  Nombre, Grado from alumno where ApellidoPaterno = '"+textAPA.Text+"' and ApellidoMaterno = '"+textAma.Text+"' and Nombre ='" + textNom.Text + "'";
                            conexionBD.Open();
                            try
                            {
                                MySqlCommand comand = new MySqlCommand(sl, conexionBD);
                                reader = comand.ExecuteReader();
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        textNom.Text = reader.GetString(0);
                                        comboGrado.Text = reader.GetString(1);
                                        MessageBox.Show("Alumno ya existe");
                                    }
                                }
                               
                                else
                                {
                                    conexionBD.Close();
                                    if (cnx.RegistrarAlumno(txtCurp.Text, textAPA.Text, textAma.Text, textNom.Text, comboSexo.Text, dateFecha.Text, Entid.Text, comboGrado.Text, comboGrupo.Text, textApaTu.Text, textAmaTu.Text, textNoTu.Text, textCorreo.Text)) ;
                                    string hora = DateTime.Now.ToString("HH:mm:ss");
                                    string Fecha = DateTime.Now.ToString("yyyy-MM-dd");
                                    string accion = "Agrego a alumno: " + textNom.Text;
                                    if (cnx.RegistrarBitacora(this.usuario, Fecha, hora, accion)) ;
                                    MessageBox.Show("Usuario Agregado con exito");
                                   
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error" + ex.Message);
                            }
                        }
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Error al buscar" + ex.Message);

                    }
                }
                else
                {
                    MessageBox.Show("Falta valores");
                }
            } //Agregamos Alumno
            if (this.accion == "modificar") //Modificamos Alumno
            {
                MySqlConnection conexionBD = Conector.Conexiones();
                conexionBD.Open();
                string sql = "UPDATE alumno SET Curp ='"+ txtCurp.Text+ "', ApellidoPaterno = '"+ textAPA.Text+"', ApellidoMaterno = '"+textAma.Text+"', Nombre= '"+textNom.Text+"', Sexo='"+comboSexo.Text+"', FechaNacimiento='"+dateFecha.Text+"', Estado='"+Entid.Text+"',Grado ='"+comboGrado.Text+"', Grupo = '"+comboGrupo.Text+"', ApellidoPaternoTutor = '"+textApaTu.Text+"', ApellidoMaternoTutor = '"+textAmaTu.Text+"', NombreTutor= '"+textNoTu.Text+"', CorreoTutor = '"+textCorreo.Text+"' WHERE (Curp ='" + this.antiguo + "')";
                try
                {
                    MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                    comando.ExecuteNonQuery();
                    string hora = DateTime.Now.ToString("HH:mm:ss");
                    string Fecha = DateTime.Now.ToString("yyyy-MM-dd");
                    string accion = "Modifico a usuario: " + this.antiguo + " a " + textNom;
                    cnx.RegistrarBitacora(this.usuario, Fecha, hora, accion);
                    MessageBox.Show("Fue actualizado exitosamente");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo actualizar registro  " + ex);
                }
                conexionBD.Close();
            } //Modificar Alumno
            if (this.accion == "eliminar")
            {

                string sql = "Select Curp from alumno where Curp ='" + txtCurp.Text + "'";
                MySqlConnection conexionBD = Conector.Conexiones();
                MySqlDataReader reader = null;
                conexionBD.Open();
                try
                {
                    MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            cn.EliminarAsistencias(txtCurp.Text);
                            cn.EliminarCalificacionMen(txtCurp.Text);
                            cn.EliminarCalificacionTri(txtCurp.Text);
                            cn.EliminarAlmno(txtCurp.Text);
                            string hora = DateTime.Now.ToString("HH:mm:ss");
                            string Fecha = DateTime.Now.ToString("yyyy-MM-dd");
                            string accion = "Elimino a usuario: " + txtCurp.Text;
                            if (cnx.RegistrarBitacora(this.usuario, Fecha, hora, accion)) ;
                            MessageBox.Show("Registro  Eliminado correctamente");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Usuario no existe");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
            } //Eliminamos Alumno
        }

        private void agregarAlumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string accion = "agregar";
            DataAlumno log = new DataAlumno(this.usuario, this.puesto, accion);
            log.Show();
            this.Close();
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

        private void DataAlumno_Load(object sender, EventArgs e)
        {
            if(this.accion == "agregar")
            {
                agregarAlumnosToolStripMenuItem.Visible = false;
                txtCurp.Enabled = false;
                btnBuscar.Visible = false;
            }
            if (this.accion == "modificar")
            {
                modificarAlumnosToolStripMenuItem.Visible = false;
            }
            if (this.accion == "eliminar")
            {
                eliminarAlumnosToolStripMenuItem.Visible = false;
            }
            if (this.puesto == "Director")
            {
            }
            //Oculta las ventanas que no puede usar la secretaria
            if (this.puesto == "Secretaria")
            {
                modificarAlumnosToolStripMenuItem.Visible = false;
                eliminarAlumnosToolStripMenuItem.Visible = false;
            }
            estados = new string[,]
            {
                {"",""},
                {"AGUASCALIENTES","AS"},
                {"BAJA CALIFORNIA","BC"},
                {"BAJA CALIFORNIA SUR","BS"},
                {"CAMPECHE","CC"},
                {"CHIAPAS","CS"},
                {"CHIHUAHUA","CH"},
                {"COAHUILA","CL"},
                {"COLIMA","CM"},
                {"DISTRITO FEDERAL","DF"},
                {"DURANGO","DG"},
                {"GUANAJUATO","GT"},
                {"GUERRERO","GR"},
                {"HIDALGO","HG"},
                {"JALISCO","JC"},
                {"MEXICO","MC"},
                {"MICHOACAN","MN"},
                {"MORELOS","MS"},
                {"NAYARIT","NT"},
                {"NUEVO LEON","NL"},
                {"OAXACA","OC"},
                {"PUEBLA","PL"},
                {"QUERETARO","QT"},
                {"QUINTANA ROO","QR"},
                {"SAN LUIS POTOSI","SP"},
                {"SINALOA","SL"},
                {"SONORA","SR"},
                {"TABASCO","TC"},
                {"TAMAULIPAS","TS"},
                {"TLAXCALA","TL"},
                {"VERACRUZ","VZ"},
                {"YUCATÁN","YN"},
                {"ZACATECAS","ZS"},
                {"NACIDO EXTRANJERO","NE"}
            };

            dateFecha.Format = DateTimePickerFormat.Custom;
            dateFecha.CustomFormat = "yyyy/MM/dd";
            int contador = 0;
            Entid.Items.Add("<Seleccione Estado>");
            Entid.SelectedIndex = 0;
            while (contador != estados.Length/2)
            {
                Entid.Items.Add(estados[contador, 0]);
                contador++;
            }
            Entid.Items.Remove("");
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            VerAlumno alu = new VerAlumno(this.usuario,this.puesto);
            alu.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e) //Nos permite generar una curp automatica que conforman los 3 a 5 años de edad
        {
            DateTime Today = DateTime.Now;
            string año = DateTime.Now.ToString("yyyy");
            string mes = DateTime.Now.ToString("MM");
            string dia = DateTime.Now.ToString("dd");
            double fecha = dateFecha.Value.Year;
            double valor;
            double revisa = Convert.ToDouble(año);

            valor = revisa - fecha;

            if (valor >= 3)
            {
                if (valor <= 5)
                {
                    if (string.IsNullOrEmpty(estados[Entid.SelectedIndex, 1]) || string.IsNullOrEmpty(textAPA.Text) || string.IsNullOrEmpty(textNom.Text))
                    {
                        MessageBox.Show("Por favor ingrese todos los datos...");
                    }
                    else
                    {
                        CURPLib.CURPLib curp = new CURPLib.CURPLib();
                        //MessageBox.Show(SEXO.ToString());
                        txtCurp.Text = curp.CURPCompleta(textAPA.Text, textAma.Text, textNom.Text, dateFecha.Text, comboSexo.Text, estados[Entid.SelectedIndex,1]);
                        //.Show(estados[comboBox1.SelectedIndex,1]);
                        pictureBox1.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("LIMITE DE EDAD 5 AÑOS");
                }
            }
            else
            {
                MessageBox.Show("MENOR DE 3 AÑOS");
            }
        }

        private void comboSexo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            MySqlDataReader reader = null;
            string sql = "Select Curp, ApellidoPaterno, ApellidoMaterno, Nombre, Sexo, FechaNacimiento, Estado, Grado, Grupo, ApellidoPaternoTutor, ApellidoMaternoTutor, NombreTutor, CorreoTutor from alumno where Curp LIKE '" + txtCurp.Text + "' Limit 1";
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
                        txtCurp.Text = reader.GetString(0);
                        textAPA.Text = reader.GetString(1);
                        textAma.Text = reader.GetString(2);
                        textNom.Text = reader.GetString(3);
                        comboSexo.Text = reader.GetString(4);
                        dateFecha.Text = reader.GetString(5);
                        Entid.Text = reader.GetString(6);
                        comboGrado.Text = reader.GetString(7);
                        comboGrupo.Text = reader.GetString(8);
                        textApaTu.Text = reader.GetString(9);
                        textAmaTu.Text = reader.GetString(10);
                        textNoTu.Text = reader.GetString(11);
                        textCorreo.Text = reader.GetString(12);
                        this.antiguo = txtCurp.Text;
                        string hora = DateTime.Now.ToString("HH:mm:ss");
                        string Fecha = DateTime.Now.ToString("yyyy-MM-dd");
                        string accion = "Busco a alumno: " + textNom.Text;
                        if (cnx.RegistrarBitacora(this.usuario, Fecha, hora, accion));
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
        }

        private void dateFecha_ValueChanged(object sender, EventArgs e)
        {
            string año = DateTime.Now.ToString("yyyy");
            string mes = DateTime.Now.ToString("MM");
            string dia = DateTime.Now.ToString("dd");
            double fecha = dateFecha.Value.Year;
            double valor;
            double revisa = Convert.ToDouble(año);
            valor = revisa - fecha;
            if (valor == 3)
            {
                comboGrado.Items.Clear();
                comboGrado.Items.Add("1");
            }
            if (valor == 4)
            {
                comboGrado.Items.Clear();
                comboGrado.Items.Add("2");
            }
            if (valor == 5)
            {
                comboGrado.Items.Clear();
                comboGrado.Items.Add("3");
            }
            if (accion == "modificar")
            {
                
               
                if (valor >= 3)
                {
                    if (valor <= 5)
                    {
                        pictureBox1.Visible = true;
                    }
                    else
                    {
                        pictureBox1.Visible = false;
                    }
                }
                else
                {
                    pictureBox1.Visible = false;
                }


                    }
        }

        private void listaPorGradoYGrupoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Listas lis = new Listas(this.usuario, this.puesto);
            lis.Show();
            this.Close();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

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
