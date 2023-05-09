using Denis.UserList.Common.Entities;
using Denis.UserList.DAL.File;

namespace Denis.UserList.BLL.Core
{
    public interface IAwardLogic
    {
        public IAwardDAO AwardDAO { get; }
        IEnumerable<Award> GetAllAwards();
        IEnumerable<Award> GetUserAwards(int userID);
        int AddAward(string? name);
        Award GetAward(int awardID);
        void DatabaseUpdate();
    }
}