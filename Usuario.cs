using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Control_Escolar_InstitutoHispanoamericanoMexicano
{
    public partial class Usuario : Form
    {
        private string usuario;
        private string puesto;
        private string accion;
        private string antiguo;
        ConexionInsertar cnx = new ConexionInsertar();
        ConexionEliminar cn = new ConexionEliminar();

        public Usuario()
        {
            InitializeComponent();
        }

        //Recibe datos
        public Usuario(string usuario, string puesto, string accion)
        {
            InitializeComponent();
            this.usuario = usuario;
            this.puesto = puesto;
            this.accion = accion;
        }

        //Muestra fecha y hora
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Text = DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToString("HH:mm:ss");
        }

        //Cerramos sesion
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

        //Nos permite ocultar y mostrar la contraseña
        private void button2_Click(object sender, EventArgs e)
        {
            if (textcontraseña.PasswordChar == '*')
            {
                textcontraseña.PasswordChar = '\0';
            }
            else
                textcontraseña.PasswordChar = '*';
        }

        //Cuenta los caracteres hasta que llegue a 65
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textusuario.MaxLength = 65;
            label4.Text = Convert.ToString(textusuario.Text.Length);
            if(label3.Text == "65")
            {            
                MessageBox.Show("Maximo de caracteres alcanzados");
            }
        }

        //nos permite bloquear caracter numerico y espacios, solo permite letras
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten letras", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        //Bloquea el boton de buscar si vamos a agregar un registro
        private void Usuario_Load(object sender, EventArgs e)
        {
            if(this.accion == "agregar")
            {
                button1.Visible = false;
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
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //Nos permite agregar usuarios que no esten registrados
            if (this.accion == "agregar")
            {
                string sql = "Select usuario, contraseña, puesto from usuario where usuario ='" + textusuario.Text + "' and  contraseña = '" + textcontraseña.Text + "'";
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
                            textusuario.Text = reader.GetString(0);
                            textcontraseña.Text = reader.GetString(1);
                            MessageBox.Show("Usuario ya existe");
                        }
                    }
                    else
                    {
                        if (cnx.RegistrarUsuario(textusuario.Text, textcontraseña.Text, combopuesto.Text)) ;
                        string hora = DateTime.Now.ToString("HH:mm:ss");
                        string Fecha = DateTime.Now.ToString("yyyy-MM-dd");
                        string accion = "Agrego a usuario: " + textusuario.Text;
                        if (cnx.RegistrarBitacora(this.usuario, Fecha, hora, accion)) ;
                        MessageBox.Show("Usuario Agregado con exito");
                        textusuario.Clear(); textcontraseña.Clear();
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);

                }
            }
            //Nos permite modificar los usuarios ya registrados
            if(this.accion == "modificar")
            {
                string usuario = textusuario.Text;
                string contraseña = textcontraseña.Text;
                string puesto = combopuesto.Text;
                MySqlConnection conexionBD = Conector.Conexiones();
                conexionBD.Open();
                string sql = "UPDATE usuario SET usuario='" + usuario + "', contraseña='" + contraseña + "', puesto='" + puesto + "' WHERE (usuario ='" + this.antiguo + "')";
                try
                {
                    MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                    comando.ExecuteNonQuery();
                    string hora = DateTime.Now.ToString("HH:mm:ss");
                    string Fecha = DateTime.Now.ToString("yyyy-MM-dd");
                    string accion = "Modifico a usuario: " + this.antiguo + " a " + usuario;
                    if (cnx.RegistrarBitacora(this.usuario, Fecha, hora, accion)) ;
                    MessageBox.Show("Fue actualizado exitosamente");
                    textusuario.Clear(); textcontraseña.Clear();
                }
                catch (Exception)
                {
                    MessageBox.Show("No se pudo actualizar registro");
                }
                conexionBD.Close();
            }
            //Nos permite eliminar los usuarios ya registrados
            if (this.accion == "eliminar")
            {
                string sql = "Select usuario, contraseña, puesto from usuario where usuario ='" + textusuario.Text + "'";
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
                            if (combopuesto.Text == "Secretatia")
                            {
                                cn.EliminarUsuario(textusuario.Text);
                                string hora = DateTime.Now.ToString("HH:mm:ss");
                                string Fecha = DateTime.Now.ToString("yyyy-MM-dd");
                                string accion = "Elimino a usuario: " + textusuario.Text;
                                if (cnx.RegistrarBitacora(this.usuario, Fecha, hora, accion)) ;
                                MessageBox.Show("Registro  Eliminado correctamente");
                                textusuario.Clear(); textcontraseña.Clear();
                            }
                            else
                            {
                                MessageBox.Show("Director no puede ser borrado");
                            }
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
            }
        }

        //Nos regresa a la bitacora
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Bitacora men = new Bitacora(this.usuario, this.puesto);
            men.Show();
            this.Close();
        }

        //Nos permite buscar un usuario con el boton buscar
        private void button1_Click(object sender, EventArgs e)
        {
            this.antiguo = textusuario.Text;
            string usuario = textusuario.Text;
            MySqlDataReader reader = null;
            string sql = "Select usuario, contraseña, puesto from usuario where usuario LIKE '" + textusuario.Text + "' Limit 1";
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
                        textusuario.Text = reader.GetString(0);
                        textcontraseña.Text = reader.GetString(1);
                        combopuesto.Text = reader.GetString(2);
                        string hora = DateTime.Now.ToString("HH:mm:ss");
                        string Fecha = DateTime.Now.ToString("yyyy-MM-dd");
                        string accion = "Busco a usuario: " + textusuario.Text;
                        if (cnx.RegistrarBitacora(this.usuario, Fecha, hora, accion)) ;
                    }

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            if(combopuesto.Text == "Director")
            {
                combopuesto.Items.Clear();
            }
            if (combopuesto.Text == "Secretaria")
            {
                combopuesto.Items.Clear();
            }
        }

        //Nos dirige a la bitacora
        private void bitacoraToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Bitacora men = new Bitacora();
            men.Show();
            this.Close();
        }

        //Nos dirige al menu
        private void menuToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Menu men = new Menu(this.usuario, this.puesto);
            men.Show();
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
            DataAlumno alu = new DataAlumno(this.usuario,this.puesto,accion);
            alu.Show();
            this.Close();
        }

        private void modificarAlumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string accion = "modificar";
            DataAlumno alu = new DataAlumno(this.usuario, this.puesto, accion);
            alu.Show();
            this.Close();
        }

        private void eliminarAlumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string accion = "eliminar";
            DataAlumno alu = new DataAlumno(this.usuario, this.puesto, accion);
            alu.Show();
            this.Close();
        }

        private void listaPorGradoYGrupoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Listas lis = new Listas(this.usuario, this.puesto);
            lis.Show();
            this.Close();
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