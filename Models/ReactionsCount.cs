using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookLoginMVC.Models
{
    public class ReactionsCount
    {
            public A[] data { get; set; }
            public Paging paging { get; set; }
            public Summary summary { get; set; }
        }

       
        public class Summary
        {
            public int total_count { get; set; }
        }

        public class A
        {
            public string id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
        }

    }
