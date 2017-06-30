using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FacebookLoginMVC.Controllers;

namespace FacebookLoginMVC.Models
{
    public class ViewModel
    {
        public List<CommentsData> commentsData { get; set; }
        public List<ReactionsData> reactionsData { get; set; }
        public List<SharesData> sharesData { get; set; }
        public string mostLikedPost { get; set; }
        public string mostSharedPost { get; set; }
        public string mostCommentedPost { get; set; }
    }
}