using System.Linq.Expressions;
using Internship.UniversityScheduler.Api.Core.CustomExceptions;
using Internship.UniversityScheduler.Api.Core.Models;
using Internship.UniversityScheduler.Api.Core.ServiceClasses.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnEntitySharedLogic.CustomExceptions;
using OnEntitySharedLogic.DatabaseGenericRepository;
using OnEntitySharedLogic.Extensions;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Services.LambdaExpressionCreator;
using OnEntitySharedLogic.Utils;

namespace Internship.UniversityScheduler.Api.Core.ServiceClasses;

[Registration(Type = RegistrationKind.Scoped)]
public class UniversityGroupService : IUniversityGroupService
{
    private readonly IStudentService _studentService;
    private readonly IDatabaseGenericRepository<UniversityGroup> _universityGroupRepository;
    private readonly ILogger<UniversityGroupService> _logger;
    private readonly IExpressionBuilder _expressionBuilder;
    private readonly IServiceProvider _serviceProvider;

    public UniversityGroupService(
        IDatabaseGenericRepository<UniversityGroup> universityGroupRepository,
        IStudentService studentService, 
        ILogger<UniversityGroupService> logger, 
        IExpressionBuilder expressionBuilder, 
        IServiceProvider serviceProvider)
    {
        _universityGroupRepository = universityGroupRepository;
        _studentService = studentService;
        _logger = logger;
        _expressionBuilder = expressionBuilder;
        _serviceProvider = serviceProvider;
    }

    public async Task<UniversityGroup?> GetUniversityGroupByQueryAsync(Expression<Func<UniversityGroup, bool>> query)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving an university group by query has been made!");
        return await _universityGroupRepository.GetEntityByQueryAsync(query);
    }

    public async Task<UniversityGroup> GetUniversityGroupByIdAsync(int id)
    {
        return await _universityGroupRepository.GetEntityByQueryAsync(group => group.Id == id) ??
               throw new EntityNotFoundException($"Couldn't find university group with id {id}");
    }

    public async Task<List<UniversityGroup>> GetAllUniversityGroupsAsync()
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> Get all university groups method has been called!");
        return await _universityGroupRepository.GetAllEntitiesAsync();
    }

    public async Task<DatabaseFeedback<UniversityGroup>> GetOrderedUniversityGroupsAsync(PaginationSetting paginationSetting)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of ordered university groups by {paginationSetting.OrderBy} property has been made!");
        var orderByExpression = _expressionBuilder.BuildOrderByExpression<UniversityGroup>(paginationSetting.OrderBy);
        var numberOfEntitiesToSkip = (paginationSetting.PageNumber - 1) * paginationSetting.PageSize;
        return await _universityGroupRepository.GetOrderedEntitiesAsync(numberOfEntitiesToSkip, paginationSetting.PageSize, orderByExpression, paginationSetting.OrderDirection);
    }
    
    public async Task<List<UniversityGroup>> GetUniversityGroupsByQueryAsync(Expression<Func<UniversityGroup, bool>> query)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of university groups by query has been made!");
        return await _universityGroupRepository.GetEntitiesByQueryAsync(query);
    }

    public async Task<DatabaseFeedback<UniversityGroup>> GetFilteredUniversityGroupsAsync(FilteringSettings filteringSettings)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of filtered university groups has been made!");
        var numberOfEntitiesToSkip = (filteringSettings.PageNumber - 1) * filteringSettings.PageSize;
        var universityGroupFilter = _serviceProvider.GetRequiredService<IFilter<UniversityGroup>>();
        return await _universityGroupRepository.GetFilteredEntitiesAsync(numberOfEntitiesToSkip, filteringSettings.PageSize, filteringSettings.FilterBy, universityGroupFilter);
    }

    public async Task<DatabaseFeedback<UniversityGroup>> GetFilteredAndOrderedUniversityGroupsAsync(FilterOrderSettings settings)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of filtered and ordered university groups has been made!");
        var numberOfEntitiesToSkip = (settings.PageNumber - 1) * settings.PageSize;
        var orderByExpression = _expressionBuilder.BuildOrderByExpression<UniversityGroup>(settings.OrderBy);
        var universityGroupFilter = _serviceProvider.GetRequiredService<IFilter<UniversityGroup>>();
        return await _universityGroupRepository.GetFilteredAndOrderedEntitiesAsync(numberOfEntitiesToSkip, settings.PageSize, orderByExpression, settings.OrderDirection, settings.FilterBy, universityGroupFilter);
    }
    
    public async Task<DatabaseFeedback<Student>> GetOrderedAndFilteredStudentsFromGroupAsync(FilterOrderSettings settings, int id)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of filtered and ordered students from university group with id {id} has been made!");
        return await _studentService.GetFilteredAndOrderedStudentsAsync(settings);
    }

    public async Task AddUniversityGroupAsync(UniversityGroup universityGroup)
    {
        await QueueAddUniversityGroupAsync(universityGroup);
        await _universityGroupRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully added an university group!");
    }

    public async Task AddUniversityGroupsAsync(List<UniversityGroup> universityGroups)
    {
        await QueueAddUniversityGroupsAsync(universityGroups);
        await _universityGroupRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully added a list of university groups!");
    }

    public async Task UpdateUniversityGroupByIdAsync(int id, UniversityGroup universityGroup, string updatedUniversityGroupJson)
    {
        await QueueUpdateUniversityGroupByIdAsync(id, universityGroup, updatedUniversityGroupJson);
        await _universityGroupRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully updated a group having the id {id}");
    }

    public async Task DeleteUniversityGroupByIdAsync(int id)
    {
        await QueueDeleteUniversityGroupByIdAsync(id);
        await _universityGroupRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted an university group having the id {id}");
    }

    public async Task DeleteAllUniversityGroupsAsync()
    {
        QueueDeleteAllUniversityGroups();
        await _universityGroupRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted all university groups");
    }

    public async Task AddStudentInGroupAsync(int studentId, int groupId)
    {
        await QueueAddStudentInGroupAsync(studentId, groupId);
        await _universityGroupRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully added student with id {studentId} in group with id {groupId}");
    }

    public async Task RemoveStudentFromGroupAsync(int studentId, int groupId)
    {
        await QueueRemoveStudentFromGroupAsync(studentId, groupId);
        await _universityGroupRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully removed student with id {studentId} from group with id {groupId}");
    }

    public async Task AddMultipleStudentsInGroupAsync(int groupId, List<int> studentIds)
    {
        var allAdded = await QueueAddMultipleStudentsInGroupAsync(groupId, studentIds);
        await _universityGroupRepository.SaveChangesAsync();
        if (allAdded)
        {
            _logger.LogInformation($"\n{DateTime.Now} ---> Successfully added all students to group with id {groupId}!");
        }
        else
        {
            _logger.LogInformation($"\n{DateTime.Now} ---> Successfully added some of the students to the group with id {groupId}!");
        }

    }

    public async Task RemoveAllStudentsFromGroupAsync(int groupId)
    {
        await QueueRemoveAllStudentsFromGroupAsync(groupId);
        await _universityGroupRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully removed students from group with id {groupId}");
    }

    public async Task QueueAddUniversityGroupAsync(UniversityGroup universityGroup)
    {
        universityGroup.ValidateEntity();
        await _universityGroupRepository.AddEntityAsync(universityGroup);
    }

    public async Task QueueAddUniversityGroupsAsync(List<UniversityGroup> universityGroups)
    {
        foreach (var universityGroup in universityGroups)
        {
            universityGroup.ValidateEntity();
            await _universityGroupRepository.AddEntityAsync(universityGroup);
        }
    }

    public async Task QueueUpdateUniversityGroupByIdAsync(int id, UniversityGroup universityGroup, string updatedUniversityGroupJson)
    {
        var existingGroup = await GetUniversityGroupByIdAsync(id);
        universityGroup.ValidateEntity();
        _universityGroupRepository.UpdateEntity(existingGroup, updatedUniversityGroupJson);
    }

    public async Task QueueDeleteUniversityGroupByIdAsync(int id)
    {
        var existingGroup = await GetUniversityGroupByIdAsync(id);
        _universityGroupRepository.DeleteEntity(existingGroup);
        await QueueRemoveAllStudentsFromGroupAsync(id);
    }

    public void QueueDeleteAllUniversityGroups()
    {
        _universityGroupRepository.DeleteAllEntities();
    }

    public async Task<bool> QueueAddMultipleStudentsInGroupAsync(int groupId, List<int> studentIds)
    {
        var allAdded = true;
        
        var existingGroup = await GetUniversityGroupByIdAsync(groupId);

        if (existingGroup.NumberOfMembers == existingGroup.MaxSize)
        {
            throw new FullUniversityGroupException($"The group with id {groupId} is full!");
        }
        
        foreach (var studentId in studentIds)
        {
            var existingStudent = await _studentService.GetStudentByIdAsync(studentId);

            if (existingStudent.UniversityGroupId is not null)
            {
                allAdded = false;
                continue;
            }
            
            if (existingGroup.NumberOfMembers > existingGroup.MaxSize)
            {
                allAdded = false;
                break;
            }

            existingStudent.UniversityGroupId = groupId;
            existingGroup.NumberOfMembers += 1;
        }

        return allAdded;
    }
    
    public async Task QueueAddStudentInGroupAsync(int studentId, int groupId)
    {
        //does the group exist?
        var existingGroup = await GetUniversityGroupByIdAsync(groupId);

        if (existingGroup.NumberOfMembers == existingGroup.MaxSize)
        {
            throw new FullUniversityGroupException($"The group with id {groupId} is full!");
        }
        
        var existingStudent = await _studentService.GetStudentByIdAsync(studentId);

        if (existingStudent.UniversityGroupId is not null)
        {
            throw new UniversityGroupStudentDuplicationException(
                $"A student having id {studentId} already exists in a group with id {existingStudent.UniversityGroupId}");
        }

        existingStudent.UniversityGroupId = groupId;
        existingGroup.NumberOfMembers += 1;
    }

    public async Task QueueRemoveStudentFromGroupAsync(int studentId, int groupId)
    {
        var existingGroup = await GetUniversityGroupByIdAsync(groupId);

        if (existingGroup.NumberOfMembers == 0)
        {
            throw new EmptyUniversityGroupException($"The university group with id {groupId} does not have any students!");
        }

        var studentFromGroup = await _studentService.GetStudentByQueryAsync(student =>
            student.Id == studentId && student.UniversityGroupId == groupId);

        if (studentFromGroup is null)
        {
            throw new EntityNotFoundException(
                $"A student having id {studentId} does not exist in the group with id {groupId}!");
        }

        studentFromGroup.UniversityGroupId = null;
        existingGroup.NumberOfMembers -= 1;
    }

    public async Task QueueRemoveAllStudentsFromGroupAsync(int groupId)
    {
        var existingGroup = await GetUniversityGroupByIdAsync(groupId);

        if (existingGroup.NumberOfMembers == 0)
        {
            return;
        }

        var existingStudentsInGroup =
            await _studentService.GetStudentsByQueryAsync(student => student.UniversityGroupId == groupId);
        
        foreach (var student in existingStudentsInGroup)
        {
            student.UniversityGroupId = null;
        }

        existingGroup.NumberOfMembers = 0;
    }
}