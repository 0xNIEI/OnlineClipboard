using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OnlineClipboard.Models
{
    [Index(nameof(custom_id), IsUnique = true)]
    public class Entry
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string content { get; set; }
        [MaxLength(16)]
        [MinLength(1)]
        public string? custom_id { get; set; }

        public bool onlyViewableOnce { get; set; }

        [Required]
        public DateTime? created_at { get; set; }
        public DateTime? expires_at { get; set; }
    }
}
