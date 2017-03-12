using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using hitfit.app.models;
using Microsoft.AspNetCore.Http.Features.Authentication;

namespace hitfit.app.controllers
{
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly HitFitDbContext _context;

        public UserController(HitFitDbContext context)
        {
            _context = context;
        }

        // GET api/uesrs
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return _context.Users;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public User GetUser(int id)
        {
            return null; // _context.Users.FirstOrDefault(u => u.Id == id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
