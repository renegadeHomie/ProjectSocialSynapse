using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookLoginMVC.Models
{

    public class CommentsCount
    {
        public B[] data { get; set; }
        public Paging paging { get; set; }
        public SummaryC summary { get; set; }
    }

    public class SummaryC
    {
        public string order { get; set; }
        public int total_count { get; set; }
        public bool can_comment { get; set; }
    }

    public class  B
    {
        public DateTime created_time { get; set; }
        public From from { get; set; }
        public string message { get; set; }
        public string id { get; set; }
    }

    //public class From
    //{
    //    public string name { get; set; }
    //    public string id { get; set; }
    //}
}
