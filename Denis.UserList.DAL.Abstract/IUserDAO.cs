using Denis.UserList.Common.Entities;

namespace Denis.UserList.DAL.Fake
{
    public interface IUserDAO
    {
        void UserAddAward(int userID, int awardID);
        IEnumerable<User> GetAllUsers();
        int AddUser(string name, DateTime birthDate);
        void DeleteUser(int userID);
    }
}