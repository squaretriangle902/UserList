namespace Denis.UserList.DAL.File
{
    public static class Common
    {
        public const string UserFileLocation = @"E:\UserList\Database\user.csv";
        public const string UsersAwardsFIleLocation = @"E:\UserList\Database\usersAwards.csv";
        public const string AwardTableLocation = @"E:\UserList\Database\award.csv";

        public static void CreateTableIfNotExists(string location)
        {
            if (!System.IO.File.Exists(location))
            {
                using var file = System.IO.File.Create(location);
            }
        }
    }
}
