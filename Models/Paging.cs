using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookLoginMVC.Models
{
    public class Paging
    {
        public Cursors cursors { get; set; }
        public string next { get; set; }
        public string previous { get; set; }
    }
    public class Cursors
    {
        public string before { get; set; }
        public string after { get; set; }
    }
}