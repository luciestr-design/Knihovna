using DataEntity;            // Nutné pro databázi
using Knihovna.Globals;      // Nutné pro AppGlobals
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace Knihovna
{
    public partial class App : Application
    {
        // Tato metoda se spustí jako úplně první věc po startu
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 1. Vytvoříme spojení do databáze
            var context = new KnihovnaContext();

            // 2. Ověříme, že databáze existuje (pokud ne, vytvoří se)
            context.Database.EnsureCreated();

            // 3. Naplníme ji testovacími daty
            context.Seed();

            // 4. DŮLEŽITÉ: Uložíme ji do Globals, aby ji viděla okna (KnihyView)
            AppGlobals.Context = context;
        }
    }
}