using Denis.UserList.Common.Entities;
using Denis.UserList.DAL.File;

namespace Denis.UserList.BLL.Core
{
    public class AwardLogic : IAwardLogic
    {
        private readonly IAwardDAO awardDAO;
        private readonly Dictionary<int, Award> awards;

        public AwardLogic()
        {
            awards = new Dictionary<int, Award>();  
            switch (Common.ReadConfigFile("award_database"))
            {
                case "awardDAO":
                    awardDAO = new AwardDAO();
                    break;
                default:
                    throw new BLLException("Cannot get awards database");
            }
        }

        public IEnumerable<Award> GetAwardsByUserID(int userID)
        {
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
            try
            {
                if (string.IsNullOrEmpty(title))
                {
                    throw new ArgumentException("title is null or empty");
                }
                return awardDAO.AddAward(new Award(title));
            }
            catch (Exception exception)
            {
                throw new BLLException("Cannot add award", exception);
            }
        }

        public Award GetAward(int awardID)
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
