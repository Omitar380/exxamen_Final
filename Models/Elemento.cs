using System;

namespace MediaVault.Models
{
    public abstract class ElementoColeccion
    {
        private int _calificacion;

        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public string Estado { get; set; } = "Pendiente"; // Pendiente, En progreso, Completado

        // Encapsulamiento con validación en la propiedad
        public int Calificacion
        {
            get => _calificacion;
            set
            {
                if (value < 1 || value > 10)
                    throw new ArgumentOutOfRangeException(nameof(Calificacion), "La calificación debe estar entre 1 y 10.");
                _calificacion = value;
            }
        }

        public abstract string Tipo { get; }
        public abstract string DatoExtra { get; set; }

        protected ElementoColeccion() { }

        protected ElementoColeccion(string titulo, string genero, string estado, int calificacion)
        {
            Titulo = titulo;
            Genero = genero;
            Estado = estado;
            Calificacion = calificacion;
        }

        // Polimorfismo: cada subclase describe su resumen específico
        public abstract string ObtenerResumen();
    }
}