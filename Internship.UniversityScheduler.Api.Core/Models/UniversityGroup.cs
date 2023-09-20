using System.ComponentModel.DataAnnotations.Schema;
using Internship.UniversityScheduler.Library.SharedEnums;
using OnEntitySharedLogic.Utils;

namespace Internship.UniversityScheduler.Api.Core.Models;

public class UniversityGroup : IEntity
{
    [Validate(0)] 
    public int NumberOfMembers { get; set; }

    [Validate(0)] 
    public int MaxSize { get; set; }

    [Column(TypeName = "varchar(256)")]
    public string Name { get; set; } = string.Empty;

    public UniversitySpecialization? Specialization { get; set; }
    public List<Student> Students { get; set; } = new();
    public Catalogue? Catalogue { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
}