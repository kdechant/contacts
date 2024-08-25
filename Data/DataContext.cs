using Microsoft.EntityFrameworkCore;
using ContactManager.Models;
using Microsoft.Extensions.Options;

namespace ContactManager.Data
{
    public class DataContext(IConfiguration config) : DbContext
    {
        private readonly IConfiguration _config = config;

        public DbSet<Contact> Contacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options
                    .UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                        optionsBuilder => optionsBuilder.EnableRetryOnFailure());
            }
        }
    }
}
