using DataEntity.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DataEntity
{
    public class KnihovnaContext : DbContext
    {
        public DbSet<Knihy> Knihy { get; set; }
        public DbSet<Uzivatel> Uzivatele { get; set; }
        public DbSet<Vypujcka> Vypujcky { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Data Source=(localdb)\\MSSQLLocalDB;" +
                    "Initial Catalog=KnihovnaDB_2025;" +
                    "Integrated Security=True;" +
                    "TrustServerCertificate=True")
                    .UseLazyLoadingProxies();
            }
        }

        public void Seed()
        {
            // Pokud už v DB něco je, nebudeme nic přidávat, aby se data neduplikovala
            if (Knihy.Any()) return;

            // --- 1. VYTVOŘENÍ 10 KNIH ---
            var k1 = new Knihy { Nazev = "Babička", Autor = "Božena Němcová", RokVydani = 1855, Zanr = "Klasika", ISBN = "978-80-123", CelkemKusu = 5, KusuKDispozici = 5 };
            var k2 = new Knihy { Nazev = "R.U.R.", Autor = "Karel Čapek", RokVydani = 1920, Zanr = "Sci-Fi", ISBN = "978-80-456", CelkemKusu = 3, KusuKDispozici = 2 };
            var k3 = new Knihy { Nazev = "1984", Autor = "George Orwell", RokVydani = 1949, Zanr = "Antiutopie", ISBN = "978-80-789", CelkemKusu = 10, KusuKDispozici = 8 };
            var k4 = new Knihy { Nazev = "Zaklínač: Poslední přání", Autor = "Andrzej Sapkowski", RokVydani = 1993, Zanr = "Fantasy", ISBN = "978-80-111", CelkemKusu = 7, KusuKDispozici = 0 };
            var k5 = new Knihy { Nazev = "Harry Potter a Kámen mudrců", Autor = "J.K. Rowlingová", RokVydani = 1997, Zanr = "Fantasy", ISBN = "978-80-222", CelkemKusu = 12, KusuKDispozici = 12 };
            var k6 = new Knihy { Nazev = "Hobit", Autor = "J.R.R. Tolkien", RokVydani = 1937, Zanr = "Fantasy", ISBN = "978-80-333", CelkemKusu = 6, KusuKDispozici = 5 };
            var k7 = new Knihy { Nazev = "Válka s Mloky", Autor = "Karel Čapek", RokVydani = 1936, Zanr = "Sci-Fi", ISBN = "978-80-444", CelkemKusu = 4, KusuKDispozici = 4 };
            var k8 = new Knihy { Nazev = "Pýcha a předsudek", Autor = "Jane Austenová", RokVydani = 1813, Zanr = "Román", ISBN = "978-80-555", CelkemKusu = 5, KusuKDispozici = 3 };
            var k9 = new Knihy { Nazev = "Malý princ", Autor = "Antoine de Saint-Exupéry", RokVydani = 1943, Zanr = "Pohádka", ISBN = "978-80-666", CelkemKusu = 8, KusuKDispozici = 6 };
            var k10 = new Knihy { Nazev = "To", Autor = "Stephen King", RokVydani = 1986, Zanr = "Horor", ISBN = "978-80-777", CelkemKusu = 2, KusuKDispozici = 2 };

            // --- 2. VYTVOŘENÍ 10 UŽIVATELŮ ---
            var u1 = new Uzivatel { Jmeno = "Jan", Prijmeni = "Novák", Email = "jan.novak@email.cz", Adresa = "Václavské nám. 1, Praha 1", Telefon = "777 111 222", Dluh = 0 };
            var u2 = new Uzivatel { Jmeno = "Petr", Prijmeni = "Svoboda", Email = "petr.svoboda@seznam.cz", Adresa = "Masarykova 10, Brno", Telefon = "608 222 333", Dluh = 0 };
            var u3 = new Uzivatel { Jmeno = "Jana", Prijmeni = "Dvořáková", Email = "jana.dvorakova@post.cz", Adresa = "Dlouhá 5, Olomouc", Telefon = "720 333 444", Dluh = 50 };
            var u4 = new Uzivatel { Jmeno = "Eva", Prijmeni = "Černá", Email = "eva.cerna@gmail.com", Adresa = "Hlavní 8, Ostrava", Telefon = "602 444 555", Dluh = 0 };
            var u5 = new Uzivatel { Jmeno = "Tomáš", Prijmeni = "Procházka", Email = "tomas.p@firma.cz", Adresa = "Zahradní 15, Plzeň", Telefon = "775 555 666", Dluh = 0 };
            var u6 = new Uzivatel { Jmeno = "Martin", Prijmeni = "Kučera", Email = "martin.k@email.cz", Adresa = "Lesní 3, Liberec", Telefon = "603 666 777", Dluh = 0 };
            var u7 = new Uzivatel { Jmeno = "Lenka", Prijmeni = "Veselá", Email = "lenka.ves@seznam.cz", Adresa = "Polní 9, Hradec Králové", Telefon = "774 777 888", Dluh = 120 };
            var u8 = new Uzivatel { Jmeno = "Pavel", Prijmeni = "Horák", Email = "pavel.horak@post.cz", Adresa = "Nádražní 20, Pardubice", Telefon = "608 888 999", Dluh = 0 };
            var u9 = new Uzivatel { Jmeno = "Alena", Prijmeni = "Němcová", Email = "alena.n@gmail.com", Adresa = "Školní 2, České Budějovice", Telefon = "723 999 000", Dluh = 0 };
            var u10 = new Uzivatel { Jmeno = "David", Prijmeni = "Marek", Email = "david.marek@email.cz", Adresa = "Husova 7, Zlín", Telefon = "731 000 111", Dluh = 0 };

            // --- 3. VYTVOŘENÍ PÁR VÝPŮJČEK DO ZAČÁTKU ---
            var v1 = new Vypujcka { Kniha = k2, Uzivatel = u1, Stav = "Vypůjčeno", DatumPujceni = DateTime.Now.AddDays(-10), DatumSplatnosti = DateTime.Now.AddDays(4) };
            var v2 = new Vypujcka { Kniha = k3, Uzivatel = u1, Stav = "Vypůjčeno", DatumPujceni = DateTime.Now.AddDays(-2), DatumSplatnosti = DateTime.Now.AddDays(12) };
            var v3 = new Vypujcka { Kniha = k6, Uzivatel = u2, Stav = "Vráceno", DatumPujceni = DateTime.Now.AddDays(-20), DatumSplatnosti = DateTime.Now.AddDays(-6), DatumVraceni = DateTime.Now.AddDays(-5) };
            var v4 = new Vypujcka { Kniha = k8, Uzivatel = u3, Stav = "Po splatnosti", DatumPujceni = DateTime.Now.AddDays(-30), DatumSplatnosti = DateTime.Now.AddDays(-16) }; // Jana má dluh a knihu po splatnosti

            // Přidání do kolekcí
            Knihy.AddRange(k1, k2, k3, k4, k5, k6, k7, k8, k9, k10);
            Uzivatele.AddRange(u1, u2, u3, u4, u5, u6, u7, u8, u9, u10);
            Vypujcky.AddRange(v1, v2, v3, v4);

            SaveChanges();
        }
    }
}
