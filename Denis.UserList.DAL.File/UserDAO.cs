using Denis.UserList.Common.Entities;
using Denis.UserList.DAL.Fake;

namespace Denis.UserList.DAL.File
{
    public class UserDAO : IUserDAO
    {
        private int maxUserID;

        public UserDAO()
        {
            CreateTableIfNotExists(DataPaths.UserTableLocation);
            var users = GetAllUsers().ToList();
            maxUserID = users.Any() ? users.Max(user => user.ID) : 0;
        }

        public IEnumerable<User> GetAllUsers()
        {
            var users = System.IO.File.ReadAllLines(DataPaths.UserTableLocation).
                Select(line => line.Split(',')).
                Select(strArr => new User(int.Parse(strArr[0]), strArr[1], DateTime.Parse(strArr[2])));
            return users;
        }

        public bool TryAddUser(string name, DateTime birthDate, out int userID)
        {
            userID = ++maxUserID;
            string userString = string.Format("{0},{1},{2}", userID.ToString(), name, birthDate.ToShortDateString());
            System.IO.File.AppendAllLines(DataPaths.UserTableLocation, new [] { userString });
            return true;
        }

        public bool TryDeleteUser(int userID)
        {
            var tempFile = Path.GetTempFileName();
            var linesToKeep = System.IO.File.ReadLines(DataPaths.UserTableLocation).
                Where(l => l.Split(',')[0] != userID.ToString());
            System.IO.File.WriteAllLines(tempFile, linesToKeep);
            System.IO.File.Delete(DataPaths.UserTableLocation);
            System.IO.File.Move(tempFile, DataPaths.UserTableLocation);
            return true;
        }

        private static void CreateTableIfNotExists(string location)
        {
            if (!System.IO.File.Exists(location))
            {
                using var file = System.IO.File.Create(location);
            }
        }

        public void AddUserAward(int userID, int awardID)
        {
            var userAwardString = string.Format("{0},{1}", userID.ToString(), awardID.ToString());
            System.IO.File.AppendAllLines(DataPaths.UsersAwardsTableLocation, new[] { userAwardString });
        }

        public void AppicationClosedHandler()
        {
            throw new NotImplementedException();
        }
    }
}