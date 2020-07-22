using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WallProj.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace WallC.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext=context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost("Register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();
                HttpContext.Session.SetString("FirstName", newUser.FirstName);
                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                return RedirectToAction("Wall");
            }
            else
            {
                return View("Index");
            }
        }
        [HttpPost("login")]
        public IActionResult Login(UserLogin userSubmission)
        {
            if(ModelState.IsValid)
            {
                // List<User>AllUsers=dbContext.Users.ToList();
                var hasher = new PasswordHasher<UserLogin>();
                var signedInUser = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.LoginEmail);
                
                if(signedInUser == null)
                {
                    ViewBag.Message="Email/Password is invalid";
                    return View("Index");
                }
                else
                {
                    var result = hasher.VerifyHashedPassword(userSubmission, signedInUser.Password, userSubmission.LoginPassword);
                    if(result==0)
                    {
                        ViewBag.Message="Email/Password is invalid";
                        return View("Index");
                    }
                }
                
                
                HttpContext.Session.SetInt32("UserId", signedInUser.UserId);
                return RedirectToAction("Wall");
            }
            else
            {
                return View("Index");
            }
            
        }
        [HttpGet("Wall")]
        public IActionResult Wall()
        {
            User userInDb=dbContext.Users.FirstOrDefault(u=>u.UserId==(int)HttpContext.Session.GetInt32("UserId"));
            ViewBag.AllMessages = dbContext.Messages
                                        .Include(fd=>fd.MessageCreator)
                                        .Include(ds=>ds.Comments)
                                        .ThenInclude(u=>u.NavUser)
                                        .OrderByDescending(f=>f.MessageId)
                                        .ToList();
            if(userInDb==null)
            {
                return RedirectToAction("Logout");
            }
            else
            {
                ViewBag.AllMessages = dbContext.Messages
                                        .Include(fd=>fd.MessageCreator)
                                        .Include(ds=>ds.Comments)
                                        .ThenInclude(u=>u.NavUser)
                                        .OrderByDescending(f=>f.MessageId)
                                        .ToList();
                ViewBag.User=userInDb;
                return View("Wall");
            }
            
        }
        [HttpPost("Wall")]
        public IActionResult CreateMessage(Message NewMessage)
        {
            if(ModelState.IsValid)
            {
                int? userInDb=HttpContext.Session.GetInt32("UserId");
                dbContext.Messages.Add(NewMessage);
                NewMessage.UserId=(int)userInDb;
                dbContext.SaveChanges();
                return Redirect("/Wall");
            }else
            {
                ViewBag.AllMessages = dbContext.Messages
                                        .Include(fd=>fd.MessageCreator)
                                        .Include(ds=>ds.Comments)
                                        .ThenInclude(u=>u.NavUser)
                                        .OrderByDescending(f=>f.MessageId)
                                        .ToList();
            return View("Wall");

            }
        }
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }
        [HttpPost("CreateComment/{MessageId}/")]
        public IActionResult CreateComment(Comment NewComment, int MessageId)
        {
            if(ModelState.IsValid)
            {
                int? userInDb=HttpContext.Session.GetInt32("UserId");
                dbContext.Comments.Add(NewComment);
                NewComment.UserId= (int)userInDb;
                NewComment.MessageId = MessageId;
                ViewBag.MessageId = MessageId;
                dbContext.SaveChanges();
                return Redirect("/Wall");
            }else
            {
                ViewBag.AllMessages = dbContext.Messages
                                        .Include(fd=>fd.MessageCreator)
                                        .Include(ds=>ds.Comments)
                                        .ThenInclude(u=>u.NavUser)
                                        .OrderByDescending(f=>f.MessageId)
                                        .ToList();
            return View("Wall");

            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}