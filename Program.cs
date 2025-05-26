using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Webbshoppen.Data;
using Webbshoppen.Menyer;
using Webbshoppen.Models;

namespace Webbshoppen
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var context = new Databas();

            //await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            await SeedData.SeedAsync(context);

            var varukorg = new List<(Produkt, int)>();
            var meny = new HuvudMeny(varukorg);
            await meny.StartSida();
            await meny.VisaMeny();
        }
    }
}
