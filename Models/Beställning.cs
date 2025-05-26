namespace Webbshoppen.Models;
public class Beställning
{
    public int Id { get; set; }
    public DateTime Datum { get; set; }
    public decimal TotalBelopp { get; set; }
    public int KundId { get; set; }
    public Kund Kund { get; set; }
    public int LeveransAlternativId { get; set; }
    public LeveransAlternativ LeveransAlternativ { get; set; }

    public int BetalningsAlternativId { get; set; }
    public BetalningsAlternativ BetalningsAlternativ { get; set; }

    public List<BeställdVara> BeställdaVaror { get; set; }
}
