using System.ComponentModel.DataAnnotations;

namespace AbySalto.Junior.Models
{
    public class Narudzba
    {
        // Primarni kljuc narudzbe
        public int Id { get; set; }
        [Required]
        public string ImeKupca { get; set; } = string.Empty;
        public StatusNarudzbe Status { get; set; }
        public DateTime DatumNarudzbe { get; set; }
        [Required]
        public string NacinPlacanja { get; set; } = string.Empty;
        [Required]
        public string AdresaDostave { get; set; } = string.Empty;
        [Required]
        public string KontaktBroj { get; set; } = string.Empty;
        public string? Napomena { get; set; }
        [Required]
        public string Valuta { get; set; } = string.Empty;

        // Popis svih artikala koji pripadaju ovoj narudzbi (relacija 1:N).
        public List<StavkaNarudzbe> Stavke { get; set; } = new List<StavkaNarudzbe>();

        // Automatski izracun ukupne cijene narudzbe na temelju cijene i kolicine svih stavki.
        public decimal UkupnaCijena => Stavke.Sum(s => s.Cijena * s.Kolicina);
    }
}