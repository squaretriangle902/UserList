using Denis.UserList.Common.Entities;
using Denis.UserList.Common.Libraries;
using Denis.UserList.DAL.Fake;
using Denis.UserList.DAL.File;

namespace Denis.UserList.BLL.Core
{
    public class UserListLogic : IUserListLogic
    {
        private const int maxAge = 150;
        private readonly IUserDAO userDAO;
        private readonly IAwardDAO awardDAO;

        public UserListLogic()
        {
            userDAO = new UserDAO();
            awardDAO = new AwardDAO();
        }

        public bool TryDeleteUser(int userID)
        {
            return userDAO.TryDeleteUser(userID);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return userDAO.GetAllUsers();
        }

        public User GetUser(int userID)
        {
            var user = GetAllUsers().First(user => user.ID == userID);
            foreach (var award in awardDAO.GetAwardsByUserID(user.ID))
            {
                user.AddAward(award);
            }
            return user;
        }

        public bool TryAddUser(string? name, DateTime birthDate, out int userID)
        {
            if (string.IsNullOrEmpty(name) || birthDate >= DateTime.Now ||
                DateTimeAdditional.CompleteYearDifference(birthDate, DateTime.Now) > maxAge)
            {
                userID = -1;
                return false;
            }
            return userDAO.TryAddUser(name, birthDate, out userID);
        }

        public IEnumerable<Award> GetAllAwards()
        {
            var awards = awardDAO.GetAllAwards();
            return awards;
        }

        public bool TryAddAward(string? name, out int awardID)
        {
            if (string.IsNullOrEmpty(name))
            {
                awardID = -1;
                return false;
            }
            return awardDAO.TryAddAward(name, out awardID);
        }

        public void AddUserAward(int userID, int awardID)
        {
            userDAO.AddUserAward(userID, awardID);
        }

        public void AppicationClosedHandler()
        {
            userDAO.AppicationClosedHandler();
            awardDAO.AppicationClosedHandler();
        }
    }
}