using Denis.UserList.Common.Entities;

namespace Denis.UserList.BLL.Core
{
    public interface IUserListLogic
    {
        IEnumerable<Award> GetAllAwards();
        IEnumerable<User> GetAllUsers();
        bool TryAddAward(string? name, out int awardID);
        bool TryAddUser(string? name, DateTime birthDate, out int id);
        bool TryDeleteUser(int userID);
        User GetUser(int userID);
        void AddUserAward(int userID, int awardID);
        void AppicationClosedHandler();
    }
}