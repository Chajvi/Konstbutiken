namespace Webbshoppen.Models
{
    public class Produkt
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public string Beskrivning { get; set; }
        public decimal Pris { get; set; }
        //Bytt från string Kategori till int KategoriId
        public int KategoriId { get; set; }
        //Också lagt till denna som nav
        public Kategori Kategori { get; set; }
        public int LagerSaldo { get; set; }
        public string Leverantör { get; set; }
        public bool Special { get; set; }
    }
}
