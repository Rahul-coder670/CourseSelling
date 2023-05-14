using Microsoft.Win32;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Tatakae.Models;

namespace Tatakae.Controllers
{   

    public class HomeController : Controller
    {
        MyWebsiteEntities crud = new MyWebsiteEntities();
        public ActionResult Index()
        {
            if (Session["user_id"] != null)
            {
                int id = Convert.ToInt32(Session["user_id"]);
                var b = crud.Users.Select(a => new { a.UId, a.Type }).FirstOrDefault(y => y.UId == id);
                if (b.Type == "Admin")
                {
                    TempData["msg"] = "Please logout to access this page!";
                    return RedirectToAction("AdminProfile", "Admin");
                }
                TempData["msg"] = "Please logout to access this page!";
                return RedirectToAction("UserProfile");
            }
            return View();
        }

        public ActionResult About()
        {
            if (Session["user_id"] != null)
            {
                int id = Convert.ToInt32(Session["user_id"]);
                var b = crud.Users.Select(a => new { a.UId, a.Type }).FirstOrDefault(y => y.UId == id);
                if (b.Type == "Admin")
                {
                    TempData["msg"] = "Please logout to access this page!";
                    return RedirectToAction("AdminProfile", "Admin");
                }
                TempData["msg"] = "Please logout to access this page!";
                return RedirectToAction("UserProfile");
            }
           

            return View();
        }

        public ActionResult Courses()
        {
            var courses = crud.Courses.OrderByDescending(y => y.CId).ToList();

            return View(courses);
        }
        public ActionResult BuyCourse(Nullable<int> id)
        {
            if (id == null)
            {
                TempData["msg"] = "Please select a course to access BuyCourse page!";
                return RedirectToAction("Courses");
            }
            var product = crud.Courses.FirstOrDefault(y => y.CId == id);
            return View(product);
        }
        public ActionResult Register()
        {
            if (Session["user_id"] != null)
            {
                int id = Convert.ToInt32(Session["user_id"]);
                var b = crud.Users.Select(a => new { a.UId, a.Type }).FirstOrDefault(y => y.UId == id);
                if (b.Type == "Admin")
                {
                    TempData["msg"] = "Please logout to access this page!";
                    return RedirectToAction("AdminProfile", "Admin");
                }
                TempData["msg"] = "Please logout to access this page!";
                return RedirectToAction("UserProfile");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Register(Register register)
        {
            if (Session["user_id"] != null)
            {
                int id = Convert.ToInt32(Session["user_id"]);
                var b = crud.Users.Select(a => new { a.UId, a.Type }).FirstOrDefault(y => y.UId == id);
                if (b.Type == "Admin")
                {
                    TempData["msg"] = "Please logout to access this page!";
                    return RedirectToAction("AdminProfile", "Admin");
                }
                TempData["msg"] = "Please logout to access this page!";
                return RedirectToAction("UserProfile");
            }
            if (ModelState.IsValid)
            {
                var b = crud.Users.Select(a => new { a.Email }).FirstOrDefault(y => y.Email == register.Email);

                if (b == null)
                {
                    User user = new User()
                    {
                        Name = register.Name,
                        Email = register.Email,
                        Password = register.Password,
                        Type = register.Type,
                    };
                    crud.Users.Add(user);
                    crud.SaveChanges();
                    ViewBag.msg = "Registration Successful";
                    return View();
                }
            }
            ViewBag.msg = "This email already exists";
            return View();

        }
        public ActionResult Login()
        {
            if (Session["user_id"] != null)
            {
                int id = Convert.ToInt32(Session["user_id"]);
                var b = crud.Users.Select(a => new { a.UId, a.Type }).FirstOrDefault(y => y.UId == id);
                if (b.Type == "Admin")
                {
                    TempData["msg"] = "Please logout to access this page!";
                    return RedirectToAction("AdminProfile", "Admin");
                }
                TempData["msg"] = "Please logout to access this page!";
                return RedirectToAction("UserProfile");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginUser login)
        {
            if (Session["user_id"] != null)
            {
                int id = Convert.ToInt32(Session["user_id"]);
                var b = crud.Users.Select(a => new {a.UId,a.Type }).FirstOrDefault(y => y.UId == id);
                if (b.Type == "Admin")
                {
                    TempData["msg"] = "Please logout to access this page!";
                    return RedirectToAction("AdminProfile", "Admin");
                }
                TempData["msg"] = "Please logout to access this page!";
                return RedirectToAction("UserProfile");
            }
            if (ModelState.IsValid)
            {
                var b = crud.Users.Select(a => new { a.UId, a.Email, a.Password,a.Type }).FirstOrDefault(y => y.Email == login.email);
                if (b != null)
                {
                    if (login.password == b.Password)
                    {
                        string user_id = b.UId.ToString();
                        Session["user_id"] = user_id;
                        if (b.Type == "Admin")
                        {
                            return RedirectToAction("AdminProfile", "Admin");
                        }
                        else
                        {
                            return RedirectToAction("UserProfile");
                        }
                    }
                    else
                    {
                    ViewBag.msg = "Your password is wrong";
                    }
                }
                else
                {
                    ViewBag.msg = "Your email is wrong";
                }
            }
            return View();
        }

        public int Calculate(DateTime date, int duration)
        {
            duration = 30 * duration;
            int days = DateTime.Now.Subtract(date).Days;
            if (duration > days)
            {
                return duration - days;
            }
            return 0;
        }
        public ActionResult UserProfile()
        {
            if (Session["user_id"] == null)
            {
                TempData["msg"] = "Please Login to access this page!";
                return RedirectToAction("Login");
            }
            int id = Convert.ToInt32(Session["user_id"]);
            var user = crud.Users.Where(a => a.UId == id).FirstOrDefault();
            if (user.Type == "Admin")
            {
                TempData["msg"] = "That page is only for users!";
                return RedirectToAction("AdminProfile", "Admin");
            }
            ViewBag.user = user;

            var record = crud.Records.Where(y => y.U_Id == id).ToList();
            var courses = crud.Courses.ToList();


            List<RecordModel> recordModels = new List<RecordModel>();

            foreach (Record record1 in record)
            {
                int temp_Pid = (int)record1.C_Id;
                DateTime temp_Pdate = (DateTime)record1.PurchaseDate;

                var course = courses.FirstOrDefault(y => y.CId == temp_Pid);

                DateTime day = (DateTime)record1.PurchaseDate;


                RecordModel recordModel = new RecordModel()
                {
                    Duration = course.Duration, // 30
                    PId = temp_Pid,
                    PDate = temp_Pdate, // 27/01/2023
                    Validity = Calculate(day, Convert.ToInt32(course.Duration))
                };
                recordModels.Add(recordModel);
            }
            return View(recordModels);
            
        }
        

        public ActionResult EditUserProfile()
        {
            if (Session["user_id"] == null)
            {
                TempData["msg"] = "Please Login to access this page!";
                return RedirectToAction("Login");
            }
            int id = Convert.ToInt32(Session["user_id"]);
            var user = crud.Users.Where(a => a.UId == id).FirstOrDefault();
            return View(user);
        }

        [HttpPost]
        public ActionResult EditUserProfile(EditUser edit)
        {
            int id = Convert.ToInt32(Session["user_id"]);
            var user = crud.Users.Where(a => a.UId == id).FirstOrDefault();

            if (edit.Name != null && edit.Name != "")
            {
                user.Name = edit.Name;
            }
            if (edit.Email != null && edit.Email != "")
            {
                user.Email = edit.Email;
            }
            if (edit.Password != null && edit.Password != "")
            {
                user.Password = edit.Password;
            }
            if (edit.Phone != null && edit.Phone != "")
            {
                user.Phone = edit.Phone;
            }
            if (edit.Dob != null)
            {
                user.Dob = edit.Dob;
            }
            if (edit.Address != null && edit.Address != "")
            {
                user.Address = edit.Address;
            }
            crud.SaveChanges();
            return RedirectToAction("UserProfile");
        }

        public ActionResult Logout()
        {
            if (Session["user_id"] == null)
            {
                TempData["msg"] = "Please Login to access this page!";
                return RedirectToAction("Login");
            }
 
            Session.Abandon();
            TempData["msg"] = "You have logged out.";
            return RedirectToAction("Login");
        }
        public ActionResult Buy(int id)
        {
            if (Session["user_id"] == null)
            {
                TempData["msg"] = "Please Login to access this page!";
                return RedirectToAction("Login");
            }
            int uid = Convert.ToInt32(Session["user_id"]);
            var user = crud.Users.Where(a => a.UId == uid).FirstOrDefault();
            if (user.Type == "Admin")
            {
                TempData["msg"] = "That page is only for user!";
                return RedirectToAction("AdminProfile", "Admin");
            }
            var pur = crud.Purchases.FirstOrDefault(y => y.UId == uid && y.CId == id && y.Status == "pending");

            if (pur != null)
            {
                ViewBag.orderId = pur.OrderId;
                return View();
            }

            var course = crud.Courses.FirstOrDefault(y => y.CId == id);
            int price = Convert.ToInt32(course.Price);
            string your_key_id = "rzp_test_pSGnSiOsRx88CA";
            string your_secret = "yMBSNuwwX9LY8LQxGSs9StdI";
            RazorpayClient client = new RazorpayClient(your_key_id, your_secret);
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", price * 100); // amount in the smallest currency unit
            options.Add("currency", "INR");
            Order order = client.Order.Create(options);
            string orderId = order["id"];
            ViewBag.orderId = orderId;

            Purchase purchase = new Purchase()
            {
                UId = uid,
                CId = id,
                OrderId = orderId,
                Status = "pending"
            };

            crud.Purchases.Add(purchase);
            crud.SaveChanges();

            return View();
        }

        public ActionResult Subscribe(string razorpay_payment_id, string razorpay_order_id, string razorpay_signature)
        {
            if (Session["user_id"] == null)
            {
                TempData["msg"] = "Please Login to access this page!";
                return RedirectToAction("Login");
            }
            int uid = Convert.ToInt32(Session["user_id"]);
            var user = crud.Users.Where(a => a.UId == uid).FirstOrDefault();
            if (user.Type == "Admin")
            {
                TempData["msg"] = "That page is only for user!";
                return RedirectToAction("AdminProfile", "Admin");
            }
            var purchase = crud.Purchases.FirstOrDefault(y => y.OrderId == razorpay_order_id);

            purchase.PaymentId = razorpay_payment_id;
            purchase.CheckSum = razorpay_signature;
            purchase.Date = DateTime.Now;
            purchase.Status = "success";
            crud.SaveChanges();


            Record record = new Record()
            {
                U_Id = Convert.ToInt32(Session["user_id"]),
                C_Id = purchase.CId,
                PurchaseDate = DateTime.Now,
            };

            crud.Records.Add(record);
            crud.SaveChanges();
            return RedirectToAction("UserProfile");
        }
     
    }
   
}