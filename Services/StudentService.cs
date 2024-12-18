
using Magistri.DTO;
using Magistri.Models;
using Microsoft.EntityFrameworkCore;

namespace Magistri.Services {
    public class StudentService {
        private ApplicationDbContext dbContext;

        public StudentService(ApplicationDbContext dbContext) {
            this.dbContext = dbContext;
        }
        private static StudentDto ModelToDto(Student student) {
            return new StudentDto {
                Id = student.Id,
                DateOfBirth = student.DateOfBirth,
                FirstName = student.FirstName,
                LastName = student.LastName,
            };
        }
        private Student DtoToModel(StudentDto studentDto) {
            return new Student {
                Id = studentDto.Id,
                DateOfBirth = studentDto.DateOfBirth,
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
            };
        }
        public async Task CreateStudentAsync(StudentDto studentDto) {
            await dbContext.Students.AddAsync(DtoToModel(studentDto));
            await dbContext.SaveChangesAsync();
        }
        internal IEnumerable<StudentDto> GetAllStudents() {
           var allStudents = dbContext.Students;

            var studentDtos = new List<StudentDto>();
            foreach (var student in allStudents) {
                studentDtos.Add(ModelToDto(student));
            }
            return studentDtos;
        }
        internal async Task<StudentDto> GetByIdAsync(int id) {
            var studentToEdit = await dbContext.Students.FirstOrDefaultAsync(student =>  student.Id == id);
            if (studentToEdit == null) {
                return null;
            }
            return ModelToDto(studentToEdit);
        }

        internal async Task UpdateAsync(int id, StudentDto updatedStudent) {
            dbContext.Update(DtoToModel(updatedStudent));
            await dbContext.SaveChangesAsync();
        }

        internal async Task DeleteAsync(int id) {
            var studentToDelete = dbContext.Students.FirstOrDefault(student=> student.Id == id);
            dbContext.Students.Remove(studentToDelete);
            await dbContext.SaveChangesAsync();
        }
    }
}
