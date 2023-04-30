using Denis.UserList.Common.Entities;

namespace Denis.UserList.DAL.File
{
    public interface IAwardDAO
    {
        IEnumerable<Award> GetAllAwards();
        IEnumerable<Award> GetAwardsByUserID(int userID);
        int AddAward(Award award);
    }
}