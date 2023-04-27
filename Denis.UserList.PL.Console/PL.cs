using Denis.UserList.Common.Entities;
using Denis.UserList.BLL.Core;

namespace Denis.UserList.PL.Console
{
    public static class PL
    {
        private const string UserIDInputMessage = "Enter user ID: ";
        private const string IncorrectUserIDInputMessage = "Incorrect user ID input";
        private const string SuccessfulUserDeletionMessage = "User deletion succeeded";
        private const string UnsuccessfulUserDeletionMessage = "User deletion failed";
        private const string IncorrectCommandMessage = "Incorrect command";
        private const string ExitCommand = "exit";
        private const string DateInputConsoleMessage = "Enter date: ";
        private const string IncorrectDateInputMessage = "Incorrect date input";
        private const string CommandInputMessage = "Enter command: ";
        private const string UserNameInputMessage = "Enter user name: ";
        private const string UserCreationFailMessage = "User creation failed";
        private const string UserCreationSucceededWithIDFormat = "User was created with ID: {0}";
        private const string DeleteUserCommand = "delete_user";
        private const string AddUserCommand = "add_user"; 
        private const string ShowUsersCommand = "show_users";
        private const string ShowAwardsCommand = "show_awards";
        private const string AddAwardCommand = "add_award";
        private const string ShowUserAwardsCommand = "show_user_awards";        
        private const string IncorrectUserNameInputMessage = "Incorrect user name";        
        private const string HelpCommand = "help";
        private const string UserAddAwardCommand = "user_add_award";

        private static IUserLogic userLogic;
        private static IAwardLogic awardLogic;

        public static void Main() 
        {
            try
            {
                userLogic = new UserLogic();
                awardLogic = new AwardLogic();
            }
            catch (Exception)
            {
                System.Console.WriteLine("Cannot find database");
                return;
            }
            while (true) 
            {
                System.Console.Write(CommandInputMessage);
                var command = System.Console.ReadLine();
                switch (command) 
                {
                    case HelpCommand:
                        Help();
                        break;
                    case ShowUsersCommand:
                        ShowUsers();
                        break;
                    case AddUserCommand:
                        AddUser();
                        break;
                    case DeleteUserCommand:
                        DeleteUser();
                        break;
                    case ShowAwardsCommand:
                        ShowAwards();
                        break;
                    case AddAwardCommand:
                        AddAward();
                        break;
                    case ShowUserAwardsCommand:
                        ShowUserAwards();
                        break;
                    case UserAddAwardCommand:
                        UserAddAward();
                        break;
                    case ExitCommand:
                        return;
                    default:
                        System.Console.WriteLine(IncorrectCommandMessage);
                        break;
                }
            }
        }

        private static void UserAddAward()
        {

            try
            {
                if (!TryReadInt("Enter award ID: ", "Incorrect award ID", out var awardID) ||
                    !TryReadInt(UserIDInputMessage, IncorrectUserIDInputMessage, out var userID))
                {
                    return;
                }
                userLogic.UserAddAward(userID, awardID);
                System.Console.WriteLine("Award added to user succesfully");
            }
            catch (Exception)
            {
                System.Console.WriteLine("Award was not added to user");
            }
        }

        private static void Help()
        {
            System.Console.WriteLine(DeleteUserCommand + " - delete user with specified ID");
            System.Console.WriteLine(AddUserCommand + " - adds user with specified name and date of birth");
            System.Console.WriteLine(ShowUsersCommand + " - shows all users");
            System.Console.WriteLine(AddAwardCommand + " - adds award with specified title");
            System.Console.WriteLine(ShowAwardsCommand + " - shows all awards");
            System.Console.WriteLine(ShowUserAwardsCommand + " - shows all user awards");
            System.Console.WriteLine(UserAddAwardCommand + " - add award to user");
        }

        private static void AddAward()
        {
            try
            {
                if (!TryReadString("Enter award title: ", "Incorrect award title", out var name))
                {
                    return;
                }
                System.Console.WriteLine("Award added succesfully", awardLogic.AddAward(name).ToString());
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
                if (!TryReadString(UserNameInputMessage, IncorrectUserNameInputMessage, out var name) ||
                    !TryReadDate(DateInputConsoleMessage, IncorrectDateInputMessage, out var birthDate))
                {
                    return;
                }
                System.Console.WriteLine(UserCreationSucceededWithIDFormat, userLogic.AddUser(name, birthDate));
            }
            catch (Exception)
            {
                System.Console.WriteLine(UserCreationFailMessage);
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
                if (!TryReadInt(UserIDInputMessage, IncorrectUserIDInputMessage, out int userID))
                {
                    return;
                }
                userLogic.DeleteUser(userID);
                System.Console.WriteLine(SuccessfulUserDeletionMessage);
            }
            catch (Exception)
            {
                System.Console.WriteLine(UnsuccessfulUserDeletionMessage);
            }
        }

        private static void ShowUserAwards()
        {
            try
            {
                if (!TryReadInt(UserIDInputMessage, IncorrectUserIDInputMessage, out int userID))
                {
                    return;
                }
                foreach (var award in awardLogic.GetAwardsByUserID(userID))
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