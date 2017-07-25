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
    public class AccountController : AppController
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

                    this.GetJwtToken(username, password);
                }

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");

            return RedirectToAction("Index", "Home");
        }

        //public IActionResult MyAccount()
        //{
            
        //}

        public IActionResult Registration()
        {
            ViewData["Message"] = "Your registration page.";
            
            if (this.Request.Method == "POST")
            {
                if (this.Request.Form.ContainsKey("submit.Register"))
                {
                    var user = new UserDto
                    {
                        Name = this.Request.Form["username"],
                        Password = this.Request.Form["password"],
                        FirstName = this.Request.Form["userfirstname"],
                        MiddleName = this.Request.Form["usermiddlename"],
                        LastName = this.Request.Form["userlastname"],
                        Birthday = Convert.ToDateTime(this.Request.Form["birthday"])
                    };

                    var stringData = JsonConvert.SerializeObject(user);

                    var createdUser = this.PostAction<UserDto>("/account/register", stringData);

                    if (createdUser != null)
                    {
                        return RedirectToAction("UpdateInfo", new { id = createdUser.Id });
                    }
                }
            }

            return View();
        }

        public IActionResult UpdateInfo(int id)
        {
            id = id == 0 ? int.Parse(Request.Cookies["userId"]) : id;

            if (this.Request.Method == "POST")
            {
                if (this.Request.Form.ContainsKey("submit.UpdateMeasurements"))
                {
                    var userMeasurements = new UserMeasurementsDto
                    {
                        UserId = id,
                        Growth = short.Parse(this.Request.Form["growth"]),
                        Weight = short.Parse(this.Request.Form["weight"])
                    };

                    var stringData = JsonConvert.SerializeObject(userMeasurements);

                    var createdUserMeasurements = this.PostAction<UserMeasurementsDto>("api/users/measurements", stringData);

                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                var user = this.GetAction<UserDto>("/api/users/", id.ToString());

                if (user != null)
                {
                    ViewBag.UserGrowth = user.UserMeasurements.Last().Growth;
                    ViewBag.UserWeight = user.UserMeasurements.Last().Weight;
                }
            }

            return View();
        }
    }
}