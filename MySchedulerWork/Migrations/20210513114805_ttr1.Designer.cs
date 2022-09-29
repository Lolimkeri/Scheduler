﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MySchedulerWork.Data;

namespace MySchedulerWork.Migrations
{
    [DbContext(typeof(MyAppContext))]
    [Migration("20210513114805_ttr1")]
    partial class ttr1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MySchedulerWork.Models.Audience", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Audiences");
                });

            modelBuilder.Entity("MySchedulerWork.Models.CourseProgram", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CourseProgram");
                });

            modelBuilder.Entity("MySchedulerWork.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AmountOfStudents")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("MySchedulerWork.Models.Pair", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AudienceId")
                        .HasColumnType("int");

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int?>("Program_SubjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AudienceId");

                    b.HasIndex("Program_SubjectId");

                    b.ToTable("Pairs");
                });

            modelBuilder.Entity("MySchedulerWork.Models.PairToFGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<int?>("PairId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("PairId");

                    b.ToTable("PairToFGroup");
                });

            modelBuilder.Entity("MySchedulerWork.Models.ProgramToGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CourseProgramId")
                        .HasColumnType("int");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CourseProgramId");

                    b.HasIndex("GroupId");

                    b.ToTable("ProgramToGroups");
                });

            modelBuilder.Entity("MySchedulerWork.Models.Program_Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("LabTeacherId")
                        .HasColumnType("int");

                    b.Property<int?>("MainTeacherId")
                        .HasColumnType("int");

                    b.Property<int?>("ProgramIdId")
                        .HasColumnType("int");

                    b.Property<int?>("SubjectId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LabTeacherId");

                    b.HasIndex("MainTeacherId");

                    b.HasIndex("ProgramIdId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Program_Subjects");
                });

            modelBuilder.Entity("MySchedulerWork.Models.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("MySchedulerWork.Models.Teacher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FutherName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("MySchedulerWork.Models.Pair", b =>
                {
                    b.HasOne("MySchedulerWork.Models.Audience", "Audience")
                        .WithMany()
                        .HasForeignKey("AudienceId");

                    b.HasOne("MySchedulerWork.Models.Program_Subject", "Program_Subject")
                        .WithMany()
                        .HasForeignKey("Program_SubjectId");
                });

            modelBuilder.Entity("MySchedulerWork.Models.PairToFGroup", b =>
                {
                    b.HasOne("MySchedulerWork.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");

                    b.HasOne("MySchedulerWork.Models.Pair", "Pair")
                        .WithMany()
                        .HasForeignKey("PairId");
                });

            modelBuilder.Entity("MySchedulerWork.Models.ProgramToGroup", b =>
                {
                    b.HasOne("MySchedulerWork.Models.CourseProgram", "CourseProgram")
                        .WithMany("ProgramToGroup")
                        .HasForeignKey("CourseProgramId");

                    b.HasOne("MySchedulerWork.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");
                });

            modelBuilder.Entity("MySchedulerWork.Models.Program_Subject", b =>
                {
                    b.HasOne("MySchedulerWork.Models.Teacher", "LabTeacher")
                        .WithMany()
                        .HasForeignKey("LabTeacherId");

                    b.HasOne("MySchedulerWork.Models.Teacher", "MainTeacher")
                        .WithMany()
                        .HasForeignKey("MainTeacherId");

                    b.HasOne("MySchedulerWork.Models.CourseProgram", "ProgramId")
                        .WithMany("Program_Subject")
                        .HasForeignKey("ProgramIdId");

                    b.HasOne("MySchedulerWork.Models.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId");
                });
#pragma warning restore 612, 618
        }
    }
}