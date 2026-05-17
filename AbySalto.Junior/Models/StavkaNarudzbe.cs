using System.Text.Json.Serialization;

namespace AbySalto.Junior.Models
{
    public class StavkaNarudzbe
    {
        // Primarni kljuc artikla
        public int Id { get; set; }

        // Naziv artikla
        public string Ime { get; set; }

        // Kolicina artikla u narudzbi
        public int Kolicina { get; set; }

        // Cijena jednog artikla
        public decimal Cijena { get; set; }

        // Strani kljuc koji povezuje artikl narudzbe sa narudzbom
        public int NarudzbaId { get; set; }

        // Navigacijsko svojstvo
        [JsonIgnore]
        public Narudzba? Narudzba { get; set; }
    }
}