using Microsoft.AspNetCore.Mvc;
using API.Data;
using System.Collections.Generic;
using API.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){
            // var users = _context.Users.ToList();
            // return users; this also works

            //return _context.Users.ToListAsync().Result; this also works
            return await _context.Users.ToListAsync(); //best practice
        }

        [Authorize]        
        [HttpGet("{id}")] //api/users/3
        public async Task<ActionResult<AppUser>> GetUsers(int id){
            // var users = _context.Users.Find(id);
            // return users;

            //return _context.Users.Find(id);
            return await _context.Users.FindAsync(id);
        }
    }
}