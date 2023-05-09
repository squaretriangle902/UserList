using Denis.UserList.Common.Entities;

namespace Denis.UserList.DAL.File
{
    public interface IAwardDAO
    {
        int MaxAwardID { get; }
        IEnumerable<Award> GetAllAwards();
        IEnumerable<Award> GetUserAwards(int userID);
        int AddAward(Award award);
        void AddAwards(IEnumerable<Award> awards);
    }
}