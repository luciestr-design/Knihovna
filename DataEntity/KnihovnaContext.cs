using DataEntity.Data; // Důležité pro entity
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DataEntity
{
    public class KnihovnaContext : DbContext
    {
        // --- ZDE DEFINUJEME TABULKY ---
        public DbSet<Knihy> Knihy { get; set; }
        public DbSet<Uzivatel> Uzivatele { get; set; }
        public DbSet<Vypujcka> Vypujcky { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Data Source=(localdb)\\MSSQLLocalDB;" +
                    "Initial Catalog=KnihovnaDB;" +
                    "Integrated Security=True;" +
                    "TrustServerCertificate=True")
                    .UseLazyLoadingProxies(); // Důležité pro načítání vazeb
            }
        }

        // --- Metoda pro vytvoření testovacích dat ---
        public void Seed()
        {
            // Pokud už databáze obsahuje knihy, nic nedělej (už je naplněná)
            if (Knihy.Any()) return;

            // 1. Vytvoříme knihy
            var k1 = new Knihy { Nazev = "Babička", Autor = "B. Němcová", CelkemKusu = 5, KusuKDispozici = 4, ISBN = "123-456" };
            var k2 = new Knihy { Nazev = "R.U.R.", Autor = "K. Čapek", CelkemKusu = 3, KusuKDispozici = 3, ISBN = "789-012", Zanr = "Sci-Fi" };

            // 2. Vytvoříme uživatele
            var u1 = new Uzivatel { Jmeno = "Jan", Prijmeni = "Novák", Email = "jan@novak.cz", Dluh = 0 };

            // 3. Vytvoříme výpůjčku (Propojíme knihu k1 a uživatele u1)
            var v1 = new Vypujcka
            {
                Kniha = k1,
                Uzivatel = u1,
                Stav = "Vypůjčeno", // Zde používáme text
                DatumPujceni = DateTime.Now.AddDays(-5),
                DatumSplatnosti = DateTime.Now.AddDays(9)
            };

            // Přidáme vše do kontextu a uložíme
            Knihy.AddRange(k1, k2);
            Uzivatele.Add(u1);
            Vypujcky.Add(v1);

            SaveChanges();
        }
    }
}
