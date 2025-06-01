using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
            Console.WriteLine("[7] Se betalningsstatestik"); //DENNA ÄR NY SEDAN REDOVISNINGEN!!
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
                case "7":
                    await BetalningsStatestik(); //DENNA ÄR NY SEDAN REDOVCISNINGEN!!
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
        Console.WriteLine("Beskrivning: ");
        string beskrivning = Console.ReadLine();
        
        //Nytt sedan redovisning
        var kategorier = await context.Kategorier.ToListAsync();
        Console.WriteLine("Välj en kategori:");
        foreach (var kategori in kategorier)
        {
            Console.WriteLine($"[{kategori.Id}] {kategori.Namn}");
        }
        Console.WriteLine("Ange id:t för kategorin: ");
        int kategoriId = int.Parse(Console.ReadLine());
        //Fram tills här.
        //Console.WriteLine("Kategori: ");      Gammal kod
        //string kategori = Console.ReadLine();     Gammal kod
        Console.WriteLine("Leverantör: ");
        string leverantör = Console.ReadLine();
        Console.WriteLine("Lagersaldo: ");
        int lagersaldo = int.Parse(Console.ReadLine());

        

        var produkt = new Produkt
        {
            Namn = namn,
            Pris = pris,
            Beskrivning = beskrivning,
            KategoriId = kategoriId,
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
        //Nytt:
        Console.Write($"Ny kategori: (id): ({produkt.KategoriId})");
        string kategoriInput = Console.ReadLine();
        if (int.TryParse(kategoriInput, out int nyKategoriId))
        {
            produkt.KategoriId = nyKategoriId;
        }
        //Här slutar det nya
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

        Console.Clear();
        Console.WriteLine("REDIGERA KUNDUPPGIFTER: ");
        Console.Write($"Namn ({kund.Namn}): ");
        string namn = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(namn)) kund.Namn = namn;

        Console.Write($"Email ({kund.Email}): ");
        string email = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(email)) kund.Email = email;

        Console.Write($"Adress ({kund.Adress}): ");
        string adress = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(adress)) kund.Adress = adress;

        await context.SaveChangesAsync();

        Console.WriteLine("\nKunduppgifterna har uppdaterats.");
        Console.WriteLine("Tryck på valfri tangent för att fortsätta.");
        Console.ReadKey();
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

        Console.Clear();
        if (beställningar.Count == 0)
        {
            Console.WriteLine("Det verkar inte som att kunden har gjort några beställningar än.");
        }

        else
        {
            Console.WriteLine("Tidigare beställningar: \n");
            foreach (var b in beställningar)
            {
                Console.WriteLine($" - ID: {b.Id} | Datum: {b.Datum:yyyy-MM-dd} | Totalt: {b.TotalBelopp} kr");
            }
        }

        Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
        Console.ReadKey();
           
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

    private async Task BetalningsStatestik() //DENNA ÄR NY SEDAN REDOVISNINGEN!!
    {
        using var context = new Databas();
        Console.Clear();
        Console.WriteLine("Statestik för olika betalningssätt\n");
        var samladData = await context.Beställningar
            .GroupBy(b => b.BetalningsAlternativ.Namn)
            .Select(g => new
            {
                Namn = g.Key,
                Antal = g.Count()
            })
            .OrderByDescending(g => g.Antal)
            .ToListAsync();
        if (!samladData.Any())
        {
            Console.WriteLine("Tyvärr hittades inga beställningar");
        }
        else
        {
            foreach (var post in samladData)
            {
                Console.WriteLine($"Betalningsalternativ: {post.Namn} - {post.Antal} st");
            }
        }

        Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
        Console.ReadKey();
    }

    }

