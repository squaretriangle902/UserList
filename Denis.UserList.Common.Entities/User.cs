using Denis.UserList.Common.Libraries;

namespace Denis.UserList.Common.Entities
{
    public class User
    {
        private readonly HashSet<Award> awards;

        public int ID { get; private set; }

        public string Name { get; private set; }

        public DateTime BirthDate { get; private set; }

        public int Age => DateTimeAdditional.CompleteYearDifference(BirthDate, DateTime.Now);

        public User(int id, string name, DateTime birthDate)
        {
            ID = id;
            Name = name;
            BirthDate = birthDate;
            awards = new HashSet<Award>();
        }

        public bool AddAward(Award award) 
        {
            return awards.Add(award);
        }

        public bool RemoveAward(Award award)
        {
            return awards.Remove(award);
        }

        public IEnumerable<Award> GetAwards()
        {
            foreach (var award in awards)
            {
                yield return award;
            }
        }

    }
}