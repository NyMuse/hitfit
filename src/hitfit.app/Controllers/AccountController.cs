using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hitfit.app.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace hitfit.app.Controllers
{
    public class AccountController : BaseController
    {
        public IActionResult Login()
        {
            ViewData["Message"] = "Login page.";

            if (this.Request.Method == "POST")
            {
                if (this.Request.Form.ContainsKey("submit.Login"))
                {
                    var username = this.Request.Form["username"];
                    var password = this.Request.Form["password"];


                }

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult Registration()
        {
            ViewData["Message"] = "Your registration page.";

            User createdUser;

            if (this.Request.Method == "POST")
            {
                if (this.Request.Form.ContainsKey("submit.Register"))
                {
                    var user = new User
                    {
                        Username = this.Request.Form["username"],
                        Password = this.Request.Form["password"],
                        FirstName = this.Request.Form["userfirstname"],
                        MiddleName = this.Request.Form["usermiddlename"],
                        LastName = this.Request.Form["userlastname"]
                    };

                    var stringData = JsonConvert.SerializeObject(user);

                    createdUser = this.PostAction<User>("/account/register", stringData);

                    if (createdUser != null)
                    {
                        return RedirectToAction("UpdateInfo", new { id = createdUser.Id });
                    }
                }

                return View();
            }

            return View();
        }

        public IActionResult UpdateInfo(int id)
        {
            var user = this.GetAction<User>("/api/users/", id.ToString());

            return View();
        }
    }
}