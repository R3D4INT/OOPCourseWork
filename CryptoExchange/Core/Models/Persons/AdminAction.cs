using System.Runtime.InteropServices.JavaScript;
using Core.Enums;
using Core.Models.BaseModels;

namespace Core.Models.Persons;

public class AdminAction(TypeOfAdminAction typeOfAction, Guid idOfAdmin) : BaseEntity 
{
    public TypeOfAdminAction TypeOfAction { get; set; } = typeOfAction;
    public Guid IdOfAdmin { get; set; } = idOfAdmin;
    public DateTime TimeStamp { get; set; } = DateTime.Now;
}