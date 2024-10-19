using System.ComponentModel.DataAnnotations.Schema;
using OnEntitySharedLogic.Utils;

namespace Internship.UniversityScheduler.Api.Core.Models;

public class Grade : IEntity
{
    [Validate(0, 10)] 
    public int Value { get; set; }

    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public int CatalogueId { get; set; }
    public Catalogue? Catalogue { get; set; }
    public Student? Student { get; set; }
    public Course? Course { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public DateTime DateOfGrade { get; set; }
}