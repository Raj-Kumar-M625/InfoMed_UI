namespace InfoMed.Models
{
    public class SweetAlertOptions
    {
        public string? Title { get; set; }
        public string? Text { get; set; }
        public string? Icon { get; set; }
        public string? ShowCancelButton { get; set; }
        public bool? DangerMode { get; set; } = false;
    }
}
