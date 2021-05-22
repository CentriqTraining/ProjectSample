using System.Data.Entity;

namespace TimeEntrySystem.Models
{
    internal class DbInit : DropCreateDatabaseIfModelChanges<TimeEntryCtx>
    {
        protected override void Seed(TimeEntryCtx context)
        {
            var Projects = TimeEntryInitializer.GetProjects();
            var Emps = TimeEntryInitializer.GetEmployees();
            foreach (var item in Projects)
            {
                context.Projects.Add(new Project() { ProjectName = item });
            }
            foreach (var item in Emps)
            {
                context.Employees.Add(new Employee()
                {
                    FirstName = item.Item1,
                    LastName = item.Item2
                });
            }
            context.SaveChanges();
            base.Seed(context);

        }
    }
}