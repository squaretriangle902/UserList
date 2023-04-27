using Denis.UserList.Common.Entities;

namespace Denis.UserList.BLL.Core
{
    public interface IAwardLogic
    {
        IEnumerable<Award> GetAllAwards();
        IEnumerable<Award> GetAwardsByUserID(int userID);
        int AddAward(string? name);
    }
}