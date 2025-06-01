using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webbshoppen.Models;

namespace Webbshoppen.Data;
internal class SeedData
{
    public static async Task SeedAsync(Databas context)
    {
        
        //Mitt inventarie
        if (!context.Kategorier.Any())
        {
            var canvasKategori = new Kategori { Namn = "Canvasdukar" };
            var penselKategori = new Kategori { Namn = "Penslar" };
            var färgKategori = new Kategori { Namn = "Färger" };

            context.Kategorier.AddRange(canvasKategori, penselKategori, färgKategori);
            await context.SaveChangesAsync();
            context.Produkter.AddRange(
                new Produkt
                {
                    Namn = "Tygcanvas",
                    Beskrivning = "Canvasduk i linne, 100 x 80 cm",
                    Pris = 599.98m,
                    KategoriId = canvasKategori.Id,
                    LagerSaldo = 15,
                    Leverantör = "TavlorAB"
                },
                new Produkt
                {
                    Namn = "Bomullscanvas",
                    Beskrivning = "Canvasduk i bomull, 50 x 50 cm",
                    Pris = 259.59m,
                    KategoriId = canvasKategori.Id,
                    LagerSaldo = 12,
                    Leverantör = "TavlorAB"
                },
                new Produkt
                {
                    Namn = "Startset Penslar",
                    Beskrivning = "Ett nybörjarset med penslar, 8 stycken i olika storlekar av syntetiskt material",
                    Pris = 359.99m,
                    KategoriId = penselKategori.Id,
                    LagerSaldo = 5,
                    Leverantör = "Penselmästarna"
                },
                new Produkt
                {
                    Namn = "Finess Pensel",
                    Beskrivning = "Vår finaste pensel gjord av hästhår",
                    Pris = 459.98m,
                    KategoriId = penselKategori.Id,
                    LagerSaldo = 3,
                    Leverantör = "Penselmästarna"
                },
                new Produkt
                {
                    Namn = "Akvarellpalett",
                    Beskrivning = "En enkelt nybörjarpalett med 8 akvarellfärger",
                    Pris = 199.99m,
                    KategoriId = färgKategori.Id,
                    LagerSaldo = 24,
                    Leverantör = "PaintPals"
                },
                new Produkt
                {
                    Namn = "Akryltuber",
                    Beskrivning = "Ett set med akrylfärger på tub. Pastellfärger i tio olika nyanser",
                    Pris = 359.99m,
                    KategoriId = färgKategori.Id,
                    LagerSaldo = 8,
                    Leverantör = "PaintPals"
                });

            
        }

        //Leveransalternativen för kunderna
        if (!context.LeveransAlternativ.Any())
        {
            context.LeveransAlternativ.AddRange(
                new LeveransAlternativ { Namn = "Dålig frakt", Pris = 299, Leveranstid = "42 veckor" },
                new LeveransAlternativ { Namn = "Lite bättre frakt", Pris = 199, Leveranstid = "4 veckor" },
                new LeveransAlternativ { Namn = "Helt okej frakt", Pris = 99, Leveranstid = "1 vecka" },
                new LeveransAlternativ { Namn = "Den bästa frakten", Pris = 29, Leveranstid = "24 timmar" });
        }

        //Betalningsalternativ för kunderna
        if (!context.BetalningsAlternativ.Any())
        {
            context.BetalningsAlternativ.AddRange(
                new BetalningsAlternativ { Namn = "Konkurssättarnas Avbetalning" },
                new BetalningsAlternativ { Namn = "Rånarbankens Kortbetalning" },
                new BetalningsAlternativ { Namn = "Mormor Gretas 30 dagars faktura" });
        }




        if (!await context.Admins.AnyAsync())
        {
            context.Admins.Add(new Admin { Email = "admin@mail.se", Lösenord = "admin123" });
            Console.WriteLine("En basadmin är skapad");
        }

        await context.SaveChangesAsync();

    }


}
