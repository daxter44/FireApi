﻿using FireApi.Entity;
using FireApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {                
        }
        
        public DbSet<Device> DeviceItems { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
