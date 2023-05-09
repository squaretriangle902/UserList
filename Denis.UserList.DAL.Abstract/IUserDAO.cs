using Denis.UserList.Common.Entities;

namespace Denis.UserList.DAL.Fake
{
    public interface IUserDAO
    {
        public int MaxUserID { get; }
        void AddUserAwards(IEnumerable<User> users);
        IEnumerable<User> GetAllUsers();
        public void AddUsers(IEnumerable<User> users);
        void DeleteUser(int userID);
    }
}