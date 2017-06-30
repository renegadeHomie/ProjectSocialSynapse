using Facebook;
using FacebookLoginMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using System.Dynamic;

namespace FacebookLoginMVC.Controllers
{
    public class HomeController : Controller
    {


        public ActionResult Index()
        {
            var User = TempData["userModel"] as MyAppUser;

            if (User != null)
            {

                return View(User);

            }
            else
                return View();
        }

        public ActionResult GetSharesURLView()
        {
            return PartialView("PartialView/GetSharesURLView");
        }

        public ActionResult ViewRanking()
        {
            var list = TempData["ReactionsList"] as List<ReactionsData>;
            var listC = TempData["CommentsList"] as List<CommentsData>;


            var listS = TempData["SharesList"] as List<SharesData>;
            string mostSharedPost = TempData["mostSharedPost"] as string;
            string mostLikedPost = TempData["mostLikedUrl"] as string;
            string mostCommentedPost = TempData["mostCommentedUrl"] as string;

            ViewModel viewModel = new ViewModel();


            viewModel.sharesData = listS;
            viewModel.mostSharedPost = mostSharedPost;
            viewModel.commentsData = listC;
            viewModel.reactionsData = list;
            viewModel.mostLikedPost = mostLikedPost;
            viewModel.mostCommentedPost = mostCommentedPost;
            return PartialView("PartialView/LoadPartial", viewModel);

        }



        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }





        [AllowAnonymous]
        public ActionResult Facebook()
        {
            var fb = new FacebookClient();
            string permissions = "";


            permissions = "email,user_friends,user_photos,user_posts,user_birthday,user_location";

            var loginUrl = fb.GetLoginUrl(new
            {

                client_id = "YOUR_CLIENT_ID",
                client_secret = "YOUR_CLIENT_SECRET",
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = permissions

            });
            return Redirect(loginUrl.AbsoluteUri);
        }




        public ActionResult FacebookCallback(string since, string until, string code, string taskName = "", string accessToken = "")
        {
            var fb = new FacebookClient();
            string startDate = Convert.ToDateTime(since).ToString("yyyy-MM-dd");
            string endDate = Convert.ToDateTime(until).ToString("yyyy-MM-dd");


            //For Login  (Generate Access Token)
            if (accessToken == "")
            {

                dynamic result = fb.Post("oauth/access_token", new
                {
                    client_id = "YOUR_CLIENT_ID",
                    client_secret = "YOUR_CLIENT_SECRET",
                    redirect_uri = RedirectUri.AbsoluteUri,
                    code = code

                });
                accessToken = result.access_token;
            }

            //Create Session
            Session["AccessToken"] = accessToken;
            fb.AccessToken = accessToken;



            dynamic me = new ExpandoObject();
            MyAppUser user = new MyAppUser();
            Class1 info = new Class1();
            Class2 Info = new Class2();
            List<Datum1> listOfAllReactions = new List<Datum1>();
            List<Datum> listOfAllPosts = new List<Datum>();
            Posts postN = new Posts();
            string email = "";


            //Login API call
            if (taskName.Equals(""))
            {
                me = fb.Get("me?fields=link,birthday,location,friends{name,link,id,picture},email,picture{url},name,id,photos.limit(16){link,picture},first_name,last_name,gender");
                string res = me.ToString();
                user = JsonConvert.DeserializeObject<MyAppUser>(res);
                email = me.email;


            }


            //Generate Ranking
            else
            {
                //API Cal to get likes data
                me = fb.Get("me?fields=posts.since(" + startDate + ").until(" + endDate + "){created_time,permalink_url,reactions{link,name,pic_large}}");


                string res = me.ToString();

                info = JsonConvert.DeserializeObject<Class1>(res);

                List<ReactionsData> list = new List<ReactionsData>();
                Reactions reactionN = new Reactions();
                Reactions reactionNew = new Reactions();
                string pagingOld = "";
                string mostLikedUrl = "";

                #region Ranking Friends based on Likes
                if (info.posts != null)
                {

                    for (var i = 0; i < info.posts.data.Count(); i++)
                    {
                        Datum Obj = new Datum();
                        Obj = info.posts.data[i];
                        listOfAllPosts.Add(Obj);
                    }


                    postN = info.posts;



                    while (postN.data != null && postN.paging != null && postN.paging.next != null)
                    {

                        dynamic nextMe = fb.Get(postN.paging.next);
                        string res1 = Convert.ToString(nextMe);

                        Posts postNew = JsonConvert.DeserializeObject<Posts>(res1);

                        for (var i = 0; i < postNew.data.Count(); i++)
                        {
                            Datum Obj = new Datum();
                            Obj = postNew.data[i];
                            listOfAllPosts.Add(Obj);
                        }

                        postN = postNew;


                    }



                    foreach (Datum obj in listOfAllPosts)
                    {
                        if (obj.reactions != null)
                        {
                            for (var j = 0; j < obj.reactions.data.Count(); j++)
                            {
                                Datum1 reactionObj = new Datum1();
                                reactionObj = obj.reactions.data[j];
                                listOfAllReactions.Add(reactionObj);

                            }
                            reactionN = obj.reactions;

                            while (reactionN != null && reactionN.data != null && reactionN.paging != null && reactionN.paging.next != null && !(reactionN.paging.next.Equals(pagingOld)))
                            {
                                dynamic nextReactions = fb.Get(obj.reactions.paging.next);
                                string res2 = Convert.ToString(nextReactions);

                                reactionNew = JsonConvert.DeserializeObject<Reactions>(res2);

                                for (var i = 0; i < reactionNew.data.Count(); i++)
                                {
                                    Datum1 Obj = new Datum1();
                                    Obj = reactionNew.data[i];
                                    listOfAllReactions.Add(Obj);
                                }
                                pagingOld = reactionN.paging.next;
                                reactionN = reactionNew;
                            }
                        }
                    }
                    //Most liked post

                    foreach (Datum post in listOfAllPosts)
                    {
                        try
                        {
                            dynamic summary = fb.Get("/" + post.id + "/reactions?summary=total_count");

                            ReactionsCount countData = JsonConvert.DeserializeObject<ReactionsCount>(Convert.ToString(summary));
                            post.total_count = countData.summary.total_count;
                        }
                        catch { }

                    }
                    mostLikedUrl = listOfAllPosts.Where(x => x.reactions != null).OrderByDescending(x => x.total_count).First().permalink_url;




                    list = listOfAllReactions.GroupBy(item => new { name = item.name, id = item.id })
                                                 .Select(group => new ReactionsData()
                                                 {
                                                     id = group.Key.id,
                                                     name = group.Key.name,
                                                     link = group.First().link,
                                                     pic_large = group.First().pic_large,
                                                     count = group.Count()
                                                 })
                                                 .OrderByDescending(item => item.count).ToList();
                }

                #endregion


                #region Ranking friends based on Comments

                //Call Graph API
                dynamic ME = fb.Get("me?fields=posts.until(" + endDate + ").since(" + startDate + "){comments,permalink_url}");

                string RES = ME.ToString();
                Info = JsonConvert.DeserializeObject<Class2>(RES);
                List<Datum1C> listOfAllComments = new List<Datum1C>();
                List<DatumC> listOfAllPostsC = new List<DatumC>();
                PostsC postNC = new PostsC();
                List<CommentsData> listC = new List<CommentsData>();
                string mostCommentedUrl = "";

                if (Info.posts != null)
                {

                    for (var i = 0; i < Info.posts.data.Count(); i++)
                    {
                        DatumC Obj = new DatumC();
                        Obj = Info.posts.data[i];
                        listOfAllPostsC.Add(Obj);
                    }


                    postNC = Info.posts;



                    while (postNC.data != null && postNC.paging != null && postNC.paging.next != null)
                    {

                        dynamic nextMeC = fb.Get(postNC.paging.next);
                        string res1C = Convert.ToString(nextMeC);

                        PostsC postNewC = JsonConvert.DeserializeObject<PostsC>(res1C);

                        for (var i = 0; i < postNewC.data.Count(); i++)
                        {
                            DatumC Obj = new DatumC();
                            Obj = postNewC.data[i];
                            listOfAllPostsC.Add(Obj);
                        }

                        postNC = postNewC;


                    }


                    Comments commentsN = new Comments();
                    string pagingOldC = "";

                    foreach (DatumC obj in listOfAllPostsC)
                    {
                        if (obj.comments != null)
                        {
                            for (var j = 0; j < obj.comments.data.Count(); j++)
                            {
                                Datum1C commentObj = new Datum1C();
                                commentObj = obj.comments.data[j];
                                listOfAllComments.Add(commentObj);

                            }
                            commentsN = obj.comments;

                            while (commentsN != null && commentsN.data != null && commentsN.paging != null && commentsN.paging.next != null && !(commentsN.paging.next.Equals(pagingOldC)))
                            {
                                dynamic nextComments = fb.Get(obj.comments.paging.next);
                                string res2C = Convert.ToString(nextComments);

                                Comments commentsNew = JsonConvert.DeserializeObject<Comments>(res2C);

                                for (var i = 0; i < commentsNew.data.Count(); i++)
                                {
                                    Datum1C Obj = new Datum1C();
                                    Obj = commentsNew.data[i];
                                    listOfAllComments.Add(Obj);
                                }
                                pagingOldC = commentsN.paging.next;
                                commentsN = commentsNew;
                            }
                        }
                    }


                    //Most commented post
                    foreach (DatumC post in listOfAllPostsC)
                    {
                        try
                        {
                            dynamic summary = fb.Get("/" + post.id + "/comments?summary=1");

                            CommentsCount countCommentsData = JsonConvert.DeserializeObject<CommentsCount>(Convert.ToString(summary));
                            post.commentsCount = countCommentsData.summary.total_count;
                        }
                        catch { }


                    }
                    try
                    {
                        mostCommentedUrl = listOfAllPostsC.Where(x => x.comments != null).OrderByDescending(x => x.commentsCount).First().permalink_url;
                    }
                    catch { }


                    listC = listOfAllComments.GroupBy(item => new { name = item.from.name, id = item.from.id })
                                                 .Select(group => new CommentsData()
                                                 {
                                                     id = group.Key.id,
                                                     name = group.Key.name,
                                                     //link = group.First().link,
                                                     //pic_large = group.First().pic_large,
                                                     count = group.Count()
                                                 })
                                                 .OrderByDescending(item => item.count).ToList();
                }
                #endregion

                #region Ranking based on Shares


                dynamic Me = fb.Get("me?fields=posts.since(" + startDate + ").until(" + endDate + "){created_time,permalink_url,shares,sharedposts{from}}");

                Class3 infoShares = JsonConvert.DeserializeObject<Class3>(Convert.ToString(Me));

                List<DatumS> listOfAllSharedPosts = new List<DatumS>();

                PostsS postNS = new PostsS();
                List<Datum1S> listOfAllShares = new List<Datum1S>();
                Sharedposts sharedPosts = new Sharedposts();
                Sharedposts sharePostsNew = new Sharedposts();
                string pagingOldS = "";
                List<SharesData> sharedWhoList = new List<SharesData>();
                string mostSharedPost = "";

                if (infoShares.posts != null)
                {

                    for (var i = 0; i < infoShares.posts.data.Count(); i++)
                    {
                        DatumS Obj = new DatumS();
                        Obj = infoShares.posts.data[i];
                        if (Obj.shares != null)
                        {

                            listOfAllSharedPosts.Add(Obj);

                        }
                    }


                    postNS = infoShares.posts;



                    while (postNS.data != null && postNS.paging != null && postNS.paging.next != null)
                    {
                        // string hope = postNS.paging.next.Replace("/v2.8/", "/v2.3/");
                        dynamic nextMeS = fb.Get(postNS.paging.next);
                        string res1S = Convert.ToString(nextMeS);

                        PostsS postNewS = JsonConvert.DeserializeObject<PostsS>(res1S);

                        for (var i = 0; i < postNewS.data.Count(); i++)
                        {
                            DatumS Obj = new DatumS();

                            Obj = postNewS.data[i];
                            if (Obj.shares != null)
                            {

                                listOfAllSharedPosts.Add(Obj);

                            }
                        }

                        postNS = postNewS;


                    }



                    foreach (DatumS Obj in listOfAllSharedPosts)
                   {
                        if (Obj.shares != null)
                        {

                            if ( Obj.sharedposts != null && Obj.sharedposts.data.Count() != 0)
                            {
                                foreach(Datum1S sharedObj in Obj.sharedposts.data)
                                {
                                    if (sharedObj.from != null)
                                    {
                                        listOfAllShares.Add(sharedObj);
                                    }
                                }

                            }

                        else 
                        { 

                            dynamic sharedBy = fb.Get("/" + Obj.id + "/sharedposts?fields=from");

                            try
                            {

                                Sharedposts sp = JsonConvert.DeserializeObject<Sharedposts>(sharedBy);

                                if (sp.data != null && sp != null)
                                {

                                    for (var j = 0; j < sp.data.Count(); j++)
                                    {
                                        Datum1S sharedObj = new Datum1S();
                                        sharedObj = sp.data[j];
                                        listOfAllShares.Add(sharedObj);

                                    }
                                    sharedPosts = sp;

                                    while (sharedPosts != null && sharedPosts.data != null && sharedPosts.paging != null && sharedPosts.paging.next != null && !(sharedPosts.paging.next.Equals(pagingOldS)))
                                    {
                                        dynamic nextShares = fb.Get(Obj.sharedposts.paging.next);
                                        string res2S = Convert.ToString(nextShares);

                                        sharePostsNew = JsonConvert.DeserializeObject<Sharedposts>(res2S);

                                        for (var i = 0; i < sharePostsNew.data.Count(); i++)
                                        {
                                            Datum1S obj = new Datum1S();
                                            obj = sharePostsNew.data[i];
                                            listOfAllShares.Add(obj);
                                        }
                                        pagingOldS = sharedPosts.paging.next;
                                        sharedPosts = sharePostsNew;
                                    }
                                }
                            }
                                
                            catch
                            {
                                try
                                {
                                    Datum1S[] data = JsonConvert.DeserializeObject<Datum1S[]>(sharedBy);
                                    if (data != null && data.Count() != 0)
                                    {

                                        for (var j = 0; j < data.Count(); j++)
                                        {
                                            Datum1S sharedObj = new Datum1S();
                                            sharedObj = data[j];
                                            listOfAllShares.Add(sharedObj);

                                        }
                                    }
                                }
                                catch{}
                            }
                            }
                        }
                                //Most Shared post


                                if (listOfAllSharedPosts.Count() != 0 && listOfAllShares.Count != 0)
                                {
                                    mostSharedPost = listOfAllSharedPosts.OrderByDescending(x => x.shares.count).First().permalink_url;

                                    sharedWhoList = listOfAllShares.GroupBy(item => new { name = item.from.name, id = item.from.id })
                                                                    .Select(group => new SharesData()
                                                                    {
                                                                        id = group.Key.id,
                                                                        name = group.Key.name,
                                                                        share_count = group.Count()
                                                                    }).OrderByDescending(item => item.share_count).ToList();


                                    foreach (SharesData obj in sharedWhoList)
                                    {
                                        if (list.Select(x => x.id).Contains(obj.id))
                                        {
                                            obj.pic_large = list.Where(x => x.id == obj.id).FirstOrDefault().pic_large;
                                            obj.link = list.Where(x => x.id == obj.id).FirstOrDefault().link;
                                        }
                                        else
                                        {
                                            dynamic pic = fb.Get("/" + obj.id + "?fields=picture");
                                            try
                                            {
                                                ProfilePicture pic_small = JsonConvert.DeserializeObject<ProfilePicture>(Convert.ToString(pic));
                                                obj.pic_large = pic_small.picture.data.url;

                                            }
                                            catch { }

                                            dynamic url = fb.Get("/" + obj.id + "?fields=link");
                                            try
                                            {
                                                FacebookProfile profile = JsonConvert.DeserializeObject<FacebookProfile>(Convert.ToString(url));
                                                obj.link = profile.link;
                                            }
                                            catch { }
                                        }
                                    }
                                    

                                    TempData["mostSharedPost"] = mostSharedPost;
                                    TempData["SharesList"] = sharedWhoList;

                                }
                            }


                               
                                    if (listOfAllSharedPosts.Count() != 0)
                                    {
                                        mostSharedPost = listOfAllSharedPosts.OrderByDescending(x => x.shares.count).First().permalink_url;
                                    }

                              



                            

                #endregion
                                try
                                {


                                    foreach (CommentsData obj in listC)
                                    {
                                        if (list.Select(x => x.id).Contains(obj.id))
                                        {
                                            obj.pic_large = list.Where(x => x.id == obj.id).FirstOrDefault().pic_large;
                                            obj.link = list.Where(x => x.id == obj.id).FirstOrDefault().link;
                                        }
                                        else
                                        {
                                            dynamic pic = fb.Get("/" + obj.id + "?fields=picture");
                                            try
                                            {
                                                ProfilePicture pic_small = JsonConvert.DeserializeObject<ProfilePicture>(Convert.ToString(pic));
                                                obj.pic_large = pic_small.picture.data.url;
                                            }
                                            catch { }


                                            dynamic url = fb.Get("/" + obj.id + "?fields=link");
                                            try
                                            {
                                                FacebookProfile profile = JsonConvert.DeserializeObject<FacebookProfile>(Convert.ToString(url));
                                                obj.link = profile.link;

                                            }
                                            catch { }


                                        }
                                    }
                                }
                                catch { }
                              
                            





                          
                        

                       


                    }
                   //Calculate overall ranking
                  //Algorithm
                 //Each comment has a weigtage = 5
                //each like = 2
                //each share = 6
                //List<OverallData> overAllRanking = new List<OverallData>();
                //foreach (ReactionsData obj in list)
                //{
                //    int score = 0;
                //    foreach (CommentsData OBJ in listC)
                //    {
                //        if (obj.id == OBJ.id)
                //        {
                //            OverallData fr = new OverallData();
                //            score = obj.count * 2 + OBJ.count * 5;
                //            fr.id = obj.id;
                //            fr.link = obj.link;
                //            fr.name = obj.name;
                //            fr.pic_large = obj.pic_large;
                //            fr.score = score;
                //            overAllRanking.Add(fr);

                //        }
                //    }
                //}

                    TempData["CommentsList"] = listC;

                    TempData["ReactionsList"] = list;
                    TempData["mostCommentedUrl"] = mostCommentedUrl;

                    TempData["mostLikedUrl"] = mostLikedUrl;
                    TempData["mostSharedPost"] = mostSharedPost;
                    TempData["SharesList"] = sharedWhoList;
                    TempData["status"] = "Ranking Results";
                    return RedirectToAction("ViewRanking", "Home");
                   
                }
            
            
            
            TempData["email"] = me.email;
            TempData["first_name"] = me.first_name;
            TempData["lastname"] = me.last_name;
            TempData["gender"] = me.gender;
            TempData["picture"] = me.picture.data.url;
            TempData["profile"] = me.link;
            try
            {
                TempData["location"] = me.location.name;

                TempData["birthday"] = me.birthday;
            }
            catch { }
            TempData["status"] = "Signed In";
            FormsAuthentication.SetAuthCookie(email, true);
            TempData["userModel"] = user;
            return RedirectToAction("Index", "Home");
           
        }
         
    }
}
