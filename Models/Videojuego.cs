namespace MediaVault.Models
{
    public class Videojuego : ElementoColeccion
    {
        public string Plataforma { get; set; } = string.Empty;

        public override string Tipo => "Videojuego";

        public override string DatoExtra
        {
            get => Plataforma;
            set => Plataforma = value;
        }

        public Videojuego() { }

        public Videojuego(string titulo, string genero, string estado, int calificacion, string plataforma)
            : base(titulo, genero, estado, calificacion)
        {
            Plataforma = plataforma;
        }

        public override string ObtenerResumen()
        {
            return $"[Juego - {Plataforma}] {Titulo} ({Genero}) | {Estado} | Nota: {Calificacion}/10";
        }
    }
}