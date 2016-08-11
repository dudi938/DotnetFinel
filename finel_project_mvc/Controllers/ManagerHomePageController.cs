using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using finel_project_mvc.Models;
using finel_project_mvc.DAL;



namespace finel_project_mvc.Controllers
{
    public class ManagerHomePageController : Controller
    {
        //private TasksDBEntities1 db = new TasksDBEntities1();

        // GET: ManagerHomePage
        public ActionResult Index()
        {
            List<Task> NonAcceptedTasks = ClassDAL.GetAllTasks();
            ViewBag.NonAcceptedTasks = NonAcceptedTasks;

            List<Worker> Workers = ClassDAL.GetAllWorker();
            return View(Workers);


        }



        // GET: ManagerHomePage/Create
        public ActionResult CreateNewTask(int? id)
        {
            Worker worker = ClassDAL.GetWorkerByID(id);
            //Task task = new Task();
            if (worker != null)
            {
                ViewBag.firstName = worker.firstName;
                ViewBag.lastName = worker.lastName;
                ViewBag.workerID = worker.workerID;
            }
            return View();
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
                task.status = "Wait";
                task.accept = 0;
                task.taskRevision = "1";
                ClassDAL.AddTask(task);
            }
            return RedirectToAction("Index");
        }




        // GET: Tasks/Delete/5
        public ActionResult DeleteTask(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = ClassDAL.GetTaskByID(id);

            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("DeleteTask")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Task task = db.Tasks.Find(id);
            //db.Tasks.Remove(task);
            //db.SaveChanges();

            ClassDAL.RemoveTask(id);

            return RedirectToAction("Index");
        }


        // GET: Tasks/Details/5
        public ActionResult TaskDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = ClassDAL.GetTaskByID(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // GET: Tasks/Edit/5
        public ActionResult EditTask(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Task task = ClassDAL.GetTaskByID(id);
            if (task == null)
            {
                return HttpNotFound();
            }


            Worker worker = ClassDAL.GetWorkerByID(task.workerID);

            if (worker != null)
            {
                ViewBag.FirstName = worker.firstName;
                ViewBag.LastName = worker.lastName;
            }

            List<Worker> Workers = ClassDAL.GetAllWorker();
            List<SelectListItem> workersItemsList = new List<SelectListItem>();



            List<SelectListItem> StatusItemsList = new List<SelectListItem>();

            SelectListItem item1 = new SelectListItem();
            SelectListItem item2 = new SelectListItem();

            item1.Text = "wait";
            item1.Value = "wait";

            item2.Text = "done";
            item2.Value = "done";

            StatusItemsList.Add(item1);
            StatusItemsList.Add(item2);



            ViewBag.StatusItemsList = StatusItemsList;



            foreach (var w in Workers)
            {
                SelectListItem item = new SelectListItem();
                item.Text = w.firstName + " " + w.lastName;
                item.Value = w.workerID.ToString();
                workersItemsList.Add(item);
            }
            ViewBag.WorkersItemsList = workersItemsList;


            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTask([Bind(Include = "taskID,workerID,status,priority,dateCreated,acceptDate,taskDescription,managerID,endDate,taskRevision,numOfHowers,accept")] Task task)
        {
            if (ModelState.IsValid)
            {
                Task UpdatedTask = ClassDAL.GetTaskByID(task.taskID);
                UpdatedTask.Worker = ClassDAL.GetWorkerByID(task.workerID);
                UpdatedTask.workerID = task.workerID;
                UpdatedTask.accept = 0;
                UpdatedTask.priority = task.priority;
                UpdatedTask.taskDescription = task.taskDescription;
                UpdatedTask.endDate = task.endDate;
                UpdatedTask.numOfHowers = task.numOfHowers;

                ClassDAL.EditTask(UpdatedTask);

                //db.Entry(task).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<Worker> workers = ClassDAL.GetAllWorker();
            ViewBag.workerID = new SelectList(workers, "workerID", "firstName", task.workerID);
            return View(task);
        }




        public ActionResult TasksOfWorker(int? id)
        {
            ViewBag.workerID = id;

            Worker worker = ClassDAL.GetWorkerByID(id);
            ViewBag.WorkerName = worker.firstName + " " + worker.lastName;

            var tasks = ClassDAL.GetTasksOfWorker(id);
            return View(tasks);
        }


        public ActionResult ShowNonAcceptedTasks()
        {
            List<Task> AllTasks = ClassDAL.GetAllTasks();
            List<Task> tasks = AllTasks.Where(t => t.accept != 1).ToList();

            return View(tasks);
        }


        public ActionResult SendTaskReminde(int? id)
        {
            Message Message = new Message();
            Task task = ClassDAL.GetTaskByID(id);
            Worker worker = ClassDAL.GetWorkerByID(task.workerID);

            if (task != null)
            {
                Message.workerID = task.workerID;
                Message.message1 = string.Format("Hi {0}, look on task ID {1}. if you have any question or problem ask me. thanks", worker.firstName, id);
                Message.nonRead = true;
                ClassDAL.AddMessage(Message);
            }
            return RedirectToAction("ShowNonAcceptedTasks");
        }


        public ActionResult HandeleTaskBeforDeleteWorker(int? id)
        {
            var tasksOfWorker = ClassDAL.GetTasksOfWorker(id);

            return View(tasksOfWorker);
        }








        // GET: ManagerHomePage/Details/5
        public ActionResult WorkerDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Worker worker = ClassDAL.GetWorkerByID(id);
            if (worker == null)
            {
                return HttpNotFound();
            }
            return View(worker);
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

                ClassDAL.AddWorker(worker);

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

            Worker worker = ClassDAL.GetWorkerByID(id);
 
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
        public ActionResult EditWorker([Bind(Include = "workerID,firstName,lastName,job,email,password,isManager,phone")] Worker worker)
        {
            if (ModelState.IsValid)
            {

                ClassDAL.EditWorker(worker);
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

            Worker worker = ClassDAL.GetWorkerByID(id);
            if (worker == null)
            {
                return HttpNotFound();
            }

            List<Task> tasks = ClassDAL.GetTasksOfWorker(id);


            //var tasks = db.Tasks.Where(t => t.workerID == id).ToList();
            if (tasks.Count > 0)
            {
                return View("HandeleTaskBeforDeleteWorker", tasks);
            }
            else return View(worker);
        }

        // POST: ManagerHomePage/Delete/5
        [HttpPost, ActionName("DeleteWorker")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteWorkerConfirmed(int id)
        {

            Worker worker = ClassDAL.GetWorkerByID(id);

            ClassDAL.RemoveMessagesOfWorker(id);

            ClassDAL.RemoveWorker(id);

            return RedirectToAction("Index");
        }

        
        public ActionResult Inbox()
        {
            List<Message> messages = ClassDAL.GetManagerMessages();
            return View(messages);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
