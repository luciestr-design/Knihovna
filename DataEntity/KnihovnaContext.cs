using Microsoft.EntityFrameworkCore;

namespace DataEntity
{
    public class KnihovnaContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Data Source=(localdb)\\MSSQLLocalDB;" +
                    "Initial Catalog=Knihovna;" +
                    "Integrated Security=True;" +
                    "TrustServerCertificate=True").UseLazyLoadingProxies();
            }


        }
    }
}
