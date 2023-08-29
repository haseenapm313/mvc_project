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
    public class UserController : Controller
    {
        MVC_ProjectEntity entity = new MVC_ProjectEntity();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult EventBooking()
        {
            try
            {
                if (Session["IdS"] != null)
                {
                    var registrationlist = entity.Registrations.ToList();
                    ViewBag.RegId = new SelectList(registrationlist, dataValueField: "RegId", dataTextField: "Firstname");
                    return View();
                }
                else
                {
                    return RedirectToAction("Details");
                }
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult EventBooking(Eventbooking booking)
        {
            try
            {
                var registrationlist = entity.Registrations.ToList();
                ViewBag.RegId = new SelectList(registrationlist, dataValueField: "RegId", dataTextField: "Firstname");
                if (ModelState.IsValid)
                {
                    if (entity.Eventbookings.Any(x => x.EventDate == booking.EventDate))
                    {
                        TempData["EventMessage"] = "Hall not Available!";
                        ModelState.Clear();
                        return View();
                    }
                    else
                    {
                        booking.Status = "Pending";
                        entity.Eventbookings.Add(booking);
                        entity.SaveChanges();
                        TempData["EventMessage"] = "Hall Available !!";
                        ModelState.Clear();
                        return View();
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
       public ActionResult ConfirmBooking()
        {
            int x = int.Parse(Session["IdS"].ToString());
            return View(entity.Eventbookings.ToList().Where(p => p.Status == "Pending" && p.RegId == x));
        }
        //Confirm booking
        public ActionResult Book(int id)
        {
            var registrationlist = entity.Registrations.ToList();
            ViewBag.RegId = new SelectList(registrationlist, dataValueField: "RegId", dataTextField: "Firstname");
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventbooking booking = entity.Eventbookings.Find(id);
            if (booking==null)
            { 
                return HttpNotFound();
            }
            return View(booking);
        }
        [HttpPost]
        public ActionResult Book(int id,Eventbooking booking)
        {

            Eventbooking book = entity.Eventbookings.Find(id);
            //var registrationlist = entity.Registrations.ToList();
            //ViewBag.RegId = new SelectList(registrationlist, dataValueField: "RegId", dataTextField: "Firstname");
            book.Status = "Booked";
            entity.Eventbookings.Add(book);
            entity.Entry(book).State = EntityState.Modified;
            
            entity.SaveChanges();
            ModelState.Clear();
            return RedirectToAction("BookingReport");
        }
      
        public ActionResult BookingReport()
        {
            int x = int.Parse(Session["IdS"].ToString());
            return View (entity .Eventbookings .Where (p=>p.Status=="Booked"&& p.RegId .Equals(x)).ToList());
        }
        //Delete Booking
        public ActionResult Delete(int id)
        {
            var registrationlist = entity.Registrations.ToList();
            ViewBag.RegId = new SelectList(registrationlist, dataValueField: "RegId", dataTextField: "Firstname");
            if(id==null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventbooking booking = entity.Eventbookings.Find(id);
            if(booking==null )
            {
                return HttpNotFound();
            }
            return View(booking);
        }
        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var registrationlist = entity.Registrations.ToList();
            ViewBag.RegId = new SelectList(registrationlist, dataValueField: "RegId", dataTextField: "Firstname");
            try
            {
                Eventbooking booking = entity.Eventbookings.Find(id);
                entity.Eventbookings.Remove(booking);
                entity.SaveChanges();
                return RedirectToAction("EventBooking");
            }
            catch
            {
                return View();
            }
        }
     

        //view profile
        public ActionResult Details(int? x)
        {
            x = int.Parse(Session["IdS"].ToString());
            if(x==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration reg_obj = entity.Registrations.Find(x);
            if(reg_obj ==null)
            {
                return HttpNotFound();
            }
            return View(reg_obj);
        }
        //edit profile
        public ActionResult Edit(int? x)
        {
            x = int.Parse(Session["IdS"].ToString());
            if (x == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration reg_obj = entity.Registrations.Find(x);
            if (reg_obj == null)
            {
                return HttpNotFound();
            }
            return View(reg_obj);
        }
        [HttpPost]
        public ActionResult Edit(Registration reg_obj)
        {

            int x = int.Parse(Session["IdS"].ToString());
            string y = Session["UsernameS"].ToString();
            string z = Session["PasswordS"].ToString();
            Registration reg = entity.Registrations.Where(p => p.RegId == x && p.Username == y && p.Password == z && p.Role == "User").FirstOrDefault();

            reg.Firstname = reg_obj.Firstname;
            reg.Lastname = reg_obj.Lastname;
            reg.Mobile = reg_obj.Mobile;
            reg.EmailID = reg_obj.EmailID;
            reg.Address = reg_obj.Address;
            reg.City = reg_obj.City;
            reg.State = reg_obj.State;
            reg.Status = "A";
            entity.SaveChanges();
            TempData["UpdateMessage"] = "Profile Updated !!";
            return View(reg);

        }
    }
}

