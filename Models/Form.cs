using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FormToPDF.Models
{
    public class Form
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string FormType { get; set; }
        public string FormName { get; set; }
    }
}