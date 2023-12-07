using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Examenes.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Examenes.Data
{
    public class YaPedidosContext : IdentityDbContext
    {
        public YaPedidosContext (DbContextOptions<YaPedidosContext> options)
            : base(options)
        {
        }

        public DbSet<Examenes.Models.Client> Client { get; set; } = default!;

        public DbSet<Examenes.Models.Address> Address { get; set; } = default!;

        public DbSet<Examenes.Models.Product> Product { get; set; } = default!;

        public DbSet<Examenes.Models.Order> Order { get; set; } = default!;
        
       protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración de relaciones uno a uno (1:1) entre Client y Address
        modelBuilder.Entity<Client>()
            .HasOne(c => c.Address)
            .WithOne(a => a.Client)
            .HasForeignKey<Address>(a => a.ClientId);

        // Configuración de relaciones uno a muchos (1:N) entre Client y Order
        modelBuilder.Entity<Client>()
            .HasMany(c => c.Orders)
            .WithOne(o => o.Client)
            .HasForeignKey(o => o.ClientId);

        // Configuración de relaciones muchos a uno (N:1) entre Order y Address (ShippingAddress)
        modelBuilder.Entity<Order>()
            .HasOne(o => o.ShippingAddress)
            .WithMany(a => a.Orders)
            .HasForeignKey(o => o.AddressId);

        // Configuración de relaciones muchos a muchos (N:N) entre Order y Product
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Products)
            .WithMany(p => p.Orders)
            .UsingEntity(j => j.ToTable("OrderProduct"));

        base.OnModelCreating(modelBuilder);
    //En realidad esto ya está arriba, creo que ahi está el problema.
    //     // Configuración de relaciones uno a uno (1:1) entre Address y Client
    //     modelBuilder.Entity<Address>()
    //         .HasOne(a => a.Client)
    //         .WithOne(c => c.Address)
    //         .HasForeignKey<Address>(a => a.ClientId);

    //     // Configuración de relaciones uno a muchos (1:N) entre Address y Order
    //     modelBuilder.Entity<Address>()
    //         .HasMany(a => a.Orders)
    //         .WithOne(o => o.ShippingAddress)
    //         .HasForeignKey(o => o.AddressId);
        }
    }
}
