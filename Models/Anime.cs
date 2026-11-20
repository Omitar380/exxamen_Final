namespace MediaVault.Models
{
    public class Anime : ElementoColeccion
    {
        public string EstudioOEpisodios { get; set; } = string.Empty;

        public override string Tipo => "Anime";

        public override string DatoExtra
        {
            get => EstudioOEpisodios;
            set => EstudioOEpisodios = value;
        }

        public Anime() { }

        public Anime(string titulo, string genero, string estado, int calificacion, string estudioOEpisodios)
            : base(titulo, genero, estado, calificacion)
        {
            EstudioOEpisodios = estudioOEpisodios;
        }

        public override string ObtenerResumen()
        {
            return $"[Anime - {EstudioOEpisodios}] {Titulo} ({Genero}) | {Estado} | Nota: {Calificacion}/10";
        }
    }
}