﻿using CargoRate.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CargoRate.Data
{
    public class AppDbContext :IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options ) : base(options)
        {

        }

        public DbSet<Cargo> Cargos { get; set; }
    }
}
