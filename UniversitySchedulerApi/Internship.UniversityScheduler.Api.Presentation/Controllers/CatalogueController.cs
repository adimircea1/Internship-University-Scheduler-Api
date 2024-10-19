using AutoMapper;
using Internship.UniversityScheduler.Api.Core.Models;
using Internship.UniversityScheduler.Api.Core.Models.Dtos.GetDtos;
using Internship.UniversityScheduler.Api.Core.Models.Dtos.PatchDto;
using Internship.UniversityScheduler.Api.Core.Models.Dtos.PostDtos;
using Internship.UniversityScheduler.Api.Core.ServiceClasses.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnEntitySharedLogic.CustomAuthorizationAttributeWithClaims;
using OnEntitySharedLogic.Models;

namespace Internship.UniversityScheduler.Api.Presentation.Controllers;

[ApiController]
[Route("catalogues")]
public class CatalogueController : ControllerBase
{
    private readonly ICatalogueService _catalogueService;
    private readonly IMapper _mapper;

    public CatalogueController(
        ICatalogueService catalogueService,
        IMapper mapper)
    {
        _catalogueService = catalogueService;
        _mapper = mapper;
    }

    [WithClaimsAuthorization("Admin")]
    [HttpPost("catalogue")]
    public async Task<ActionResult> AddCatalogueAsync([FromBody] CatalogueInputDto catalogue)
    {
        await _catalogueService.AddCatalogueAsync(_mapper.Map<Catalogue>(catalogue));
        return Ok("Successfully added a catalogue!");
    }

    [WithClaimsAuthorization("Admin")]
    [HttpPost]
    public async Task<ActionResult> AddCataloguesAsync([FromBody] List<CatalogueInputDto> catalogues)
    {
        await _catalogueService.AddCataloguesAsync(catalogues
            .Select(catalogue => _mapper.Map<Catalogue>(catalogue))
            .ToList());
        return Ok("Successfully added a list of catalogues!");
    }

    [WithClaimsAuthorization("Professor", "Admin")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CatalogueDto>> GetCatalogueByIdAsync([FromRoute] int id)
    {
        var existingCatalogue = await _catalogueService.GetCatalogueByIdAsync(id);
        return Ok(_mapper.Map<CatalogueDto>(existingCatalogue));
    }

    [Authorize]
    [HttpGet("from/{universityGroupId:int}")]
    public async Task<ActionResult<CatalogueDto>> GetCatalogueByUniversityGroupIdAsync([FromRoute] int universityGroupId)
    {
        var existingCatalogue = await _catalogueService.GetCatalogueByUniversityGroupIdAsync(universityGroupId);
        return Ok(_mapper.Map<CatalogueDto>(existingCatalogue));
    }

    [WithClaimsAuthorization("Professor", "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<CatalogueDto>>> GetAllCataloguesAsync()
    {
        return Ok((await _catalogueService.GetAllCataloguesAsync())
            .Select(catalogue => _mapper.Map<CatalogueDto>(catalogue))
            .ToList());
    }
    
    [WithClaimsAuthorization("Professor", "Admin")]
    [HttpPost("ordered")]
    public async Task<ActionResult<DatabaseFeedback<CatalogueDto>>> GetOrderedCataloguesAsync([FromBody] PaginationSetting paginationSetting)
    {
        var databaseFeedback = await _catalogueService.GetOrderedCataloguesAsync(paginationSetting);
        return Ok(new DatabaseFeedback<CatalogueDto>
        {
            Entities = databaseFeedback.Entities.Select(catalogue => _mapper.Map<CatalogueDto>(catalogue)).ToList(),
            NumberOfEntities = databaseFeedback.NumberOfEntities
        });
    }

    [WithClaimsAuthorization("Professor", "Admin")]
    [HttpPost("filtered")]
    public async Task<ActionResult<DatabaseFeedback<CatalogueDto>>> GetFilteredCataloguesAsync([FromBody] FilteringSettings filteringSettings)
    {
        var queryableCatalogues = await _catalogueService.GetFilteredCataloguesAsync(filteringSettings);
        return Ok(new DatabaseFeedback<CatalogueDto>
        {
            Entities = queryableCatalogues.Entities.Select(catalogue => _mapper.Map<CatalogueDto>(catalogue)).ToList(),
            NumberOfEntities = queryableCatalogues.NumberOfEntities
        });
    }
    
    [WithClaimsAuthorization("Professor", "Admin")]
    [HttpPost("filtered-ordered")]
    public async Task<ActionResult<DatabaseFeedback<CatalogueDto>>> GetFilteredAndOrderedCataloguesAsync([FromBody] FilterOrderSettings settings)
    {
        var databaseFeedback = await _catalogueService.GetFilteredAndOrderedCataloguesAsync(settings);
        return Ok(new DatabaseFeedback<CatalogueDto>
        {
            Entities = databaseFeedback.Entities.Select(catalogue => _mapper.Map<CatalogueDto>(catalogue)).ToList(),
            NumberOfEntities = databaseFeedback.NumberOfEntities
        });
    }
    
    [WithClaimsAuthorization("Professor", "Admin")]
    [HttpPatch("{id:int}")]
    public async Task<ActionResult> UpdateCatalogueByIdAsync([FromRoute] int id, [FromBody] CatalogueUpdatedInputDto catalogue)
    {
        var catalogueJson = HttpContext.Items["RequestBody"] as string;
        await _catalogueService.UpdateCatalogueByIdAsync(id, _mapper.Map<Catalogue>(catalogue), catalogueJson!);
        return Ok($"Successfully updated catalogue with id {id}");
    }

    [WithClaimsAuthorization("Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteCatalogueByIdAsync([FromRoute] int id)
    {
        await _catalogueService.DeleteCatalogueByIdAsync(id);
        return NoContent();
    }

    [WithClaimsAuthorization("Admin")]
    [HttpDelete]
    public async Task<ActionResult> DeleteAllCataloguesAsync()
    {
        await _catalogueService.DeleteAllCataloguesAsync();
        return NoContent();
    }
}