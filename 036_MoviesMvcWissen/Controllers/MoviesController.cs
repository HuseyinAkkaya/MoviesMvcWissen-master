using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _036_MoviesMvcWissen.Contexts;
using _036_MoviesMvcWissen.Entities;

namespace _036_MoviesMvcWissen.Controllers
{
    public class MoviesController : Controller
    {
         MoviesContext db = new MoviesContext();
        // GET: Movies
        public ViewResult Index()
        {
            var model = db.Movies.ToList();
            return View(model);
        }


        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult Add()
        //{

        //    return View();
        //}
    }
}