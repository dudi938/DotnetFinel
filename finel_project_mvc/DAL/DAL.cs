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
            using (TasksDBEntities2 db = new TasksDBEntities2())
            {
                db.Workers.Add(newWorker);
                db.SaveChanges();
            }
        }

        public static void AddTask(Task newTask)
        {
            using (TasksDBEntities2 db = new TasksDBEntities2())
            {
                db.Tasks.Add(newTask);
                db.SaveChanges();
            }
        }

        public static void EditWorker(Worker worker)
        {
            using (TasksDBEntities2 db = new TasksDBEntities2())
            {
                db.Entry(worker).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static void EditTask(Task task)
        {
            using (TasksDBEntities2 db = new TasksDBEntities2())
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static void RemoveWorker(int? id)
        {
            using (TasksDBEntities2 db = new TasksDBEntities2())
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
            using (TasksDBEntities2 db = new TasksDBEntities2())
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
            using (TasksDBEntities2 db = new TasksDBEntities2())
            {
                return db.Workers.ToList();
            }
        }

        public static List<Task> GetAllTasks()
        {
            using (TasksDBEntities2 db = new TasksDBEntities2())
            {
                List<Task> Tasks = db.Tasks.Include(t => t.Worker).ToList();
                return Tasks;
            }
        }

        public static Worker GetWorkerByID(int? id)
        {
            using (TasksDBEntities2 db = new TasksDBEntities2())
            {
                Worker w = db.Workers.Find(id);
                return w;
            }

        }

        public static Task GetTaskByID(int? id)
        {
            using (TasksDBEntities2 db = new TasksDBEntities2())
            {
                Task task = db.Tasks.Include(t => t.Worker).Where(t => t.taskID == id).FirstOrDefault();
                return task;
            }
        }


        public static List<Task> GetTasksOfWorker(int? WorkerByID)
        {
            using (TasksDBEntities2 db = new TasksDBEntities2())
            {
                List<Task> TasksList = db.Tasks.Where(t => t.workerID == WorkerByID).Include(t => t.Worker).ToList();
                return TasksList;
            }
        }


        public static List<Message> GetMessagesWorker(int? WorkerByID)
        {
            using (TasksDBEntities2 db = new TasksDBEntities2())
            {
                List<Message> Messages = db.Messages.Include(m => m.Worker).Where(M => M.workerID == WorkerByID).ToList();
                return Messages;
            }
        }

        public static void RemoveMessagesOfWorker(int? WorkerByID)
        {
            using (TasksDBEntities2 db = new TasksDBEntities2())
            {
                List<Message> Messages = db.Messages.Where(M => M.workerID == WorkerByID).ToList();
                db.Messages.RemoveRange(Messages);
                db.SaveChanges();
            }
        }


        public static void AddMessage(Message message)
        {
            using (TasksDBEntities2 db = new TasksDBEntities2())
            {               
                db.Messages.Add(message);
                db.SaveChanges();    
            }
        }

        public static void AddMessageToManager(Message message)
        {
            using (TasksDBEntities2 db = new TasksDBEntities2())
            {
                int ManagerId = db.Workers.Where(w => w.isManager == 1).FirstOrDefault().workerID;

                message.workerID = ManagerId;
                db.Messages.Add(message);
                db.SaveChanges();
            }
        }

        public static List<Message> GetWorkerMessages(int id)
        {
            using (TasksDBEntities2 db = new TasksDBEntities2())
            {
                return db.Messages.Include(m => m.Worker).Where(m => m.workerID == id).ToList();
            }
        }

        public static Message GetMessagesByID(int? id)
        {
            using (TasksDBEntities2 db = new TasksDBEntities2())
            {
                return db.Messages.Include(m => m.Worker).Where(m => m.messageID == id).FirstOrDefault();
            }
        }

        public static void EditMessages(Message message)
        {
            using (TasksDBEntities2 db = new TasksDBEntities2())
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static List<Message> GetManagerMessages()
        {
            using (TasksDBEntities2 db = new TasksDBEntities2())
            {
                return db.Messages.Include(m => m.Worker).Where(m => m.Worker.isManager == 1).ToList();
            }
        }
    }
}