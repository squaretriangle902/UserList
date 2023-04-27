using Denis.UserList.Common.Entities;

namespace Denis.UserList.DAL.Fake
{
    public interface IUserDAO
    {
        void AddUserAward(int userID, int awardID);
        void AppicationClosedHandler();
        IEnumerable<User> GetAllUsers();
        bool TryAddUser(string name, DateTime birthDate, out int id);
        bool TryDeleteUser(int userID);
    }
}