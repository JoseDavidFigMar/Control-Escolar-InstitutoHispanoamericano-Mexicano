using MySql.Data.MySqlClient;

namespace Control_Escolar_InstitutoHispanoamericanoMexicano
{
    class ConexionEliminar
    {
        public static MySqlConnection cn = new MySqlConnection("server = 192.168.0.9; database = preescolar; Uid = ControlEscolar; pwd=123456789");

        public bool EliminarUsuario(string usuario) //Nos permite eliminar usuarios
        {
            cn.Open();
            MySqlCommand cmd = new MySqlCommand(string.Format("delete from usuario where usuario = '{0}'", usuario), cn);
            int filasafectadas = cmd.ExecuteNonQuery();
            cn.Close();
            if (filasafectadas > 0)
                return true;

            else
                return false;

        }

        public bool EliminarAlmno(string curp) //Nos permite eliminar alumno
        {
            cn.Open();
            MySqlCommand cmd = new MySqlCommand(string.Format("delete from alumno where Curp = '{0}'", curp), cn);
            int filasafectadas = cmd.ExecuteNonQuery();
            cn.Close();
            if (filasafectadas > 0)
                return true;

            else
                return false;

        }
        public bool EliminarAsistencias(string curp) //Nos permite eliminar alumno
        {
            cn.Open();
            MySqlCommand cmd = new MySqlCommand(string.Format("delete from asistencias where Curp = '{0}'", curp), cn);
            int filasafectadas = cmd.ExecuteNonQuery();
            cn.Close();
            if (filasafectadas > 0)
                return true;

            else
                return false;

        }
        public bool EliminarCalificacionMen(string curp) //Nos permite eliminar alumno
        {
            cn.Open();
            MySqlCommand cmd = new MySqlCommand(string.Format("delete from calificacionmensual where Curp = '{0}'", curp), cn);
            int filasafectadas = cmd.ExecuteNonQuery();
            cn.Close();
            if (filasafectadas > 0)
                return true;

            else
                return false;

        }
        public bool EliminarCalificacionTri(string curp) //Nos permite eliminar alumno
        {
            cn.Open();
            MySqlCommand cmd = new MySqlCommand(string.Format("delete from calificaciontrimestre where Curp = '{0}'", curp), cn);
            int filasafectadas = cmd.ExecuteNonQuery();
            cn.Close();
            if (filasafectadas > 0)
                return true;

            else
                return false;

        }
    }
}
