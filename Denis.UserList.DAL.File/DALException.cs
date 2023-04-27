namespace Denis.UserList.DAL.File
{
    internal class DALException : Exception
    {
        public DALException() : base("DAL exception")
        {
        }

        public DALException(string? message) : base(message)
        {
        }

        public DALException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
