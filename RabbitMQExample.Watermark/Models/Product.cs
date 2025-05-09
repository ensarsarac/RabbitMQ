﻿using System.ComponentModel.DataAnnotations;

namespace RabbitMQExample.Watermark.Models
{
	public class Product
	{
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? ImageUrl { get; set; }
    }
}
