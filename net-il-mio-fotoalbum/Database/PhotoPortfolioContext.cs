﻿using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Database
{
    public class PhotoPortfolioContext : DbContext
    {
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=PhotoPortfolioDb;Integrated Security=True;TrustServerCertificate=True");
        }
    }
}
