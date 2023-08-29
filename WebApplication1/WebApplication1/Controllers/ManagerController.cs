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
    public class ManagerController : Controller
    {
        MVC_ProjectEntity entity = new MVC_ProjectEntity();
        // GET: Manager
       
        public ActionResult HallDetails()
        {
            return View(entity.HallServices.ToList());
        }
        //Halldetails create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(HallService service)
        {
            if(ModelState.IsValid )
            {
                if (entity .HallServices .Any (x=>x.Categoryname==service .Categoryname ))
                {
                    ViewBag.Notification = "Categoryname already Exists";
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    service.Status = "A";
                    entity.HallServices.Add(service);
                    entity.SaveChanges();
                    ModelState.Clear();
                    TempData["Message"] = "Hall Service entered!";
                    return View("HallDetails");
                }
            }
            return View();
        }

        //halldetails edit
        public ActionResult Edit(int id)
        {
            HallService service = entity.HallServices.Find(id);
            if (id == null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (service  == null)
            {
                return HttpNotFound();
            }
            return View(service );

        }
        [HttpPost]
        public ActionResult Edit(HallService service)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    entity.Entry(service).State = EntityState.Modified;
                    entity.SaveChanges();
                    return RedirectToAction("HallDetails");
                }
                return View(service);
            }
            catch
            {
                return View();
            }


        }
        public ActionResult Delete(int id)
        {
            HallService service = entity.HallServices.Find(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }
        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
        try         
            {
                HallService service = entity.HallServices.Find(id);
                entity.HallServices.Remove(service);
                entity.SaveChanges();
                return RedirectToAction("HallDetails");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult SearchUser(string searchBy,string search)
        {
            if (searchBy == "Mobile")
            {
                return View(entity.Registrations.Where(x => x.Role == "User" &&( x.Mobile == search || search == null)).ToList());
            }
            else
            {
                return View(entity.Registrations.Where(x => x.Role == "User" && (x.Firstname.StartsWith(search) || search == null)).ToList());
            }
        }

       //Manager Profile
       public ActionResult ManagerProfile(int ?x)
        {
            x = int.Parse(Session["IdM"] .ToString ());
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
        //edit profile
       public ActionResult EditProfile(int? x)
        {
            x = int.Parse(Session["IdM"].ToString());
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
       public ActionResult EditProfile(Registration reg_obj)
        {
            int x = int.Parse(Session["IdM"].ToString());
            string y = Session["UsernameM"].ToString();
            string z = Session["PasswordM"].ToString();
            Registration reg = entity.Registrations.Where(p => p.RegId == x && p.Username == y && p.Password == z && p.Role == "Manager").FirstOrDefault();

            reg.Firstname = reg_obj.Firstname;
            reg.Lastname = reg_obj.Lastname;
            reg.Mobile = reg_obj.Mobile;
            reg.EmailID = reg_obj.EmailID;
            reg.Address = reg_obj.Address;
            reg.City = reg_obj.City;
            reg.State = reg_obj.State;
            reg.Status = "Accepted";
            entity.SaveChanges();
            TempData["UpdateMessage"] = "profile Updated !!";
            return View(reg);
          
        }
        public ActionResult BookingOrder()
        {
            var registrationlist = entity.Registrations.ToList();
            ViewBag.RegId = new SelectList(registrationlist, dataValueField: "RegId", dataTextField: "Firstname");
            return View(entity.Eventbookings .ToList().Where(p=>p.Status =="Booked"));
        }
        public ActionResult Finish(int id)
        {

            var registrationlist = entity.Registrations.ToList();
            ViewBag.RegId = new SelectList(registrationlist, dataValueField: "RegId", dataTextField: "Firstname");
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventbooking booking = entity.Eventbookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }
        [HttpPost]
        public ActionResult Finish(int id,Eventbooking booking)
        {
            //var registrationlist = entity.Registrations.ToList();
            //ViewBag.RegId = new SelectList(registrationlist, dataValueField: "RegId", dataTextField: "Firstname");
            
                Eventbooking book = entity.Eventbookings.Find(id);
                book.Status = "Finished";
                entity.Eventbookings.Add(book);
                entity.Entry(book).State = EntityState.Modified;
                entity.SaveChanges();
                TempData["ClearMessage"] = "Event Finished!";
                ModelState.Clear();
                return View(booking);
          
        }
        public ActionResult Decline(int id)
        {
            var registrationlist = entity.Registrations.ToList();
            ViewBag.RegId = new SelectList(registrationlist, dataValueField: "RegId", dataTextField: "Firstname");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventbooking booking = entity.Eventbookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
         
        }
        [HttpPost,ActionName("Decline")]
        public ActionResult DeclineConfirm(int id)
        {
            var registrationlist = entity.Registrations.ToList();
            ViewBag.RegId = new SelectList(registrationlist, dataValueField: "RegId", dataTextField: "Firstname");
            try
            {
                Eventbooking booking = entity.Eventbookings.Find(id);
                entity.Eventbookings.Remove(booking);
                entity.SaveChanges();
                return RedirectToAction("BookingOrder");
            }
            catch
            {
                return View();
            }
        }
    


        public ActionResult UserBookingDetails()
        {
            using (MVC_ProjectEntity entity = new MVC_ProjectEntity())
            {
                var list = entity.Eventbookings.Include("Registration").ToList();
                return View(list);
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
