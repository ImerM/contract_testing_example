using ProviderAPI.Model;
using System.Collections.Generic;

namespace ProviderAPI.Repositories
{
    public interface IUserRepository
    {
        public List<User> List();
        public User Get(int id);
        public void SetState(List<User> users);
    }
}
