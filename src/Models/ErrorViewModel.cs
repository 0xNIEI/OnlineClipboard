namespace OnlineClipboard.Models
{
    public class ErrorViewModel
    {
        public bool FromException { get; set; }
        public string? RequestId { get; set; }
        public string? Path { get; set; }
        public System.Exception? Exception { get; set; }
        public bool IsDevelopmentEnvironment { get; set; }
    }
}