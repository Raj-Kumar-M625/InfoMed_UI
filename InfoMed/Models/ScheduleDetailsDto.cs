using System.ComponentModel.DataAnnotations;

namespace InfoMed.Models
{
    public class ScheduleDetailsDto
    {
        public int IdScheduleDetail { get; set; }
        public int IdScheduleMaster { get; set; }        
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Topic { get; set; }
        public string? TopicName { get; set; }
        public bool IsActive { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan Time { get; set; }

    }
}
