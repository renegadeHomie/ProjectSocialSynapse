using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookLoginMVC.Models
{
    public class ProfilePicture
    {
       
        public Picture picture { get; set; }
        public string id { get; set; }
     }

        public class Picture
        {
        public Data data { get; set; }
        }

        public class Data
        {
        public string url { get; set; }
        }

    }
