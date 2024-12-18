using Magistri.DTO;
using Magistri.Models;
using Magistri.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Magistri.Services;

public class GradeService {
    ApplicationDbContext dbContext;

    public GradeService(ApplicationDbContext dbContext) { // konstruktor
        this.dbContext = dbContext;
    }

    private GradeDto ModelToDto(Grade gradeToEdit) {
        return new GradeDto {
            Id = gradeToEdit.Id,
            Date = gradeToEdit.Date,
            Mark = gradeToEdit.Mark,
            StudentId = gradeToEdit.Student.Id,
            SubjectId = gradeToEdit.Subject.Id,
            Topic = gradeToEdit.Topic
        };
    }

    private async Task<Grade> DtoToModelAsync(GradeDto newGrade) {
        return new Grade {
            Id = newGrade.Id,
            Date = DateTime.Now,
            Mark = newGrade.Mark,
            Student = await dbContext.Students.FirstOrDefaultAsync(s => s.Id == newGrade.StudentId),
            Subject = await dbContext.Subjects.FirstOrDefaultAsync(s => s.Id == newGrade.SubjectId),
            Topic = newGrade.Topic
        };
    }

    internal async Task<GradeDto> GetGradeByIdAsync(int id) {
        var gradeToEdit = await dbContext.Grades.Include(g => g.Student).Include(gr=>gr.Subject).FirstOrDefaultAsync(gr => gr.Id == id);
        if (gradeToEdit == null) {
            return null;
        }
        return ModelToDto(gradeToEdit);
    }

    public async Task<IEnumerable<GradesVM>> GetAllGradesAsync() {
        var grades = await dbContext.Grades.Include(g => g.Student).Include(g => g.Subject).ToListAsync();
        List<GradesVM> gradesVMs = new List<GradesVM>();
        foreach (var grade in grades) {
            gradesVMs.Add(new GradesVM {
             Id = grade.Id,
             Date = grade.Date,
             Mark = grade.Mark,
             StudentName = grade.Student.FirstName,
             StudentLastName = grade.Student.LastName,
             SubjectName = grade.Subject.Name,
             Topic = grade.Topic
            });
        }
        return gradesVMs;
    }
    public async Task CreateGradeAsync(GradeDto newGrade) {
        Grade gradeToInsert = await DtoToModelAsync(newGrade);
        await dbContext.Grades.AddAsync(gradeToInsert);
        await dbContext.SaveChangesAsync();
    }
    internal async Task UpdateGradeAsync(GradeDto editedGrade) {
        dbContext.Grades.Update(await DtoToModelAsync(editedGrade));
        await dbContext.SaveChangesAsync();
    }
    internal async Task<GradesDropdownsVM> GetDropdownsDataAsync() {
        var gradesDropdownsData = new GradesDropdownsVM() {
            Students = await dbContext.Students.OrderBy(student => student.LastName).ToListAsync(),
            Subjects = await dbContext.Subjects.OrderBy(subject => subject.Name).ToListAsync()
        };
        return gradesDropdownsData;
    }
    internal async Task DeleteGradeAsync(int id) {
        var gradeToDelete = await dbContext.Grades.FirstOrDefaultAsync(gr=>gr.Id == id);
        dbContext.Grades.Remove(gradeToDelete);
        await dbContext.SaveChangesAsync();
    }
}