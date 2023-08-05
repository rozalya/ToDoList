using Microsoft.AspNetCore.Identity;
using ToDoList.Core.Models;

namespace ToDoList.Core.Contracts
{
    public interface IAdministrationService
    {
        public Task<EditRoleViewModel> EditRole(string id);
        public Task<List<UserRoleViewModel>> EditUsersInRole(string roleId);
        public Task EditUsersInRole(List<UserRoleViewModel> model, string roleId);
        public Task<EditUserViewModel> EditUser(string id);
        public Task<IdentityResult> EditUser(EditUserViewModel model);

        public Task<UserClaimsViewModel> ManageUserClaims(string id);

        public Task<List<UserRolesViewModel>> ManageUserRoles(string id);
    }
}
