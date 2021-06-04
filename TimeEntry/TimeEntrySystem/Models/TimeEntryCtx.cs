using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TimeEntrySystem.Models
{
    public class TimeEntryCtx : DbContext
    {
        public static string DBConnectionString = "";
        public TimeEntryCtx() :base(DBConnectionString)
        {
            Database.SetInitializer<TimeEntryCtx>(new DbInit());
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<LogEntry> EntryLogs { get; set; }
    }
}