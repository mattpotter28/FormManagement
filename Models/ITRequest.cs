using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FormToPDF.Models
{
    public class ITRequest
    {
        public Guid ID { get; set; }
        public string name { get; set; }
        public string empNum { get; set; }
        public DateTime date { get; set; }
        public string manager { get; set; }
        public string contact { get; set; }
        public string location { get; set; }
        public string status { get; set; }
        public string address { get; set; }
        public string priority { get; set; }
        public string tempDesired { get; set; }
        public DateTime desired { get; set; }
        public string request { get; set; }
        public string condition { get; set; }
        public string dSpecs { get; set; }
        public string lSpecs { get; set; }
        public string tempReturn { get; set; }
        public DateTime returnDate { get; set; }
        public string hardware { get; set; }
        public string app { get; set; }
        public string other { get; set; }
        public string comments { get; set; }
        public List<SelectListItem> getLocations { get; set; }
    }
}