﻿using FireApi.Database.Entity;
using FireApi.Database.Entity.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireApi.Database
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {                
        }
        
        public DbSet<Device> Device { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Firm> Firm { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<DeviceDoc> DeviceDocs { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>().ToTable("Devices");
            modelBuilder.Entity<DeviceDoc>().ToTable("DeviceDocs");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Client>().ToTable("Clients");
            modelBuilder.Entity<Firm>().ToTable("Firms");
            modelBuilder.Entity<Address>().ToTable("Address");

        }

    }
}
