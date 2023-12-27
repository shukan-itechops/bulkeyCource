using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BulkeyWebRazor.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Category Name")]
        [MaxLength(30, ErrorMessage = "Maximum Length is 30 charactar")]
        public string name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display No must be between 1-100")]
        public int displayOder { get; set; }
    }
}
