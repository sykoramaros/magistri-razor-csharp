using Magistri.DTO;
using Magistri.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Magistri.Controllers;
[Authorize]
public class GradesController : Controller {
    GradeService gradeService;

    public GradesController(GradeService gradeService) {
        this.gradeService = gradeService;
    }
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> IndexAsync() {
        var gradeVMs = await gradeService.GetAllGradesAsync();
        return View(gradeVMs);
    }

    private async Task FillSelects() {
        var dropdownsData = await gradeService.GetDropdownsDataAsync();
        ViewBag.Students = new SelectList(dropdownsData.Students, "Id", "FullName");
        ViewBag.Subjects = new SelectList(dropdownsData.Subjects, "Id", "Name");
    }
    
    [HttpGet]
    public async Task<IActionResult> CreateAsync() {
        await FillSelects();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(GradeDto newGrade) {
        await gradeService.CreateGradeAsync(newGrade);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> EditAsync(int id) {
        var gradeToEdit = await gradeService.GetGradeByIdAsync(id);
        if (gradeToEdit == null) {
            return View("NotFound");
        }
        await FillSelects();
        return View(gradeToEdit);
    }

    [HttpPost]
    public async Task<IActionResult> EditAsync(GradeDto editedGrade) {
        await gradeService.UpdateGradeAsync(editedGrade);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteAsync(int id) {
        await gradeService.DeleteGradeAsync(id);
        return RedirectToAction("Index");
    }
    
}