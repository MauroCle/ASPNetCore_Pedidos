using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Examenes.Models;
using Microsoft.AspNetCore.Identity;
using Examenes.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace Examenes.Controllers;

[Authorize(Roles = "Administrador")]
public class UsersController :Controller{

    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UsersController(

        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {

        _userManager = userManager;
        _roleManager = roleManager;
    }
    public IActionResult Index()
    {
        //listar todos los usuarios
       var users = _userManager.Users.ToList();
        return View(users);
    }

    //Edit Usuairos
    public async Task<IActionResult> Edit(string Id)
    {
        if(string.IsNullOrEmpty(Id))
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(Id);
       // ViewData["Roles"] = _roleManager.Roles.Select(x=> x.UserName).ToList();
         var userViewModel = new UserEditViewModel();
        userViewModel.UserName = user.UserName ?? string.Empty; // ?? string.Empty => Si es null le pone una string vacia
        userViewModel.Email = user.Email ?? string.Empty;
        userViewModel.Roles = new SelectList(_roleManager.Roles.ToList());
        
        return View(userViewModel);
    }
    [HttpPost]
      public async Task<IActionResult> Edit(UserEditViewModel model)
      {
        var user = await _userManager.FindByNameAsync(model.UserName);
         
        if(user == null)
        {
            return NotFound();
        }

        await _userManager.AddToRoleAsync(user,model.Role);
        return RedirectToAction("Index");
      }


    public async Task<IActionResult> Delete(string Id)
    {
        var user = await _userManager.FindByIdAsync(Id);

        if (user == null)
        {
            return NotFound(); 
        }

        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded)
        {
            return RedirectToAction("Index");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }
}