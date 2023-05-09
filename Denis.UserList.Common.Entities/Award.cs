namespace Denis.UserList.Common.Entities
{
    public class Award
    {
        public int ID { get; set; }
        public string Title { get; private set; }

        public Award(int id, string title)
        {
            ID = id;
            Title = title;
        }
    }
}
