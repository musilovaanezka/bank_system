﻿using Microsoft.EntityFrameworkCore;
using stin.Models;

namespace stin.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Ucet> Ucty { get; set; }

        public DbSet<Klient> Klienti { get; set; }
    }
}
