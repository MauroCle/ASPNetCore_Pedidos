using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;

namespace Examenes.ViewModels;

public class UserEditViewModel{
    public string UserName{get;set;}
    public string Email{get;set;}
    public string Role{get;set;}
    public SelectList Roles{get;set;}
}
