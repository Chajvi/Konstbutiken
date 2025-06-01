using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Webbshoppen.Data;
using Webbshoppen.Models;

namespace Webbshoppen.Menyer;
internal class ShopMeny
{
    private List<(Produkt, int)> _varukorg;
    public ShopMeny(List<(Produkt, int)> varukorg)
    {
        _varukorg = varukorg;
    }


    public async Task VisaProdukter()
    {
        using var context = new Databas();
        bool igång = true;

        while (igång)
        {
            Console.Clear();
            var produkter = await context.Produkter.ToListAsync();
            Console.WriteLine("PRODUKTERENA!\n");
            foreach (var produkt in produkter)
            {
                Console.WriteLine($"[{produkt.Id}] {produkt.Namn} {produkt.Pris} kr");
            }

            Console.WriteLine("\n[S] Söka produkt");
            Console.WriteLine("\nSkriv ID på den vara ni vill se mer om eller tryck 0 för att komma tillbaka till huvudmenyn.");
            string input = Console.ReadLine()?.Trim();
            if (input?.ToUpper() == "S")
            {
                await ProduktSök();
            }
            if (int.TryParse(input, out int produktId))
            {
                if (produktId == 0)
                {
                    igång = false;
                }

                else
                {
                    await ProduktDetaljer(produktId);
                }
            }
        }

            
    }

    private async Task ProduktDetaljer (int produktId)
    {
        using var context = new Databas();
        var produkt = await context.Produkter.FindAsync(produktId);
        if (produkt == null)
        {
            Console.WriteLine("Vi kunde inte hitta produkten du letade efter.");
            Console.ReadKey();
            return;
        }

        Console.Clear();
        Console.WriteLine("PRODUKTINFO\n");
        Console.WriteLine($"Namn: {produkt.Namn}");
        Console.WriteLine($"Pris: {produkt.Pris} kr");
        Console.WriteLine($"Kategori: {produkt.KategoriId}");
        Console.WriteLine($"Beskrivning: {produkt.Beskrivning}");
        Console.WriteLine($"Lagersaldo: {produkt.LagerSaldo}");

        Console.Write("\n Vill du lägga till produkten i varukorgen? (JA/NEJ)");
        string svar = Console.ReadLine()?.Trim().ToUpper();
        if (svar == "JA")
        {
            int antal;
            while (true)
            {
                Console.WriteLine("Hur måga vill du köpa?");
                string input = Console.ReadLine();
                if (int.TryParse(input, out antal) && antal > 0)
                {
                    break;
                }

                Console.WriteLine("Något gick fel, ange ett positivt heltal.");
            }

            _varukorg.Add((produkt, antal));
            Console.WriteLine($"{antal} {produkt.Namn} har lagts till i varukorgen.");
           
        }

        else if (svar == "NEJ")
        {
            Console.WriteLine("Okej, inget las in!");
        }

        else
        {
            Console.WriteLine("Du måste skriva ja eller nej.");
        }

        Console.WriteLine("Tryck på valfri tangent för att fortsätta");
        Console.ReadKey();

    }

    private async Task ProduktSök()
    {
        using var context = new Databas();
        Console.Clear();
        Console.WriteLine("Vad söker du?");
        string sökord = Console.ReadLine()?.ToLower();
        sökord = sökord?.ToLower()?.Trim();
        var resultat = await context.Produkter
            .Where(p =>
                p.Namn.ToLower().Contains(sökord) ||
                p.Beskrivning.ToLower().Contains(sökord))
            .ToListAsync();

        Console.Clear();
        Console.WriteLine($"RESULTAT: {sökord}\n");
        if (resultat.Count == 0)
        {
            Console.WriteLine("Det fanns inga produkter med de sökorden.");
        }

        else
        {
            foreach (var p in resultat)
            {
                Console.WriteLine($"[{p.Id} {p.Namn}]");
            }

            Console.WriteLine("\n Skriv Id:t på produkten du vill veta mer om eller 0 för att gå tillbaka.");
            string val = Console.ReadLine();
            if (int.TryParse(val, out int produktId))
            {
                if (produktId == 0)
                    return;
                await ProduktDetaljer(produktId);
            }

        }
    }
}
