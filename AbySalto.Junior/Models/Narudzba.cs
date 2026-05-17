namespace AbySalto.Junior.Models
{
    public class Narudzba
    {
        // Primarni kljuc narudzbe
        public int Id { get; set; }
        public string ImeKupca { get; set; }
        public StatusNarudzbe Status { get; set; }
        public DateTime DatumNarudzbe { get; set; }
        public string NacinPlacanja { get; set; }
        public string AdresaDostave { get; set; }
        public string KontaktBroj { get; set; }
        public string Napomena { get; set; }
        public string Valuta { get; set; }

        // Popis svih artikala koji pripadaju ovoj narudzbi (relacija 1:N).
        public List<StavkaNarudzbe> Stavke { get; set; } = new List<StavkaNarudzbe>();

        // Automatski izracun ukupne cijene narudzbe na temelju cijene i kolicine svih stavki.
        public decimal UkupnaCijena => Stavke.Sum(s => s.Cijena * s.Kolicina);
    }
}