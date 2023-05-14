using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace Tatakae.Models
{
    public class Register
    {
        [Required(ErrorMessage = "Name is requiured")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is requiured")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "password is requiured")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm password is requiured")]
        [Compare("password", ErrorMessage = "password and confirm password didnt matched")]
        public string Cpassword { get; set; }
        public string Type { get; set; }
    }
     
    public class LoginUser
    {
        [Required(ErrorMessage = "Email is requiured")]
        [EmailAddress]
        public string email { get; set; }
        [Required(ErrorMessage = "password is requiured")]
        public string password { get; set; }
    }

    public class RecordModel
    {
        public string Duration { get; set; }
        public DateTime PDate { get; set; }
        public int Validity { get; set; }
        public int PId { get; set; }
    }

    public class ShowAll
    {
        public IEnumerable<User> users { get; set; }
        public IEnumerable<Cours> courses { get; set; }
    }

    public class AddCourse
    {
        [Required]
        public string Coursename { get; set; }
        [Required]
        [AllowHtml]
        public string Description { get; set; }

        [Required]
        public string Duration { get; set; }
        [Required]
        public string Price { get; set; }
        
    }

    public class EditUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }


    }
}

