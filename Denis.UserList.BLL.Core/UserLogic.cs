using Denis.UserList.Common.Entities;
using Denis.UserList.Common.Libraries;
using Denis.UserList.DAL.Fake;
using Denis.UserList.DAL.File;

namespace Denis.UserList.BLL.Core
{
    public class UserLogic : IUserLogic
    {
        private const int maxAge = 150;

        private readonly IUserDAO userDAO;
        private readonly Dictionary<int, User> userCache;
        private int LastID;

        public UserLogic()
        {
            userCache = new Dictionary<int, User>();
            LastID = 0;
            switch (Common.ReadConfigFile("user_database"))
            {
                case "userDAO":
                    userDAO = new UserDAO();
                    break;
                default:
                    throw new BLLException("Cannot get user database");
            }
        }

        public void DeleteUser(int userID)
        {
            if (userCache.Remove(userID))
            {
                return;
            }
            try
            {
                userDAO.DeleteUser(userID);
            }
            catch (Exception exception)
            {
                throw new BLLException("Cannot delete user", exception);
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            return userCache.Values.Union(GetAllUsersFromDatabase());
        }

        public User GetUser(int userID)
        {
            if (userCache.TryGetValue(userID, out User? user))
            {
                return user;
            }
            return GetUserFromDatabase(userID);
        }



        public int AddUser(string? name, DateTime birthDate)
        {
            if (string.IsNullOrEmpty(name) || 
                birthDate >= DateTime.Now || 
                DateTimeAdditional.CompleteYearDifference(birthDate, DateTime.Now) > maxAge)
            {
                throw new BLLException("Incorrect user state");
            }
            LastID--;
            userCache.Add(LastID, new User(LastID, name, birthDate));
            return LastID;
        }

        public void UserAddAward(int userID, int awardID, IAwardLogic awardLogic)
        {
            if (userCache.TryGetValue(userID, out User? user))
            {
                user.AddAward(awardLogic.GetAward(awardID));
            }    
        }

        public void ApplicationCloseEventHandler()
        {
            foreach (var user in userCache.Values)
            {
                var newUserID = AddUserToDatabase(user);
                foreach (var award in user.GetAwards())
                {
                    UserAddAwardToDatabase(newUserID, award.ID);
                }
            }
        }

        private User GetUserFromDatabase(int userID)
        {
            try
            {
                return userDAO.GetAllUsers().First(user => user.ID == userID);
            }
            catch (Exception exception)
            {
                throw new BLLException("Cannot get user by ID", exception);
            }
        }

        private void UserAddAwardToDatabase(int userID, int awardID)
        {
            try
            {
                userDAO.UserAddAward(userID, awardID);
            }
            catch (Exception exception)
            {
                throw new BLLException("Cannot add user award", exception);
            }
        }

        private int AddUserToDatabase(User user)
        {
            try
            {
                return userDAO.AddUser(user);
            }
            catch (Exception exception)
            {
                throw new BLLException("Cannot add user", exception);
            }
        }

        private IEnumerable<User> GetAllUsersFromDatabase()
        {
            try
            {
                return userDAO.GetAllUsers();
            }
            catch (Exception exception)
            {
                throw new BLLException("Cannot get all users", exception);
            }
        }
    }
}
