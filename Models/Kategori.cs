namespace Webbshoppen.Models
{
    public class Kategori
    {
        public int Id { get; set; }
        public string Namn { get; set; }

        public List<Produkt> Produkter { get; set; }
    }
}
