using Magistri.DTO;
using Magistri.Models;
using Microsoft.EntityFrameworkCore;

namespace Magistri.Services;

public class SubjectService {
    private ApplicationDbContext dbContext;
    
    public SubjectService(ApplicationDbContext dbContext) {
        this.dbContext = dbContext;
    }
    private static Subject DtoToModel(SubjectDto subjectDto) {
        return new Subject {
            Id = subjectDto.Id,
            Name = subjectDto.Name
        };
    }
    private static SubjectDto ModelToDto(Subject subject) {
        return new SubjectDto {
            Id = subject.Id,
            Name = subject.Name
        };
    }
    public IEnumerable<SubjectDto> GetAllSubjects() {
        var allSubjects = dbContext.Subjects;
        var subjectsDtos = new List<SubjectDto>();
        foreach (var subject in allSubjects) {
            subjectsDtos.Add(ModelToDto(subject));            
        }
        return subjectsDtos;
    }
    public async Task AddSubjectAsync(SubjectDto subjectDto) {
        await dbContext.Subjects.AddAsync(
            DtoToModel(subjectDto));
        await dbContext.SaveChangesAsync();
    }
    private async Task<Subject> VerifyExistenceAsync(int id) {
        var subject = await dbContext.Subjects.FirstOrDefaultAsync(subject => subject.Id == id);
        if (subject == null) {
            return null;
        }
        return subject;
    }
    internal async Task<SubjectDto> GetSubjectByIdAsync(int id) {
        var subject = await VerifyExistenceAsync(id);
        return ModelToDto(subject);
    }

    internal async Task UpdateAsync(SubjectDto subjectDto, int id) {
        dbContext.Subjects.Update(DtoToModel(subjectDto));
        await dbContext.SaveChangesAsync();
    }

    internal async Task DeleteAsync(int id) {
        var subjectToDelete = await dbContext.Subjects.FirstOrDefaultAsync(subject => subject.Id == id);
        dbContext.Subjects.Remove(subjectToDelete);
        await dbContext.SaveChangesAsync();
    }
    
}