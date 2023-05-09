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
        private readonly IAwardLogic awardLogic;
        private readonly Dictionary<int, User> userCache;
        private bool isCacheActual;
        private int maxID;

        public UserLogic(IAwardLogic awardLogic)
        {
            userCache = new Dictionary<int, User>();
            isCacheActual = false;
            switch (Common.ReadConfigFile("user_database"))
            {
                case "userDAO":
                    userDAO = new FileUserDAO(awardLogic.AwardDAO);
                    break;
                default:
                    throw new BLLException("Cannot get user database");
            }
            this.awardLogic = awardLogic;
            maxID = userDAO.MaxUserID;
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
            if (!isCacheActual)
            {
                CacheAddAllUsers();
            }
            foreach (var user in userCache.Values)
            {
                yield return user;
            }
        }

        public User GetUser(int userID)
        {
            return GetUserInternal(userID);
        }

        public IEnumerable<Award> GetUserAwards(int userID)
        {
            foreach (var award in GetUserInternal(userID).GetAwards())
            {
                yield return award;
            }
        }

        public int AddUser(string? name, DateTime birthDate)
        {
            if (IsUserValid(name, birthDate))
            {
                throw new BLLException("Incorrect user state");
            }
            userCache.Add(++maxID, new User(maxID, name, birthDate));
            return maxID;
        }

        public void AddUserAward(int userID, int awardID)
        {
            GetUserInternal(userID).AddAward(awardLogic.GetAward(awardID));
        }

        public void DatabaseUpdate()
        {
            awardLogic.DatabaseUpdate();
            var users = userCache.Values;
            userDAO.AddUsers(users);
            userDAO.AddUserAwards(users);
        }

        private void CacheAddUser(int userID)
        {
            try
            {
                var user = userDAO.GetAllUsers().First(user => user.ID == userID);
                user.AddAwards(awardLogic.GetUserAwards(userID));
                userCache.Add(userID, user);
            }
            catch (Exception exception)
            {
                throw new BLLException("Cannot find user by ID", exception);
            }
        }

        private void CacheAddAllUsers()
        {
            try
            {
                foreach (var user in userDAO.GetAllUsers())
                {
                    user.AddAwards(awardLogic.GetUserAwards(user.ID));
                    userCache.Add(user.ID, user);
                }
                isCacheActual = true;
            }
            catch (Exception exception)
            {
                throw new BLLException("Cannot get all users", exception);
            }
        }

        private User GetUserInternal(int userID)
        {
            if (!userCache.ContainsKey(userID))
            {
                CacheAddUser(userID);
            }
            if (!userCache.TryGetValue(userID, out User? user) || user is null)
            {
                throw new BLLException("Cannot find user by ID");
            }
            return user;
        }

        private static bool IsUserValid(string? name, DateTime birthDate)
        {
            return string.IsNullOrEmpty(name) || 
                   birthDate >= DateTime.Now || 
                   DateTimeAdditional.CompleteYearDifference(birthDate, DateTime.Now) > maxAge;
        }
    }
}
