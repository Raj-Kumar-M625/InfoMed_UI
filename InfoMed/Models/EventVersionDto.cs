﻿using System.ComponentModel.DataAnnotations.Schema;

namespace InfoMed.Models
{
    public class EventVersionDto
    {
        public int IdEventVersion { get; set; }
        public int IdEvent { get; set; }
        public int IdVersion { get; set; }
        public string VersionStatus { get; set; }
        public string EventWebPageName { get; set; }
        public string EventName { get; set; }
        public string EventType { get; set; }
        public string VenueName { get; set; }
        public string VenueAddress { get; set; }
        public string VenueLatLong { get; set; }
        public string EventHomeContent { get; set; }
        public string? FooterText { get; set; }
        public string? FacebookLink { get; set; }
        public string? TwitterLink { get; set; }
        public string? LinkedInLink { get; set; }
        public string? CopyrightText { get; set; }
        public bool ShowHurryUpContent { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? EventStartDate { get; set; }
        public DateTime? EventEndDate { get; set; }
        public int NoOfDays { get; set; }
        public bool? EventView { get; set; }
        public int? CreatedBy { get; set; }
        public bool? ShowPayNowButton { get; set; } = true;
        public bool? ShowPayAtVenueButton { get; set; } = true;
        public string? EventBackgroundImage { get; set; }
        public string? EventBackgroundColor { get; set; } = "#FF2327";
        public bool? ShowBookTicketButton { get; set; } = true;
        [NotMapped]
        public IFormFile? EventBackgroundLogoMap { get; set; }
    }
}
