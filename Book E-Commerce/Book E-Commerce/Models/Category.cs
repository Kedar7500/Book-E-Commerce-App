﻿using System.ComponentModel.DataAnnotations;

namespace Book_E_Commerce.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public int DisplayOrder { get; set; }

    }
}
