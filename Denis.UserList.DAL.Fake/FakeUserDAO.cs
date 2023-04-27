using Denis.UserList.Common.Entities;

namespace Denis.UserList.DAL.Fake
{
    public class FakeUserDAO : IUserDAO
    {
        private readonly LinkedList<User> users;
        private int maxID;

        public FakeUserDAO()
        {
            maxID = 0;
            users = new LinkedList<User>();
            TryAddUser("Denis", new DateTime(2000, 10, 22), out _);
        }

        public bool TryAddUser(string name, DateTime birthDate, out int id)
        {
            id = ++maxID;
            users.AddLast(new User(id, name, birthDate));
            return true;
        }

        public IEnumerable<User> GetAllUsers()
        {
            foreach (var user in users)
            {
                yield return user;
            }
        }

        public bool TryDeleteUser(int userID)
        {
            foreach (var user in users)
            {
                if (user.ID == userID)
                {
                    users.Remove(user);
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<Award> GetAllAwards()
        {
            throw new NotImplementedException();
        }

        public bool TryAddAward(string name, out int awardID)
        {
            throw new NotImplementedException();
        }

        public void AddUserAward(int userID, int awardID)
        {
            throw new NotImplementedException();
        }

        public void AppicationClosedHandler()
        {
            throw new NotImplementedException();
        }
    }
}