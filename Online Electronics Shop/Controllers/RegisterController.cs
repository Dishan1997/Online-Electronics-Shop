
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

using Online_Electronics_Shop.DB;


namespace Online_Electronics_Shop.Controllers
{
    public class RegisterController : Controller
    {
        dbOnlineElectronicsShopEntities db = new dbOnlineElectronicsShopEntities();


        public ActionResult Index()
        {
            return View();
        }


        public JsonResult SaveData(SiteUser model)
        {

            string output = IsEmailAvailable(model.Email) ? "Not in use" : "Already in used";

            if (output == "Not in use")
            {
                model.IsValid = false;
                db.SiteUsers.Add(model);
                db.SaveChanges();
                BuildEmailTemplate(model.ID);
                return Json("Registration Successfull", JsonRequestBehavior.AllowGet);
            }

            else
            {


                return Json("This Email Id is already used", JsonRequestBehavior.AllowGet);
            }



        }


        public ActionResult Confirm(int regId)
        {
            ViewBag.regID = regId;
            return View();
        }


        public JsonResult RegisterConfirm(int regId)
        {
            SiteUser Data = db.SiteUsers.Where(x => x.ID == regId).FirstOrDefault();
            Data.IsValid = true;
            db.SaveChanges();
            var msg = "Your Email Is Verified!";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public void BuildEmailTemplate(int regID)
        {
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "Text" + ".cshtml");
            var regInfo = db.SiteUsers.Where(x => x.ID == regID).FirstOrDefault();
            var url = "http://localhost:62596/" + "Register/Confirm?regId=" + regID;
            body = body.Replace("ViewBag.ConfirmationLink", url);
            body = body.ToString();
            BuildEmailTemplate("Your Account Is Successfully Created", body, regInfo.Email);
        }


        public static void BuildEmailTemplate(string subjectText, string bodyText, string sendTo)
        {
            string from, to, bcc, cc, subject, body;
            from = "arlibrary64@gmail.com";
            to = sendTo.Trim();
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();
            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }
            if (!string.IsNullOrEmpty(cc))
            {
                mail.CC.Add(new MailAddress(cc));
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SendEmail(mail);
        }

        public static void SendEmail(MailMessage mail)
        {
            SmtpClient client = new SmtpClient();

            client.Host = "smtp.gmail.com";
            client.Timeout = 10000;
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("arlibrary64@gmail.com", "adminarl");
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult CheckValidUser(SiteUser model)
        {
            string result = "Fail";
            var DataItem = db.SiteUsers.Where(x => x.Email == model.Email && x.Password == model.Password).SingleOrDefault();
            if (DataItem != null)
            {
                Session["UserID"] = DataItem.ID.ToString();
                Session["UserName"] = DataItem.Username.ToString();
                result = "Success";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsEmailAvailable1(string Email)
        {
            SiteUser Data = db.SiteUsers.Where(x => x.Email == Email).FirstOrDefault();
            var msg = "Already in use";
            // string result = "UsedEmailId";
            // var DataItem = db.SiteUsers.Where(user=>user.Email==model.Email).SingleOrDefault();
            if (Data != null)
            {
                //result = "NotUsedEmailId";
                msg = "Not in use";

            }

            return Json(msg, JsonRequestBehavior.AllowGet);

        }
        public bool IsEmailAvailable(string Email)
        {
            return !db.SiteUsers.Any(user => user.Email == Email);

            // return Json(!db.SiteUsers.Any(user=>user.Email==Email),JsonRequestBehavior.AllowGet);

        }





        public ActionResult AfterLogin()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {

                return RedirectToAction("Index");
            }
        }






        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index");
            // return View("~/Views/Home/Index");
        }







    }
}