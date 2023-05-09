using Denis.UserList.Common.Entities;
using System.Xml.Linq;

namespace Denis.UserList.DAL.File
{
    public class FileAwardDAO : IAwardDAO
    {
        public int MaxAwardID { get; private set; }

        public FileAwardDAO()
        {
            InitializeFileSources();
            var awards = GetAllAwards().ToList();
            MaxAwardID = awards.Any() ? awards.Max(award => award.ID) : 0;
        }

        public IEnumerable<Award> GetAllAwards()
        {
            try
            {
                return System.IO.File.ReadAllLines(Common.AwardFileLocation).
                    Select(line => line.Split(',')).
                    Select(strArr => new Award(int.Parse(strArr[0]), strArr[1]));
            }
            catch (Exception exception)
            {
                throw new DALException("Cannot get all awards", exception);
            }
        }

        public IEnumerable<Award> GetUserAwards(int userID)
        {
            try
            {
                var awardsID = System.IO.File.ReadAllLines(Common.UsersAwardsFileLocation).
                    Select(line => line.Split(',')).
                    Where(stringArray => stringArray[0] == userID.ToString()).
                    Select(strArr => strArr[1]);
                return System.IO.File.ReadAllLines(Common.AwardFileLocation).
                    Select(line => line.Split(',')).
                    Where(stringArray => awardsID.Contains(stringArray[0])).
                    Select(stringArray => new Award(int.Parse(stringArray[0]), stringArray[1]));
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
                award.ID = ++MaxAwardID;
                System.IO.File.AppendAllLines(Common.AwardFileLocation, new[] { AwardDatabaseEntry(award) });
                return award.ID;
            }
            catch (Exception exception)
            {
                throw new DALException("Cannot add award", exception);
            }
        }

        public void AddAwards(IEnumerable<Award> awards)
        {
            try
            {
                var databaseIDs = GetAllAwards().Select(award => award.ID);
                System.IO.File.AppendAllLines(Common.AwardFileLocation, 
                    AwardDatabaseEntries(awards.Where(award => !databaseIDs.Contains(award.ID))));
            }
            catch (Exception exception)
            {
                throw new DALException("Cannot add award", exception);
            }
        }

        private IEnumerable<string> AwardDatabaseEntries(IEnumerable<Award> awards)
        {
            var awardDatabaseEntries = new List<string>();
            foreach (var award in awards)
            {
                award.ID = ++MaxAwardID;
                awardDatabaseEntries.Add(AwardDatabaseEntry(award));
            }
            return awardDatabaseEntries;
        }

        private static string AwardDatabaseEntry(Award award)
        {
            return string.Format("{0},{1}", award.ID.ToString(), award.Title);
        }

        private static void InitializeFileSources()
        {
            Common.CreateTableIfNotExists(Common.AwardFileLocation);
            Common.CreateTableIfNotExists(Common.UsersAwardsFileLocation);
        }
    }
}
