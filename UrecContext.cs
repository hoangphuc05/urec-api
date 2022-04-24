
using Microsoft.EntityFrameworkCore;

namespace UREC_api
{
    public class UrecContext: DbContext
    {
        public DbSet<QRToken> QRTokens { get; set; }
        public DbSet<Student> Students { get; set; }

        public string DbPath { get; }

        public UrecContext(DbContextOptions<UrecContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Student>()
                .HasIndex(student => student.Uid)
                .IsUnique();
        }

        //public UrecContext()
        //{
        //    var folder = Environment.SpecialFolder.LocalApplicationData;
        //    var path = Environment.GetFolderPath(folder);
        //    DbPath = System.IO.Path.Join(path, "urec.db");
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");
    }
}
