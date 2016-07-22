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
    public class ManagerHomePageController : Controller
    {
        private TasksDBEntities1 db = new TasksDBEntities1();

        // GET: ManagerHomePage
        public ActionResult Index()
        {
            List<Task> NonAcceptedTasks = db.Tasks.Where(w => w.accept != 1).ToList();
            ViewBag.NonAcceptedTasks = NonAcceptedTasks;

            return View(db.Workers.ToList());
        }

        // GET: ManagerHomePage/Details/5
        public ActionResult WorkerDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Worker worker = db.Workers.Find(id);
            if (worker == null)
            {
                return HttpNotFound();
            }
            return View(worker);
        }


        // GET: ManagerHomePage/Create
        public ActionResult CreateNewTask(int? id)
        {
            Worker worker = db.Workers.Find(id);
            Task task = new Task();
            if (worker != null)
            {
                ViewBag.firstName = worker.firstName;
                ViewBag.lastName = worker.lastName;
                ViewBag.workerID = worker.workerID;
            }
            return View(task);
        }

        // POST: ManagerHomePage/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewTask([Bind(Include = "workerID,priority,taskDescription,endDate,numOfHowers")] Task task)
        {
            if (ModelState.IsValid)
            {
                task.dateCreated = DateTime.Now;
                task.priority = "Medume";
                task.status = "Wait";
                task.accept = 0;
                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }



        // GET: ManagerHomePage/Create
        public ActionResult CreateNewWorker()
        {
            return View();
        }

        // POST: ManagerHomePage/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewWorker([Bind(Include = "workerID,firstName,lastName,job,phone,isManager,password,email")] Worker worker)
        {
            if (ModelState.IsValid)
            {
                Random rnd = new Random();
                worker.isManager = 0;
                worker.password = rnd.Next(100000, 999999).ToString();
                db.Workers.Add(worker);
                db.SaveChanges();

                ClassMail.SendPasswordOfNewWorker(worker.firstName, worker.email, worker.password);

                return RedirectToAction("Index");
            }

            return View(worker);
        }

        // GET: ManagerHomePage/Edit/5
        public ActionResult EditWorker(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Worker worker = db.Workers.Find(id);
 
            if (worker == null)
            {
                return HttpNotFound();
            }
            return View(worker);
        }

        // POST: ManagerHomePage/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditWorker([Bind(Include = "workerID,firstName,lastName,job,phone,isManager,password,email")] Worker worker)
        {
            if (ModelState.IsValid)
            {
                db.Entry(worker).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(worker);
        }

        // GET: ManagerHomePage/Delete/5
        public ActionResult DeleteWorker(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Worker worker = db.Workers.Find(id);
            if (worker == null)
            {
                return HttpNotFound();
            }
            return View(worker);
        }

        // POST: ManagerHomePage/Delete/5
        [HttpPost, ActionName("DeleteWorker")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteWorkerConfirmed(int id)
        {

            Worker worker = db.Workers.Find(id);
            db.Workers.Remove(worker);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult ShowNonAcceptedTasks()
        {
            var Tasks = db.Tasks.Where(t => t.accept != 1);
            return View(Tasks);
        }


        public ActionResult SendTaskReminde(int? id)
        {
            worker_inbox Message = new worker_inbox();
            Task task = db.Tasks.Find(id);

            if (task != null)
            {
                Message.WorkerID = task.workerID;
                Message.Message = string.Format("Hi {0}, look on task ID {1}. if you have any question or problem ask me. thanks",db.Workers.Where(w => w.workerID == task.workerID).FirstOrDefault().firstName,id);
                Message.NonRead = true;
                db.worker_inbox.Add(Message);
                db.SaveChanges();
            }
            return RedirectToAction("ShowNonAcceptedTasks");
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
