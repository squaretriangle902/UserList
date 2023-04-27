namespace Denis.UserList.DAL.File
{
    public static class Common
    {
        public const string UserTableLocation = @"E:\UserList\Database\users.txt";
        public const string UsersAwardsTableLocation = @"E:\UserList\Database\usersAwards.txt";
        public const string AwardTableLocation = @"E:\UserList\Database\awards.txt";

        public static void CreateTableIfNotExists(string location)
        {
            if (!System.IO.File.Exists(location))
            {
                using var file = System.IO.File.Create(location);
            }
        }
    }
}
