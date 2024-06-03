using BLL.Interfaces.IServiceInterfaces;
using Core.Enums;
using Core.Models;
using Core.Models.BaseModels;
using Core.Models.Persons;
using DAL.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BLL.Implementations;

public class AdminService : GenericService<Admin>, IAdminService
{
    private readonly IUserService _userService;

    private readonly ISupportService _supportService;
    public AdminService(IGenericRepository<Admin> repository, IUserService userService, ISupportService supportService) :
        base(repository)
    {
        _userService = userService;
        _supportService = supportService;
    }
    public async Task<Admin> CreateAdmin(Guid idOfUser)
    {
        try
        {
            var user = await _userService.GetSingleByCondition(e => e.Id == idOfUser);
            if (user == null)
            {
                throw new Exception("Failed to get user");
            }

            var admin = new Admin(user, 5 ,500);
            await _userService.Delete(e => e.Id == admin.Id);
            await Add(admin);
            return admin;
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to create support {e.Message}");
        }
    }

    public async Task BanUser(Guid idOfUser, Guid idOfAdmin)
    {
        try
        {
            var user = await _userService.GetSingleByCondition(e => e.Id == idOfUser);
            user.IsBanned = true;
            await _userService.Update(user, e => e.Id == user.Id);
            var admin = await GetSingleByCondition(e => e.Id == idOfAdmin);
            admin.HistoryOfActions.Add(new AdminAction(TypeOfAdminAction.BanUser, idOfAdmin));
            await Update(admin, e => e.Id == idOfAdmin);
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to ban user {e.Message}");
        }
    }
    public async Task UnbanUserAccount(Guid idOfUser, Guid idOfAdmin)
    {
        try
        {
            var user = await _userService.GetSingleByCondition(e => e.Id == idOfUser);
            user.IsBanned = false;
            await _userService.Update(user, e => e.Id == user.Id);
            var admin = await GetSingleByCondition(e => e.Id == idOfAdmin);
            admin.HistoryOfActions.Add(new AdminAction(TypeOfAdminAction.UnbanUser, idOfAdmin));
            await Update(admin, e => e.Id == idOfAdmin);
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to unban user {e.Message}");
        }
    }
    public async Task DeleteUserAccount(Guid idOfUser, Guid idOfAdmin)
    {
        try
        {
            await _userService.Delete(e => e.Id == idOfUser);
            var admin = await GetSingleByCondition(e => e.Id == idOfAdmin);
            admin.HistoryOfActions.Add(new AdminAction(TypeOfAdminAction.DeleteUser, idOfAdmin));
            await Update(admin, e => e.Id == idOfAdmin);
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to delete user {e.Message}");
        }
    }

    public async Task ChangeUserInfo(Guid idOfUser, ProfileBase newProfileBase, Guid idOfAdmin)
    {
        try
        {
            var user = await _userService.GetSingleByCondition(e => e.Id == idOfUser);
            if (newProfileBase == null)
            {
                throw new Exception($"Empty profile info");
            }

            user.Name = newProfileBase.Name;
            user.Surname = newProfileBase.Surname;
            user.PhoneNumber = newProfileBase.PhoneNumber;
            user.Adress = newProfileBase.Adress;
            user.Age = newProfileBase.Age;
            user.Email = newProfileBase.Email;
            user.Country = newProfileBase.Country;
            user.Gender = newProfileBase.Gender;

            await _userService.Update(user, e => e.Id == user.Id);
            var admin = await GetSingleByCondition(e => e.Id == idOfAdmin);
            admin.HistoryOfActions.Add(new AdminAction(TypeOfAdminAction.ChangeInfo, idOfAdmin));
            await Update(admin, e => e.Id == idOfAdmin);
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to change user info {e.Message}");
        }
    }
}