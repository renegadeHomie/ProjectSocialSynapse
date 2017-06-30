using Microsoft.AspNet.Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookLoginMVC.Models
{
    public class ReactionsData
    {
        public string id { get; set; }
        public string name { get; set; }
        public string link { get; set; }
        public string pic_large { get; set; }
        public int count { get; set; }
    }
}