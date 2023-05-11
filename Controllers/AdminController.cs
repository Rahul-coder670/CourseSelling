using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Tatakae.Models;

namespace Tatakae.Controllers
{
    public class AdminController : Controller
    {   MyWebsiteEntities crud = new MyWebsiteEntities();
        
        public ActionResult AdminProfile()
        {
            Session["user_id"] ="1";


            if (Session["user_id"] == null)
            {
                TempData["msg"] = "Please Login to access this page!";
                return RedirectToAction("Login","Home");
            }
            
                int id = Convert.ToInt32(Session["user_id"]);
                var b = crud.Users.Where(a => a.UId == id).FirstOrDefault();
                if (b.Type == "User")
                {
                    TempData["msg"] = "Only admins can access this page!";
                    return RedirectToAction("UserProfile" , "Home");
                }
                ViewBag.user = b;
                var tables = new ShowAll()
                {
                    users = crud.Users.ToList(),
                    courses = crud.Courses.ToList()
                };

                return View(tables);
        }

        public ActionResult AdminRegistration()
        {
            if (Session["user_id"] == null)
            {
                TempData["msg"] = "Admin Login required to access admin registration page!";
                return RedirectToAction("Login", "Home");
            }
                int id = Convert.ToInt32(Session["user_id"]);
                var b = crud.Users.Select(a => new { a.UId, a.Type }).FirstOrDefault(y => y.UId == id);
                if (b.Type == "User")
                {
                    TempData["msg"] = "Only admins can access this page!";
                    return RedirectToAction("UserProfile", "Home");
                }
            return View();
        }

        [HttpPost]
        public ActionResult AdminRegistration(Register register)
        {
            if (ModelState.IsValid)
            {
                var b = crud.Users.Select(a => new { a.Email }).FirstOrDefault(y => y.Email == register.email);

                if (b == null)
                {
                    User user = new User()
                    {
                        Name = register.name,
                        Email = register.email,
                        Password = register.password,
                        Type = register.type,
                    };
                    crud.Users.Add(user);
                    crud.SaveChanges();
                    ViewBag.msg = "Registration Successful";
                    return RedirectToAction("AdminProfile");
                }
            }
            ViewBag.msg = "This email already exists";
            return View();

        }

        public ActionResult DeleteUsers(int id)
        {
            if (Session["user_id"] == null)
            {
                TempData["msg"] = "Please Login to access this page!";
                return RedirectToAction("Login", "Home");
            }
            int uid = Convert.ToInt32(Session["user_id"]);
            var b = crud.Users.Select(a => new { a.UId, a.Type }).FirstOrDefault(y => y.UId == uid);
            if (b.Type == "User")
            {
                TempData["msg"] = "Only admins can access this page!";
                return RedirectToAction("UserProfile", "Home");
            }
            var user = crud.Users.FirstOrDefault(y => y.UId == id);
            crud.Users.Remove(user);
            crud.SaveChanges();
            TempData["msg"] = "Record Removed";
            return RedirectToAction("AdminProfile");
        }
        public ActionResult EditCourses(int id)
        { 
            if (Session["user_id"] == null)
            {
                TempData["msg"] = "Please Login to access this page!";
                return RedirectToAction("../Home/Login");
            }
            int uid = Convert.ToInt32(Session["user_id"]);
            var b = crud.Users.Select(a => new { a.UId, a.Type }).FirstOrDefault(y => y.UId == uid);
            if (b.Type == "User")
            {
                TempData["msg"] = "Only admins can access this page!";
                return RedirectToAction("UserProfile", "Home");
            }
            var course = crud.Courses.FirstOrDefault(y => y.CId == id);
            return View(course);
        }
        [HttpPost]
        public ActionResult EditCourses(int id,AddCourse addCourse)
        {
            var course = crud.Courses.FirstOrDefault(y => y.CId == id);
            if(addCourse.Coursename != null && addCourse.Coursename != "")
            {
            course.CoursesName = addCourse.Coursename;
            }
            if (addCourse.Description != null && addCourse.Description != "")
            {
                course.Description = addCourse.Description;
            }
            if (addCourse.Duration != null && addCourse.Duration != "")
            {
                course.Duration = addCourse.Duration;
            }
            if (addCourse.Price != null && addCourse.Price != "")
            {
                course.Price = addCourse.Price;
            }
            crud.SaveChanges();
            return RedirectToAction("AdminProfile");
        }


        public ActionResult DeleteCourses(int id)
        {
            if (Session["user_id"] == null)
            {
                TempData["msg"] = "Please Login to access this page!";
                return RedirectToAction("../Home/Login");
            }
            int uid = Convert.ToInt32(Session["user_id"]);
            var b = crud.Users.Select(a => new { a.UId, a.Type }).FirstOrDefault(y => y.UId == uid);
            if (b.Type == "User")
            {
                TempData["msg"] = "Only admins can access this page!";
                return RedirectToAction("UserProfile", "Home");
            }
            var course = crud.Courses.FirstOrDefault(y => y.CId == id);
            crud.Courses.Remove(course);
            crud.SaveChanges();
            TempData["msg"] = "Record Removed";
            return RedirectToAction("AdminProfile");
        }

        public ActionResult AddNewCourse()
        {
            if (Session["user_id"] == null)
            {
                TempData["msg"] = "Please login to access this page!";
                return RedirectToAction("Login" , "Home");
            }
            int uid = Convert.ToInt32(Session["user_id"]);
            var b = crud.Users.Select(a => new { a.UId, a.Type }).FirstOrDefault(y => y.UId == uid);
            if (b.Type == "User")
            {
                TempData["msg"] = "Only admins can access this page!";
                return RedirectToAction("UserProfile", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddNewCourse(AddCourse add)
        {
            if (ModelState.IsValid)
            {
                var b = crud.Courses.Select(a => new { a.CoursesName }).FirstOrDefault(y => y.CoursesName == add.Coursename);

                if (b == null)
                {
                    Cours course = new Cours()
                    {
                        CoursesName = add.Coursename,
                        Description= add.Description,
                        Duration= add.Duration,
                        Price = add.Price
                    };
                    crud.Courses.Add(course);
                    crud.SaveChanges();
                    ViewBag.msg = "New Course Added";
                    return RedirectToAction("AdminProfile");
                }
            }
           
            ViewBag.msg = "This course already exists";
            return View();
        }
    }
}