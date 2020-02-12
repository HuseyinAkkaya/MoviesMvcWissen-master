using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _036_MoviesMvcWissen.Entities;

namespace _036_MoviesMvcWissen.Models
{
    public class DirectorsEditViewModel
    {
        public Director Director { get; set; }
        public MultiSelectList Movies { get; set; }
        public List<int> moviesIds { get; set; }
    }
}