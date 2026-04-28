using BLL.AbstractServices;
using BLL.Dtos.User;

namespace PL.Controllers;

using Microsoft.AspNetCore.Mvc;

public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // GET: /User
    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetAllAsync();
        return View(users);
    }

    // GET: /User/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();

        return View(user);
    }

    // GET: /User/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /User/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        await _userService.CreateUserAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    // GET: /User/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();

        var dto = new UpdateUserDto
        {
            Name = user.Name,
            Role = user.Role,
            IsActive = user.IsActive
        };

        return View(dto);
    }

    // POST: /User/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateUserDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await _userService.UpdateUserAsync(id, dto);
        if (!result) return NotFound();

        return RedirectToAction(nameof(Index));
    }

    // POST: /User/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _userService.DeleteUserAsync(id);
        return RedirectToAction(nameof(Index));
    }
}