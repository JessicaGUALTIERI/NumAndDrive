using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NumAndDrive.Database;
using NumAndDrive.ViewModels;

namespace NumAndDrive.Models.Repositories
{
	public class AdminRepository : IAdminRepository
	{
        private readonly NumAndDriveDbContext? _db;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminRepository(NumAndDriveDbContext db, UserManager<User> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }
        public AdminRepository()
        {

        }

        public byte[] GetCSVFile()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "users_not_created.csv");

            return System.IO.File.ReadAllBytes(filePath);
        }

        /// <summary>
        /// Go through database to list all users.
        /// </summary>
        /// <returns>A list of users</returns>
        public List<User> GetUsers()
        {
            return _db.Users
                .Where(x => x.ArchiveDate == null
                && x.Id != "ec6138af-b5fd-4c62-8c73-f7b717ce1434")
                .OrderBy(x => x.LastName)
                .ToList();
        }

        /// <summary>
        /// Go through database to lit all users according to the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>A list of users matching the given name</returns>
        public SearchUserViewModel GetUsersByName(string name)
        {
            SearchUserViewModel toReturn = new SearchUserViewModel()
            {
                Users = _db.Users.Where(x =>
                                            x.ArchiveDate == null
                                            && x.Id != "ec6138af-b5fd-4c62-8c73-f7b717ce1434"
                                            && (x.FirstName.ToLower().Contains(name.ToLower())
                                            || x.LastName.ToLower().Contains(name.ToLower()))
                                        ).OrderBy(x => x.LastName)
                                        .ToList(),
                Query = name
            };
            return toReturn;
        }

        /// <summary>
        /// Go through database to count how many users are registered.
        /// </summary>
        /// <returns>The number of registered users</returns>
        public int GetNumberOfUsersInDatabase()
        {
            return _db.Users.Count();
        }

        /// <summary>
        /// Go through database to count how many journeys are registered.
        /// </summary>
        /// <returns>The number of registered journeys</returns>
        public int GetNumberOfJourneysInDatabase()
        {
            return _db.Journeys.Count();
        }

        /// <summary>
        /// Go through database and look for the first company.
        /// </summary>
        /// <returns>Return the first company.</returns>
        public Company GetCompany()
        {
            return _db.Companies.FirstOrDefault();
        }

        /// <summary>
        /// Go through database to find the user matching the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The user and their details</returns>
        public User GetUserDetails(string id)
        {
            return _db.Users.Find(id);
        }

        /// <summary>
        /// Go through database to find the user matching the given id. All their informations are then edited to be anonymous and the archive date is added.
        /// </summary>
        /// <param name="user"></param>
        public async Task ArchiveUser(User user)
        {
            if (user.Id != "ec6138af-b5fd-4c62-8c73-f7b717ce1434")
            {
                user.FirstName = "utilisateur";
                user.LastName = "utilisateur";
                user.Email = null;
                user.NormalizedEmail = null;
                user.PhoneNumber = null;
                user.Email = null;
                user.UserName = "utilisateur";
                user.NormalizedUserName = "UTILISATEUR";

                user.ArchiveDate = DateOnly.FromDateTime(DateTime.UtcNow);
                _db.SaveChanges();
            }
        }

        /// <summary>
        /// Create a user in database based on the information given through the view model
        /// </summary>
        /// <param name="userViewModel"></param>
        public async Task CreateSingleUser(CreateUserViewModel userViewModel)
        {
            User user = new User
            {
                LastName = userViewModel.LastName,
                FirstName = userViewModel.FirstName,
                Email = userViewModel.Email,
                PhoneNumber = userViewModel.PhoneNumber,
                StatusId = userViewModel.StatusId,
                DepartmentId = userViewModel.DepartmentId,
                UserTypeId = 11,
                ProfilePicturePath = "/img/profile-pic-blue.png",
                UserName = userViewModel.Email
            };
            string password = PasswordGenerator();
            if (IsUserValid(user))
            {
                await _userManager.CreateAsync(user, password);
                await _userManager.AddToRoleAsync(user, "User");
                _db.SaveChanges();
            }
        }

        /// <summary>
        /// Go through database to find all the statuses
        /// </summary>
        /// <returns>A list of all statuses</returns>
        public List<Status> GetStatuses()
        {
            return _db.Statuses.ToList();
        }

        /// <summary>
        /// Go through database to find all the departments
        /// </summary>
        /// <returns>A list of all departments</returns>
        public List<Department> GetDepartments()
        {
            return _db.Departments.ToList();
        }

        /// <summary>
        /// Go through the database to find the user who needs to be edited, then edit the datas according to the given view model
        /// </summary>
        /// <param name="newValues"></param>
        /// <param name="oldUser"></param>
        public async Task EditUser(EditUserViewModel newValues, User oldUser)
        {
            oldUser.LastName = newValues.LastName;
            oldUser.FirstName = newValues.FirstName;
            oldUser.PhoneNumber = newValues.PhoneNumber;
            _db.SaveChanges();
        }

        /// <summary>
        /// Write in a CSV file all users' emails when they are not added in database
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="mail"></param>
        public void WriteFileForNotCreatedUser(StreamWriter sw, string mail)
        {
            sw.WriteLine(mail);
        }

        /// <summary>
        /// Upload users from a CSV file injected through a form. All datas of each user is checked to be valid before being uploaded to database. Also, each user's username is checked to be unique before being uploaded to database.
        /// </summary>
        /// <param name="admin"></param>
        /// <returns>Async task</returns>
        public async Task UploadUsersFromCSVFile(AdminViewModel admin)
        {
            if (admin != null)
            {
                string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../wwwroot/data/");
                string filePath = folderPath + admin.File.FileName;
                CreateFolder(folderPath);
                CreateFile(filePath, admin);

                string webRootPath = _webHostEnvironment.WebRootPath;
                string path = Path.Combine(webRootPath, "users_not_created.csv");
                StreamWriter streamWriter = new StreamWriter(path);
                List<User> usersToAdd = ReadCSVFile(streamWriter, filePath);

                DeleteFolder(folderPath);
                await UploadUsers(streamWriter, usersToAdd);

                streamWriter.Close();
            }
        }

        /// <summary>
        /// Create a folder at the given path
        /// </summary>
        /// <param name="path"></param>
        public void CreateFolder(string path)
		{
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Delete a folder at the given path
        /// </summary>
        /// <param name="path"></param>
        public void DeleteFolder(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        } 

        /// <summary>
        /// Create a file at the given path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="admin"></param>
        public void CreateFile(string path, AdminViewModel admin)
        {
            if (admin != null)
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    admin.File.CopyTo(stream);
                }
            }
        }

        /// <summary>
        /// Go through a CSV file and isolate the right datas. The right datas are then checked through methods to be valid. They are given to a new user and this user is added to a list of users. This list can be used to upload all of its users to database.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>A list of users with valid datas</returns>
        public List<User> ReadCSVFile(StreamWriter sw, string path)
        {
            StreamReader streamReader = new StreamReader(path);
            string line;
            string[] values;
            line = streamReader.ReadLine(); // Skip the first line
            List<User> usersToAdd = new List<User>();
            while (!streamReader.EndOfStream)
            {
                line = streamReader.ReadLine();
                values = line.Split(';');
                if (HasEnoughColumns(values))
                {
                    string lastName = values[0];
                    string firstName = values[1];
                    string mail = values[2];
                    string phoneNumber = values[3];

                    if (AreDatasValid(lastName, firstName, mail, phoneNumber))
                    {
                        User user = new User()
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            ArchiveDate = null,
                            ProfilePicturePath = "/img/profile-pic-blue.png",
                            Email = mail,
                            PhoneNumber = phoneNumber,
                            DepartmentId = 1,
                            UserTypeId = 1,
                            StatusId = 1,
                            AccessFailedCount = 0,
                            LockoutEnabled = false,
                            TwoFactorEnabled = false,
                            PhoneNumberConfirmed = false,
                            EmailConfirmed = false,
                            UserName = mail
                        };
                        if (IsUserUniqueInDatabaseAndLocalList(user, usersToAdd))
                        {
                            usersToAdd.Add(user);
                        }
                        else
                        {
                            if (values[2] != null && values[2].GetType() == typeof(string))
                            {
                                WriteFileForNotCreatedUser(sw, values[2]);
                            }
                        }
                    } else
                    {
                        if (values[2] != null && values[2].GetType() == typeof(string))
                        {
                            WriteFileForNotCreatedUser(sw, values[2]);
                        }
                    }
                } else
                {
                    if (values[2] != null && values[2].GetType() == typeof(string))
                    {
                        WriteFileForNotCreatedUser(sw, values[2]);
                    }
                }
            }
            streamReader.Close();
            return usersToAdd;
        }

        /// <summary>
        /// Upload to database each user of the list of users
        /// </summary>
        /// <param name="users"></param>
        /// <returns>Async task</returns>
        public async Task UploadUsers(StreamWriter sw, List<User> users)
        {
            foreach (User user in users)
            {
                string password = PasswordGenerator();
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    WriteFileForNotCreatedUser(sw, user.Email);
                }
                await _userManager.AddToRoleAsync(user, "User");
            }
            _db.SaveChanges();
        }

        /// <summary>
        /// Create a random password with letters and special characters
        /// </summary>
        /// <returns>A string representing the created password</returns>
        public string PasswordGenerator()
        {
            Random random = new Random();
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";
            int charactersLength = characters.Length;
            string password = "";
            while (!IsPasswordValid(password))
            {
                int randomCharacterIndex = random.Next(0, charactersLength);
                password += characters[randomCharacterIndex];
                if (password.Length == 12 && !IsPasswordValid(password))
                    password = "";
            }
            return password;
        }

        /// <summary>
        /// Check if the password matches the requirements (length : minimum 8, maximum 12 included; minimum one uppercase; minimum one lowercase; minimum one non-alphabetic; minimum one digit)
        /// </summary>
        /// <param name="password"></param>
        /// <returns>true if the password matches all the requirements; otherwise, false</returns>
        public bool IsPasswordValid(string password)
        {
            return MinimumOneUppercaseLetter(password) && MinimumOneLowercaseLetter(password) && MinimumOneDigit(password) && MinimumOneNonAlphanumericCharacter(password) && IsPasswordLengthValid(password);
        }

        /// <summary>
        /// Check if the password contains at least one uppercase letter
        /// </summary>
        /// <param name="password"></param>
        /// <returns>true if the password contains at least one uppercase letter; otherwise, false</returns>
        public bool MinimumOneUppercaseLetter(string password)
        {
            return Regex.IsMatch(password, "[A-Z]");
        }

        /// <summary>
        /// Check if the password contains at least one lowercase letter
        /// </summary>
        /// <param name="password"></param>
        /// <returns>true if the password contains at least one lowercase letter; otherwise, false</returns>
        public bool MinimumOneLowercaseLetter(string password)
        {
            return Regex.IsMatch(password, "[a-z]");
        }

        /// <summary>
        /// Check if the password contains at least one digit
        /// </summary>
        /// <param name="password"></param>
        /// <returns>true if the password contains at least one digit; otherwise, false</returns>
        public bool MinimumOneDigit(string password)
        {
            return Regex.IsMatch(password, @"[\d]");
        }

        /// <summary>
        /// Check if the password contains at least one non-alphabetic character
        /// </summary>
        /// <param name="password"></param>
        /// <returns>true if the password contains at least one non-alphabetic character; otherwise, false</returns>
        public bool MinimumOneNonAlphanumericCharacter(string password)
        {
            return Regex.IsMatch(password, @"[!@#$%^&*()_+]");
        }

        /// <summary>
        /// Check if the password contains at least 8 characters and maximum 12 included
        /// </summary>
        /// <param name="password"></param>
        /// <returns>true if the length is valid; otherwise, false</returns>
        public bool IsPasswordLengthValid(string password)
        {
            return password.Length >= 8 && password.Length <= 12;
        }

        // Validation for the CSV file
        /// <summary>
        /// Check if the CSV file has enough columns to be used by the program. 7 are required.
        /// </summary>
        /// <param name="values"></param>
        /// <returns>true if the file has enough columns; otherwise, false</returns>
        public bool HasEnoughColumns(string[] values)
        {
            return values.Count() == 4; 
        }

        /// <summary>
        /// Check if a name is valid.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>true if the name is valid; otherwise, false</returns>
        public bool IsNameValid(string name)
        {
            if (name != null)
            {
                bool validator = true;
                name = name.Trim();

                if (name.Length < 1)
                    validator = false;
                else if (name.Length > 100)
                    validator = false;
                else if (!Regex.IsMatch(name, @"^[A-Za-zÀ-ÖØ-öø-ÿ' -]+$"))
                    validator = false;
                else if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                    validator = false;

                return validator;
            }
            return false;
        }

        /// <summary>
        /// Check if the email address is valid.
        /// </summary>
        /// <param name="mail"></param>
        /// <returns>true if the email address is valid; otherwise, false</returns>
        public bool IsEmailValid(string mail)
        {
            if (mail != null)
            {
                mail = mail.Trim();

                if (string.IsNullOrEmpty(mail) || string.IsNullOrWhiteSpace(mail))
                    return false;
                else if (!Regex.IsMatch(mail, @"^[a-zA-Z0-9]+[a-zA-Z0-9._%+-]*@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                    return false;
                else if (mail.Length > 256)
                    return false;

                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if a phone number is valid following the standards (only numbers, length of 10 numbers, only french format)
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns>true if the phone number is valid; otherwise, false</returns>
        public bool IsPhoneNumberValid(string phoneNumber)
        {
            if (phoneNumber != null)
            {
                phoneNumber = phoneNumber.Trim();
                phoneNumber = phoneNumber.Replace(" ", "");

                if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrWhiteSpace(phoneNumber))
                    return false;
                else if (phoneNumber.Length != 10)
                    return false;
                else if (!Regex.IsMatch(phoneNumber, @"^0\d{9}$"))
                    return false;

                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if all datas from CSV file are valid including first name, last name, email address and phone number
        /// </summary>
        /// <param name="lastName"></param>
        /// <param name="firstName"></param>
        /// <param name="mail"></param>
        /// <param name="phoneNumber"></param>
        /// <returns>true if all datas are valid; otherwise, false</returns>
        /// <seealso cref="IsNameValid(string)"/>
        /// <seealso cref="IsEmailValid(string)"/>
        /// <seealso cref="IsPhoneNumberValid(string)"/>
        public bool AreDatasValid(string lastName, string firstName, string mail, string phoneNumber)
        {
            return IsNameValid(lastName) && IsNameValid(firstName) && IsEmailValid(mail) && IsPhoneNumberValid(phoneNumber); 
        }

        /// <summary>
        /// Check if the user's username already exists in the database and in the given list of users
        /// </summary>
        /// <param name="userToCheck"></param>
        /// <param name="users"></param>
        /// <returns>true if the user is unique in both database and local list; otherwise, false</returns>
        public bool IsUserUniqueInDatabaseAndLocalList(User userToCheck, List<User> users)
        {
            return IsUserUniqueInList(userToCheck, users) && IsUserUniqueInDatabase(userToCheck);
        }

        /// <summary>
        /// Check if the user's username already exists in the database
        /// </summary>
        /// <param name="userToCheck"></param>
        /// <returns>true if the user is unique or if database is empty; otherwise, false</returns>
        public bool IsUserUniqueInDatabase(User userToCheck)
        {
            if (_db.Users.ToList() != null)
            {
                List<User> databaseUsers = _db.Users.ToList();
                return IsUserUniqueInList(userToCheck, databaseUsers);
            }
            return true;
        }

        /// <summary>
        /// Check if the user's username already exists in the given list of users
        /// </summary>
        /// <param name="userToCheck"></param>
        /// <param name="list"></param>
        /// <returns>true if the user is unique; otherwise, false</returns>
        public bool IsUserUniqueInList(User userToCheck, List<User> list)
        {
            foreach (User user in list)
            {
                if (user.UserName == userToCheck.UserName)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Check if the given user is valid : if the names, email and phone number are valid.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>true if all informations of the user are valid; otherwise, false</returns>
        public bool IsUserValid(User user)
        {
            bool result = false;
            if (user != null)
                result = IsNameValid(user.FirstName)
                    && IsNameValid(user.LastName)
                    && IsEmailValid(user.Email)
                    && IsPhoneNumberValid(user.PhoneNumber);
            return result;
        }

        /// <summary>
        /// Check if the given editUserViewModel is valid : if the names and phone number are valid.
        /// </summary>
        /// <param name="editUserViewModel"></param>
        /// <returns></returns>
        public bool IsEditUserViewModelValid(EditUserViewModel editUserViewModel)
        {
            bool result = false;
            if (editUserViewModel != null)
                result = IsNameValid(editUserViewModel.FirstName)
                    && IsNameValid(editUserViewModel.LastName)
                    && IsPhoneNumberValid(editUserViewModel.PhoneNumber);
            return result;
        }
    }
}

