using Denis.UserList.Common.Entities;
using Denis.UserList.DAL.Fake;
using Denis.UserList.DAL.File;
using System.Linq;

namespace Denis.UserList.BLL.Core
{
    public class AwardLogic : IAwardLogic
    {
        private readonly Dictionary<int, Award> awardCache;
        private int maxAwardID;
        private bool isCacheActual;

        public IAwardDAO AwardDAO { get; private set; }

        public AwardLogic()
        {
            awardCache = new Dictionary<int, Award>();  
            switch (Common.ReadConfigFile("award_database"))
            {
                case "awardDAO":
                    AwardDAO = new FileAwardDAO();
                    break;
                default:
                    throw new BLLException("Cannot get awards database");
            }
            maxAwardID = AwardDAO.MaxAwardID;
        }

        public IEnumerable<Award> GetUserAwards(int userID)
        {
            try
            {
                return AwardDAO.GetUserAwards(userID);
            }
            catch (Exception exception)
            {
                throw new BLLException("Cannot get awards by user ID", exception);
            }
        }

        public IEnumerable<Award> GetAllAwards()
        {
            if (!isCacheActual)
            {
                CacheAddAllAwards();
            }
            foreach (var award in awardCache.Values)
            {
                yield return award;
            }
        }

        public int AddAward(string? title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("title is null or empty");
            }
            awardCache.Add(++maxAwardID, new Award(maxAwardID, title));
            return maxAwardID;
        }

        public Award GetAward(int awardID)
        {
            return GetAwardInternal(awardID);
        }

        public void DatabaseUpdate()
        {
            AwardDAO.AddAwards(awardCache.Values);
        }

        private void CacheAddAward(int awardID)
        {
            try
            {
                awardCache.Add(awardID, AwardDAO.GetAllAwards().First(award => award.ID == awardID));
            }
            catch (Exception exception)
            {
                throw new BLLException("Cannot get user by ID", exception);
            }
        }

        private Award GetAwardInternal(int awardID)
        {
            if (!awardCache.ContainsKey(awardID))
            {
                CacheAddAward(awardID);
            }
            if (!awardCache.TryGetValue(awardID, out Award? award) || award is null)
            {
                throw new BLLException("Cannot find user by ID");
            }
            return award;
        }

        private void CacheAddAllAwards()
        {
            try
            {
                isCacheActual = true;
                foreach (var award in AwardDAO.GetAllAwards())
                {
                    awardCache.Add(award.ID, award);
                }
            }
            catch (Exception exception)
            {
                throw new BLLException("Cannot get all awards", exception);
            }
        }

    }
}
