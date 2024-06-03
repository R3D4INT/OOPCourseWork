using Core.Models.BaseModels;
using Core.Models.Persons;


namespace BLL.Interfaces.IServiceInterfaces;

public interface IAdminService : IGenericService<Admin>
{
    Task<Admin> CreateAdmin(Guid idOfUser);
    Task BanUser(Guid idOfUser, Guid idOfAdmin);
    Task DeleteUserAccount(Guid idOfUser, Guid idOfAdmin);
    Task UnbanUserAccount(Guid idOfUser, Guid idOfAdmin);
    Task ChangeUserInfo(Guid idOfUser, ProfileBase newProfileBase, Guid idOfAdmin);
}