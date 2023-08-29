using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        MVC_ProjectEntity entity = new MVC_ProjectEntity();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BookingReport()
        {
            return View(entity.Eventbookings.ToList().Where(p => p.Status == "Booked"));
        }
        public ActionResult FinishedReport()
        {
            return View(entity.Eventbookings.ToList().Where(p => p.Status == "Finished"));
        }
        public ActionResult HallDetails()
        {
            return View(entity.HallServices.ToList());
        }
        public ActionResult Search(string searching)
        {
            return View(entity.Eventbookings.Where(p =>p.Status =="Booked" && p.EventDate.Contains(searching)).ToList());
        }
        public ActionResult ManagerList()
        {
            return View(entity.Registrations.ToList().Where(p => p.Role == "Manager"&& p.Status=="A"));
        }
        public ActionResult Accept(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration reg_obj = entity.Registrations.Find(id);
            if (reg_obj == null)
            {
                return HttpNotFound();
            }
            return View(reg_obj);
        }
        [HttpPost]
        public ActionResult Accept(int id, Registration reg_obj)
        {
            Registration reg = entity.Registrations.Find(id);
            reg_obj.Status = "Accepted";
            entity.Registrations.Add(reg);
            entity.Entry(reg).State = EntityState.Modified;
            entity.SaveChanges();
            ModelState.Clear();

            return RedirectToAction("ManagerList");
            
        }
     
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration reg_obj = entity.Registrations.Find(id);
            if (reg_obj == null)
            {
                return HttpNotFound();
            }
            return View(reg_obj);

        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            try
            {
                Registration reg_obj = entity.Registrations.Find(id);
            
                entity.Registrations .Remove(reg_obj);
                entity.SaveChanges();
                return RedirectToAction("BookingReport");
            }
            catch
            {
                return View();
            }
        }
      

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}