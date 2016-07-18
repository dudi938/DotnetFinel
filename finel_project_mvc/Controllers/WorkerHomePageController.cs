using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using finel_project_mvc.Models;


namespace finel_project_mvc.Controllers
{
    public class WorkerHomePageController : Controller
    {
        private TasksDBEntities1 db = new TasksDBEntities1();


        // GET: WorkerHomePage
        public ActionResult Index(int id)
        {
            var Worker = db.Workers.Where(w => w.workerID.Equals(id)).FirstOrDefault();
            var tasks = db.Tasks.Include(t => t.Worker).Where(w => w.workerID == Worker.workerID);

            var NonAcceptedTasksCount = (from task in tasks
                                        where task.accept != 0x01
                                        select task).ToList().Count;


            ViewBag.WorkerId = Worker.workerID;
            ViewBag.WorkerFirstName = Worker.firstName;
            ViewBag.WorkerLastName = Worker.lastName;
            ViewBag.NonAcceptedTasks = NonAcceptedTasksCount;

            return View(tasks.ToList());
        }

        // GET: WorkerHomePage/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }

            ViewBag.workerId = task.workerID;

            return View(task);
        }

        // GET: WorkerHomePage/Create
        public ActionResult Create(int id)
        {
            var Worker = db.Workers.Where(w => w.workerID.Equals(id)).FirstOrDefault();

            ViewBag.WorkerId = Worker.workerID;
            ViewBag.WorkerFirstName = Worker.firstName;
            ViewBag.WorkerLastName = Worker.lastName;
            return View();
        }

        // POST: WorkerHomePage/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "taskID,workerID,status,priority,dateCreated,acceptDate,taskDescription,managerID,endDate,taskRevision,numOfHowers,accept")] Task task)
        {
            if (ModelState.IsValid)
            {
                    task.workerID = Convert.ToInt32(Request.Form["workerId"]);
                    db.Tasks.Add(task);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = task.workerID });                            
            }

            ViewBag.workerID = new SelectList(db.Workers, "workerID", "firstName", task.workerID);
            return View(task);
        }

        // GET: WorkerHomePage/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.workerID = new SelectList(db.Workers, "workerID", "firstName", task.workerID);
            return View(task);
        }

        // POST: WorkerHomePage/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "taskID,workerID,status,priority,dateCreated,acceptDate,taskDescription,managerID,endDate,taskRevision,numOfHowers,accept")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = task.workerID });
            }
            ViewBag.workerID = new SelectList(db.Workers, "workerID", "firstName", new { id = task.workerID });
            return View(task);
        }

        // GET: WorkerHomePage/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: WorkerHomePage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = task.workerID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult Accept(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = db.Tasks.Find(id);

            if(task != null)
            {
                task.accept = 1;
                task.acceptDate = DateTime.Now.Date;
            }

            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", new { id = task.workerID });
        }
    }
}
