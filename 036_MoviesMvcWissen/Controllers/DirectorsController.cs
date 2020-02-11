using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using _036_MoviesMvcWissen.Contexts;
using _036_MoviesMvcWissen.Entities;

namespace _036_MoviesMvcWissen.Controllers
{
    public class DirectorsController : Controller
    {
        private MoviesContext db = new MoviesContext();

        // GET: Directors
        public ActionResult Index()
        {
            return View(db.Directors.ToList());
        }

        // GET: Directors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Director director = db.Directors.Find(id);
            if (director == null)
            {
                return HttpNotFound();
            }
            return View(director);
        }

        // GET: Directors/Create
        public ActionResult Create()
        {

            var movies = db.Movies.ToList();

            ViewBag.MovieList = new MultiSelectList(movies, "Id", "Name");
            
            return View();
        }

        // POST: Directors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // public ActionResult Create([Bind(Include = "Id,Name,Surname,Retired")] Director director)
        [ActionName("Create")]
        public ActionResult CreateNew(List<int> ddlmovieList)
        {   
            var director = new Director()
            {

                Name = Request.Form["Name"],
                Surname = Request.Form["Surname"],
                Retired = Request.Form["Retired"] != "false",
                MovieDirectors = new List<MovieDirector>()
            };
            

            if(string.IsNullOrWhiteSpace(director.Name))
                ModelState.AddModelError("Name","Directory name is required");
            if(string.IsNullOrWhiteSpace(director.Surname))
                ModelState.AddModelError("Surname","Directory surname is required");
            if (director.Name.Length>100)
                ModelState.AddModelError("Name", "Directory name must be max 100 characters");
             if (director.Surname.Length>100)
                ModelState.AddModelError("Surname", "Directory surname must be max 100 characters");


             ddlmovieList.ForEach(e => director.MovieDirectors.Add(new MovieDirector() { MovieId = e,DirectorId = director.Id}));

            if (ModelState.IsValid)
            {
                db.Directors.Add(director);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(director);
        }

        // GET: Directors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Director director = db.Directors.Find(id);
            if (director == null)
            {
                return HttpNotFound();
            }

            var movies = db.Movies.ToList();
            var selectedIds = director.MovieDirectors.Select(e => e.MovieId).ToList();
            ViewBag.Movies = new MultiSelectList(movies, "Id", "Name", selectedIds);
            
            return View(director);
        }

        // POST: Directors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Surname,Retired")] Director director,List<int> selectedIds)
        {
            
            if (ModelState.IsValid)
            {
                var newdirector = db.Directors.Find(director.Id);
                newdirector.Name = director.Name;
                newdirector.Surname = director.Surname;
                newdirector.Retired = director.Retired;
                newdirector.MovieDirectors = new List<MovieDirector>();

                var movieDirectors = db.MovieDirectors.Where(e => e.DirectorId == director.Id).ToList();
                movieDirectors.ForEach(e => db.Entry(e).State = EntityState.Deleted);
            
                selectedIds?.ForEach(e=> newdirector.MovieDirectors.Add(new MovieDirector(){DirectorId = director.Id,MovieId = e}));


                db.Entry(newdirector).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(director);
        }

        // GET: Directors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Director director = db.Directors.Find(id);
            if (director == null)
            {
                return HttpNotFound();
            }
            return View(director);
        }

        // POST: Directors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Director director = db.Directors.Find(id);
            db.Directors.Remove(director);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
