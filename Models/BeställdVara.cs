namespace Webbshoppen.Models
{
    public class BeställdVara
    {
        public int Id { get; set; }
        public int BeställningsId { get; set; }
        public Beställning Beställning { get; set; }

        public int ProduktId { get; set; }
        public Produkt Produkt { get; set; }

        public int Antal { get; set; }
        public decimal PrisVidKöp { get; set; }
    }
}
