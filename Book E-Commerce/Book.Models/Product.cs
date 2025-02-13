using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Models
{
    public class Product
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [MaxLength(20)]
        public string ISBN { get; set; }

        [Required]
        [MaxLength(15)]
        public string Author { get; set; }

        [Required]
        [Display(Name ="Least Price")]
        [Range(1,1000)]
        public double LeastPrice { get; set; }

        [Required]
        [Display(Name ="Price for 1-50")]
        [Range(1,1000)]
        public double Price { get; set; }

        [Required]
        [Display(Name ="Price for 50+")]
        [Range(1,1000)]
        public double Price50 { get; set; }

        [Required]
        [Display(Name ="Price for 100+")]
        [Range(1,1000)]
        public double Price100 { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public  Category Category { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }


    }
}
