using System.Threading.Tasks;
using CCTVSite.Contexts;
using CCTVSite.Models;
using CCTVSite.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CCTVSite.Areas.Admin.Controllers;

[Area("Admin")]

public class EmployeeController(AppDbContext _context, IWebHostEnvironment _environment) : Controller
{
    public async Task <IActionResult> Index()
    {
        var employees = await _context.Employees.Select(employee => new EmployeeGetVM()
        {
            Id = employee.Id,
            FullName = employee.FullName,
            PositionName = employee.Position.Title,
            ImagePath = employee.ImagePath
        }).ToListAsync();


        return View();
    }

    public async Task<IActionResult> Create()
    {
        await SendCategoriesWithViewBag();
        return View();
    }

    

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeCreateVM vm)
    {
        await SendCategoriesWithViewBag();

        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var isExistPosition = await _context.Positions.AnyAsync(x => x.Id == vm.PositionId);

        if(!isExistPosition)
        {
            ModelState.AddModelError("PositionId", "This position is not found");
            return View(vm);
        }

        if(vm.Image.Length > 2 * 1024 * 1024)
        {
            ModelState.AddModelError("Image", "Image maximum size must be 2 mb");
            return View(vm);
        }

        if(!vm.Image.ContentType.Contains("image"))
        {
            ModelState.AddModelError("Image", "You can only upload file with image type");
            return View(vm);
        }

        string uniqueFileName = Guid.NewGuid().ToString() + vm.Image.FileName;
        string folderPath = Path.Combine(_environment.WebRootPath, "assets", "images");

        string path = Path.Combine(folderPath, uniqueFileName);

        using FileStream stream = new(path, FileMode.Create);

        await vm.Image.CopyToAsync(stream);

        Employee employee = new()
        {
            FullName = vm.FullName,
            PositionId = vm.PositionId,
            ImagePath = uniqueFileName
        };

        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private async Task SendCategoriesWithViewBag()
    {
        var positions = await _context.Positions.Select(c => new SelectListItem()
        {
            Text = c.Title,
            Value = c.Id.ToString()

        }).ToListAsync();
        ViewBag.Positions = positions;
    }

}
