using Magistri.DTO;
using Magistri.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Magistri.Controllers;
[Authorize] // ochrana pred nepovolanymi navstevniky
// CRUD
public class SubjectsController : Controller {
    private SubjectService subjectService;
    
    public SubjectsController(SubjectService subjectService) {
        this.subjectService = subjectService;
    }
    [Authorize]
    public IActionResult Index() {
        IEnumerable<SubjectDto> allSubjects = subjectService.GetAllSubjects();
        return View(allSubjects);
    }
    [Authorize(Roles = "Admin, Teacher")]
    //Create
    public IActionResult Create() {
        return View();
    }
    [Authorize(Roles = "Admin, Teacher")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(SubjectDto subjectDto) {
        await subjectService.AddSubjectAsync(subjectDto);
        return RedirectToAction("Index");
    }
    //Update (Edit)
    [Authorize(Roles = "Admin, Teacher")]
    [HttpPost]
    public async Task<IActionResult> UpdateAsync(int id) {
        var subjectToEdit = await subjectService.GetSubjectByIdAsync(id);
        if (subjectToEdit == null) {
            return NotFound();
        }
        return View(subjectToEdit);
    }
    [Authorize(Roles = "Admin, Teacher")]
    [HttpPost]
    public async Task<IActionResult> UpdateAsync(SubjectDto subjectDto, int id) {
        await subjectService.UpdateAsync(subjectDto, id);
        return RedirectToAction("Index");
    }
    //Delete
    [Authorize(Roles = "Admin, Teacher")]
    [HttpPost]
    public async Task<IActionResult> DeleteAsync(int id) {
        var subjectToDelete = await subjectService.GetSubjectByIdAsync(id);
        if (subjectToDelete == null) {
            return View("NotFound"); // nebo NotFound();
        }

        await subjectService.DeleteAsync(id);
        return RedirectToAction("Index");
    }
    
}