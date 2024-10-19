// using Internship.UniversityScheduler.Api.Core.CustomExceptions;
// using Internship.UniversityScheduler.Api.Core.Models;
// using Internship.UniversityScheduler.Api.Core.Models.Enums;
// using Internship.UniversityScheduler.Api.Core.ServiceClasses;
// using Internship.UniversityScheduler.Api.Core.ServiceClasses.Abstractions;
// using Internship.UniversityScheduler.Api.Infrastructure;
// using Microsoft.EntityFrameworkCore;
// using OnEntitySharedLogic.CustomExceptions;
// using OnEntitySharedLogic.DatabaseGenericRepository;
//
// namespace Internship.UniversityScheduler.UnitTester;
//
// public class OnUniversityGroupTesting
// {
//     private readonly IUniversityGroupService _universityGroupService;
//     private readonly IStudentService _studentService;
//
//     private readonly UniversityGroup _validUniversityGroup;
//     private readonly Student _validStudent;
//
//     public OnUniversityGroupTesting()
//     {
//         var options = new DbContextOptionsBuilder<DataContext>()
//             .UseInMemoryDatabase(databaseName: "InMemoryDb")
//             .Options;
//
//         var dataContext = new DataContext(options);
//
//         _studentService = new StudentService(new DatabaseGenericRepository<Student>(dataContext));
//         _universityGroupService =
//             new UniversityGroupService(new DatabaseGenericRepository<UniversityGroup>(dataContext), _studentService);
//
//         _validStudent = new Student
//         {
//             FirstName = "student",
//             LastName = "valid",
//             Email = "student@valid.com",
//             Cnp = "1112223334445",
//             StudyYear = 2
//         };
//
//         _validUniversityGroup = new UniversityGroup
//         {
//             Name = "group",
//             DiscordLink = "Link",
//             MaxSize = 2,
//             NumberOfMembers = 0,
//             Specialization = UniversitySpecialization.EconomicAndInformatics
//         };
//     }
//
//     [Fact]
//     public async Task
//         UniversityGroupService_AddStudentInGroupAsync_Throws_FullUniversityGroupException_If_Exceeding_The_MaxSize()
//     {
//         _validUniversityGroup.MaxSize = 1;
//         _validUniversityGroup.NumberOfMembers = 1;
//         await _universityGroupService.AddUniversityGroupAsync(_validUniversityGroup);
//         await _studentService.AddStudentAsync(_validStudent);
//         await Assert.ThrowsAsync<FullUniversityGroupException>(async () =>
//             await _universityGroupService.AddStudentInGroupAsync(_validStudent.Id, _validUniversityGroup.Id));
//         await _studentService.DeleteAllStudentsAsync();
//         await _universityGroupService.DeleteAllUniversityGroupsAsync();
//     }
//
//     [Fact]
//     public async Task
//         UniversityGroupService_RemoveStudentFromGroupAsync_Throws_EmptyUniversityGroupException_If_Trying_To_Remove_Student_From_Empty_Group()
//     {
//         await _universityGroupService.AddUniversityGroupAsync(_validUniversityGroup);
//         await _studentService.AddStudentAsync(_validStudent);
//         await Assert.ThrowsAsync<EmptyUniversityGroupException>(async () =>
//             await _universityGroupService.RemoveStudentFromGroupAsync(_validStudent.Id, _validUniversityGroup.Id));
//         await _studentService.DeleteAllStudentsAsync();
//         await _universityGroupService.DeleteAllUniversityGroupsAsync();
//     }
//
//     [Fact]
//     public async Task
//         UniversityGroupService_AddStudentInGroupAsync_Throws_EntityNotFoundException_If_Adding_A_Non_Existent_Student()
//     {
//         await _universityGroupService.AddUniversityGroupAsync(_validUniversityGroup);
//         await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
//             await _universityGroupService.AddStudentInGroupAsync(_validStudent.Id, _validUniversityGroup.Id));
//         await _universityGroupService.DeleteAllUniversityGroupsAsync();;
//     }
//
//     [Fact]
//     public async Task
//         UniversityGroupService_AddStudentInGroupAsync_Throws_EntityNotFoundException_If_Adding_Student_In_A_Non_Existent_Group()
//     {
//         await _studentService.AddStudentAsync(_validStudent);
//         await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
//             await _universityGroupService.AddStudentInGroupAsync(_validStudent.Id, _validUniversityGroup.Id));
//         await _studentService.DeleteAllStudentsAsync();
//     }
//
//     [Fact]
//     public async Task
//         UniversityGroupService_AddStudentInGroupAsync_Throws_UniversityGroupStudentDuplicationException_If_The_Same_Student_Is_Added_Twice()
//     {
//         await _universityGroupService.AddUniversityGroupAsync(_validUniversityGroup);
//         await _studentService.AddStudentAsync(_validStudent);
//         await _universityGroupService.AddStudentInGroupAsync(_validStudent.Id, _validUniversityGroup.Id);
//         await Assert.ThrowsAsync<UniversityGroupStudentDuplicationException>(async () =>
//                 await _universityGroupService.AddStudentInGroupAsync(_validStudent.Id, _validUniversityGroup.Id));
//         await _studentService.DeleteAllStudentsAsync();
//         await _universityGroupService.DeleteAllUniversityGroupsAsync();
//     }
//
//     [Fact]
//     public async Task UniversityGroupService_AddStudentInGroupAsync_Successfully_Adds_Student_In_Group()
//     {
//         await _universityGroupService.AddUniversityGroupAsync(_validUniversityGroup);
//         await _studentService.AddStudentAsync(_validStudent);
//         await _universityGroupService.AddStudentInGroupAsync(_validStudent.Id, _validUniversityGroup.Id);
//         var existingGroup = await _universityGroupService.GetUniversityGroupByIdAsync(_validUniversityGroup.Id);
//         Assert.Equal(1, existingGroup.NumberOfMembers);
//         Assert.NotNull(await _studentService.GetStudentByQueryAsync(student => student.UniversityGroupId == _validUniversityGroup.Id));
//         await _studentService.DeleteAllStudentsAsync();
//         await _universityGroupService.DeleteAllUniversityGroupsAsync();
//     }
// }