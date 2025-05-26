namespace Webbshoppen.Models
{
    public class Kund
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public string Adress { get; set; }
        public string Email { get; set; }

        public List<Beställning> Beställningar { get; set; }
    }
}
