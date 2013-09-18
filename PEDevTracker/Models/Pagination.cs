using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PEDevTracker.Models
{
    public class Pagination
    {
        public long NumberOfPages { get; set; }
        public int PageNumber { get; set; }
        public string Action { get; set; }
        public object RouteValues { get; set; }

        public Pagination(long numberOfResults, int resultsPerPage, int? page, string action, object routeValues)
        {
            this.NumberOfPages = numberOfResults / resultsPerPage + (numberOfResults % resultsPerPage > 0 ? 1 : 0);
            this.PageNumber = (page != null ? (int)page : 0);
            this.Action = action;
            this.RouteValues = routeValues;
        }
    }
}