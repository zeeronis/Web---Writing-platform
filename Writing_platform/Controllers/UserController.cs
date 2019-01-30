using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Writing_platform.Models;

namespace Writing_platform.Controllers
{
    public class UserController : Controller
    {

        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return Redirect("/home");

            return View(new Models.User());
        }

        [HttpPost]
        public ActionResult Register(Models.User user)
        {
            if (User.Identity.IsAuthenticated) return Redirect("/home");

            using (ReadBookDB db = new ReadBookDB())
            {
                if(db.Users.Any(x => x.UserName == user.UserName))
                {
                    ViewBag.ErrorMessage = "User Name Already Exist";
                    return View("Register", user);
                }
                if (db.Users.Any(x => x.Email == user.Email))
                {
                    ViewBag.ErrorMessage = "Email Already Exist";
                    return View("Register", user);
                }

                db.Users.Add(user);
                db.SaveChanges();
            }

            ViewBag.SuccessMessage = "Registration Suссessful.";
            FormsAuthentication.SetAuthCookie(user.UserName, false);
            return Redirect("/home");
        }


        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return Redirect("/home");

            return View(new User());
        }

        [HttpPost]
        public ActionResult Login(Models.User _user, string ReturnUrl)
        {
            if (User.Identity.IsAuthenticated) return Redirect("/home");

            using (ReadBookDB db = new ReadBookDB())
            {
                var user = db.Users.FirstOrDefault(x => x.UserName == _user.UserName);
                if(user == null || user.UserName == "DELETED")
                {
                    ViewBag.ErrorMessage = "Login not existed";
                    return View();
                }
                if (user.Password != _user.Password)
                {
                    ViewBag.ErrorMessage = "Wrong password";
                    return View();
                }
            }
            
            FormsAuthentication.SetAuthCookie(_user.UserName, false);
            if(ReturnUrl == null || ReturnUrl == String.Empty)
            {
                return Redirect("/home");
            }
            return Redirect(ReturnUrl);
        }

        public ActionResult Profile(string id)
        {
            using (ReadBookDB db = new ReadBookDB())
            {
                var user = new Models.User();
                GetCurrentUser(db, out Models.User currUser);
                if (Int32.TryParse(id, out int ID))
                {
                    user = db.Users.FirstOrDefault(x => x.UserID == ID);
                    if (user != null)
                    {
                        if(user.UserID == currUser.UserID)
                        {
                            ViewBag.IsCurrentUser = true;
                        }
                        else
                        {
                            ViewBag.IsCurrentUser = false;
                        }

                        return View(user);
                    }
                }
                else if (User.Identity.IsAuthenticated)
                {
                    ViewBag.IsCurrentUser = true;
                    return Redirect("/user/profile/" + currUser.UserID);
                }
            }
            return Redirect("/shared/error");
        }

        [Authorize]
        public ActionResult EditProfile()
        {
            return View(new Models.EditUser());
        }

        [HttpPost, ActionName("EditProfileEmail")]
        [Authorize]
        public ActionResult EditProfileEmail(Models.EditUser _user)
        {
            using (ReadBookDB db = new ReadBookDB())
            {
                if (db.Users.Any(x => x.Email == _user.Email))
                {
                    ViewBag.ErrorMessage = "Email Already Exist.";
                    return View("EditProfile");
                }

                if (_user.CurrentEmailPassword != GetCurrentUser(db, out Models.User currUser).Password)
                {
                    ViewBag.ErrorMessage = "Wrong password";
                    return View("EditProfile");
                }

                ViewBag.SuccessMessage = "Email changed.";
                currUser.Email = _user.Email;
                currUser.ConfirmPassword = _user.CurrentEmailPassword;
                db.Entry(currUser).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View("EditProfile");
        }

        [HttpPost, ActionName("EditProfilePassword")]
        [Authorize]
        public ActionResult EditProfilePassword(Models.EditUser _user)
        {
                using (ReadBookDB db = new ReadBookDB())
                {
                    if (_user.CurrentPassword != GetCurrentUser(db, out Models.User currUser).Password)
                    {
                        ViewBag.ErrorMessage = "Wrong password";
                        return View("EditProfile");
                    }

                    ViewBag.SuccessMessage = "Password changed.";
                    currUser.Password = _user.NewPassword;
                    currUser.ConfirmPassword = _user.NewPassword;
                    db.Entry(currUser).State = EntityState.Modified;
                    db.SaveChanges();
                }
            return View("EditProfile");
        }

        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("/home");
        }


        [Authorize]
        public ActionResult DeleteProfile()
        {
            return View(new EditUser());
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteProfile(EditUser user)
        {
            using (ReadBookDB db = new ReadBookDB())
            {
                if (GetCurrentUser(db, out Models.User currUser).Password == user.CurrentPassword)
                {
                    FormsAuthentication.SignOut();
                    currUser.UserName = "DELETED";
                    currUser.Password = "D";
                    currUser.ConfirmPassword = "D";
                    currUser.Email = "D";
                    db.Entry(currUser).State = EntityState.Modified;
                    db.SaveChanges();
                    return Redirect("/home");
                }
            }
            
            ViewBag.ErrorMessage = "Wrong password";
            return View(new EditUser());
        }

        public Models.User GetCurrentUser()
        {
            var user = new Models.User();
            using (ReadBookDB db = new ReadBookDB())
            {
                GetCurrentUser(db, out user);
            }
            return user;
        }

        public Models.User GetCurrentUser(ReadBookDB db, out Models.User user)
        {
            if (User.Identity.IsAuthenticated)
            {
                return user = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            }
            return user = new Models.User();
        }
    }
}