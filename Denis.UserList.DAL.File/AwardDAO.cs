using Denis.UserList.Common.Entities;
using System.Xml.Linq;

namespace Denis.UserList.DAL.File
{
    public class AwardDAO : IAwardDAO
    {
        private int maxAwardID;

        public AwardDAO()
        {
            InitializeFileSources();
            var awards = GetAllAwards().ToList();
            maxAwardID = awards.Any() ? awards.Max(award => award.ID) : 0;
        }

        public IEnumerable<Award> GetAllAwards()
        {
            try
            {
                return System.IO.File.ReadAllLines(Common.AwardTableLocation).
                    Select(line => line.Split(',')).
                    Select(strArr => new Award(int.Parse(strArr[0]), strArr[1]));
            }
            catch (Exception exception)
            {
                throw new DALException("Cannot get all awards", exception);
            }
        }

        public IEnumerable<Award> GetAwardsByUserID(int userID)
        {
            try
            {
                var awardsID = System.IO.File.ReadAllLines(Common.UsersAwardsFIleLocation).
                    Select(line => line.Split(',')).
                    Where(strArr => strArr[0] == userID.ToString()).
                    Select(strArr => strArr[1]);
                return System.IO.File.ReadAllLines(Common.AwardTableLocation).
                    Select(line => line.Split(',')).
                    Where(strArr => awardsID.Contains(strArr[0])).
                    Select(strArr => new Award(int.Parse(strArr[0]), strArr[1]));
            }
            catch (Exception exception)
            {
                throw new DALException("Cannot get award by user ID", exception);
            }
        }

        public int AddAward(Award award)
        {
            try
            {
                award.ID = ++maxAwardID;
                string userString = string.Format("{0},{1}", award.ID.ToString(), award.Title);
                System.IO.File.AppendAllLines(Common.AwardTableLocation, new[] { userString });
                return award.ID;
            }
            catch (Exception exception)
            {
                throw new DALException("Cannot add award", exception);
            }
        }

        private static void InitializeFileSources()
        {
            Common.CreateTableIfNotExists(Common.AwardTableLocation);
            Common.CreateTableIfNotExists(Common.UsersAwardsFIleLocation);
        }
    }
}
