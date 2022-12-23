using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Control_Escolar_InstitutoHispanoamericanoMexicano
{
    class Conexionbuscar
    {
        public static MySqlConnection cnx = new MySqlConnection("server = 192.168.0.9; database = preescolar; Uid = ControlEscolar; pwd=123456789");
        private DataSet ds;

        //Mostrar Bitacora
        public static DataTable MostrarBitacora()
        {
            DataTable tab = new DataTable();
            MySqlDataAdapter cosa = new MySqlDataAdapter("select usuario, fecha, accion from bitacora order by fecha desc", cnx);
            try
            {

                cnx.Open();
                cosa.Fill(tab);
                cnx.Close();

            }

            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }
            finally { cnx.Close(); }
            return tab;
        }

        //Buscar Usuarios
        public DataTable BuscarUsuario(string usuario) 
        {
            cnx.Open();
            MySqlCommand cmd = new MySqlCommand(string.Format("select usuario, fecha, tiempo, accion from bitacora where usuario like '%{0}%'", usuario), cnx);
            MySqlDataAdapter ad = new MySqlDataAdapter(cmd);
            ds = new DataSet();
            ad.Fill(ds, "tabla");
            cnx.Close();
            return ds.Tables["tabla"];
        }

        public static DataTable MostrarAlumno()
        {
            DataTable tab = new DataTable();
            MySqlDataAdapter cosa = new MySqlDataAdapter("select Curp, ApellidoPaterno, ApellidoMaterno,Nombre,Grado,Grupo, CorreoTutor from alumno", cnx);
            try
            {

                cnx.Open();
                cosa.Fill(tab);
                cnx.Close();

            }

            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }
            finally { cnx.Close(); }
            return tab;
        }

        public DataTable BuscarAlumnoGradoGrupo(string Grado, string Grupo) //Buscar Alumnos
        {
            cnx.Open();
            MySqlCommand cmd = new MySqlCommand(string.Format("select Curp, ApellidoPaterno, ApellidoMaterno, Nombre, sexo  from alumno where Grado like '" + Grado + "' and Grupo like '" + Grupo + "'"), cnx);
            MySqlDataAdapter ad = new MySqlDataAdapter(cmd);
            ds = new DataSet();
            ad.Fill(ds, "tabla");
            cnx.Close();
            return ds.Tables["tabla"];
        }

        public DataTable BuscarCurp(string Curp) //Buscar Alumnos
        {
            cnx.Open();
            MySqlCommand cmd = new MySqlCommand(string.Format("select Curp, ApellidoPaterno, ApellidoMaterno, Nombre, sexo, Grado, Grupo  from alumno where Curp like '%{0}%'", Curp ), cnx);
            MySqlDataAdapter ad = new MySqlDataAdapter(cmd);
            ds = new DataSet();
            ad.Fill(ds, "tabla");
            cnx.Close();
            return ds.Tables["tabla"];
        }

        public DataTable BuscarApellido(string Apellido) //Buscar Alumnos
        {
            cnx.Open();
            MySqlCommand cmd = new MySqlCommand(string.Format("select Curp, ApellidoPaterno, ApellidoMaterno, Nombre, sexo, Grado, Grupo  from alumno where ApellidoPaterno like '%{0}%'",  Apellido), cnx);
            MySqlDataAdapter ad = new MySqlDataAdapter(cmd);
            ds = new DataSet();
            ad.Fill(ds, "tabla");
            cnx.Close();
            return ds.Tables["tabla"];
        }
    }
}
