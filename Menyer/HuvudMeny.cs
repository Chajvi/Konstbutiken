using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webbshoppen.Data;
using Webbshoppen.Models;

namespace Webbshoppen.Menyer;
public class HuvudMeny
{
    private Kund _inloggadKund = null;
    private readonly List<(Produkt, int)> _varukorg;
    public HuvudMeny(List<(Produkt, int)> varukorg)
    {
        _varukorg = varukorg;
    }


    public async Task StartSida()
    {
        using var context = new Databas();
        Console.Clear();
        Console.WriteLine("VÄLKOMMEN TILL KONSTBUTIKEN!");
        var special = await context.Produkter
            .Where(p => p.Special)
            .Take(3)
            .ToListAsync();     
       
        
        Console.WriteLine("SPECIELLT UTVALDA PRODUKTER!!\n");
        Console.WriteLine("Notera att det inte finns något speciellt med produkterna.\n");
        foreach (var produkt in special)
          {
             Console.WriteLine($"{produkt.Namn} - {produkt.Pris} kr");
             Console.WriteLine($"{produkt.Beskrivning}");
             Console.WriteLine();
          }
        

        Console.WriteLine("Tryck på valfri tangent för att komma till shopmenyn.");
        Console.ReadKey();
    }



    public async Task VisaMeny()
    {
        bool igång = true;
        while(igång)
        {
            Console.Clear();
            Console.WriteLine("VÄLKOMMEN TILL KONSTSHOPPEN\n");
            Console.WriteLine("[1] Shoppa");
            Console.WriteLine("[2] Varukorgen");
            Console.WriteLine("[3] Kassan");
            Console.WriteLine("[4] Logga in");
            Console.WriteLine("[5] Skapa Konto");
            Console.WriteLine("[6] Logga in som admin");
            Console.WriteLine("[0] Lämna shoppen");
            Console.Write("Du valde: ");

            string val = Console.ReadLine();

            switch (val)
            {
                case "1":
                    await VisaShopMeny();
                    break;
                case "2":
                    await VisaVarukorg();
                    break;
                case "3": 
                    await Kassa();
                    break;
                case "4":
                    await LoggaIn();
                    break;
                case "5":
                    await SkapaKonto();
                    break;
                case "6":
                    await LoggaInSomAdmin();
                    break;
                case "0":
                    igång = false;
                    break;
                default:
                    Console.WriteLine("Valet finns inte som alternativ, var snäll välj ett menyval.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task VisaShopMeny()
    {
        if (_inloggadKund == null)
        {
            Console.WriteLine("Bara inloggade kunder får shoppa här.");
            Console.ReadKey();
            return;
        }

        var shopMeny = new ShopMeny(_varukorg);
        await shopMeny.VisaProdukter();
    }

    private Task VisaVarukorg()
    {
        var meny = new VarukorgMeny(_varukorg);
        meny.VisaMeny();
        return Task.CompletedTask;
    }

    private async Task LoggaIn()
    {
        using var context = new Databas();
        Console.Clear();
        Console.WriteLine("LOGGA IN");
        Console.Write("Email: ");
        string email = Console.ReadLine();

        var kund = await context.Kunder.FirstOrDefaultAsync(k => k.Email == email);
        if (kund == null)
        {
            Console.WriteLine("Det finns ingen kund registrerad med den emailen.");
        }

        else
        {
            _inloggadKund = kund;
            Console.WriteLine($"Välkommen tillbaka {kund.Namn}!");
        }

        Console.ReadKey();
    }

    private async Task SkapaKonto()
    {
        using var context = new Databas();
        Console.Clear();
        Console.WriteLine("SKAPA KONTO");
        Console.WriteLine("Namn: ");
        string namn = Console.ReadLine();
        Console.WriteLine("Email: ");
        string email = Console.ReadLine();
        Console.WriteLine("Adress: ");
        string adress = Console.ReadLine();

        var kund = new Kund
        {
            Namn = namn,
            Email = email,
            Adress = adress
        };

        context.Kunder.Add(kund);
        await context.SaveChangesAsync();
        Console.WriteLine("Kontot är registrerat! Nu kan du logga in och shoppa!");
        Console.ReadKey();
    }



    private async Task LoggaInSomAdmin()
    {
        using var context = new Databas();
        Console.Clear();
        Console.WriteLine("INLOGGNING ADMIN");

        Console.Write("Email: ");
        string email = Console.ReadLine();
        Console.Write("Lösenord: ");
        string lösenord = Console.ReadLine();
        var admin = await context.Admins.FirstOrDefaultAsync(a => a.Email == email && a.Lösenord == lösenord);
        if (admin == null)
        {
            Console.WriteLine("Du skrev fel mail eller lösenord.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Du är nu inloggad som admin!");
        Console.ReadKey();
        var adminMeny = new AdminMeny();
        await adminMeny.VisaAdminMeny();
    }


    
    

    private async Task Kassa()
    {
        if (_inloggadKund == null)
        {
            Console.WriteLine("Bara inloggade kunder kan se varukorgen.");
            Console.ReadKey();
            return;
        }
        Console.Clear();
        Console.WriteLine("KASSAN");

        if (_varukorg.Count == 0)
        {
            Console.WriteLine("Du har inte lagt till något i varukorgen än.");
            Console.WriteLine("Tryck på valfri tangent för att fortsätta.");
            Console.ReadKey();
            return;
        }

        decimal total = 0;
        foreach (var (produkt, antal) in _varukorg)
        {
            total += produkt.Pris * antal;
        }
        decimal skatt = total * 0.25m;
        Console.WriteLine($"Totalbelopp: {total} kr varav {skatt} är momsen\n");
        Console.WriteLine("Vill du fortsätta med beställningen? JA/NEJ");
        string svar = Console.ReadLine()?.ToUpper();

        if (svar != "JA")
        {
            Console.WriteLine("Något hände, beställningen avbröts. Tryck på valfri tangent för att fortsätta.");
            Console.ReadKey();
            return;
        }

        using var context = new Databas();
        {

            //Välja hur sdet ska levereras
            Console.Clear();
            var leveransalternativ = await context.LeveransAlternativ.ToListAsync();
            Console.WriteLine("När vill du att varorna ska levereras?");
            for (int i = 0; i < leveransalternativ.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {leveransalternativ[i].Namn}(+{leveransalternativ[i].Pris} kr)");
            }

            Console.Write("\nDu valde: ");
            int leveransVal = int.Parse(Console.ReadLine());
            var valdLeverans = leveransalternativ[leveransVal - 1];


            //Välj hur de ska betalas
            Console.Clear();
            var betalningsalternativ = await context.BetalningsAlternativ.ToListAsync();
            Console.WriteLine("Hur vill du betala för varorna?");
            for (int i = 0; i <betalningsalternativ.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {betalningsalternativ[i].Namn}");
            }

            Console.Write("\nDu valde: ");
            int betalningsVal = int.Parse(Console.ReadLine());
            var valdBetalning = betalningsalternativ[betalningsVal - 1];

            //Visa hur mycket aklt kommer kosta innan de skickas iväg
            Console.Clear();
            decimal slutpris = total + valdLeverans.Pris;
            Console.WriteLine($"Delsumma: {total} kr");
            Console.WriteLine($"Frakt ({valdLeverans.Namn}: {valdLeverans.Pris} kr)");
            Console.WriteLine($"Totalsumma att betala: {slutpris} kr");
            Console.WriteLine("\nVill du skicka iväg beställningen? JA/NEJ");
            string skicka = Console.ReadLine()?.Trim().ToUpper();

            if (skicka == "JA")
            {
                var beställning = new Beställning
                {
                    Datum = DateTime.Now,
                    KundId = _inloggadKund.Id,
                    LeveransAlternativId = valdLeverans.Id,
                    BetalningsAlternativId = valdBetalning.Id,
                    TotalBelopp = slutpris,
                    BeställdaVaror = _varukorg.Select(v => new BeställdVara
                    {
                        ProduktId = v.Item1.Id,
                        Antal = v.Item2
                    }).ToList()
                };

                context.Beställningar.Add(beställning);
                foreach (var (produkt, antal) in _varukorg)
                {
                    var dbProdukt = await context.Produkter.FindAsync(produkt.Id);
                    if (dbProdukt != null && dbProdukt.LagerSaldo >= antal)
                    {
                        dbProdukt.LagerSaldo -= antal;
                    }
                }
                await context.SaveChangesAsync();

                Console.WriteLine($"\nBeställningen har skickats och kommer till dig om {valdLeverans.Leveranstid}");
                _varukorg.Clear();
                Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                Console.ReadKey();
                return;
            }
            else
            {
                Console.WriteLine("Något hände, beställningen avbröts. Tryck på valfri tangent för att fortsätta.");
                Console.ReadKey();
                return;
            }

        }
    }
}
