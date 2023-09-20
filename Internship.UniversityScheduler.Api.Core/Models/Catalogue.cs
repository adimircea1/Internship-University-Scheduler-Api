using System.ComponentModel.DataAnnotations.Schema;
using OnEntitySharedLogic.Utils;

namespace Internship.UniversityScheduler.Api.Core.Models;

public class Catalogue : IEntity
{
    public List<Grade> Grades { get; set; } = new();
    public int UniversityGroupId { get; set; }
    public UniversityGroup? UniversityGroup { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
}