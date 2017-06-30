using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookLoginMVC.Models
{
    public class Class3
    {
        public PostsS posts { get; set; }
        public string id { get; set; }
    }
    public class PostsS
    {
            public DatumS[] data { get; set; }
            public Paging paging { get; set; }
        }

        //public class Paging
        //{
        //    public string previous { get; set; }
        //    public string next { get; set; }
        //}

        public class DatumS
        {
            public string link { get; set; }
            public string permalink_url { get; set; }
            public string id { get; set; }
            public DateTime created_time { get; set; }
            public Shares shares { get; set; }
            public Sharedposts sharedposts { get; set; }
        }

        public class Shares
        {
            public int count { get; set; }
        }

        public class Sharedposts
        {
            public Datum1S[] data { get; set; }
            public Paging paging { get; set; }
        }

        //public class Paging1
        //{
        //    public Cursors cursors { get; set; }
        //    public string next { get; set; }
        //}

        //public class Cursors
        //{
        //    public string before { get; set; }
        //    public string after { get; set; }
        //}

        public class Datum1S
        {
            public From from { get; set; }
            public DateTime created_time { get; set; }
            public string id { get; set; }
        }

        //public class From
        //{
        //    public string name { get; set; }
        //    public string id { get; set; }
        //}

    }
