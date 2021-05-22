using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeEntrySystem.Models;

namespace TimeEntrySystem.Controllers
{
    public class HomeController : Controller
    {
        private TimeEntryCtx db = new TimeEntryCtx();

        // GET: Home
        public ActionResult Index()
        {
            return View(db.EntryLogs.Include("Project").Include("Employee").ToList());
        }

        // GET: Home/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogEntry logEntry = db.EntryLogs.Find(id);
            if (logEntry == null)
            {
                return HttpNotFound();
            }
            return View(logEntry);
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            var model = new LogEntryViewModel()
            {
                Employees = db
                .Employees.Select(emp => new SelectListItem()
                    {
                        Text = emp.LastName + ", " + emp.FirstName,
                        Value = emp.ID.ToString()
                    }).ToList(),
                Projects = db.Projects
                    .Select(proj => new SelectListItem()
                    {
                        Text = proj.ProjectName,
                        Value = proj.ID.ToString()
                    }).ToList()
            };

            return View(model);
        }

        // POST: Home/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CreatedDate,Hours,Project,Employee")] LogEntryViewModel logEntry)
        {
            if (ModelState.IsValid)
            {
                var Employee = db.Employees.FirstOrDefault(emp => emp.ID == logEntry.Employee);
                var Project = db.Projects.FirstOrDefault(proj => proj.ID == logEntry.Project);

                if (Employee == null)
                {
                    ModelState.AddModelError("Employee", "You must choose an employee to submit time");
                    return View(logEntry);
                }
                else if (Project == null)
                {
                    ModelState.AddModelError("Project", "You must choose an project to submit time");
                    return View(logEntry);
                }
                else
                {
                    db.EntryLogs.Add(new LogEntry()
                    {
                        CreatedDate = DateTime.Now,
                        Hours = logEntry.Hours,
                        Project = Project,
                        Employee = Employee
                    });
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(logEntry);
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogEntry logEntry = db.EntryLogs.Find(id);
            if (logEntry == null)
            {
                return HttpNotFound();
            }
            return View(logEntry);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CreatedDate")] LogEntry logEntry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(logEntry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(logEntry);
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogEntry logEntry = db.EntryLogs.Find(id);
            if (logEntry == null)
            {
                return HttpNotFound();
            }
            return View(logEntry);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LogEntry logEntry = db.EntryLogs.Find(id);
            db.EntryLogs.Remove(logEntry);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
