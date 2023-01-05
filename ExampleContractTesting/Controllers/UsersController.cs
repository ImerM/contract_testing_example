using Microsoft.AspNetCore.Mvc;
using ProviderAPI.Repositories;
using ProviderAPI.Model;
using System.Collections.Generic;

namespace ProviderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository Repository;


        public UsersController(IUserRepository userRepository)
        {
            this.Repository = userRepository;
        }

        // GET /api/users
        [HttpGet(Name = "GetAllUsers")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            List<User> users = Repository.List();
            return users;
        }

        // GET /api/users/{id}
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = Repository.Get(id);
            if (user == null)
            {
                return new NotFoundResult();
            }
            return user;
        }
    }
}