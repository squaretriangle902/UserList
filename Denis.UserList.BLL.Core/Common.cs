namespace Denis.UserList.BLL.Core
{
    public static class Common
    {
        public const string ConfigFileLocation = @"E:\UserList\config.txt";

        public static string? ReadConfigFile(string configName)
        {
            return File.ReadAllLines(Common.ConfigFileLocation).
                Select(line => line.Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries)).
                Where(strArray => strArray[0] == configName).Last()[1];
        }
    }
}

