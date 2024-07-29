using System;
using Microsoft.AspNetCore.Identity;
using NumAndDrive.ViewModels;

namespace NumAndDrive.Models.Repositories
{
	public interface IAdminRepository
	{
        Task UploadUsersFromCSVFile(AdminViewModel adminViewModel);
        string PasswordGenerator();
        bool IsUserValid(User user);
        bool IsEditUserViewModelValid(EditUserViewModel editUserViewModel);
        int GetNumberOfUsersInDatabase();
        List<User> GetUsers();
        SearchUserViewModel GetUsersByName(string name);
        User GetUserDetails(string id);
        Task ArchiveUser(User user);
        Task CreateSingleUser(CreateUserViewModel userViewModel);
        List<Status> GetStatuses();
        List<Department> GetDepartments();
        Task EditUser(EditUserViewModel newValues, User user);
        byte[] GetCSVFile();
        public int GetNumberOfJourneysInDatabase();
        public Company GetCompany();
    }
}

