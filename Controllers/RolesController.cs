using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Examenes.Models;
using Microsoft.AspNetCore.Identity;
using Examenes.ViewModels;

namespace Clase11.Controllers;

public class RolesController :Controller{
    private readonly ILogger<RolesController> _logger;

    private readonly RoleManager<IdentityRole> _roleManager;
    public RolesController(ILogger<RolesController> logger,
            RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _roleManager = roleManager;
    }

    public IActionResult Index()
    {
        //listar todos los usuarios
       var roles = _roleManager.Roles.ToList();
        return View(roles);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(RoleCreateViewModel model)
    {
        //agregar validaciones y duplicidad
        var role = new IdentityRole(model.RoleName);
        _roleManager.CreateAsync(role);

        return RedirectToAction("Index");
    }
}