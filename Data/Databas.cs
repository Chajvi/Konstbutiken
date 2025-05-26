using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Webbshoppen.Models;

namespace Webbshoppen.Data
{
    public class Databas : DbContext
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Beställning> Beställningar { get; set; }
        public DbSet<BeställdVara> BeställVaror { get; set; }
        public DbSet<BetalningsAlternativ> BetalningsAlternativ { get; set; }
        public DbSet<Kategori> Kategorier { get; set; }
        public DbSet<Kund> Kunder { get; set; }
        public DbSet<LeveransAlternativ> LeveransAlternativ { get; set; }
        public DbSet<Produkt> Produkter { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Lokal databas
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WebbshoppenDB;Trusted_Connection=True;");
        }
    }
}
