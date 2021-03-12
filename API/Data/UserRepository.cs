using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await _context.Users
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(p => p.Photos).SingleOrDefaultAsync(x => x.UserName == username);
            //Now, this is going to give us a problem and 
            //it's going to give us a "circular reference problem" 
            //because if we take a look at our entities. 
            //our app user has a collection of photos, 
            //so that's what we're going to return now that we add this "eagle loading". 
            //If we get to our photos collection or photos entity, 
            //then we have an app user inside here. 
            //And what's going to be returned is for each photo, 
            //it's going to attempt  to return an app user. 
            //And our app user has a collection of photos and our photo has an app user, etc, etc. 
            //This is going to give us a circular reference exception. 
            //It's not going to do any harm. 
            //We're not going to break the Internet by running this request. 
            //But let's give this a go and see what happens. 
            //Now we need to return our photos. that's a fact.
            //And what we do to solve this problem is we simply shape our data before we return it. 
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
                .Include(p => p.Photos)
                .ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            //If something has changed, something has been saved, 
            //then it's going to return a value greater than zero 
            //because the "SaveChangesAsync" returns an integer from this particular method 
            //for a number of changes that have been saved in a database. 
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            //Now, this one's a bit different 
            //because we're not actually changing anything in the database. 
            //But what we are going to do is mark this entity as it has been modified.

            _context.Entry(user).State = EntityState.Modified;
        }

        /* public async Task DeleteUser(AppUser member)
        {
            _context.Users.Remove(member);            
            await _context.SaveChangesAsync();               
        } */

        public async Task DeleteUser(string username) //GREAT WORK MY MATE! in MyCreations
        {
            /* _context.Users.Remove(member => {
                member => _context.Users.Where(member.username == username)
            });            
            await _context.SaveChangesAsync();   */         

            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if(user != null)
            {
                _context.Entry(user).State= EntityState.Deleted;
                await _context.SaveChangesAsync();
            }    
        }
    }
}