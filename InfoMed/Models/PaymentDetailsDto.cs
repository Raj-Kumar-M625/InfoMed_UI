using System.ComponentModel.DataAnnotations.Schema;

namespace InfoMed.Models
{
    public class PaymentDetailsDto
    {
        public int IdPaymentArea { get; set; }
        public int? IdEvent { get; set; }
        public int IdEventVersion { get; set; }
        public string PaymentTextContent { get; set; }
        public string? QRCodeImage { get; set; }
        [NotMapped]
        public IFormFile? PaymentMap { get; set; }
    }
}
