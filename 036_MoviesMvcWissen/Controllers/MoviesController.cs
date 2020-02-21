using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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
        public ViewResult Index(MoviesIndexViewModel moviesIndexViewModel)
        {


            if (moviesIndexViewModel == null || string.IsNullOrWhiteSpace(moviesIndexViewModel.YearId))
            {
                moviesIndexViewModel = new MoviesIndexViewModel();
                moviesIndexViewModel.Movies = GetList();
            }
            else
            {
                moviesIndexViewModel.Movies = db.Movies.Where(e => e.ProductionYear.Equals(moviesIndexViewModel.YearId)).ToList();
            }

            var qr = db.Movies.AsQueryable();
            qr = qr.Where(a => a.ProductionYear.Equals(""));


            var years = new List<SelectListItem>();
            //for (int i = DateTime.Now.Year; i >= 1950; i--)
            //    years.Add(new SelectListItem()
            //    {
            //        Text = i.ToString(),
            //        Value = i.ToString()
            //    });

            years = db.Movies.GroupBy(e => e.ProductionYear).Select(m => new SelectListItem()
            {
                Text = m.Key,
                Value = m.Key
            }).ToList();
            years.Insert(0, new SelectListItem() { Text = @"--All--", Value = "" });

            moviesIndexViewModel.Years = new SelectList(years, "Value", "Text", moviesIndexViewModel.YearId);



            ViewData["count"] = moviesIndexViewModel.Movies.Count;
            return View(moviesIndexViewModel);
        }

        public ViewResult List(MoviesIndexViewModel moviesIndexViewModel)
        {
            if (moviesIndexViewModel == null)
                moviesIndexViewModel = new MoviesIndexViewModel();

            var moviesQuery = db.Movies.AsQueryable();



            moviesIndexViewModel.Movies = moviesQuery.ToList();

            return View(moviesIndexViewModel);

        }


        public ActionResult GetMoviesFromSession()
        {
            var model = new MoviesIndexViewModel()
            {
                Movies = GetList(false)
            };
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
        public ActionResult Add(string Name, int ProductionYear, string BoxOfficeReturn, List<int> Directors, HttpPostedFileBase Image)
        {
            try
            {

                string filePath = ImageFilePath(Name, Image);

                var entity = new Movie()
                {
                    Id = 0,
                    Name = Name,
                    ProductionYear = ProductionYear.ToString(),
                    BoxOfficeReturn = Convert.ToDouble(BoxOfficeReturn.Replace(',', '.'), CultureInfo.InvariantCulture),
                    FilePath = filePath
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

                if (filePath != null)
                {

                    Image.SaveAs(Server.MapPath(filePath));
                }

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


        private string ImageFilePath(string Name, HttpPostedFileBase Image)
        {
            string filePath = null;
            if (Image != null && Image.ContentLength > 0)
            {

                //var fileName = Path.GetFileName(Image.FileName);//Cagil.jpg
                var fileNeme = DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + Name + Path.GetExtension(Image.FileName);

                if (Path.GetExtension(fileNeme).ToLower().Equals(".jpg") ||
                    Path.GetExtension(fileNeme).ToLower().Equals(".jpeg") ||
                    Path.GetExtension(fileNeme).ToLower().Equals(".png") ||
                    Path.GetExtension(fileNeme).ToLower().Equals(".bmp"))
                {
                    filePath = ConfigurationManager.AppSettings["FilesFolder"] + "/Movies/" + fileNeme;

                }
            }

            return filePath;
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
        public ActionResult Edit([Bind(Include = "Id,Name,ProductionYear")]Movie movie, string BoxOfficeReturn, List<int> directorIds, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {


                var newMovie = db.Movies.Find(movie.Id);
                newMovie.Name = movie.Name;
                newMovie.ProductionYear = movie.ProductionYear;
                newMovie.MovieDirectors = new List<MovieDirector>();
                if (!string.IsNullOrWhiteSpace(BoxOfficeReturn))
                    newMovie.BoxOfficeReturn =
                        Convert.ToDouble(BoxOfficeReturn.Replace(',', '.'), CultureInfo.InvariantCulture);



                string oldFilePath = newMovie.FilePath;
                if (ImageFilePath(movie.Name, Image) != null)
                    newMovie.FilePath = ImageFilePath(movie.Name, Image);



                var movieDirectors = db.MovieDirectors.Where(e => e.MovieId == movie.Id).ToList();
                movieDirectors.ForEach(e => db.Entry(e).State = EntityState.Deleted);

                directorIds?.ForEach(e => newMovie.MovieDirectors.Add(new MovieDirector() { MovieId = newMovie.Id, DirectorId = e }));




                db.Entry(newMovie).State = EntityState.Modified;
                db.SaveChanges();

                if (ImageFilePath(movie.Name, Image) != null)
                {
                    if (oldFilePath != null && System.IO.File.Exists(Server.MapPath(oldFilePath)))
                    {
                        System.IO.File.Delete(Server.MapPath(oldFilePath));
                    }

                    Image.SaveAs(Server.MapPath(newMovie.FilePath));
                }



                TempData["Info"] = "Record successfully updated in database";
                return RedirectToAction("Index");

            }

            return View(movie);
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

            if (entity.FilePath != null && System.IO.File.Exists(Server.MapPath(entity.FilePath)))
            {
                System.IO.File.Delete(Server.MapPath(entity.FilePath));
            }


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