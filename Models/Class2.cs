using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookLoginMVC.Models
{
    
     public class Class2
     {
        


        public PostsC posts { get; set; }
        public string id { get; set; }
  
     }

        public class PostsC
        {
        public DatumC[] data { get; set; }
        public Paging paging { get; set; }
        }

        //public class Paging
        //{
        //public string previous { get; set; }
        //public string next { get; set; }
        //}

        public class DatumC
        {
            public int commentsCount { get; set; }
        public string id { get; set; }
        public string permalink_url { get; set; }
        public Comments comments { get; set; }
        }

        public class Comments
        {
        public Datum1C[] data { get; set; }
        public Paging paging { get; set; }
        }

        //public class Paging1
        //{
        //public Cursors cursors { get; set; }
        //}

        //public class Cursors
        //{
        //public string before { get; set; }
        //public string after { get; set; }
        //}

        public class Datum1C
        {
        public DateTime created_time { get; set; }
        public From from { get; set; }
        public string message { get; set; }
        public string id { get; set; }
        }

        public class From
        {

        public string name { get; set; }
        public string id { get; set; }
        

    }
}