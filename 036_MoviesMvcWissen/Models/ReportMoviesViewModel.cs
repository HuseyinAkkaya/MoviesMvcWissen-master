using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _036_MoviesMvcWissen.Models
{
    public class ReportMoviesViewModel
    {
        public List<ReportMoviesModel> ReportMoviesModels { get; set; }
        public int RecordCount { get; set; }
        public int RecordsPerPageCount { get; set; }
        public int PageNumber { get; set; } = 1;
        public SelectList PageNumbers { get; set; }



    }
}