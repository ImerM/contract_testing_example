using ProviderAPI.Model;
using System.Collections.Generic;
namespace ProviderAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private List<User> State { get; set; }

        public UserRepository()
        {
            State = new List<User>()
            {
                new User(3, "Jack Smith", "Admin", "V2"),
                new User(8, "John Doe", "Moderator", "V1")
            };
        }

        public void SetState(List<User> state)
        {
            this.State = state;
        }

        List<User> IUserRepository.List()
        {
            return State;
        }

        public User Get(int id)
        {
            return State.Find(q => q.id == id);
        }
    }
}
