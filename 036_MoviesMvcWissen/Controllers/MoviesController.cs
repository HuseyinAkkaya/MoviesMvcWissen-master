using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _036_MoviesMvcWissen.Contexts;
using _036_MoviesMvcWissen.Entities;
using _036_MoviesMvcWissen.Models;

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
            var directors = db.Directors.ToList().Select(e => new SelectListItem()
            {
                Value = e.Id.ToString(),
                Text = e.Name + " " + e.Surname

            }).ToList();

            ViewData["directors"] = new MultiSelectList(directors, "Value", "Text");
            return View();
        }

        [HttpPost]
        public ActionResult Add(string Name, int ProductionYear, string BoxOfficeReturn, List<int> Directors)
        {
            try
            {
                var entity = new Movie()
                {
                    Id = 0,
                    Name = Name,
                    ProductionYear = ProductionYear.ToString(),
                    BoxOfficeReturn = Convert.ToDouble(BoxOfficeReturn.Replace(',', '.'), CultureInfo.InvariantCulture),
                    //MovieDirectors = new List<MovieDirector>()
                };
                //foreach (var director in Directors)
                //{
                //    entity.MovieDirectors.Add(new MovieDirector() { MovieId = 0, DirectorId = director });
                //}
                entity.MovieDirectors = new List<MovieDirector>();
                if (Directors != null)
                    entity.MovieDirectors = Directors.Select(e => new MovieDirector()
                    {
                        DirectorId = e,
                        MovieId = entity.Id
                    }).ToList();


                db.Movies.Add(entity);

                db.SaveChanges();
                Debug.WriteLine("Added Entity Id: " + entity.Id);
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


            var directors = db.Directors.ToList().Select(e => new DirectorModel()
            {
                Id = e.Id,
                FullName = e.Name + " " + e.Surname

            }).ToList();

            var selectedIds = model.MovieDirectors.Select(e => e.DirectorId).ToList();
            ViewBag.directors = new MultiSelectList(directors, "Id", "FullName", selectedIds);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Name,ProductionYear")]Movie movie, string BoxOfficeReturn, List<int> directorIds)
        {

            var newMovie = db.Movies.Find(movie.Id);
            newMovie.Name = movie.Name;
            newMovie.ProductionYear = movie.ProductionYear;
            newMovie.MovieDirectors = new List<MovieDirector>();
            if (!string.IsNullOrWhiteSpace(BoxOfficeReturn))
                newMovie.BoxOfficeReturn =
                    Convert.ToDouble(BoxOfficeReturn.Replace(',', '.'), CultureInfo.InvariantCulture);

            var movieDirectors = db.MovieDirectors.Where(e => e.MovieId == movie.Id).ToList();
            movieDirectors.ForEach(e => db.Entry(e).State = EntityState.Deleted);
           
            directorIds?.ForEach(e => newMovie.MovieDirectors.Add(new MovieDirector() { MovieId = newMovie.Id, DirectorId = e }));

            db.Entry(newMovie).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Info"] = "Record successfully updated in database";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Id is required");
            var model = db.Movies.Find(id.Value);

            return View(model);

        }

        [ActionName("Delete")]
        [HttpPost]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Id is required");
            var entity = db.Movies.Find(id.Value);
            db.Movies.Remove(entity);
            TempData["Info"] = "Record successfully deleted from database";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Id is required");
            var model = db.Movies.Find(id.Value);
            return View(model);
        }

        public ActionResult Welcome()
        {
            var result = "Welcome to Movies MVC";
            return PartialView("_Welcome", result);
        }
    }
}