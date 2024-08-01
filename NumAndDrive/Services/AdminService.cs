using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NumAndDrive.Database;
using NumAndDrive.Models;
using NumAndDrive.ViewModels;

namespace NumAndDrive.Services

{
	public class AdminService : IAdminService
    {
        private readonly NumAndDriveDbContext? _db;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminService(NumAndDriveDbContext db, UserManager<User> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public AdminService()
        {
        }

        /// <summary>
        /// Go through database to list all users (except admin user).
        /// </summary>
        /// <returns>A list of users</returns>
        public List<User> GetUsers()
        {
            return _db?.Users
                .Where(x => x.ArchiveDate == null
                && (x.FirstName != "admin" && x.LastName != "admin"))
                .OrderBy(x => x.LastName)
                .ToList()
                ?? new List<User>();
        }

        /// <summary>
        /// Go through database to lit all users according to the given name (except admin user).
        /// </summary>
        /// <param name="query">first name or last name</param>
        /// <returns>A list of users matching the given name</returns>
        public SearchUserViewModel GetUsersByName(string query)
        {
            List<User> users = _db?.Users.Where(x =>
                                            x.ArchiveDate == null
                                            && (x.FirstName != "admin" && x.LastName != "admin")
                                            && (x.FirstName.ToLower().Contains(query.ToLower())
                                            || x.LastName.ToLower().Contains(query.ToLower()))
                                        ).OrderBy(x => x.LastName)
                                        .ToList() ?? new List<User>();
            SearchUserViewModel results = new SearchUserViewModel()
            {
                Users = users,
                Query = query
            };
            return results;
        }

        /// <summary>
        /// Go through database to count how many users are registered minus the admin user.
        /// </summary>
        /// <returns>The number of registered users</returns>
        public int GetNumberOfUsers()
        {
            return _db?.Users.Count() > 0 ? _db.Users.Count() - 1 : 0;
        }

        /// <summary>
        /// Go through database to count how many journeys are registered.
        /// </summary>
        /// <returns>The number of registered journeys</returns>
        public int GetNumberOfJourneys()
        {
            return _db?.Journeys.Count() > 0 ? _db.Journeys.Count() : 0;
        }

        /// <summary>
        /// Go through database and look for the first company.
        /// </summary>
        /// <returns>Return the first company.</returns>
        public Company GetCompany()
        {
            return _db?.Companies.FirstOrDefault() ?? new Company();
        }

        /// <summary>
        /// Go through database to find the user matching the given id.
        /// </summary>
        /// <param name="id">guid</param>
        /// <returns>The user and their details</returns>
        public User GetUser(string id)
        {
            return _db?.Users.Find(id) ?? new User();
        }

        /// <summary>
        /// Update the given user in database to anonymise all their informations and update the archive date.
        /// </summary>
        /// <param name="user">user to archive</param>
        /// <returns>True if user is archived; otherwise, false.</returns>
        public bool ArchiveUser(User user)
        {
            if (user.FirstName != "admin" && user.LastName != "admin")
            {
                user.FirstName = "utilisateur";
                user.LastName = "utilisateur";
                user.Email = null;
                user.NormalizedEmail = null;
                user.PhoneNumber = null;
                user.UserName = user.Id;
                user.NormalizedUserName = user.Id;

                user.ArchiveDate = DateOnly.FromDateTime(DateTime.UtcNow);
                int? entries = _db?.SaveChanges();
                return entries == 1;
            }
            return false;
        }

        /// <summary>
        /// Create a user in database based on the information given through the view model
        /// </summary>
        /// <param name="userViewModel">datas from form</param>
        /// <returns>True if user is created; otherwise, false.</returns>
        public async Task<bool> CreateSingleUser(UserViewModel userViewModel)
        {
            if (IsUserViewModelValid(userViewModel))
            {
                User user = new User
                {
                    LastName = userViewModel.User.LastName,
                    FirstName = userViewModel.User.FirstName,
                    Email = userViewModel.User.Email,
                    PhoneNumber = userViewModel.User.PhoneNumber,
                    StatusId = userViewModel.User.StatusId,
                    DepartmentId = userViewModel.User.DepartmentId,
                    UserTypeId = 11,
                    ProfilePicturePath = "/img/profile-pic-blue.png",
                    UserName = userViewModel.User.Email
                };
                string password = PasswordGenerator();
                if (IsUserValid(user))
                {
                    await _userManager.CreateAsync(user, password);
                    await _userManager.AddToRoleAsync(user, "User");
                    int? entries = _db?.SaveChanges();
                    return entries == 1;
                }
            }
            return false;
        }

        /// <summary>
        /// Go through database to find all the statuses.
        /// </summary>
        /// <returns>A list of all statuses</returns>
        public List<Status> GetStatuses()
        {
            return _db?.Statuses.ToList() ?? new List<Status>();
        }

        /// <summary>
        /// Go through database to find all the departments
        /// </summary>
        /// <returns>A list of all departments</returns>
        public List<Department> GetDepartments()
        {
            return _db?.Departments.ToList() ?? new List<Department>();
        }

        /// <summary>
        /// Go through the database to find the user who needs to be edited, then edit the datas according to the given view model
        /// </summary>
        /// <param name="viewModel">datas from form</param>
        /// <returns>True if user is edited; otherwise, false.</returns>
        public bool EditUser(UserViewModel viewModel)
        {
            User? user = _db?.Users.Find(viewModel.User.Id);
            if (user != null && IsUserViewModelValid(viewModel))
            {
                user.FirstName = viewModel.User.FirstName;
                user.LastName = viewModel.User.LastName;
                user.PhoneNumber = viewModel.User.PhoneNumber;
                int? entries = _db?.SaveChanges();
                return entries == 3;
            }
            return false;
        }

        /// <summary>
        /// Upload users from a CSV file injected through a form. All datas of each user is checked to be valid before being uploaded to database. Also, each user's username is checked to be unique before being uploaded to database.
        /// </summary>
        /// <param name="admin"></param>
        /// <returns>Async task</returns>
        public async Task<UploadUsersViewModel> UploadUsersFromCSVFile(UploadUsersViewModel viewModel)
        {
            if (viewModel != null)
            {
                CreateFolderAndFile(viewModel);
                List<User> usersToAdd = ReadCSVFile(viewModel);
                DeleteFolder();
                await UploadUsers(viewModel, usersToAdd);
            }
            return viewModel ?? new UploadUsersViewModel();
        }

        /// <summary>
        /// Create a folder at the given path
        /// </summary>
        /// <param name="viewModel"></param>
        public void CreateFolderAndFile(UploadUsersViewModel viewModel)
		{
            string? folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../wwwroot/data/");
            string? filePath = folderPath + viewModel.File.FileName;
            if (folderPath != null && filePath != null && !Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    viewModel.File.CopyTo(stream);
                }
            }
        }

        /// <summary>
        /// Delete a folder at the given path
        /// </summary>
        public void DeleteFolder()
        {
            string? folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../wwwroot/data/");
            if (folderPath != null && Directory.Exists(folderPath))
                Directory.Delete(folderPath, true);
        }

        /// <summary>
        /// Go through a CSV file and isolate the right datas. The right datas are then checked through methods to be valid. They are given to a new user and this user is added to a list of users. This list can be used to upload all of its users to database.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>A list of users with valid datas</returns>
        public List<User> ReadCSVFile(UploadUsersViewModel viewModel)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string path = Path.Combine(webRootPath, "users_not_created.csv");
            byte[] file = File.ReadAllBytes(path);
            string? folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../wwwroot/data/");
            string? filePath = folderPath + viewModel.File.FileName;
            StreamWriter streamWriter = new StreamWriter(path);
            StreamReader streamReader = new StreamReader(filePath);
            string line;
            string[] values;
            line = streamReader.ReadLine();
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
                    } 
                } else
                {
                    if (values[2] != null && values[2].GetType() == typeof(string))
                    {
                        streamWriter.WriteLine(values[2]);
                        viewModel.NumberUsersNotValidated++;
                    }
                }
            }
            streamReader.Close();
            return usersToAdd;
        }

        /// <summary>
        /// Upload to database each user of the list of users
        /// </summary>
        /// <param name="viewModel">to update number of users not validated</param>
        /// <param name="users">list of users to upload</param>
        /// <returns></returns>
        public async Task UploadUsers(UploadUsersViewModel viewModel, List<User> users)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string path = Path.Combine(webRootPath, "users_not_created.csv");
            StreamWriter streamWriter = new StreamWriter(path);
            foreach (User user in users)
            {
                string password = PasswordGenerator();
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    streamWriter.WriteLine(user.Email);
                    viewModel.NumberUsersNotValidated++;
                }
                await _userManager.AddToRoleAsync(user, "User");
            }
            streamWriter.Close();
            _db?.SaveChanges();
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
            if (_db?.Users.ToList() != null)
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
        /// Check if the given userViewModel is valid : if the names and phone number are valid.
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <returns></returns>
        public bool IsUserViewModelValid(UserViewModel userViewModel)
        {
            bool result = false;
            if (userViewModel != null)
                result = IsNameValid(userViewModel.User.FirstName)
                    && IsNameValid(userViewModel.User.LastName)
                    && IsPhoneNumberValid(userViewModel.User.PhoneNumber);
            return result;
        }
    }
}

