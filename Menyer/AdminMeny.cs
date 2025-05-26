using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webbshoppen.Data;
using Webbshoppen.Models;

namespace Webbshoppen.Menyer;
internal class AdminMeny
{
    public async Task VisaAdminMeny()
    {
        bool igång = true;
        while (igång)
        {
            Console.Clear();
            Console.WriteLine("ADMIN MENY");
            Console.WriteLine("[1] Lägg till en ny produkt");
            Console.WriteLine("[2] Ändra befintlig produkt");
            Console.WriteLine("[3] Ta bort en produkt");
            Console.WriteLine("[4] Hantera kunder");
            Console.WriteLine("[5] Se tidigare beställningar");
            Console.WriteLine("[6] Ändra speciella produkter");
            Console.WriteLine("[0] Logga ut");
            Console.Write("Val: ");
            string val = Console.ReadLine();

            switch (val)
            {
                case "1":
                    await LäggTillProdukt();
                    break;
                case "2":
                    await ÄndraProdukt();
                    break;
                case "3":
                    await TabortProdukt();
                    break;
                case "4":
                    await HanteraKund();
                    break;
                case "5":
                    await BeställningsHistorik();
                    break;
                case "6":
                    await SpecialProdukter();
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

    private async Task LäggTillProdukt()
    {
        using var context = new Databas();
        Console.Clear();
        Console.WriteLine("LÄGG TILL PRODUKT");

        Console.WriteLine("Namn: ");
        string namn = Console.ReadLine();
        Console.WriteLine("Pris: ");
        decimal pris = decimal.Parse(Console.ReadLine());
        Console.WriteLine("Kategori: ");
        string kategori = Console.ReadLine();
        Console.WriteLine("Leverantör: ");
        string leverantör = Console.ReadLine();
        Console.WriteLine("Lagersaldo: ");
        int lagersaldo = int.Parse(Console.ReadLine());

        var produkt = new Produkt
        {
            Namn = namn,
            Pris = pris,
            Kategori = kategori,
            Leverantör = leverantör,
            LagerSaldo = lagersaldo
        };

        context.Produkter.Add(produkt);
        await context.SaveChangesAsync();
        Console.WriteLine("Den nya produkten har lagts till!");
        Console.ReadKey();
    }

    private async Task ÄndraProdukt()
    {
        using var context = new Databas();
        Console.Clear();
        Console.WriteLine("ÄNDRA PRODUKTER");

        var produkter = await context.Produkter.ToListAsync();
        foreach (var p in produkter)
        {
            Console.WriteLine($"[{p.Id}] {p.Namn} ({p.Pris}) kr");
        }
        Console.WriteLine("\nSkriv in Id nummer på den produkt du vill ändra på: ");
        int id = int.Parse(Console.ReadLine());
        var produkt = await context.Produkter.FindAsync(id);
        if (produkt == null)
        {
            Console.WriteLine("Det finns ingen produkt med det Id:t");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Lämna fältet tomt för att behålla det gamla innehållet.");
        Console.Write($"Nytt Namn: ({produkt.Namn})");
        string namn = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(namn)) produkt.Namn = namn;
        Console.Write($"Nytt Pris: ({produkt.Pris})");
        string prisInput = Console.ReadLine();
        if (decimal.TryParse(prisInput, out decimal pris)) produkt.Pris = pris;
        Console.Write($"Ny beskrivning: ({produkt.Beskrivning})");
        string beskrivning = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(beskrivning)) produkt.Beskrivning = beskrivning;
        Console.Write($"Ny kategori: ({produkt.Kategori})");
        string kategori = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(kategori)) produkt.Kategori = kategori;
        Console.Write($"Ny leverantör: ({produkt.Leverantör})");
        string leverantör = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(leverantör)) produkt.Leverantör = leverantör;
        Console.Write($"Nytt lagersaldo: ({produkt.LagerSaldo})");
        string lagersaldoInput = Console.ReadLine();
        if (int.TryParse(lagersaldoInput, out int saldo)) produkt.LagerSaldo = saldo;


        await context.SaveChangesAsync();
        Console.WriteLine("Produkten har nu uppdaterats!");
        Console.ReadKey();
    }

    private async Task TabortProdukt()
    {
        using var context = new Databas();
        Console.Clear();
        Console.WriteLine("Ta bort produkt");
        var produkter = await context.Produkter.ToListAsync();
        foreach (var p in produkter)
        {
            Console.WriteLine($"[{p.Id}] {p.Namn}");
        }

        Console.Write("\n Skriv in Id på produkten du vill ta bort: ");
        int id = int.Parse(Console.ReadLine());
        var produkt = await context.Produkter.FindAsync(id);
        if (produkt == null)
        {
            Console.WriteLine("Det finns ingen produkt med det id:t");
        }
        else
        {
            context.Produkter.Remove(produkt);
            await context.SaveChangesAsync();
            Console.WriteLine("Produkten har tagits bort ur sortimentet.");
        }

        Console.ReadKey();
    }

    private async Task HanteraKund()
    {
        using var context = new Databas();
        Console.Clear();
        Console.WriteLine("Hantera kunder");
        var kunder = await context.Kunder.ToListAsync();
        foreach (var k in kunder)
        {
            Console.WriteLine($"[{k.Id} {k.Namn} - {k.Email}]");
        }

        Console.Write("\n Ange Id på kunden vars uppgifter du vill redigera: ");
        int id = int.Parse(Console.ReadLine());
        var kund = await context.Kunder.FindAsync(id);
        if (kund == null)
        {
            Console.WriteLine("Det fanns ingen kund med det id:t");
            Console.ReadKey();
            return;
        }

        //Om jag hinner, lägg även till här att man kan lämna tomt för att behålla det gamla värdet
    }

    private async Task BeställningsHistorik()
    {
        using var context = new Databas();
        Console.Clear();
        Console.WriteLine("BESTÄLLNINGS HISTORIK");
        var kunder = await context.Kunder.ToListAsync();
        foreach(var k in kunder)
        {
            Console.WriteLine($"[{k.Id}] {k.Namn} {k.Email}");
        }

        Console.WriteLine("\nAnge Id på kunden: ");
        int id = int.Parse(Console.ReadLine());
        var beställningar = await context.Beställningar
            .Where (b => b.KundId == id)
            .Include(b => b.BeställdaVaror).ThenInclude(bv => bv.Produkt)
            .Include(b => b.Kund)
            .Include (b => b.LeveransAlternativ)
            .Include (b => b.BetalningsAlternativ)
            .ToListAsync();
           
    }

    private async Task SpecialProdukter()
    {
        using var context = new Databas();
        Console.Clear();
        Console.WriteLine("VÄLJ SPECIALPRODUKTER FÖR STARTSIDAN");
        var produkter = await context.Produkter.ToListAsync();
        foreach (var p in produkter)
        {
            string status = p.Special ? "(SPECIAL)" : "";
            Console.WriteLine($"[{p.Id} {p.Namn} {status}]");
        }

        Console.WriteLine("\n Skriv Id:t på produkten du vill ska få en speciell plats på startsidan");
        string input = Console.ReadLine();
        if (int.TryParse(input, out int id))
        {
            var produkt = await context.Produkter.FindAsync(id);
            if (produkt == null)
            {
                Console.WriteLine("Tyvärr så hittade vi inte produkten");
            }

            else
            {
                
                var specialAntal = await context.Produkter.CountAsync(p => p.Special);
                if (!produkt.Special && specialAntal >= 3)
                {
                    Console.WriteLine($"Du har redan max antal specialprodukter. Du får inte ha mer än 3 åt gången.");
                }

                else
                {
                    produkt.Special = !produkt.Special;
                    await context.SaveChangesAsync();
                    Console.WriteLine("Produkterna är nu tillagda som speciella!");
                }
            }

            

            Console.WriteLine("\n Tryck på valfri tangent för att frotsätta.");
            Console.ReadKey();
        }
    }

    }

