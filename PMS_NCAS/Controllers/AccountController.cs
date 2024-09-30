using PMS_NCAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS_NCAS.Controllers
{
    public class AccountController : Controller
    {
        private UserRepository userRepository = new UserRepository();

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                
                if (!userRepository.IsUsernameAvailable(model.Username))
                {
                    ModelState.AddModelError("Username", "Username is already taken.");
                    return View(model);
                }

                model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

                bool isRegistered = userRepository.Register(model);
                if (isRegistered)
                {
                    return RedirectToAction("Login");
                }
            }
            return View(model);
        }

        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                User user = userRepository.Login(username, password);

                if (user != null)
                {
                    Session["UserId"] = user.UserId;
                    Session["Username"] = user.Username;
                    Session["Role"] = user.Role; 

                    
                    if (user.Role == "Admin")
                    {
                        return RedirectToAction("Index", "Home"); 
                    }
                    else if (user.Role == "Requestor")
                    {
                        return RedirectToAction("Requestor", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }
            return View();
        }



        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}