using Internship.AuthorizationAuthentication.Api.Core.Models.AuthenticationAuthorizationModels;
using Internship.AuthorizationAuthentication.Api.Core.Models.Input;
using Internship.AuthorizationAuthentication.Api.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using OnEntitySharedLogic.DatabaseGenericRepository;

namespace Internship.AuthorizationAuthentication.Api.Infrastructure;

public class DataContext : DbContext, IDataContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<User>? Users { get; set; }
    public DbSet<StudentRegisterRequest>? StudentRegisterRequests { get; set; }
    public DbSet<ProfessorRegisterRequest>? ProfessorRegisterRequests { get; set; }

    
    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //set primary keys
        modelBuilder.Entity<User>()
            .HasKey(user => user.Id);

        modelBuilder.Entity<RefreshToken>()
            .HasKey(token => token.Id);

        modelBuilder.Entity<StudentRegisterRequest>()
            .HasKey(request => request.Id);
        
        modelBuilder.Entity<ProfessorRegisterRequest>()
            .HasKey(request => request.Id);

        //set user-token-relation
        modelBuilder.Entity<RefreshToken>()
            .HasOne(refreshToken => refreshToken.User)
            .WithOne(user => user.RefreshToken)
            .HasForeignKey<RefreshToken>(refreshToken => refreshToken.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        //set unique properties
        modelBuilder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();
        
        modelBuilder.Entity<User>()
            .HasIndex(user => user.PersonalEmail)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(user => user.UserName)
            .IsUnique();
        
        //set required properties
        modelBuilder.Entity<User>()
            .Property(user => user.UserName)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(user => user.Email)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(user => user.UserName)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(user => user.Role)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(user => user.PersonalEmail)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(user => user.HashedPassword)
            .IsRequired();
        
        modelBuilder.Entity<User>()
            .Property(user => user.PasswordSalt)
            .IsRequired();
        
        modelBuilder.Entity<RefreshToken>()
            .Property(token => token.RefreshTokenValue)
            .IsRequired();
        
        //Fix DateOnly mapping
        modelBuilder.Entity<StudentRegisterRequest>()
            .Property(request => request.Birthdate)
            .HasConversion(new DateOnlyToStringConverter());
        
        modelBuilder.Entity<ProfessorRegisterRequest>()
            .Property(request => request.Birthdate)
            .HasConversion(new DateOnlyToStringConverter());
    }
}