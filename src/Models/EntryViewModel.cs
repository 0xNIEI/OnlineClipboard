using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OnlineClipboard.Models
{
    public class EntryViewModel
    {
        public int id { get; set; }
        [DisplayName("Content")]
        [Required]
        public string content { get; set; }
        [DisplayName("Custom ID")]
        [MaxLength(16)]
        [MinLength(1)]
        public string? custom_id { get; set; }
        [DisplayName("Only viewable once")]
        public bool onlyViewableOnce { get; set; }
        public string? validationError { get; set; }
        public double expiresIn { get; set; }
        public EntryViewModel(Entry entry)
        {
            id = entry.id;
            content = Base64Decode(entry.content);
            custom_id = entry.custom_id;
            onlyViewableOnce = entry.onlyViewableOnce;
        }

        public EntryViewModel()
        {
            content = "";
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
