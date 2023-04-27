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
        private const string AddUserAwardCommand = "add_user_award";
        private static readonly IUserListLogic userListLogic;

        static PL()
        {
            userListLogic = new UserListLogic();
        }

        public static void Main() 
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(AppicationClosedHandler);
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
                        TryAddUser();
                        break;
                    case DeleteUserCommand:
                        TryDeleteUser();
                        break;
                    case ShowAwardsCommand:
                        ShowAwards();
                        break;
                    case AddAwardCommand:
                        TryAddAward();
                        break;
                    case ShowUserAwardsCommand:
                        ShowUserAwards();
                        break;
                    case AddUserAwardCommand:
                        AddUserAward();
                        break;
                    case ExitCommand:
                        return;
                    default:
                        System.Console.WriteLine(IncorrectCommandMessage);
                        break;
                }
            }
        }

        private static void AppicationClosedHandler(object? sender, EventArgs e)
        {
            userListLogic.AppicationClosedHandler();
        }

        private static bool AddUserAward()
        {
            if (!TryReadInt("Enter award ID: ", "Incorrect award ID", out var awardID) ||
                !TryReadInt(UserIDInputMessage, IncorrectUserIDInputMessage, out var userID))
            {
                return false;
            }
            userListLogic.AddUserAward(userID, awardID);
            return true;
        }

        private static void Help()
        {
            System.Console.WriteLine(DeleteUserCommand + " - delete user with specified ID");
            System.Console.WriteLine(AddUserCommand + " - adds user with specified name and date of birth");
            System.Console.WriteLine(ShowUsersCommand + " - shows all users");
            System.Console.WriteLine(AddAwardCommand + " - adds award with specified title");
            System.Console.WriteLine(ShowAwardsCommand + " - shows all awards");
            System.Console.WriteLine(ShowUserAwardsCommand + " - shows all user awards");
            System.Console.WriteLine(AddUserAwardCommand + " - add award to user");
        }

        private static bool TryAddAward()
        {
            if (!TryReadString("Enter award title: ", "Incorrect award title", out var name))
            {
                return false;
            }
            if (userListLogic.TryAddAward(name, out int awardID))
            {
                System.Console.WriteLine("Award added succesfully", awardID.ToString());
                return true;
            }
            System.Console.WriteLine("Award was not added");
            return false;
        }

        private static void ShowAwards()
        {
            foreach (var award in userListLogic.GetAllAwards())
            {
                ShowAward(award);
            }
        }

        private static void ShowAward(Award award)
        {
            System.Console.WriteLine("ID: {0}, Title: {1}", award.ID.ToString(), award.Title.ToString());

        }

        private static bool TryAddUser()
        {
            if (!TryReadString(UserNameInputMessage, IncorrectUserNameInputMessage, out var name) ||
                !TryReadDate(DateInputConsoleMessage, IncorrectDateInputMessage, out var birthDate))
            {
                return false;
            }
            if (userListLogic.TryAddUser(name, birthDate, out int userID))
            {
                System.Console.WriteLine(UserCreationSucceededWithIDFormat, userID.ToString());
                return true;
            }
            System.Console.WriteLine(UserCreationFailMessage);
            return false;
        }

        private static void ShowUsers()
        {
            foreach (var user in userListLogic.GetAllUsers()) 
            {
                ShowUser(user);
            }
        }

        private static void ShowUser(User user)
        {
            System.Console.WriteLine("ID: {0}; Name: {1}; Birth date: {2}; Age: {3}", user.ID.ToString(), user.Name,
                user.BirthDate.ToShortDateString(), user.Age.ToString());
        }

        private static bool TryDeleteUser()
        {
            if (!TryReadInt(UserIDInputMessage, IncorrectUserIDInputMessage, out int userID))
            {
                return false;
            }
            if (userListLogic.TryDeleteUser(userID))
            {
                System.Console.WriteLine(SuccessfulUserDeletionMessage);
                return true;
            }
            System.Console.WriteLine(UnsuccessfulUserDeletionMessage);
            return false;
        }

        private static bool ShowUserAwards()
        {
            if (!TryReadInt(UserIDInputMessage, IncorrectUserIDInputMessage, out int userID))
            {
                return false;
            }
            foreach (var award in userListLogic.GetUser(userID).GetAwards())
            {
                ShowAward(award);
            }
            return true;
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