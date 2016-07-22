using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace finel_project_mvc
{
    static public class ClassMail
    {



        static public bool SendPasswordOfNewWorker(string firstName,string mail,string password)
        {
            string sub  = string.Format("Generate new password wizard");
            string bode = string.Format("Hi {0}, Wellcome to my company. your password to TTM task portal is: {1}. please log in with your email {2} and password and change to your password. Good luck.", firstName, password, mail);

            return SendMessage(sub, bode, mail);

        }




        static public bool SendMessage(string Sub, string Body,string SendTo)
        {
            var fromAddress = new MailAddress("dotnet4918@gmail.com", "TTM - System message");
            var toAddress = new MailAddress(SendTo);
            const string fromPassword = "49184918dot";
            string subject = Sub;
            string body = Body;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                try
                {
                    smtp.Send(message);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }                       
    }
}

