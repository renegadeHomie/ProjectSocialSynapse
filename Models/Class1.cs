using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookLoginMVC.Models
{

        public class Class1
        {
        public Posts posts { get; set; }
        public string id { get; set; }
        }

        public class Posts
        {
        public Datum[] data { get; set; }
        public Paging paging { get; set; }
        }

       

        public class Datum
        {
        public DateTime created_time { get; set; }
        public string permalink_url { get; set; }
        public string id { get; set; }
        public Reactions reactions { get; set; }
        public int total_count { get; set; }
        }

        public class Reactions
        {
        public Datum1[] data { get; set; }
        public Paging paging { get; set; }
       
        }

        public class Datum1
        {
            public string id { get; set; }
            public string name { get; set; }
            public string link { get; set; }
            public string pic_large { get; set; }
            public string type { get; set; }
        }

        //public class Paging1
        //{
        //public Cursors cursors { get; set; }
        //public string next { get; set; }
        //}

        //public class Cursors
        //{
        //public string before { get; set; }
        //public string after { get; set; }
        //}
        //public class Paging
        //{
        //public string previous { get; set; }
        //public string next { get; set; }
        //}

       


    }