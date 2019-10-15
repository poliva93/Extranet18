using AutoMapper;
using Extranet_EF;
using ExtranetMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

namespace ExtranetMVC.Api
{
    public class UserController : ApiController
    {
        private ExtranetDB db = new ExtranetDB();

        [HttpGet]
        [Route("api/user/{username}")]
        //20181114 : commentata perchè ritornava tutti i campi di user. Lo uso solo per il nome.
        //public IHttpActionResult users(string username)
        //{
        //    var user = db.Users.Where(u => u.Username == username).FirstOrDefault();
        //    if(user == null)
        //    {
        //       return Content(HttpStatusCode.NoContent, "Utente non trovato");
        //    }
        //    return Ok(Mapper.Map<User>(user));
        //}
        public IHttpActionResult users(string username)
        {
            var user = db.Users.Where(u => u.Username == username).FirstOrDefault();
            if (user == null)
            {
                return Content(HttpStatusCode.NoContent, "Utente non trovato");
            }
            return Ok(new { Username = user.Username, FirstName = user.FirstName });
        }
    }
}
