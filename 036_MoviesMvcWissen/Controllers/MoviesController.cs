using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
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
            var model = GetList();
            ViewData["count"] = model.Count;
            return View(model);
        }


        public ActionResult GetMoviesFromSession()
        {
            List<Movie> model = GetList(false);
            return View("Index", model);
        }


        [NonAction]
        public List<Movie> GetList(bool removeSession = true)
        {
            List<Movie> entites;
            if (Session["movies"] == null || removeSession)
            {
                entites = db.Movies.ToList();
                Session["movies"] = entites;
            }
            else
            {
                entites = Session["movies"] as List<Movie>;
            }
            return entites;
        }


        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.Message = "Please enter movie information";
            return View();
        }

        [HttpPost]
        public ActionResult Add(string Name, int ProductionYear, string BoxOfficeReturn)
        {
            try
            {
                var entity = new Movie()
                {
                    Name = Name,
                    ProductionYear = ProductionYear.ToString(),
                    BoxOfficeReturn = Convert.ToDouble(BoxOfficeReturn.Replace(',', '.'), CultureInfo.InvariantCulture)
                };
                db.Movies.Add(entity);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            TempData["Info"] = "Record successfully saved to database";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
                //return HttpNotFound();
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var model = db.Movies.Find(id.Value);

            List<SelectListItem> list = new List<SelectListItem>();
            for (var i = DateTime.Now.Year; i >= 1960; i--)
            {
                list.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            ViewBag.Dates = new SelectList(list, "Text", "Value", model.ProductionYear);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Name,ProductionYear")]Movie movie, string BoxOfficeReturn)
        {

            var newMovie = db.Movies.Find(movie.Id);
            newMovie.Name = movie.Name;
            newMovie.ProductionYear = movie.ProductionYear;
            if (!string.IsNullOrWhiteSpace(BoxOfficeReturn))
                newMovie.BoxOfficeReturn =
                    Convert.ToDouble(BoxOfficeReturn.Replace(',', '.'), CultureInfo.InvariantCulture);
            db.Entry(newMovie).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if(!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,"Id is required");
            var model = db.Movies.Find(id.Value);

            return View(model);

        }

        [ActionName("Delete")]
        [HttpPost]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Id is required");
            var entity = db.Movies.Find(id);
            db.Movies.Remove(entity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}