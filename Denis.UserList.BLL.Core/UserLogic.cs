using Denis.UserList.Common.Entities;
using Denis.UserList.Common.Libraries;
using Denis.UserList.DAL.Fake;
using Denis.UserList.DAL.File;
using System;
using System.Xml.Linq;

namespace Denis.UserList.BLL.Core
{
    public class UserLogic : IUserLogic
    {
        private const int maxAge = 150;
        private readonly IUserDAO userDAO;

        public UserLogic()
        {
            userDAO = new UserDAO();
        }

        public void DeleteUser(int userID)
        {
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
            try
            {
                return userDAO.GetAllUsers();
            }
            catch (Exception exception)
            {
                throw new BLLException("Cannot get all users", exception);
            }
        }

        public User GetUser(int userID)
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

        public int AddUser(string? name, DateTime birthDate)
        {
            if (string.IsNullOrEmpty(name) || birthDate >= DateTime.Now ||
                DateTimeAdditional.CompleteYearDifference(birthDate, DateTime.Now) > maxAge)
            {
                throw new BLLException("Cannot add user");
            }
            try
            {
                return userDAO.AddUser(name, birthDate);
            }
            catch (Exception exception)
            {
                throw new BLLException("Cannot add user", exception);
            }
        }

        public void UserAddAward(int userID, int awardID)
        {
            try
            {
                userDAO.UserAddAward(userID, awardID);
            }
            catch (Exception exception)
            {
                throw new BLLException("Cannot add user", exception);
            }
        }
    }
}
