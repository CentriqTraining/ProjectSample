using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TimeEntrySystem.Models
{
    public class LogEntry
    {
        public int ID { get; set; }
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime CreatedDate { get; set; }
        public int Hours { get; set; }
        public Project Project { get; set; }
        public Employee Employee { get; set; }
    }
}