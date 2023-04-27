using Denis.UserList.Common.Entities;

namespace Denis.UserList.DAL.File
{
    public interface IAwardDAO
    {
        void AppicationClosedHandler();
        IEnumerable<Award> GetAllAwards();
        IEnumerable<Award> GetAwardsByUserID(int userID);
        bool TryAddAward(string name, out int awardID);
    }
}