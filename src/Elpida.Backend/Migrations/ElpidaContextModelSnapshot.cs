﻿// <auto-generated />
using System;
using Elpida.Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Elpida.Backend.Migrations
{
    [DbContext(typeof(ElpidaContext))]
    partial class ElpidaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Benchmark.BenchmarkModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ScoreComparison")
                        .HasColumnType("int");

                    b.Property<string>("ScoreUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("Uuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Uuid")
                        .IsUnique();

                    b.ToTable("Benchmarks");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Benchmark.BenchmarkTaskModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("BenchmarkId")
                        .HasColumnType("bigint");

                    b.Property<bool>("CanBeDisabled")
                        .HasColumnType("bit");

                    b.Property<bool>("CanBeMultiThreaded")
                        .HasColumnType("bit");

                    b.Property<bool>("IsCountedOnResults")
                        .HasColumnType("bit");

                    b.Property<long>("IterationsToRun")
                        .HasColumnType("bigint");

                    b.Property<long>("TaskId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("BenchmarkId");

                    b.HasIndex("TaskId");

                    b.ToTable("BenchmarkTaskModel");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Cpu.CpuModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("AdditionalInfo")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Architecture")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Caches")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Features")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Frequency")
                        .HasColumnType("bigint");

                    b.Property<string>("ModelName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Smt")
                        .HasColumnType("bit");

                    b.Property<string>("Vendor")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Vendor", "ModelName", "AdditionalInfo")
                        .IsUnique();

                    b.ToTable("Cpus");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.ElpidaVersion.ElpidaVersionModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("CompilerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CompilerVersion")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("VersionBuild")
                        .HasColumnType("int");

                    b.Property<int>("VersionMajor")
                        .HasColumnType("int");

                    b.Property<int>("VersionMinor")
                        .HasColumnType("int");

                    b.Property<int>("VersionRevision")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VersionMajor", "VersionMinor", "VersionRevision", "VersionBuild", "CompilerName", "CompilerVersion")
                        .IsUnique();

                    b.ToTable("ElpidaVersions");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Os.OsModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Category", "Name", "Version")
                        .IsUnique();

                    b.ToTable("Oses");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Result.ResultModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Affinity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("BenchmarkId")
                        .HasColumnType("bigint");

                    b.Property<long>("ElpidaVersionId")
                        .HasColumnType("bigint");

                    b.Property<double>("JoinOverhead")
                        .HasColumnType("float");

                    b.Property<double>("LockOverhead")
                        .HasColumnType("float");

                    b.Property<double>("LoopOverhead")
                        .HasColumnType("float");

                    b.Property<long>("MemorySize")
                        .HasColumnType("bigint");

                    b.Property<double>("NotifyOverhead")
                        .HasColumnType("float");

                    b.Property<double>("NowOverhead")
                        .HasColumnType("float");

                    b.Property<long>("OsId")
                        .HasColumnType("bigint");

                    b.Property<long>("PageSize")
                        .HasColumnType("bigint");

                    b.Property<double>("Score")
                        .HasColumnType("float");

                    b.Property<double>("SleepOverhead")
                        .HasColumnType("float");

                    b.Property<double>("TargetTime")
                        .HasColumnType("float");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<long>("TopologyId")
                        .HasColumnType("bigint");

                    b.Property<double>("WakeupOverhead")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("BenchmarkId");

                    b.HasIndex("ElpidaVersionId");

                    b.HasIndex("OsId");

                    b.HasIndex("TopologyId");

                    b.ToTable("Results");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Result.TaskResultModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("BenchmarkResultId")
                        .HasColumnType("bigint");

                    b.Property<long>("InputSize")
                        .HasColumnType("bigint");

                    b.Property<double>("MarginOfError")
                        .HasColumnType("float");

                    b.Property<double>("Max")
                        .HasColumnType("float");

                    b.Property<double>("Mean")
                        .HasColumnType("float");

                    b.Property<double>("Min")
                        .HasColumnType("float");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<long>("ResultId")
                        .HasColumnType("bigint");

                    b.Property<long>("SampleSize")
                        .HasColumnType("bigint");

                    b.Property<double>("StandardDeviation")
                        .HasColumnType("float");

                    b.Property<long>("TaskId")
                        .HasColumnType("bigint");

                    b.Property<double>("Tau")
                        .HasColumnType("float");

                    b.Property<double>("Time")
                        .HasColumnType("float");

                    b.Property<double>("Value")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ResultId");

                    b.HasIndex("TaskId");

                    b.ToTable("TaskResultModel");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Statistics.BenchmarkStatisticsModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("BenchmarkId")
                        .HasColumnType("bigint");

                    b.Property<long>("CpuId")
                        .HasColumnType("bigint");

                    b.Property<string>("FrequencyClasses")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("MarginOfError")
                        .HasColumnType("float");

                    b.Property<double>("Max")
                        .HasColumnType("float");

                    b.Property<double>("Mean")
                        .HasColumnType("float");

                    b.Property<double>("Min")
                        .HasColumnType("float");

                    b.Property<long>("SampleSize")
                        .HasColumnType("bigint");

                    b.Property<double>("StandardDeviation")
                        .HasColumnType("float");

                    b.Property<double>("Tau")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("BenchmarkId");

                    b.HasIndex("CpuId", "BenchmarkId")
                        .IsUnique();

                    b.ToTable("BenchmarkStatistics");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Task.TaskModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InputDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InputName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InputProperties")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InputUnit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OutputDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OutputName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OutputProperties")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OutputUnit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ResultAggregation")
                        .HasColumnType("int");

                    b.Property<string>("ResultDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResultName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ResultType")
                        .HasColumnType("int");

                    b.Property<string>("ResultUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("Uuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Uuid")
                        .IsUnique();

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Topology.TopologyModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("CpuId")
                        .HasColumnType("bigint");

                    b.Property<string>("Root")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TopologyHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("TotalLogicalCores")
                        .HasColumnType("int");

                    b.Property<int>("TotalNumaNodes")
                        .HasColumnType("int");

                    b.Property<int>("TotalPackages")
                        .HasColumnType("int");

                    b.Property<int>("TotalPhysicalCores")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CpuId", "TopologyHash")
                        .IsUnique();

                    b.ToTable("Topologies");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Benchmark.BenchmarkTaskModel", b =>
                {
                    b.HasOne("Elpida.Backend.Data.Abstractions.Models.Benchmark.BenchmarkModel", "Benchmark")
                        .WithMany("Tasks")
                        .HasForeignKey("BenchmarkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Elpida.Backend.Data.Abstractions.Models.Task.TaskModel", "Task")
                        .WithMany()
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Benchmark");

                    b.Navigation("Task");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Result.ResultModel", b =>
                {
                    b.HasOne("Elpida.Backend.Data.Abstractions.Models.Benchmark.BenchmarkModel", "Benchmark")
                        .WithMany()
                        .HasForeignKey("BenchmarkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Elpida.Backend.Data.Abstractions.Models.ElpidaVersion.ElpidaVersionModel", "ElpidaVersion")
                        .WithMany()
                        .HasForeignKey("ElpidaVersionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Elpida.Backend.Data.Abstractions.Models.Os.OsModel", "Os")
                        .WithMany()
                        .HasForeignKey("OsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Elpida.Backend.Data.Abstractions.Models.Topology.TopologyModel", "Topology")
                        .WithMany()
                        .HasForeignKey("TopologyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Benchmark");

                    b.Navigation("ElpidaVersion");

                    b.Navigation("Os");

                    b.Navigation("Topology");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Result.TaskResultModel", b =>
                {
                    b.HasOne("Elpida.Backend.Data.Abstractions.Models.Result.ResultModel", "Result")
                        .WithMany("TaskResults")
                        .HasForeignKey("ResultId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Elpida.Backend.Data.Abstractions.Models.Task.TaskModel", "Task")
                        .WithMany()
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Result");

                    b.Navigation("Task");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Statistics.BenchmarkStatisticsModel", b =>
                {
                    b.HasOne("Elpida.Backend.Data.Abstractions.Models.Benchmark.BenchmarkModel", "Benchmark")
                        .WithMany()
                        .HasForeignKey("BenchmarkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Elpida.Backend.Data.Abstractions.Models.Cpu.CpuModel", "Cpu")
                        .WithMany()
                        .HasForeignKey("CpuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Benchmark");

                    b.Navigation("Cpu");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Topology.TopologyModel", b =>
                {
                    b.HasOne("Elpida.Backend.Data.Abstractions.Models.Cpu.CpuModel", "Cpu")
                        .WithMany()
                        .HasForeignKey("CpuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cpu");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Benchmark.BenchmarkModel", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Result.ResultModel", b =>
                {
                    b.Navigation("TaskResults");
                });
#pragma warning restore 612, 618
        }
    }
}
