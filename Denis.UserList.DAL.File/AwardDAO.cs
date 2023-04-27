using Denis.UserList.Common.Entities;

namespace Denis.UserList.DAL.File
{
    public class AwardDAO : IAwardDAO
    {
        private int maxAwardID;

        public AwardDAO()
        {
            CreateTableIfNotExists(DataPaths.AwardTableLocation);
            CreateTableIfNotExists(DataPaths.UsersAwardsTableLocation);
            var awards = GetAllAwards().ToList();
            maxAwardID = awards.Any() ? awards.Max(award => award.ID) : 0;
        }

        public void AppicationClosedHandler()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Award> GetAllAwards()
        {
            return System.IO.File.ReadAllLines(DataPaths.AwardTableLocation).
                Select(line => line.Split(',')).
                Select(strArr => new Award(int.Parse(strArr[0]), strArr[1]));
        }

        public IEnumerable<Award> GetAwardsByUserID(int userID)
        {
            var awardsID = System.IO.File.ReadAllLines(DataPaths.UsersAwardsTableLocation).
                Select(line => line.Split(',')).
                Where(strArr => strArr[0] == userID.ToString()).
                Select(strArr => strArr[1]);
            return System.IO.File.ReadAllLines(DataPaths.AwardTableLocation).
                Select(line => line.Split(',')).
                Where(strArr => awardsID.Contains(strArr[0])).
                Select(strArr => new Award(int.Parse(strArr[0]), strArr[1]));
        }

        public bool TryAddAward(string name, out int awardID)
        {
            awardID = ++maxAwardID;
            string userString = string.Format("{0},{1}", awardID.ToString(), name);
            System.IO.File.AppendAllLines(DataPaths.AwardTableLocation, new[] { userString });
            return true;
        }

        private void CreateTableIfNotExists(string location)
        {
            if (!System.IO.File.Exists(location))
            {
                using var file = System.IO.File.Create(location);
            }
        }
    }
}
