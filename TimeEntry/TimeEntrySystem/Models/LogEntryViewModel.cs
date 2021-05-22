using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimeEntrySystem.Models
{
    public class LogEntryViewModel
    {
        public List<SelectListItem> Employees { get; set; }
        public List<SelectListItem> Projects { get; set; }
        [Required]
        public int Employee { get; set; }
        [Required]
        public int Project { get; set; }
        [Required]
        [Range(1, 40)]
        public int Hours { get; set; }
    }
}