﻿using System.ComponentModel.DataAnnotations.Schema;

namespace InfoMed.Models
{
    public class SponserDto
    {
        public int IdEventSponsor { get; set; }
        public int IdEvent { get; set; }
        public int IdEventVersion { get; set; }
        public string SponsorType { get; set; }
        public string SponsorName { get; set; }
        public string? SponsorShowText { get; set; }
        public string? SponsorUrl { get; set; }
        public string SponsorLogo { get; set; }
        public int OrderNumber { get; set; }
        public bool Status { get; set; }   
        
        [NotMapped]
        public IFormFile? SponserLogoMap { get; set; }        
    }
}
