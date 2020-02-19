using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _036_MoviesMvcWissen.Contexts;
using _036_MoviesMvcWissen.Models;
using MulticastDelegate = System.MulticastDelegate;

namespace _036_MoviesMvcWissen.Controllers
{
    public class ReportsController : Controller
    {
        MoviesContext db = new MoviesContext();
        // GET: Reports
        public ActionResult Movies(ReportMoviesViewModel reportModel)
        {

            var movieQuery = db.Movies.AsQueryable();
            var directorQuery = db.Directors.AsQueryable();
            var movieDirectorQuery = db.MovieDirectors.AsQueryable();
            var reviewQuery = db.Reviews.AsQueryable();

            var query = from m in movieQuery
                        join md in movieDirectorQuery
                            on m.Id equals md.MovieId
                            into movie_MovieDirector
                        from subMovieMovieDirector in movie_MovieDirector.DefaultIfEmpty()
                        join d in directorQuery
                        on subMovieMovieDirector.DirectorId equals d.Id
                        into MovieDirector_Director
                        from subMovieDirector_Director in MovieDirector_Director.DefaultIfEmpty()
                        join r in reviewQuery
                            on subMovieMovieDirector.MovieId equals r.MovieId
                            into movie_review
                        from subMovieReview in movie_review.DefaultIfEmpty()

                        select new ReportMoviesModel()
                        {

                            MovieId = m.Id,
                            MovieName = m.Name,
                            _MovieBoxOfficeReturn = m.BoxOfficeReturn,
                            MovieProductionYear = m.ProductionYear,
                            DirectorFullName = subMovieDirector_Director.Name + " " + subMovieDirector_Director.Surname,
                            _DirectorRetired = subMovieDirector_Director.Retired,
                            ReviewContent = subMovieReview.Content,
                            ReviewRating = subMovieReview.Rating,
                            ReviewReviewer = subMovieReview.Reviewer
                        };

            var recordCount = query.Count();

            reportModel.RecordsPerPageCount= Convert.ToInt32(ConfigurationManager.AppSettings["MovieReportRecordsPerPage"]);

            query = query.OrderBy(e=>e.MovieId).Skip((reportModel.PageNumber - 1) * reportModel.RecordCount)
                .Take(reportModel.RecordsPerPageCount);
            

            var list = query.ToList().Select(e => new ReportMoviesModel()
            {
                MovieId = e.MovieId,
                MovieName = e.MovieName,
                MovieBoxOfficeReturn = e._MovieBoxOfficeReturn.HasValue ? e._MovieBoxOfficeReturn.Value.ToString("C", new CultureInfo("tr")) : "",
                MovieProductionYear = e.MovieProductionYear,
                DirectorFullName = e.DirectorFullName,
                DirectorRetired = e._DirectorRetired.HasValue ? (e._DirectorRetired.Value ? "Yes" : "No") : "unknown",

                ReviewContent = e.ReviewContent,
                ReviewRating = e.ReviewRating,
                ReviewReviewer = e.ReviewReviewer
            }).ToList();

            
            reportModel.ReportMoviesModels = list;
            reportModel.RecordCount = recordCount;

            return View(reportModel);
        }
    }
}