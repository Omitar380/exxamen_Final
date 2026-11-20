using MediaVault.Data;
using MediaVault.Models;
using System;
using System.Collections.Generic;

namespace MediaVault.Services
{
    public class ColeccionServicio
    {
        private readonly ElementoDAO _dao;

        public ColeccionServicio()
        {
            _dao = new ElementoDAO();
        }

        public ElementoColeccion ConstruirYValidar(int id, string tipo, string titulo, string genero, string estado, string calificacionTexto, string datoExtra)
        {
            if (tipo != "Videojuego" && tipo != "Anime")
                throw new ArgumentException("Selecciona una categoría válida.");

            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("El título no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(genero))
                throw new ArgumentException("Debes ingresar un género.");

            if (estado != "Pendiente" && estado != "En progreso" && estado != "Completado")
                throw new ArgumentException("Selecciona un estado válido.");

            if (string.IsNullOrWhiteSpace(datoExtra))
                throw new ArgumentException(tipo == "Videojuego"
                    ? "Debes indicar la plataforma (ej. Switch, PC, PS5)."
                    : "Debes indicar los episodios o estudio (ej. 24 eps / Madhouse).");

            if (!int.TryParse(calificacionTexto, out int calificacion) || calificacion < 1 || calificacion > 10)
                throw new ArgumentException("La calificación debe ser un número entero entre 1 y 10.");

            ElementoColeccion elemento = tipo == "Videojuego"
                ? new Videojuego(titulo.Trim(), genero.Trim(), estado, calificacion, datoExtra.Trim())
                : new Anime(titulo.Trim(), genero.Trim(), estado, calificacion, datoExtra.Trim());

            elemento.Id = id;
            return elemento;
        }

        public void Registrar(string tipo, string titulo, string genero, string estado, string calificacionTexto, string datoExtra)
        {
            var elemento = ConstruirYValidar(0, tipo, titulo, genero, estado, calificacionTexto, datoExtra);
            _dao.Insertar(elemento);
        }

        public List<ElementoColeccion> Listar() => _dao.ObtenerTodos();

        public void Modificar(int id, string tipo, string titulo, string genero, string estado, string calificacionTexto, string datoExtra)
        {
            if (id <= 0)
                throw new ArgumentException("Selecciona primero un elemento de la lista para modificar.");

            var elemento = ConstruirYValidar(id, tipo, titulo, genero, estado, calificacionTexto, datoExtra);
            _dao.Actualizar(elemento);
        }

        public void Eliminar(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Selecciona primero un elemento de la lista para eliminar.");

            _dao.Eliminar(id);
        }
    }
}