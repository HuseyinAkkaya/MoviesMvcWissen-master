using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace _036_MoviesMvcWissen.Models
{
    public class ReportMoviesViewModel
    {
        public List<ReportMoviesModel> ReportMoviesModels { get; set; }
        public int RecordCount { get; set; }
        public int RecordsPerPageCount { get; set; }
        public int PageNumber { get; set; } = 1;


    }
}