using System;
using NumAndDrive.Database;

namespace NumAndDrive.Models
{
	public class UserRepository
	{
		private readonly NumAndDriveDbContext Database;

		public UserRepository(NumAndDriveDbContext db)
		{
			Database = db;
		}

		public IEnumerable<User> GetUsers()
		{
			var users = Database.Users.ToList();
			return users;
		}

        public User GetUserById(string id)
        {
            var users = Database.Users.ToList();
			User user = users.FirstOrDefault(x => x.Id == id);
            return user??null;
        }
    }
}

