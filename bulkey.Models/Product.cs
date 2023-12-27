using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace bulkey.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author {  get; set; }
        [Required]
        [DisplayName("List Price")]
        [Range(1,1000,ErrorMessage ="Price Must be in Between 1-1000")]
        public double ListPrice { get; set; }
        [Required]
        [DisplayName("Price for 1-50")]
        [Range(1, 50,ErrorMessage = "Price is Must be In Between 1-50")]
        public double Price { get; set; }
        [Required]
        [DisplayName("Price for 50+")]
        [Range(51, 100,ErrorMessage = "Price is must be in Between 51-100")]
        public double Price50 { get; set; }
        [Required]
        [DisplayName("Price for 100+")]
        [Range(101, 1000,ErrorMessage ="Price is must be in Between 101-1000")]
        public double Price100 { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
    }
}
