using DataEntity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;

namespace Knihovna.Globals
{
    public static class AppGlobals
    {
        // Zde je oprava s "null!" - slibujeme, že to naplníme při startu
        public static KnihovnaContext Context { get; set; } = null!;

        // Metoda pro tlačítko "Storno" - vrátí změny v paměti zpět
        public static void Vratit()
        {
            foreach (var entity in Context.ChangeTracker.Entries())
            {
                if (entity.State == EntityState.Modified) entity.Reload();
                if (entity.State == EntityState.Added) entity.State = EntityState.Detached;
                if (entity.State == EntityState.Deleted) entity.State = EntityState.Unchanged;
            }
        }

        // Kontrola, zda jsou neuložené změny (např. při zavírání okna)
        public static bool HasUnsavedChanges()
        {
            return Context.ChangeTracker.Entries().Any(e =>
                e.State == EntityState.Added ||
                e.State == EntityState.Modified ||
                e.State == EntityState.Deleted);
        }

        // Hlavní metoda Uložit - včetně kontroly chyb (validace)
        public static void UlozitData()
        {
            // 1. Validace entit
            var entitiesToValidate = Context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(e => e.Entity);

            var validationErrors = new List<ValidationResult>();

            foreach (var entity in entitiesToValidate)
            {
                var validationContext = new ValidationContext(entity);
                Validator.TryValidateObject(entity, validationContext, validationErrors, validateAllProperties: true);
            }

            // Pokud jsou chyby, vypíšeme je a neukládáme
            if (validationErrors.Any())
            {
                string text = string.Join("\n", validationErrors.Select(e => $"- {e.ErrorMessage}"));
                MessageBox.Show("Nelze uložit data, obsahují chyby:\n" + text, "Chyba validace", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 2. Uložení do databáze
            try
            {
                Context.SaveChanges();
                MessageBox.Show("Data byla úspěšně uložena.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při ukládání:\n" + ex.Message, "Chyba DB", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}