using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBOOK.Models
{
    public class Ebook
    {
        [Key]
        public int Id { get; set; }
        [Required]

        public string Title { get; set; }
        public string Auteur { get; set; }
        public string Description { get; set; }
        public float Prix { get; set; }
        public int DisplayOrder { get; set; }

        public string ImageUrl { get; set; }
        [NotMapped]
        public IFormFile BookImage { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
      
    }
}
