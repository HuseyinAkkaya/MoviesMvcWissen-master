using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _036_MoviesMvcWissen.Entities;

namespace _036_MoviesMvcWissen.Models
{
    public class MoviesIndexViewModel
    {
        public List<Movie> Movies { get; set; }

        public SelectList Years { get; set; }
        public string YearId { get; set; }


    }
}