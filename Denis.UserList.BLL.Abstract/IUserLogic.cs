﻿using Denis.UserList.Common.Entities;

namespace Denis.UserList.BLL.Core
{
    public interface IUserLogic
    {
        IEnumerable<User> GetAllUsers();
        User GetUser(int userID);
        int AddUser(string? name, DateTime birthDate);
        void DeleteUser(int userID);
        void AddUserAward(int userID, int awardID);
        void DatabaseUpdate();
        IEnumerable<Award> GetUserAwards(int userID);
    }
}