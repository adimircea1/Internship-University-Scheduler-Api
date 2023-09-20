﻿// <auto-generated />
using System;
using Internship.UniversityScheduler.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Internship.UniversityScheduler.Api.Infrastructure.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230919111743_MadePersonalEmailOfStudentUnique")]
    partial class MadePersonalEmailOfStudentUnique
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Internship.UniversityScheduler.Api.Core.Models.Attendance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CourseId")
                        .HasColumnType("integer");

                    b.Property<string>("DateOfTheCourse")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("StudentId")
                        .HasColumnType("integer");

                    b.Property<string>("TimeOfTheCourse")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("Attendances");
                });

            modelBuilder.Entity("Internship.UniversityScheduler.Api.Core.Models.Catalogue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("UniversityGroupId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UniversityGroupId")
                        .IsUnique();

                    b.ToTable("Catalogues");
                });

            modelBuilder.Entity("Internship.UniversityScheduler.Api.Core.Models.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasColumnType("varchar(256)");

                    b.Property<int>("Domain")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfCredits")
                        .HasColumnType("integer");

                    b.Property<int>("ProfessorId")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProfessorId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("Internship.UniversityScheduler.Api.Core.Models.Grade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CatalogueId")
                        .HasColumnType("integer");

                    b.Property<int>("CourseId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DateOfGrade")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("StudentId")
                        .HasColumnType("integer");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CatalogueId");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("Internship.UniversityScheduler.Api.Core.Models.Professor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BirthdayDate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("varchar(30)");

                    b.Property<int>("Speciality")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.ToTable("Professors");
                });

            modelBuilder.Entity("Internship.UniversityScheduler.Api.Core.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BirthdayDate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PersonalEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("varchar(30)");

                    b.Property<int>("StudyYear")
                        .HasColumnType("integer");

                    b.Property<int?>("UniversityGroupId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("PersonalEmail")
                        .IsUnique();

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.HasIndex("UniversityGroupId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("Internship.UniversityScheduler.Api.Core.Models.UniversityGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("MaxSize")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(256)");

                    b.Property<int>("NumberOfMembers")
                        .HasColumnType("integer");

                    b.Property<int>("Specialization")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("UniversityGroups");
                });

            modelBuilder.Entity("Internship.UniversityScheduler.Api.Core.Models.Attendance", b =>
                {
                    b.HasOne("Internship.UniversityScheduler.Api.Core.Models.Course", "Course")
                        .WithMany("Attendances")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Internship.UniversityScheduler.Api.Core.Models.Student", "Student")
                        .WithMany("Attendances")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Internship.UniversityScheduler.Api.Core.Models.Catalogue", b =>
                {
                    b.HasOne("Internship.UniversityScheduler.Api.Core.Models.UniversityGroup", "UniversityGroup")
                        .WithOne("Catalogue")
                        .HasForeignKey("Internship.UniversityScheduler.Api.Core.Models.Catalogue", "UniversityGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UniversityGroup");
                });

            modelBuilder.Entity("Internship.UniversityScheduler.Api.Core.Models.Course", b =>
                {
                    b.HasOne("Internship.UniversityScheduler.Api.Core.Models.Professor", "Professor")
                        .WithMany("Courses")
                        .HasForeignKey("ProfessorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Professor");
                });

            modelBuilder.Entity("Internship.UniversityScheduler.Api.Core.Models.Grade", b =>
                {
                    b.HasOne("Internship.UniversityScheduler.Api.Core.Models.Catalogue", "Catalogue")
                        .WithMany("Grades")
                        .HasForeignKey("CatalogueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Internship.UniversityScheduler.Api.Core.Models.Course", "Course")
                        .WithMany("Grades")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Internship.UniversityScheduler.Api.Core.Models.Student", "Student")
                        .WithMany("Grades")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Catalogue");

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Internship.UniversityScheduler.Api.Core.Models.Student", b =>
                {
                    b.HasOne("Internship.UniversityScheduler.Api.Core.Models.UniversityGroup", "UniversityGroup")
                        .WithMany("Students")
                        .HasForeignKey("UniversityGroupId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("UniversityGroup");
                });

            modelBuilder.Entity("Internship.UniversityScheduler.Api.Core.Models.Catalogue", b =>
                {
                    b.Navigation("Grades");
                });

            modelBuilder.Entity("Internship.UniversityScheduler.Api.Core.Models.Course", b =>
                {
                    b.Navigation("Attendances");

                    b.Navigation("Grades");
                });

            modelBuilder.Entity("Internship.UniversityScheduler.Api.Core.Models.Professor", b =>
                {
                    b.Navigation("Courses");
                });

            modelBuilder.Entity("Internship.UniversityScheduler.Api.Core.Models.Student", b =>
                {
                    b.Navigation("Attendances");

                    b.Navigation("Grades");
                });

            modelBuilder.Entity("Internship.UniversityScheduler.Api.Core.Models.UniversityGroup", b =>
                {
                    b.Navigation("Catalogue");

                    b.Navigation("Students");
                });
#pragma warning restore 612, 618
        }
    }
}
