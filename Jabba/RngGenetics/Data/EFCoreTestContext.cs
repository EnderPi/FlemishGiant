using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RngGenetics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RngGenetics.Data
{
    internal class EFCoreTestContext : DbContext
    {
        public DbSet<SboxFourBits> Sboxes { get; set; }
        public DbSet<BackgroundTask> BackgroundTasks { get; set; }

        public static readonly Microsoft.Extensions.Logging.LoggerFactory _myLoggerFactory =
    new LoggerFactory(new[] {
        new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
    });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Integrated Security = SSPI; Database=EFCoreTest;Trusted_Connection=True; TrustServerCertificate=True");
            optionsBuilder.UseLoggerFactory(_myLoggerFactory);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
        }
    }
}
