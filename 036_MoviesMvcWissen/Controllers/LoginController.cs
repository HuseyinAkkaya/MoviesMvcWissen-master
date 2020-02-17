using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using _036_MoviesMvcWissen.Contexts;
using _036_MoviesMvcWissen.Entities;

namespace _036_MoviesMvcWissen.Controllers
{

    public class LoginController : Controller
    {
        MoviesContext db = new MoviesContext();
        // GET: Login

        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Index(User user, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(e => e.UserName == user.UserName && e.Password == user.Password))
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, false);


                    return ReturnUrl == null
                        ? RedirectToAction("Index", "Movies")
                        : (ActionResult)Redirect(ReturnUrl);

                }




                ViewBag.Message = "User Name or Pasword is incorrect!";
                return View(user);
            }

            ViewBag.Message = "User Name or Pasword is invalid!";
            return View(user);
        }

        public ActionResult Logout()
        {

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Movies");
        }
    }
}