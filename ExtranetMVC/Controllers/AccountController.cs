using Extranet_EF;
using ExtranetMVC.CustomAuthentication;
using ExtranetMVC.Models;
using Newtonsoft.Json;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Extranet_MVC.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {

        public ActionResult testlogin()
        {
            return View();
        }
        public ActionResult InternetExplorer()
        {
            return View();
        }

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login(string ReturnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return LogOut();
            }
            ViewBag.ReturnUrl = ReturnUrl;
            LoginView login = new LoginView();
            login.ChangedPSW = false;
            ViewBag.Changed = false;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginView loginView, string ReturnUrl = "")
        {
            if (ModelState.IsValid)
            {
                loginView.ChangedPSW = false;
                loginView.UserName = loginView.UserName.TrimEnd();

                if (Membership.ValidateUser(loginView.UserName, loginView.Password))
                {
                    var user = (CustomMembershipUser)Membership.GetUser(loginView.UserName, false);
                    if (user == null)
                    {
                        ModelState.AddModelError("Error", "Something Wrong : Username or Password invalid!");
                        ViewBag.UserName = loginView.UserName;
                        ViewBag.PSW = loginView.Password;
                        return View(loginView);
                    }
                    string oldPassword = loginView.Password.ToString();
                    
                    
                    
                    if (loginView.NewPassword!=null)
                    {
                        string newPassword = loginView.NewPassword.ToString();
                        if (loginView.ConfirmPassword != newPassword)
                        {
                            ModelState.AddModelError("Error", "Something Wrong : Confirm Password doesn't match with new password!");
                            return View(loginView);
                        }

                        if (oldPassword == newPassword)
                        {
                            ModelState.AddModelError("Error", "Something Wrong : New Password can't be the same as the old one!");
                            //return View(loginView);
                        }
                        


                        if (newPassword.Length < 8)
                        {
                            ModelState.AddModelError("Error", "New password lenght must be 8 or more.");
                            //return View(loginView);
                        }
                        if (Regex.IsMatch(newPassword, @"^(?=.*[a-z])(?=.*[A-Z]).+$", RegexOptions.ECMAScript)==false) //both, lower and upper case
                        {
                            ModelState.AddModelError("Error", "New password must contains at least 1 Uppercase character (A-Z) and 1 Lowercase(a-z) character");
                            //return View(loginView);
                        }
                        if (Regex.IsMatch(newPassword, @"[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript)==false) //^[A-Z]+$
                        {
                            ModelState.AddModelError("Error", "New password must contain at least 1 special character");
                            //return View(loginView);
                        }
                        if(ModelState.IsValid==false)
                        {
                            ViewBag.UserName = loginView.UserName;
                            ViewBag.PSW = loginView.Password;

                            return View(loginView);

                        }
                        user.ChangePassword(oldPassword, newPassword);
                        //var userp = dbContext.Users.Where(x => x.Username == loginView.UserName).FirstOrDefault();
                        //userp.Password = newPassword;
                        //dbContext.Entry(user).State = EntityState.Modified;
                        //dbContext.SaveChanges();
                        //loginView.ChangedPSW = true;
                        CustomSerializeModel userModel = new CustomSerializeModel()
                        {
                            UserId = user.UserId,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            RoleName = user.Roles.Select(r => r.RoleName).ToList(),
                            DOI = user.DOI
                        };

                        string userData = JsonConvert.SerializeObject(userModel);
                        bool remember = false;
                        DateTime scadenza = DateTime.Now.AddMinutes(15);
                        if (loginView.RememberMe == true)
                        {
                            remember = true;
                            scadenza.AddYears(1);
                        }
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                     (

                     1, loginView.UserName, DateTime.Now, scadenza, remember, userData //v1.7 : per rimanere connessi
                     );

                        string enTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie faCookie = new HttpCookie("edi_auth", enTicket);
                        Response.Cookies.Add(faCookie);
                        ViewBag.Changed = true;
                        return View(loginView);


                        //aggiungere pagina conferma cambio psw

                    }


                    
                    if (user != null)
                    {
                        //if (loginView.NewPassword.ToString() != " " && loginView.ConfirmPassword.ToString() != " ")
                        //{
                        //    Membership.ChangePassword(loginView.UserName, loginView.NewPassword, loginView.ConfirmPassword);

                        //}

                        CustomSerializeModel userModel = new CustomSerializeModel()
                        {
                            UserId = user.UserId,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            RoleName = user.Roles.Select(r => r.RoleName).ToList(),
                            DOI = user.DOI
                        };

                        string userData = JsonConvert.SerializeObject(userModel);
                        bool remember = false;
                        DateTime scadenza = DateTime.Now.AddMinutes(15);
                        if (loginView.RememberMe == true)
                        {
                            remember = true;
                            scadenza.AddYears(1);
                        }
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                     (

                     1, loginView.UserName, DateTime.Now, scadenza, remember, userData //v1.7 : per rimanere connessi
                     );

                        string enTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie faCookie = new HttpCookie("edi_auth", enTicket);
                        Response.Cookies.Add(faCookie);
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {

                            return RedirectToAction("Index");
                        }
                    }


                }
            }
            ModelState.AddModelError("Error", "Something Wrong : Username or Password invalid");
            return View(loginView);
        }







        //[HttpGet]
        //public ActionResult Registration()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Registration(RegistrationView registrationView)
        //{
        //    bool statusRegistration = false;
        //    string messageRegistration = string.Empty;

        //    if (ModelState.IsValid)
        //    {
        //        // Email Verification
        //        string userName = Membership.GetUserNameByEmail(registrationView.Email);
        //        if (!string.IsNullOrEmpty(userName))
        //        {
        //            ModelState.AddModelError("Warning Email", "Sorry: Email already Exists");
        //            return View(registrationView);
        //        }

        //        //Save User Data 
        //        using (ExtranetDB dbContext = new ExtranetDB())
        //        {
        //            var user = new Users()
        //            {
        //                Username = registrationView.Username,
        //                FirstName = registrationView.FirstName,
        //                LastName = registrationView.LastName,
        //                Email = registrationView.Email,
        //                Password = registrationView.Password,
        //                ActivationCode = Guid.NewGuid(),
        //            };

        //            dbContext.Users.Add(user);
        //            dbContext.SaveChanges();
        //        }

        //        //Verification Email
        //        VerificationEmail(registrationView.Email, registrationView.ActivationCode.ToString());
        //        messageRegistration = "Your account has been created successfully. ^_^";
        //        statusRegistration = true;
        //    }
        //    else
        //    {
        //        messageRegistration = "Something Wrong!";
        //    }
        //    ViewBag.Message = messageRegistration;
        //    ViewBag.Status = statusRegistration;

        //    return View(registrationView);
        //}

        //[HttpGet]
        //public ActionResult ActivationAccount(string id)
        //{
        //    bool statusAccount = false;
        //    using (ExtranetDB dbContext = new ExtranetDB())
        //    {
        //        var userAccount = dbContext.Users.Where(u => u.ActivationCode.ToString().Equals(id)).FirstOrDefault();

        //        if (userAccount != null)
        //        {
        //            userAccount.IsActive = true;
        //            dbContext.SaveChanges();
        //            statusAccount = true;
        //        }
        //        else
        //        {
        //            ViewBag.Message = "Something Wrong !!";
        //        }

        //    }
        //    ViewBag.Status = statusAccount;
        //    return View();
        //}

        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie("edi_auth", "");
            cookie.Expires = DateTime.Now.AddYears(-2);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }

        //[NonAction]
        //public void VerificationEmail(string email, string activationCode)
        //{
        //    var url = string.Format("/Account/ActivationAccount/{0}", activationCode);
        //    var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, url);

        //    var fromEmail = new MailAddress("edp@dellorto.it", "Activation Account - Dellorto Extranet");
        //    var toEmail = new MailAddress(email);

        //    var fromEmailPassword = "******************";
        //    string subject = "Activation Account !";

        //    string body = "<br/> Please click on the following link in order to activate your account" + "<br/><a href='" + link + "'> Activation Account ! </a>";

        //    var smtp = new SmtpClient
        //    {
        //        Host = "192.168.23.69",
        //        Port = 29,
        //        EnableSsl = false,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
        //    };

        //    using (var message = new MailMessage(fromEmail, toEmail)
        //    {
        //        Subject = subject,
        //        Body = body,
        //        IsBodyHtml = true

        //    })

        //        smtp.Send(message);

        //}
        [HttpGet]
        public ActionResult Loginbck(string ReturnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return LogOut();
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Loginbck(LoginView loginView, string ReturnUrl = "")
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(loginView.UserName, loginView.Password))
                {
                    var user = (CustomMembershipUser)Membership.GetUser(loginView.UserName, false);
                    if (user != null)
                    {
                        CustomSerializeModel userModel = new CustomSerializeModel()
                        {
                            UserId = user.UserId,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            RoleName = user.Roles.Select(r => r.RoleName).ToList()
                        };

                        string userData = JsonConvert.SerializeObject(userModel);
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                            (
                            1, loginView.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
                            );

                        string enTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie faCookie = new HttpCookie("edi_auth", enTicket);
                        Response.Cookies.Add(faCookie);
                    }

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            ModelState.AddModelError("", "Something Wrong : Username or Password invalid ^_^ ");
            return View(loginView);
        }


    }
}