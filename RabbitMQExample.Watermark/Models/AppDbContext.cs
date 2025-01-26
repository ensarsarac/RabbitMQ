﻿using Microsoft.EntityFrameworkCore;

namespace RabbitMQExample.Watermark.Models
{
	public class AppDbContext : DbContext
	{
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }

        public DbSet<Product> Products{ get; set; }
    }
}
