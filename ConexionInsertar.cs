using MySql.Data.MySqlClient;

namespace Control_Escolar_InstitutoHispanoamericanoMexicano
{
    class ConexionInsertar
    {
        public static MySqlConnection cnx = new MySqlConnection("server = 192.168.0.9; database = preescolar; Uid = ControlEscolar; pwd=123456789");

        //Ingresamos valores a nuestra bitacora
        public bool RegistrarBitacora(string usuario, string fecha, string tiempo, string accion)
        {
            cnx.Open();
            MySqlCommand cmd = new MySqlCommand(string.Format("insert into bitacora values ('{0}', '{1}', '{2}') ", new string[]  { usuario, fecha+" "+tiempo, accion}), cnx);
            int filasafectadas = cmd.ExecuteNonQuery();
            cnx.Close();
            if (filasafectadas > 0)
                return true;
            else
                return false;
        }
        
        //Registrar Usuarios para poder iniciar sesion
        public bool RegistrarUsuario(string usuario, string contraseña, string puesto)
        {
            cnx.Open();
            MySqlCommand cmd = new MySqlCommand(string.Format("insert into usuario values ('{0}', '{1}', '{2}') ", new string[] { usuario, contraseña, puesto}), cnx);
            int filasafectadas = cmd.ExecuteNonQuery();
            cnx.Close();
            if (filasafectadas > 0)
                return true;
            else
                return false;
        }

        public bool RegistrarAlumno(string Curp, string ApellidoPaterno, string ApellidoMaterno, string Nombre, string Sexo, string Fecha, string Estado, string Grado, string Grupo, string ApellidoPaternoTurnto, string ApellidoMaternoTutor, string NombreTutor, string Correo)
        {
            cnx.Open();
            MySqlCommand cmd = new MySqlCommand(string.Format("insert into alumno values ('{0}', '{1}', '{2}','{3}','{4}', '{5}', '{6}','{7}','{8}', '{9}', '{10}','{11}','{12}') ", new string[] { Curp,ApellidoPaterno,ApellidoMaterno,Nombre,Sexo,Fecha,Estado,Grado,Grupo,ApellidoPaternoTurnto,ApellidoMaternoTutor,NombreTutor,Correo}), cnx);
            int filasafectadas = cmd.ExecuteNonQuery();
            cnx.Close();
            if (filasafectadas > 0)
                return true;
            else
                return false;
        }
        
        public bool RegistrarBoletaInterna(string Curp, string Grado, string Grupo, string Mes, string Trimestre, string Materia, string Calificacion, string CalificacionDigito)
        {
            cnx.Open();
            MySqlCommand cmd = new MySqlCommand(string.Format("insert into calificacionmensual values ('{0}', '{1}', '{2}','{3}','{4}', '{5}', '{6}', '{7}') ", new string[] { Curp, Grado, Grupo, Mes, Trimestre, Materia, Calificacion, CalificacionDigito }), cnx);
            int filasafectadas = cmd.ExecuteNonQuery();
            cnx.Close();
            if (filasafectadas > 0)
                return true;
            else
                return false;
        }

        public bool RegistrarAsistencia(string Curp, string Grado, string Grupo, string Mes, string Trimestre, string Asistencias, string Inasistencias )
        {
            cnx.Open();
            MySqlCommand cmd = new MySqlCommand(string.Format("insert into asistencias values ('{0}', '{1}', '{2}','{3}','{4}', '{5}', '{6}') ", new string[] { Curp, Grado, Grupo, Mes, Trimestre, Asistencias, Inasistencias}), cnx);
            int filasafectadas = cmd.ExecuteNonQuery();
            cnx.Close();
            if (filasafectadas > 0)
                return true;
            else
                return false;
        }
        public bool RegistrarBoletaExterna(string Curp, string Grado, string Grupo, string Trimestre, string Materia, string Observacion)
        {
            cnx.Open();
            MySqlCommand cmd = new MySqlCommand(string.Format("insert into calificaciontrimestre values ('{0}', '{1}', '{2}','{3}','{4}', '{5}') ", new string[] { Curp, Grado, Grupo, Trimestre, Materia, Observacion}), cnx);
            int filasafectadas = cmd.ExecuteNonQuery();
            cnx.Close();
            if (filasafectadas > 0)
                return true;
            else
                return false;
        }

    }
}
