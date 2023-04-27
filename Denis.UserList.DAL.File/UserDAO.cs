using Denis.UserList.Common.Entities;
using Denis.UserList.DAL.Fake;
using System.Xml.Linq;

namespace Denis.UserList.DAL.File
{
    public class UserDAO : IUserDAO
    {
        private int maxUserID;

        public UserDAO()
        {
            InitializeFileSources();
            var users = GetAllUsers().ToList();
            maxUserID = users.Any() ? users.Max(user => user.ID) : 0;
        }

        public IEnumerable<User> GetAllUsers()
        {
            try
            {
                return System.IO.File.ReadAllLines(Common.UserTableLocation).
                    Select(line => line.Split(',')).
                    Select(strArr => new User(int.Parse(strArr[0]), strArr[1], DateTime.Parse(strArr[2])));
            }
            catch (Exception exception)
            {
                throw new DALException("Cannot get all users", exception);
            }
        }

        public int AddUser(string name, DateTime birthDate)
        {
            try
            {
                var userID = ++maxUserID;
                string userString = string.Format("{0},{1},{2}", userID.ToString(), name, birthDate.ToShortDateString());
                System.IO.File.AppendAllLines(Common.UserTableLocation, new[] { userString });
                return userID;
            }
            catch (Exception exception)
            {
                throw new DALException("Cannot add user", exception);
            }
        }

        public void DeleteUser(int userID)
        {
            try
            {
                var tempFile = Path.GetTempFileName();
                var linesToKeep = System.IO.File.ReadLines(Common.UserTableLocation).
                    Where(l => l.Split(',')[0] != userID.ToString());
                System.IO.File.WriteAllLines(tempFile, linesToKeep);
                System.IO.File.Delete(Common.UserTableLocation);
                System.IO.File.Move(tempFile, Common.UserTableLocation);
            }
            catch (Exception exception)
            {
                throw new DALException("Cannot delete user", exception);
            }
        }

        public void UserAddAward(int userID, int awardID)
        {
            try
            {
                var userAwardString = string.Format("{0},{1}", userID.ToString(), awardID.ToString());
                System.IO.File.AppendAllLines(Common.UsersAwardsTableLocation, new[] { userAwardString });
            }
            catch (Exception exception)
            {
                throw new DALException("Cannot add award to user", exception);
            }
        }

        public static void InitializeFileSources()
        {
            Common.CreateTableIfNotExists(Common.UserTableLocation);
            Common.CreateTableIfNotExists(Common.UsersAwardsTableLocation);
        }
    }
}