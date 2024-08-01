using System;
using Microsoft.AspNetCore.Identity;
using NumAndDrive.Models;
using NumAndDrive.ViewModels;

namespace NumAndDrive.Services
{
	public interface IAdminService
	{
        public List<User> GetUsers();
        public SearchUserViewModel GetUsersByName(string query);
        public int GetNumberOfUsers();
        public int GetNumberOfJourneys();
        public Company GetCompany();
        public User GetUser(string id);
        public bool ArchiveUser(User user);
        public Task<bool> CreateSingleUser(UserViewModel userViewModel);
        public List<Status> GetStatuses();
        public List<Department> GetDepartments();
        public bool EditUser(UserViewModel viewModel);
        public Task<UploadUsersViewModel> UploadUsersFromCSVFile(UploadUsersViewModel viewModel);
    }
}

