using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using hitfit.app.models;

namespace hitfit.app.controllers
{
    [Route("api/users")]
    public class CommonController : Controller
    {
        private readonly HitFitDbContext _context;

        public CommonController(HitFitDbContext context)
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
