using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FormToPDF
{
    public class LocationHandler
    {
        public static List<SelectListItem> getLocationList()
        {
            List<SelectListItem> locations = new List<SelectListItem>();
            var loc = HttpContext.Current.Server.MapPath("~/Data/locations.csv");
            if (System.IO.File.Exists(loc))
            {
                string[] lines = System.IO.File.ReadAllLines(loc);
                foreach (string line in lines)
                {
                    string[] data = line.Split(',');
                    if (data[1] == "1")
                    {
                        locations.Add(new SelectListItem { Text = data[0], Value = data[0] });
                    }
                }
            }
            return locations;
        }
    }
}