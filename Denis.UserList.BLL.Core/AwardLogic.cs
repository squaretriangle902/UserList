using Denis.UserList.Common.Entities;
using Denis.UserList.DAL.File;

namespace Denis.UserList.BLL.Core
{
    public class AwardLogic : IAwardLogic
    {
        private readonly IAwardDAO awardDAO;
        private readonly Dictionary<int, Award> awardCache;
        private int LastID;

        public AwardLogic()
        {
            awardCache = new Dictionary<int, Award>();  
            switch (Common.ReadConfigFile("award_database"))
            {
                case "awardDAO":
                    awardDAO = new AwardDAO();
                    break;
                default:
                    throw new BLLException("Cannot get awards database");
            }
        }

        public IEnumerable<Award> GetAwardsByUserID(int userID, IUserLogic userLogic)
        {
            if (userID < 0)
            {
                return userLogic.GetUser(userID).GetAwards();
            }
            return GetAwardsByUserIDFromDatabase(userID);
        }

        private IEnumerable<Award> GetAwardsByUserIDFromDatabase(int userID)
        {
            try
            {
                return awardDAO.GetAwardsByUserID(userID);
            }
            catch (Exception exception)
            {
                throw new BLLException("Cannot get awards by user ID", exception);
            }
        }

        public IEnumerable<Award> GetAllAwards()
        {
            return awardCache.Values.Union(GetAllAwardsFromDatabase());
        }

        private IEnumerable<Award> GetAllAwardsFromDatabase()
        {
            try
            {
                return awardDAO.GetAllAwards();
            }
            catch (Exception exception)
            {
                throw new BLLException("Cannot get all award", exception);
            }
        }

        public int AddAward(string? title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("title is null or empty");
            }
            LastID--;
            awardCache.Add(LastID, new Award(LastID, title));
            return LastID;
        }

        private int AddAwardToDatabase(Award award)
        {
            try
            {
                return awardDAO.AddAward(award);
            }
            catch (Exception exception)
            {
                throw new BLLException("Cannot add award", exception);
            }
        }

        public Award GetAward(int awardID)
        {
            if (awardCache.TryGetValue(awardID, out Award? award))
            {
                return award;
            }
            return GetAwardFromDatabase(awardID);
        }

        public void ApplicationCloseEventHandler()
        {
            foreach (var award in awardCache.Values)
            {
                var newUserID = AddAwardToDatabase(award);
            }
        }

        private Award GetAwardFromDatabase(int awardID)
        {
            try
            {
                return awardDAO.GetAllAwards().First(award => award.ID == awardID);
            }
            catch (Exception exception)
            {
                throw new BLLException("Cannot get user by ID", exception);
            }
        }
    }
}
