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
    public class WorkerHomePageController : Controller
    {
       // private TasksDBEntities1 db = new TasksDBEntities1();


        // GET: WorkerHomePage
        public ActionResult Index(int id)
        {
            // pack all nedded data
            var Worker = ClassDAL.GetWorkerByID(id);
            var tasks = ClassDAL.GetTasksOfWorker(Worker.workerID);

            var NonAcceptedTasksCount = (from task in tasks
                                        where task.accept != 0x01
                                        select task).ToList().Count;

            var MessageList = ClassDAL.GetWorkerMessages(id);

            ViewBag.Messages = MessageList;
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

            Task task = ClassDAL.GetTaskByID(id);
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
            var Worker = ClassDAL.GetWorkerByID(id);

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
                ClassDAL.AddTask(task);
                return RedirectToAction("Index", new { id = task.workerID });                            
            }

            ViewBag.workerID = new SelectList(ClassDAL.GetAllWorker(), "workerID", "firstName", task.workerID);
            return View(task);
        }

        // GET: WorkerHomePage/Edit/5
        public ActionResult Edit(int? id)
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


            List<SelectListItem> StatusItemsList = new List<SelectListItem>();
            List<SelectListItem> AcceptItemsList = new List<SelectListItem>();

            SelectListItem item1 = new SelectListItem();
            SelectListItem item2 = new SelectListItem();
            SelectListItem item3 = new SelectListItem();
            SelectListItem item4 = new SelectListItem();

            item1.Text  = "wait";
            item1.Value = "wait";

            item2.Text  = "done";
            item2.Value = "done";

            StatusItemsList.Add(item1);
            StatusItemsList.Add(item2);

            item3.Text = "yes";
            item3.Value = "1";

            item4.Text = "no";
            item4.Value = "0";

            AcceptItemsList.Add(item3);
            AcceptItemsList.Add(item4);


            
            ViewBag.StatusItemsList = StatusItemsList;
            ViewBag.AcceptItemsList = AcceptItemsList;

            ViewBag.workerID = new SelectList(ClassDAL.GetAllWorker(), "workerID", "firstName", task.workerID);
            return View(task);
        }

        // POST: WorkerHomePage/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]                                                 
        public ActionResult Edit([Bind(Include = "workerID,taskID,status,taskDescription,accept")] Task task)
        {
            Task t = ClassDAL.GetTaskByID(task.taskID); 
            task.workerID = t.workerID;

            if (ModelState.IsValid)
            {
                ClassDAL.EditTask(task);
                return RedirectToAction("Index", new { id = task.workerID });
            }
            ViewBag.workerID = new SelectList(ClassDAL .GetAllWorker(), "workerID", "firstName", new { id = task.workerID });
            return View(task);
        }

        // GET: WorkerHomePage/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: WorkerHomePage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            int workerId = ClassDAL.GetTaskByID(id).workerID;

            ClassDAL.RemoveTask(id);
            return RedirectToAction("Index", new { id = workerId });
        }



        public ActionResult Accept(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = ClassDAL.GetTaskByID(id);

            if(task != null)
            {
                task.accept = 1;
                task.acceptDate = DateTime.Now.Date;
            }

            ClassDAL.EditTask(task);

            return RedirectToAction("Index", new { id = task.workerID });
        }

        public ActionResult MarkMessageAsRead(int? id)
        {
            worker_inbox message = ClassDAL.GetMessagesByID(id);

           if (message != null)
           {
               message.NonRead = false;
           }

            ClassDAL.EditMessages(message);


           return RedirectToAction("Index", new { id = message.WorkerID });
        }


        // GET: Wworker change password
        public ActionResult CreateNewPassword(int id)
        {
            var Worker = ClassDAL.GetWorkerByID(id);

            ViewBag.WorkerId = Worker.workerID;
            ViewBag.WorkerFirstName = Worker.firstName;
            ViewBag.WorkerLastName = Worker.lastName;
            return View(Worker);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewPassword([Bind(Include = "workerID,firstName,lastName,job,phone,isManager,password,email")] Worker worker)
        {
            if (ModelState.IsValid)
            {
                ClassDAL.EditWorker(worker);
                return RedirectToAction("Index", new { id = worker.workerID });
            }
            return View();
        }

        public ActionResult ShowAllMessages(int? id)
        {
            var MessagesList = ClassDAL.GetMessagesWorker(id);

            ViewBag.workerID = id;
            return View(MessagesList.ToList());
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
