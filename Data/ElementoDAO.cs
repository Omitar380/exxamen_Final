using MediaVault.Models;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace MediaVault.Data
{
    public class ElementoDAO
    {
        private readonly ConexionSQLite _conexionDb;

        public ElementoDAO()
        {
            _conexionDb = new ConexionSQLite();
        }

        // CREATE
        public void Insertar(ElementoColeccion elemento)
        {
            using var conexion = _conexionDb.ObtenerConexion();
            conexion.Open();

            string sql = @"INSERT INTO Elementos (Tipo, Titulo, Genero, Estado, Calificacion, DatoExtra)
                           VALUES (@Tipo, @Titulo, @Genero, @Estado, @Calificacion, @DatoExtra);";

            using var cmd = new SqliteCommand(sql, conexion);
            cmd.Parameters.AddWithValue("@Tipo", elemento.Tipo);
            cmd.Parameters.AddWithValue("@Titulo", elemento.Titulo);
            cmd.Parameters.AddWithValue("@Genero", elemento.Genero);
            cmd.Parameters.AddWithValue("@Estado", elemento.Estado);
            cmd.Parameters.AddWithValue("@Calificacion", elemento.Calificacion);
            cmd.Parameters.AddWithValue("@DatoExtra", elemento.DatoExtra);
            cmd.ExecuteNonQuery();
        }

        // READ
        public List<ElementoColeccion> ObtenerTodos()
        {
            var lista = new List<ElementoColeccion>();

            using var conexion = _conexionDb.ObtenerConexion();
            conexion.Open();

            string sql = "SELECT Id, Tipo, Titulo, Genero, Estado, Calificacion, DatoExtra FROM Elementos ORDER BY Id DESC;";
            using var cmd = new SqliteCommand(sql, conexion);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string tipo = reader.GetString(1);
                ElementoColeccion item = tipo switch
                {
                    "Videojuego" => new Videojuego(),
                    "Anime" => new Anime(),
                    _ => throw new System.InvalidOperationException($"La categoría '{tipo}' almacenada en SQLite no es válida.")
                };

                item.Id = reader.GetInt32(0);
                item.Titulo = reader.GetString(2);
                item.Genero = reader.GetString(3);
                item.Estado = reader.GetString(4);
                item.Calificacion = reader.GetInt32(5);
                item.DatoExtra = reader.GetString(6);

                lista.Add(item);
            }

            return lista;
        }

        // UPDATE
        public void Actualizar(ElementoColeccion elemento)
        {
            using var conexion = _conexionDb.ObtenerConexion();
            conexion.Open();

            string sql = @"UPDATE Elementos
                           SET Tipo = @Tipo,
                               Titulo = @Titulo,
                               Genero = @Genero,
                               Estado = @Estado,
                               Calificacion = @Calificacion,
                               DatoExtra = @DatoExtra
                           WHERE Id = @Id;";

            using var cmd = new SqliteCommand(sql, conexion);
            cmd.Parameters.AddWithValue("@Id", elemento.Id);
            cmd.Parameters.AddWithValue("@Tipo", elemento.Tipo);
            cmd.Parameters.AddWithValue("@Titulo", elemento.Titulo);
            cmd.Parameters.AddWithValue("@Genero", elemento.Genero);
            cmd.Parameters.AddWithValue("@Estado", elemento.Estado);
            cmd.Parameters.AddWithValue("@Calificacion", elemento.Calificacion);
            cmd.Parameters.AddWithValue("@DatoExtra", elemento.DatoExtra);
            cmd.ExecuteNonQuery();
        }

        // DELETE
        public void Eliminar(int id)
        {
            using var conexion = _conexionDb.ObtenerConexion();
            conexion.Open();

            string sql = "DELETE FROM Elementos WHERE Id = @Id;";
            using var cmd = new SqliteCommand(sql, conexion);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }
    }
}