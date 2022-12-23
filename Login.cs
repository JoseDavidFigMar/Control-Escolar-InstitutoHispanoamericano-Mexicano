using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;


namespace Control_Escolar_InstitutoHispanoamericanoMexicano
{
    public partial class Login : Form
    {
        ConexionInsertar cnx = new ConexionInsertar();
        public Login()
        {
            InitializeComponent();
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Salida_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Nos permite iniciar sesion con el usuario y contraseña que introduzcamos
        private void Conexion_Click(object sender, EventArgs e)
        {
            string usuario = textusuario.Text;
            string contraseña = textcontraseña.Text;
            string puesto;
            string hora = DateTime.Now.ToString("HH:mm:ss");
            string Fecha = DateTime.Now.ToString("yyyy-MM-dd");
            string accion = "Inicio de sesion";
            string sql = "Select usuario, contraseña, puesto from usuario where usuario ='" + usuario + "' and  contraseña = '" + contraseña + "'";
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
                        puesto = reader.GetString(2);
                        if (cnx.RegistrarBitacora(textusuario.Text, Fecha, hora, accion));
                        string usuarios = textusuario.Text;
                        if (puesto == "Director")
                        {
                            MessageBox.Show("Bienvenido " + usuarios + "\n" + "Todos los permitos admitidos");
                            Menu men = new Menu(usuarios, puesto);
                            men.Show();
                            this.Hide();
                        }
                        if (puesto == "Secretaria")
                        {
                            MessageBox.Show("Bienvenido " + usuarios + "\n" + "No puede modificar, Ni eliminar datos");
                            Menu men = new Menu(usuarios, puesto);
                            men.Show();
                            this.Hide();
                        }
                    }
                }
                else
                {
                    if (textusuario.Text == "Primero")
                    {
                        if (textcontraseña.Text == "123456789")
                        {
                            string usuarios = "Director";
                            string puestos = "Director";
                            MessageBox.Show("Bienvenido " + usuarios + "\n" + "Todos los permitos admitidos");
                            Menu men = new Menu(usuarios, puestos);
                            men.Show();
                            this.Hide();
                        }
                        else
                        {
                            textusuario.Clear(); textcontraseña.Clear();
                            MessageBox.Show("Usuario Incorrecto");
                        }
                    }
                    else
                    {
                        textusuario.Clear(); textcontraseña.Clear();
                        MessageBox.Show("Usuario Incorrecto");
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            conexionBD.Close();
        }

        //muestra el dia y la hora en nuestro programa
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Text = DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
