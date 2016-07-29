using finel_project_mvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace finel_project_mvc.DAL
{
    public static class ClassDAL
    {
        public static void AddWorker(Worker newWorker)
        {
            using (TasksDBEntities1 db = new TasksDBEntities1())
            {
                db.Workers.Add(newWorker);
                db.SaveChanges();
            }
        }

        public static void AddTask(Task newTask)
        {
            using (TasksDBEntities1 db = new TasksDBEntities1())
            {
                db.Tasks.Add(newTask);
                db.SaveChanges();
            }
        }


        public static void EditWorker(Worker worker)
        {
            using (TasksDBEntities1 db = new TasksDBEntities1())
            {
                db.Entry(worker).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static void EditTask(Task task)
        {
            using (TasksDBEntities1 db = new TasksDBEntities1())
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static void RemoveWorker(int? id)
        {
            using (TasksDBEntities1 db = new TasksDBEntities1())
            {
                Worker w = db.Workers.Find(id);

                if (w != null)
                {
                    db.Workers.Remove(w);
                    db.SaveChanges();
                }
            }

        }

        public static void RemoveTask(int ?id)
        {
            using (TasksDBEntities1 db = new TasksDBEntities1())
            {
                Task t = db.Tasks.Find(id);

                if (t != null)
                {
                    db.Tasks.Remove(t);
                    db.SaveChanges();
                }
            }

        }


        public static List<Worker> GetAllWorker()
        {
            using (TasksDBEntities1 db = new TasksDBEntities1())
            {
                return db.Workers.ToList();
            }
        }

        public static List<Task> GetAllTasks()
        {
            using (TasksDBEntities1 db = new TasksDBEntities1())
            {
                List<Task> Tasks = db.Tasks.Include(t => t.Worker).ToList();
                return Tasks;
            }
        }

        public static Worker GetWorkerByID(int? id)
        {
            using (TasksDBEntities1 db = new TasksDBEntities1())
            {
                Worker w = db.Workers.Find(id);
                return w;
            }

        }

        public static Task GetTaskByID(int? id)
        {
            using (TasksDBEntities1 db = new TasksDBEntities1())
            {
                Task task = db.Tasks.Include(t => t.Worker).Where(t => t.taskID == id).FirstOrDefault();
                return task;
            }
        }


        public static List<Task> GetTasksOfWorker(int? WorkerByID)
        {
            using (TasksDBEntities1 db = new TasksDBEntities1())
            {
                List<Task> TasksList = db.Tasks.Where(t => t.workerID == WorkerByID).Include(t => t.Worker).ToList();
                return TasksList;
            }
        }


        public static List<worker_inbox> GetMessagesWorker(int? WorkerByID)
        {
            using (TasksDBEntities1 db = new TasksDBEntities1())
            {
                List<worker_inbox> Messages = db.worker_inbox.Where(M => M.WorkerID == WorkerByID).ToList();
                return Messages;
            }
        }

        public static void RemoveMessagesOfWorker(int? WorkerByID)
        {
            using (TasksDBEntities1 db = new TasksDBEntities1())
            {
                List<worker_inbox> Messages = db.worker_inbox.Where(M => M.WorkerID == WorkerByID).ToList();
                db.worker_inbox.RemoveRange(Messages);
                db.SaveChanges();
            }
        }


        public static void AddMessage(worker_inbox message)
        {
            using (TasksDBEntities1 db = new TasksDBEntities1())
            {
                db.worker_inbox.Add(message);
                db.SaveChanges();    
            }
        }

        public static List<worker_inbox> GetWorkerMessages(int id)
        {
            using (TasksDBEntities1 db = new TasksDBEntities1())
            {
                return db.worker_inbox.Include(m => m.Worker).Where(m => m.WorkerID == id).ToList();
            }
        }

        public static worker_inbox GetMessagesByID(int? id)
        {
            using (TasksDBEntities1 db = new TasksDBEntities1())
            {
                return db.worker_inbox.Include(m => m.Worker).Where(m => m.MessageID == id).FirstOrDefault();
            }
        }

        public static void EditMessages(worker_inbox message)
        {
            using (TasksDBEntities1 db = new TasksDBEntities1())
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}