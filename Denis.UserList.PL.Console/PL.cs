using Denis.UserList.Common.Entities;
using Denis.UserList.BLL.Core;

namespace Denis.UserList.PL.Console
{
    public static class PL
    {
        private static IUserLogic userLogic;
        private static IAwardLogic awardLogic;

        public static void Main() 
        {

            try
            {
                awardLogic = new AwardLogic();
                userLogic = new UserLogic(awardLogic);
            }
            catch (Exception)
            {
                System.Console.WriteLine("Cannot find database");
                return;
            }
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomainProcessExit);
            while (true) 
            {
                System.Console.Write(StringConstants.CommandInputMessage);
                var command = System.Console.ReadLine();
                switch (command) 
                {
                    case StringConstants.HelpCommand:
                        Help();
                        break;
                    case StringConstants.ShowUsersCommand:
                        ShowUsers();
                        break;
                    case StringConstants.AddUserCommand:
                        AddUser();
                        break;
                    case StringConstants.DeleteUserCommand:
                        DeleteUser();
                        break;
                    case StringConstants.ShowAwardsCommand:
                        ShowAwards();
                        break;
                    case StringConstants.AddAwardCommand:
                        AddAward();
                        break;
                    case StringConstants.ShowUserAwardsCommand:
                        ShowUserAwards();
                        break;
                    case StringConstants.UserAddAwardCommand:
                        AddUserAward();
                        break;
                    case "Update":
                        UpdateDatabase();
                        break;
                    case StringConstants.ExitCommand:
                        return;
                    default:
                        System.Console.WriteLine(StringConstants.IncorrectCommandMessage);
                        break;
                }
            }
        }

        private static void CurrentDomainProcessExit(object? sender, EventArgs e)
        {
            UpdateDatabase();
        }

        private static void UpdateDatabase()
        {
            awardLogic.DatabaseUpdate();
            userLogic.DatabaseUpdate();
        }

        private static void AddUserAward()
        {

            try
            {
                if (!TryReadInt(StringConstants.UserIDInputMessage, StringConstants.IncorrectUserIDInputMessage, out var userID) ||
                    !TryReadInt("Enter award ID: ", "Incorrect award ID", out var awardID))
                {
                    return;
                }
                userLogic.AddUserAward(userID, awardID);
                System.Console.WriteLine("Award added to user succesfully");
            }
            catch (Exception)
            {
                System.Console.WriteLine("Award was not added to user");
            }
        }

        private static void Help()
        {
            System.Console.WriteLine(StringConstants.DeleteUserCommand + " - delete user with specified ID");
            System.Console.WriteLine(StringConstants.AddUserCommand + " - adds user with specified name and date of birth");
            System.Console.WriteLine(StringConstants.ShowUsersCommand + " - shows all users");
            System.Console.WriteLine(StringConstants.AddAwardCommand + " - adds award with specified title");
            System.Console.WriteLine(StringConstants.ShowAwardsCommand + " - shows all awards");
            System.Console.WriteLine(StringConstants.ShowUserAwardsCommand + " - shows all user awards");
            System.Console.WriteLine(StringConstants.UserAddAwardCommand + " - add award to user");
        }

        private static void AddAward()
        {
            try
            {
                if (!TryReadString("Enter award title: ", "Incorrect award title", out var name))
                {
                    return;
                }
                var awardID = awardLogic.AddAward(name);
                System.Console.WriteLine("Award added succesfully", awardID.ToString());
            }
            catch (Exception)
            {
                System.Console.WriteLine("Award was not added");
            }
        }

        private static void ShowAwards()
        {
            try
            {
                foreach (var award in awardLogic.GetAllAwards())
                {
                    ShowAward(award);
                }
            }
            catch (Exception)
            {
                System.Console.WriteLine("Cannot get all awards");
            }
        }

        private static void ShowAward(Award award)
        {
            System.Console.WriteLine("ID: {0}, Title: {1}", award.ID.ToString(), award.Title.ToString());
        }

        private static void AddUser()
        {
            try
            {
                if (!TryReadString(StringConstants.UserNameInputMessage, StringConstants.IncorrectUserNameInputMessage, out var name) ||
                    !TryReadDate(StringConstants.DateInputConsoleMessage, StringConstants.IncorrectDateInputMessage, out var birthDate))
                {
                    return;
                }
                System.Console.WriteLine(StringConstants.UserCreationSucceededWithIDFormat, userLogic.AddUser(name, birthDate));
            }
            catch (Exception)
            {
                System.Console.WriteLine(StringConstants.UserCreationFailMessage);
            }
        }

        private static void ShowUsers()
        {
            try
            {
                foreach (var user in userLogic.GetAllUsers())
                {
                    ShowUser(user);
                }
            }
            catch (Exception)
            {
                System.Console.WriteLine("Cannot get all users");
            }
        }

        private static void ShowUser(User user)
        {
            System.Console.WriteLine("ID: {0}; Name: {1}; Birth date: {2}; Age: {3}", user.ID.ToString(), user.Name,
                user.BirthDate.ToShortDateString(), user.Age.ToString());
        }

        private static void DeleteUser()
        {
            try
            {
                if (!TryReadUserID(out var userID))
                {
                    return;
                }
                userLogic.DeleteUser(userID);
                System.Console.WriteLine(StringConstants.SuccessfulUserDeletionMessage);
            }
            catch (Exception)
            {
                System.Console.WriteLine(StringConstants.UnsuccessfulUserDeletionMessage);
            }
        }

        private static bool TryReadUserID(out int userID)
        {
            return TryReadInt(StringConstants.UserIDInputMessage, StringConstants.IncorrectUserIDInputMessage, out userID);
        }

        private static void ShowUserAwards()
        {
            try
            {
                if (!TryReadUserID(out var userID))
                {
                    return;
                }
                foreach (var award in userLogic.GetUserAwards(userID))
                {
                    ShowAward(award);
                }
            }
            catch (Exception)
            {
                System.Console.WriteLine("Cannot show user awards");
            }
        }

        private static bool TryReadString(string inputConsoleMessage, string incorrectInputMessage, out string? result)
        {
            System.Console.Write(inputConsoleMessage);
            result = System.Console.ReadLine();
            if (string.IsNullOrEmpty(result))
            {
                System.Console.WriteLine(incorrectInputMessage);
                return false;
            }
            return true;
        }

        private static bool TryReadDate(string inputConsoleMessage, string incorrectInputMessage, out DateTime result)
        {
            System.Console.Write(inputConsoleMessage);
            if (!DateTime.TryParse(System.Console.ReadLine(), out result))
            {
                System.Console.WriteLine(incorrectInputMessage);
                return false;
            }
            return true;
        }

        private static bool TryReadInt(string consoleMessage, string incorrectInputMessage, out int result)
        {
            System.Console.Write(consoleMessage);
            if (!int.TryParse(System.Console.ReadLine(), out result))
            {
                System.Console.WriteLine(incorrectInputMessage);
                return false;
            }
            return true;
        }
    }
}