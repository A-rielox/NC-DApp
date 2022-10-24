using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using Microsoft.AspNetCore.Authorization;
using API.Entities;

namespace API.Controllers
{
    public class BuggyController : BaseController
    {
        private readonly DataContext _context;

        public BuggyController(DataContext context)
        {
            _context = context;
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        // GET: api/buggy/auth
        [HttpGet("auth")]
        [Authorize]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        // GET: api/buggy/not-found
        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _context.AppUsers.Find(-1);

            if (thing == null) return NotFound();

            return Ok(thing);
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        // GET: api/buggy/server-error
        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            // me retorna null y al aplicarle un metodo ( .ToString() ) da una excepcion ( null reference
            // exception )
            var thing = _context.AppUsers.Find(-1);

            var thingToReturn = thing.ToString();

            return thingToReturn;
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        // GET: api/buggy/bad-request
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This was not a good request.");
        }
    }
}
