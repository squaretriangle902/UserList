namespace Denis.UserList.PL.Console
{
    public class PLException : Exception
    {
        public PLException()
        {
        }

        public PLException(string? message) : base(message)
        {
        }

        public PLException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
