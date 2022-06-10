﻿// <auto-generated />
using System;
using Elpida.Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Elpida.Backend.Data.Sqlite.Migrations
{
    [DbContext(typeof(ElpidaContext))]
    [Migration("20211017104330_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.11");

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Benchmark.BenchmarkModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ScoreComparison")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ScoreUnit")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Uuid")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Uuid")
                        .IsUnique();

                    b.ToTable("Benchmarks");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Benchmark.BenchmarkTaskModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("BenchmarkId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CanBeDisabled")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CanBeMultiThreaded")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsCountedOnResults")
                        .HasColumnType("INTEGER");

                    b.Property<long>("IterationsToRun")
                        .HasColumnType("INTEGER");

                    b.Property<long>("TaskId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BenchmarkId");

                    b.HasIndex("TaskId");

                    b.ToTable("BenchmarkTaskModel");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Cpu.CpuModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AdditionalInfo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Architecture")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Caches")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Features")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("Frequency")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ModelName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Smt")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Vendor")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Vendor", "ModelName", "AdditionalInfo")
                        .IsUnique();

                    b.ToTable("Cpus");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.ElpidaVersion.ElpidaVersionModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CompilerName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CompilerVersion")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("VersionBuild")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VersionMajor")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VersionMinor")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VersionRevision")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("VersionMajor", "VersionMinor", "VersionRevision", "VersionBuild", "CompilerName", "CompilerVersion")
                        .IsUnique();

                    b.ToTable("ElpidaVersions");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Os.OsModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Category", "Name", "Version")
                        .IsUnique();

                    b.ToTable("Oses");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Result.BenchmarkResultModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Affinity")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("BenchmarkId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ElpidaVersionId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("JoinOverhead")
                        .HasColumnType("REAL");

                    b.Property<double>("LockOverhead")
                        .HasColumnType("REAL");

                    b.Property<double>("LoopOverhead")
                        .HasColumnType("REAL");

                    b.Property<long>("MemorySize")
                        .HasColumnType("INTEGER");

                    b.Property<double>("NotifyOverhead")
                        .HasColumnType("REAL");

                    b.Property<double>("NowOverhead")
                        .HasColumnType("REAL");

                    b.Property<long>("OsId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("PageSize")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Score")
                        .HasColumnType("REAL");

                    b.Property<double>("SleepOverhead")
                        .HasColumnType("REAL");

                    b.Property<double>("TargetTime")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("TEXT");

                    b.Property<long>("TopologyId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("WakeupOverhead")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("BenchmarkId");

                    b.HasIndex("ElpidaVersionId");

                    b.HasIndex("OsId");

                    b.HasIndex("TopologyId");

                    b.ToTable("BenchmarkResults");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Result.TaskResultModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("BenchmarkResultId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("InputSize")
                        .HasColumnType("INTEGER");

                    b.Property<double>("MarginOfError")
                        .HasColumnType("REAL");

                    b.Property<double>("Max")
                        .HasColumnType("REAL");

                    b.Property<double>("Mean")
                        .HasColumnType("REAL");

                    b.Property<double>("Min")
                        .HasColumnType("REAL");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<long>("SampleSize")
                        .HasColumnType("INTEGER");

                    b.Property<double>("StandardDeviation")
                        .HasColumnType("REAL");

                    b.Property<long>("TaskId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Tau")
                        .HasColumnType("REAL");

                    b.Property<double>("Time")
                        .HasColumnType("REAL");

                    b.Property<double>("Value")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("BenchmarkResultId");

                    b.HasIndex("TaskId");

                    b.ToTable("TaskResultModel");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Statistics.BenchmarkStatisticsModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("BenchmarkId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("CpuId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FrequencyClasses")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("MarginOfError")
                        .HasColumnType("REAL");

                    b.Property<double>("Max")
                        .HasColumnType("REAL");

                    b.Property<double>("Mean")
                        .HasColumnType("REAL");

                    b.Property<double>("Min")
                        .HasColumnType("REAL");

                    b.Property<long>("SampleSize")
                        .HasColumnType("INTEGER");

                    b.Property<double>("StandardDeviation")
                        .HasColumnType("REAL");

                    b.Property<double>("Tau")
                        .HasColumnType("REAL");

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
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("InputDescription")
                        .HasColumnType("TEXT");

                    b.Property<string>("InputName")
                        .HasColumnType("TEXT");

                    b.Property<string>("InputProperties")
                        .HasColumnType("TEXT");

                    b.Property<string>("InputUnit")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OutputDescription")
                        .HasColumnType("TEXT");

                    b.Property<string>("OutputName")
                        .HasColumnType("TEXT");

                    b.Property<string>("OutputProperties")
                        .HasColumnType("TEXT");

                    b.Property<string>("OutputUnit")
                        .HasColumnType("TEXT");

                    b.Property<int>("ResultAggregation")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ResultDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ResultName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ResultType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ResultUnit")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Uuid")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Uuid")
                        .IsUnique();

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Topology.TopologyModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("CpuId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Root")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TopologyHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalLogicalCores")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TotalNumaNodes")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TotalPackages")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TotalPhysicalCores")
                        .HasColumnType("INTEGER");

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

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Result.BenchmarkResultModel", b =>
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
                    b.HasOne("Elpida.Backend.Data.Abstractions.Models.Result.BenchmarkResultModel", "BenchmarkResult")
                        .WithMany("TaskResults")
                        .HasForeignKey("BenchmarkResultId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Elpida.Backend.Data.Abstractions.Models.Task.TaskModel", "Task")
                        .WithMany()
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BenchmarkResult");

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

            modelBuilder.Entity("Elpida.Backend.Data.Abstractions.Models.Result.BenchmarkResultModel", b =>
                {
                    b.Navigation("TaskResults");
                });
#pragma warning restore 612, 618
        }
    }
}
