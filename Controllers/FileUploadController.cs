using System.Globalization;
using System.Xml;
using Magistri.DTO;
using Magistri.Services;
using Microsoft.AspNetCore.Mvc;

namespace Magistri.Controllers;

public class FileUploadController : Controller {
    StudentService studentService;

    public FileUploadController(StudentService studentService) {    // konstruktor
        this.studentService = studentService;
    }

    [HttpPost]
    // GET
    public async Task<IActionResult> UploadAsync(IFormFile file) {
        string filePath = Path.GetFullPath(file.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create)) {
            await file.CopyToAsync(stream);     // stream je proud bitu
            stream.Close();
            
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filePath);     //Loadxml?
            XmlElement root = xmlDocument.DocumentElement;

            foreach (XmlNode node in root.SelectNodes("/Students/Student")) {
                StudentDto studentDto = new StudentDto() {
                    DateOfBirth = DateOnly.Parse(node.ChildNodes[2].InnerText, CultureInfo.CreateSpecificCulture("cs-CZ")), // datum, mena, formatovani tisicu
                    FirstName = node.ChildNodes[0].InnerText,
                    LastName = node.ChildNodes[1].InnerText,
                };
                await studentService.CreateStudentAsync(studentDto);


            }
        }
        return RedirectToAction("Index", "Students");
    }
}