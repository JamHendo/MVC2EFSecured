using System.Web.Mvc;
using MVC2EFSecured.UI.MVC.Models;
using System.Net.Mail; //mail
using System.Net; //mail
using System; // Exception handling

namespace MVC2EFSecured.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel cvm)
        {
            //  Check that the cvm data just passed into the method is a valid ContactViewModel
            if (ModelState.IsValid)
            {
                string message = $"You have recieved an email from {cvm.Name} with a subject of {cvm.Subject}. " +
                    $"Please respond to {cvm.Email} with your response to the following message:<br/>{cvm.Message}";

                MailMessage mm = new MailMessage("no-reply@thehendersoncollective.net", "no-reply@thehendersoncollective.net", cvm.Subject, message);

                //Mail message Properties

                //Allow HTML formatting in the email
                mm.IsBodyHtml = true;

                mm.Priority = MailPriority.High;

                //Respnd to the sender and not the admin @ your site URL.com
                mm.ReplyToList.Add(cvm.Email);
                mm.CC.Add("yourCCperson@yoursite.com");

                //SmtpClient - This is the info from the host that allows this message to be sent
                SmtpClient client = new SmtpClient("mail.yourmaildomain.com");

                client.Credentials = new NetworkCredential("admin@yoursite.com", "Your password"); //Clear text password is not best practice
                //set the port
                client.Port = 8889;
                //try to send the email
                try
                {//attempt to send
                    client.Send(mm);

                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"We're sorry your resquest could not be completed at this time. Please try again later. Error message:<br/> {ex.StackTrace}";
                    return View(cvm);
                }
                


                return View();

            }else // The ModelState is NOT valid
            {                // Send back the form with the completed inputs so the user can try again and not have to re-type everything

                return View(cvm);
            }
        }
            
    }
}
