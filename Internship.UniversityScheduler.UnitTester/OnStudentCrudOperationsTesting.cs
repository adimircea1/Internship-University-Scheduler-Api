// using System.ComponentModel.DataAnnotations;
// using Internship.UniversityScheduler.Api.Core.Models;
// using Internship.UniversityScheduler.Api.Core.ServiceClasses;
// using Internship.UniversityScheduler.Api.Core.ServiceClasses.Abstractions;
// using Internship.UniversityScheduler.Api.Infrastructure;
// using Microsoft.EntityFrameworkCore;
// using OnEntitySharedLogic.CustomExceptions;
// using OnEntitySharedLogic.DatabaseGenericRepository;
//
// namespace Internship.UniversityScheduler.UnitTester
// {
//     public class OnStudentCrudOperationsTesting
//     {
//         private readonly IStudentService _studentService;
//         private readonly Student _validStudent;
//         private readonly Student _invalidStudent;
//
//         public OnStudentCrudOperationsTesting()
//         {
//             var options = new DbContextOptionsBuilder<DataContext>()
//                 .UseInMemoryDatabase(databaseName: "InMemoryDb")
//                 .Options;
//
//             var dataContext = new DataContext(options);
//
//             _studentService = new StudentService(new DatabaseGenericRepository<Student>(dataContext));
//             _validStudent = new Student
//             {
//                 FirstName = "student",
//                 LastName = "valid",
//                 Email = "student@valid.com",
//                 Cnp = "1112223334445",
//                 StudyYear = 2
//             };
//
//             _invalidStudent = new Student
//             {
//                 FirstName = "student",
//                 LastName = "valid",
//                 Email = "student@valid.com",
//                 Cnp = "111213334445",
//                 StudyYear = 2
//             };
//         }
//
//         [Fact]
//         public async Task StudentService_AddStudentAsync_Adds_Valid_Student_In_Database_Ok()
//         {
//             await _studentService.AddStudentAsync(_validStudent);
//             var addedStudent = await _studentService.GetStudentByIdAsync(_validStudent.Id);
//             Assert.NotNull(addedStudent);
//             await _studentService.DeleteAllStudentsAsync();
//         }
//
//         [Fact]
//         public async Task StudentService_AddStudentAsync_Throws_ValidationException_For_Invalid_Student()
//         {
//             await Assert.ThrowsAsync<ValidationException>(async () => await _studentService.AddStudentAsync(_invalidStudent));
//         }
//
//         [Fact]
//         public async Task StudentService_GetStudentByIdAsync_Throws_EntityNotFoundException_When_Trying_To_Get_A_Non_Existent_Student()
//         {
//             await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _studentService.GetStudentByIdAsync(_validStudent.Id));
//         }
//
//         [Fact]
//         public async Task StudentService_GetStudentByQueryAsync_Returns_Null_If_Student_Not_Existent_By_Query()
//         {
//             await _studentService.AddStudentAsync(_validStudent);
//
//             //get by id
//             Assert.Null(await _studentService.GetStudentByQueryAsync(student => student.Id == 1902));
//
//             //get by firstName
//             Assert.Null(await _studentService.GetStudentByQueryAsync(student => student.FirstName == "Some random first name"));
//
//             //get by email
//             Assert.Null(await _studentService.GetStudentByQueryAsync(student => student.Email == "Some random email"));
//             await _studentService.DeleteAllStudentsAsync();
//         }
//
//         [Fact]
//         public async Task StudentService_DeleteStudentByIdAsync_Throws_EntityNotFoundException_When_Trying_To_Delete_A_Non_Existent_Student()
//         {
//             await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _studentService.DeleteStudentByIdAsync(_validStudent.Id));
//         }
//
//         [Fact]
//         public async Task StudentService_UpdateStudentByIdAsync_Throws_EntityNotFoundException_When_Trying_To_Update_A_Non_Existent_Student()
//         {
//             await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _studentService.UpdateStudentByIdAsync(_validStudent.Id, new Student()));
//         }
//         
//
//         [Fact]
//         public async Task StudentService_DeleteStudentById_Deletes_Existing_Student_Ok()
//         {
//             await _studentService.AddStudentAsync(_validStudent);
//             await _studentService.DeleteStudentByIdAsync(_validStudent.Id);
//             await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _studentService.GetStudentByIdAsync(_validStudent.Id));
//         }
//
//         [Fact]
//         public async Task StudentService_UpdateStudentById_Updates_Old_Student_With_Valid_New_Student()
//         {
//             await _studentService.AddStudentAsync(_validStudent);
//             var oldFirstName = _validStudent.FirstName;
//             await _studentService.UpdateStudentByIdAsync(_validStudent.Id, new Student { FirstName = "New First Name" });
//
//             Assert.NotEqual(oldFirstName, _validStudent.FirstName);
//             Assert.Equal("New First Name", _validStudent.FirstName);
//             await _studentService.DeleteAllStudentsAsync();
//         }
//     }
// }
