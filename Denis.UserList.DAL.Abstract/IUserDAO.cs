using Denis.UserList.Common.Entities;

namespace Denis.UserList.DAL.Fake
{
    public interface IUserDAO
    {
        void UserAddAward(int userID, int awardID);
        IEnumerable<User> GetAllUsers();
        int AddUser(User user);
        void DeleteUser(int userID);
    }
}