using System;
using MySql.Data.MySqlClient;

namespace Control_Escolar_InstitutoHispanoamericanoMexicano
{
    class Conector
    {
        public static MySqlConnection Conexiones() //Nos permite conectarsos a la base de datos
        {
            string cadenaConexion = "server = 192.168.0.9; database = preescolar; Uid = ControlEscolar; pwd=123456789";
            try
            {
                MySqlConnection conexionDB = new MySqlConnection(cadenaConexion);

                return conexionDB;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error:" + ex.Message);

                return null;
            }
        }
    }
}
