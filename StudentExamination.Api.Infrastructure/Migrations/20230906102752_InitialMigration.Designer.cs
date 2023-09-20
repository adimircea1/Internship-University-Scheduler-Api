﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using StudentExamination.Api.Infrastructure.Repository;

#nullable disable

namespace StudentExamination.Api.Infrastructure.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230906102752_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("StudentExamination.Api.Core.Models.ExaminationModels.AnswerOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ProblemId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProblemId");

                    b.ToTable("AnswerOption");
                });

            modelBuilder.Entity("StudentExamination.Api.Core.Models.ExaminationModels.CorrectAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ProblemId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProblemId");

                    b.ToTable("CorrectAnswer");
                });

            modelBuilder.Entity("StudentExamination.Api.Core.Models.ExaminationModels.Exam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AvailableFrom")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("AvailableUntil")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CourseId")
                        .HasColumnType("integer");

                    b.Property<int>("ExamDuration")
                        .HasColumnType("integer");

                    b.Property<int?>("FinalGrade")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("FirstVisitTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("PartialGradingAllowed")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Exams");
                });

            modelBuilder.Entity("StudentExamination.Api.Core.Models.ExaminationModels.ExamAttendance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ExamId")
                        .HasColumnType("integer");

                    b.Property<int>("StudentId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ExamId");

                    b.ToTable("ExamAttendance");
                });

            modelBuilder.Entity("StudentExamination.Api.Core.Models.ExaminationModels.Problem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ExamId")
                        .HasColumnType("integer");

                    b.Property<int>("Points")
                        .HasColumnType("integer");

                    b.Property<int>("ProblemType")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ExamId");

                    b.ToTable("Problems");
                });

            modelBuilder.Entity("StudentExamination.Api.Core.Models.ExaminationModels.AnswerOption", b =>
                {
                    b.HasOne("StudentExamination.Api.Core.Models.ExaminationModels.Problem", "Problem")
                        .WithMany("AnswerOptions")
                        .HasForeignKey("ProblemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Problem");
                });

            modelBuilder.Entity("StudentExamination.Api.Core.Models.ExaminationModels.CorrectAnswer", b =>
                {
                    b.HasOne("StudentExamination.Api.Core.Models.ExaminationModels.Problem", "Problem")
                        .WithMany("CorrectAnswers")
                        .HasForeignKey("ProblemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Problem");
                });

            modelBuilder.Entity("StudentExamination.Api.Core.Models.ExaminationModels.ExamAttendance", b =>
                {
                    b.HasOne("StudentExamination.Api.Core.Models.ExaminationModels.Exam", "Exam")
                        .WithMany("ExamAttendances")
                        .HasForeignKey("ExamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exam");
                });

            modelBuilder.Entity("StudentExamination.Api.Core.Models.ExaminationModels.Problem", b =>
                {
                    b.HasOne("StudentExamination.Api.Core.Models.ExaminationModels.Exam", "Exam")
                        .WithMany("Problems")
                        .HasForeignKey("ExamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exam");
                });

            modelBuilder.Entity("StudentExamination.Api.Core.Models.ExaminationModels.Exam", b =>
                {
                    b.Navigation("ExamAttendances");

                    b.Navigation("Problems");
                });

            modelBuilder.Entity("StudentExamination.Api.Core.Models.ExaminationModels.Problem", b =>
                {
                    b.Navigation("AnswerOptions");

                    b.Navigation("CorrectAnswers");
                });
#pragma warning restore 612, 618
        }
    }
}
