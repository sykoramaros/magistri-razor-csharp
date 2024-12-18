using Magistri.DTO;
using Magistri.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Magistri.Controllers {
    [Authorize] // ochrana pred nepovolanymi navstevniky
    public class StudentsController : Controller {
        private StudentService studentService;

        public StudentsController(StudentService service) {
            studentService = service;
        }
        // role chtere maji na obsah stranky vstup povoleny
        [HttpGet]
        public IActionResult Index() {
            var allStudents = studentService.GetAllStudents();
            return View(allStudents);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(StudentDto studentDto) {
            await studentService.CreateStudentAsync(studentDto);
            return RedirectToAction("Index");
        }
        
        [Authorize(Roles = "Admin, Teacher")] 
        [HttpGet]
        public async Task<IActionResult> EditAsync(int id) {
            var studentToEdit = await studentService.GetByIdAsync(id);
            if (studentToEdit == null) {
                return View("NotFound");
            }
            return View(studentToEdit);
        }
        
        [Authorize(Roles = "Admin, Teacher")]
        [HttpPost]
        public async Task<IActionResult> EditAsync(int id, StudentDto student) {
            await studentService.UpdateAsync(id, student);
            return RedirectToAction("Index");
        }
        
        [Authorize(Roles = "Admin")]    // mazani z databaze se nikdy delat nema ale ma se nastavit LockoutEnd
        [HttpPost]
        public async Task<IActionResult> Delete(int id) {
            var studentToDelete = await studentService.GetByIdAsync(id); 
            if (studentToDelete == null) {
                return View("NotFound");
            }
            await studentService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
