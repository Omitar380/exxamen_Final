using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace MediaVault.Data
{
    public class ConexionSQLite
    {
        private readonly string _connectionString;

        public ConexionSQLite()
        {
            string directorioDatos = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "MediaVault");
            Directory.CreateDirectory(directorioDatos);

            string dbPath = Path.Combine(directorioDatos, "mediavault.db");
            _connectionString = $"Data Source={dbPath}";
            InicializarBaseDeDatos();
        }

        public SqliteConnection ObtenerConexion()
        {
            return new SqliteConnection(_connectionString);
        }

        private void InicializarBaseDeDatos()
        {
            using var conexion = ObtenerConexion();
            conexion.Open();

            string sql = @"
                CREATE TABLE IF NOT EXISTS Elementos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Tipo TEXT NOT NULL,
                    Titulo TEXT NOT NULL,
                    Genero TEXT NOT NULL,
                    Estado TEXT NOT NULL,
                    Calificacion INTEGER NOT NULL,
                    DatoExtra TEXT NOT NULL
                );";

            using var comando = new SqliteCommand(sql, conexion);
            comando.ExecuteNonQuery();
        }
    }
}