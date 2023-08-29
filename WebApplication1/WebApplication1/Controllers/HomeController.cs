using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        MVC_ProjectEntity entity = new MVC_ProjectEntity();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult HallDetails()
        {
            return View(entity.HallServices.ToList());
        }
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(Registration Reg_obj)
        {
            if (entity .Registrations .Any(x=>x.Username ==Reg_obj.Username ))
            {
                ViewBag.Notification = "This account already exists";
                ModelState.Clear();
                return View();
            }
            else
            {
                Reg_obj.Status = "A";
                entity.Registrations.Add(Reg_obj);
                //try
                //{
                //    entity.SaveChanges();
                //}
                //catch (DbEntityValidationException ex)
                //{
                //}
                entity.SaveChanges();
                TempData["RegistrationMessage"] = "Registered Successfully!!";
                ModelState.Clear();
                return View();
            }
            
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Registration reg_obj,TblAdmin adm_obj)
        {
            var UserLogin = entity.Registrations.Where(x => x.Role == "User" && x.Username == reg_obj.Username && x.Password == reg_obj.Password).FirstOrDefault();
            var ManagerLogin = entity.Registrations.Where(x => x.Role == "Manager" && x.Username == reg_obj.Username && x.Password == reg_obj.Password).FirstOrDefault();
            var AdminLogin = entity.TblAdmins.Where(x => x.Username == adm_obj.Username && x.Password == adm_obj.Password).FirstOrDefault();
            if(UserLogin!=null)
            {
                Session["IdS"] = UserLogin.RegId.ToString();
                Session["UsernameS"] = UserLogin.Username.ToString();
                Session["PasswordS"] = UserLogin.Password.ToString();
                Session["FirstnameS"] = UserLogin.Firstname.ToString();
                return RedirectToAction("Details", "User");
            }
            else if(ManagerLogin!=null )
            {
                if (ManagerLogin .Status=="Accepted")
                {
                    Session["IdM"] = ManagerLogin.RegId.ToString();
                    Session["RoleM"] = ManagerLogin.Role.ToString();
                    Session["UsernameM"] = ManagerLogin.Username.ToString();
                    Session["PasswordM"] = ManagerLogin.Password.ToString();
                    Session["FirstnameM"] = ManagerLogin.Firstname.ToString();
                    return RedirectToAction("HallDetails", "Manager");
                }
                else
                {
                    ViewBag.Notification = "Wait For Admin Response";
                    return View();
                }
            }
            else if (AdminLogin!=null )
            {
                Session["UsernameA"] = "admin";
                Session["PasswordA"] = "admin";
                return RedirectToAction("BookingReport", "Admin");
            }
            else
            {
               
                ViewBag.Notification = "Wrong Username or Password";
                return View();
            }
        }
    }
}
