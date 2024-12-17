﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using proje.Data;

#nullable disable

namespace proje.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241217124445_mig1")]
    partial class mig1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CourseStudent", b =>
                {
                    b.Property<int>("Courses_SelectedCourse_ID")
                        .HasColumnType("int");

                    b.Property<int>("StudentsStudent_ID")
                        .HasColumnType("int");

                    b.HasKey("Courses_SelectedCourse_ID", "StudentsStudent_ID");

                    b.HasIndex("StudentsStudent_ID");

                    b.ToTable("CourseStudent");
                });

            modelBuilder.Entity("proje.Models.Entities.Course", b =>
                {
                    b.Property<int>("Course_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Course_ID"));

                    b.Property<string>("Course_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Credits")
                        .HasColumnType("int");

                    b.Property<int?>("Instructor_ID")
                        .HasColumnType("int");

                    b.Property<int>("Instructor_ID1")
                        .HasColumnType("int");

                    b.HasKey("Course_ID");

                    b.HasIndex("Instructor_ID1");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("proje.Models.Entities.Instructor", b =>
                {
                    b.Property<int>("Instructor_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Instructor_ID"));

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("First_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Last_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Instructor_ID");

                    b.ToTable("Instructors");
                });

            modelBuilder.Entity("proje.Models.Entities.Student", b =>
                {
                    b.Property<int>("Student_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Student_ID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("First_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Last_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Major")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Student_ID");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("CourseStudent", b =>
                {
                    b.HasOne("proje.Models.Entities.Course", null)
                        .WithMany()
                        .HasForeignKey("Courses_SelectedCourse_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("proje.Models.Entities.Student", null)
                        .WithMany()
                        .HasForeignKey("StudentsStudent_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("proje.Models.Entities.Course", b =>
                {
                    b.HasOne("proje.Models.Entities.Instructor", "Instructor")
                        .WithMany("Courses")
                        .HasForeignKey("Instructor_ID1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Instructor");
                });

            modelBuilder.Entity("proje.Models.Entities.Instructor", b =>
                {
                    b.Navigation("Courses");
                });
#pragma warning restore 612, 618
        }
    }
}
