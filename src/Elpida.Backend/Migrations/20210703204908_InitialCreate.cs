﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elpida.Backend.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Benchmarks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ScoreUnit = table.Column<string>(type: "TEXT", nullable: false),
                    ScoreComparison = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Benchmarks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cpus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Architecture = table.Column<string>(type: "TEXT", nullable: false),
                    Vendor = table.Column<string>(type: "TEXT", nullable: false),
                    ModelName = table.Column<string>(type: "TEXT", nullable: false),
                    Frequency = table.Column<long>(type: "INTEGER", nullable: false),
                    Smt = table.Column<bool>(type: "INTEGER", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "TEXT", nullable: false),
                    Caches = table.Column<string>(type: "TEXT", nullable: false),
                    Features = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cpus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Elpidas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VersionMajor = table.Column<int>(type: "INTEGER", nullable: false),
                    VersionMinor = table.Column<int>(type: "INTEGER", nullable: false),
                    VersionRevision = table.Column<int>(type: "INTEGER", nullable: false),
                    VersionBuild = table.Column<int>(type: "INTEGER", nullable: false),
                    CompilerVersion = table.Column<string>(type: "TEXT", nullable: false),
                    CompilerName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elpidas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Oses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Version = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ResultName = table.Column<string>(type: "TEXT", nullable: false),
                    ResultDescription = table.Column<string>(type: "TEXT", nullable: false),
                    ResultUnit = table.Column<string>(type: "TEXT", nullable: false),
                    ResultAggregation = table.Column<int>(type: "INTEGER", nullable: false),
                    ResultType = table.Column<int>(type: "INTEGER", nullable: false),
                    InputName = table.Column<string>(type: "TEXT", nullable: true),
                    InputDescription = table.Column<string>(type: "TEXT", nullable: true),
                    InputUnit = table.Column<string>(type: "TEXT", nullable: true),
                    InputProperties = table.Column<string>(type: "TEXT", nullable: true),
                    OutputName = table.Column<string>(type: "TEXT", nullable: true),
                    OutputDescription = table.Column<string>(type: "TEXT", nullable: true),
                    OutputUnit = table.Column<string>(type: "TEXT", nullable: true),
                    OutputProperties = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BenchmarkStatistics",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BenchmarkId = table.Column<long>(type: "INTEGER", nullable: false),
                    CpuId = table.Column<long>(type: "INTEGER", nullable: false),
                    SampleSize = table.Column<long>(type: "INTEGER", nullable: false),
                    Max = table.Column<double>(type: "REAL", nullable: false),
                    Min = table.Column<double>(type: "REAL", nullable: false),
                    Mean = table.Column<double>(type: "REAL", nullable: false),
                    StandardDeviation = table.Column<double>(type: "REAL", nullable: false),
                    Tau = table.Column<double>(type: "REAL", nullable: false),
                    MarginOfError = table.Column<double>(type: "REAL", nullable: false),
                    FrequencyClasses = table.Column<string>(type: "TEXT", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenchmarkStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BenchmarkStatistics_Benchmarks_BenchmarkId",
                        column: x => x.BenchmarkId,
                        principalTable: "Benchmarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BenchmarkStatistics_Cpus_CpuId",
                        column: x => x.CpuId,
                        principalTable: "Cpus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Topologies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CpuId = table.Column<long>(type: "INTEGER", nullable: false),
                    TopologyHash = table.Column<string>(type: "TEXT", nullable: false),
                    TotalLogicalCores = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalPhysicalCores = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalNumaNodes = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalPackages = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalDepth = table.Column<int>(type: "INTEGER", nullable: false),
                    Root = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topologies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Topologies_Cpus_CpuId",
                        column: x => x.CpuId,
                        principalTable: "Cpus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BenchmarkTaskModel",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BenchmarkId = table.Column<long>(type: "INTEGER", nullable: false),
                    TaskId = table.Column<long>(type: "INTEGER", nullable: false),
                    CanBeDisabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    IterationsToRun = table.Column<long>(type: "INTEGER", nullable: false),
                    IsCountedOnResults = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanBeMultiThreaded = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenchmarkTaskModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BenchmarkTaskModel_Benchmarks_BenchmarkId",
                        column: x => x.BenchmarkId,
                        principalTable: "Benchmarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BenchmarkTaskModel_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BenchmarkResults",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ElpidaId = table.Column<long>(type: "INTEGER", nullable: false),
                    OsId = table.Column<long>(type: "INTEGER", nullable: false),
                    TopologyId = table.Column<long>(type: "INTEGER", nullable: false),
                    CpuId = table.Column<long>(type: "INTEGER", nullable: false),
                    BenchmarkId = table.Column<long>(type: "INTEGER", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Affinity = table.Column<string>(type: "TEXT", nullable: false),
                    MemorySize = table.Column<long>(type: "INTEGER", nullable: false),
                    PageSize = table.Column<long>(type: "INTEGER", nullable: false),
                    NotifyOverhead = table.Column<double>(type: "REAL", nullable: false),
                    WakeupOverhead = table.Column<double>(type: "REAL", nullable: false),
                    SleepOverhead = table.Column<double>(type: "REAL", nullable: false),
                    NowOverhead = table.Column<double>(type: "REAL", nullable: false),
                    LockOverhead = table.Column<double>(type: "REAL", nullable: false),
                    LoopOverhead = table.Column<double>(type: "REAL", nullable: false),
                    JoinOverhead = table.Column<double>(type: "REAL", nullable: false),
                    TargetTime = table.Column<double>(type: "REAL", nullable: false),
                    Score = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenchmarkResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BenchmarkResults_Benchmarks_BenchmarkId",
                        column: x => x.BenchmarkId,
                        principalTable: "Benchmarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BenchmarkResults_Cpus_CpuId",
                        column: x => x.CpuId,
                        principalTable: "Cpus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BenchmarkResults_Elpidas_ElpidaId",
                        column: x => x.ElpidaId,
                        principalTable: "Elpidas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BenchmarkResults_Oses_OsId",
                        column: x => x.OsId,
                        principalTable: "Oses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BenchmarkResults_Topologies_TopologyId",
                        column: x => x.TopologyId,
                        principalTable: "Topologies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskResults",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TopologyId = table.Column<long>(type: "INTEGER", nullable: false),
                    CpuId = table.Column<long>(type: "INTEGER", nullable: false),
                    BenchmarkResultId = table.Column<long>(type: "INTEGER", nullable: false),
                    TaskId = table.Column<long>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<double>(type: "REAL", nullable: false),
                    Time = table.Column<double>(type: "REAL", nullable: false),
                    InputSize = table.Column<long>(type: "INTEGER", nullable: false),
                    SampleSize = table.Column<long>(type: "INTEGER", nullable: false),
                    Max = table.Column<double>(type: "REAL", nullable: false),
                    Min = table.Column<double>(type: "REAL", nullable: false),
                    Mean = table.Column<double>(type: "REAL", nullable: false),
                    StandardDeviation = table.Column<double>(type: "REAL", nullable: false),
                    Tau = table.Column<double>(type: "REAL", nullable: false),
                    MarginOfError = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskResults_BenchmarkResults_BenchmarkResultId",
                        column: x => x.BenchmarkResultId,
                        principalTable: "BenchmarkResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskResults_Cpus_CpuId",
                        column: x => x.CpuId,
                        principalTable: "Cpus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskResults_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskResults_Topologies_TopologyId",
                        column: x => x.TopologyId,
                        principalTable: "Topologies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkResults_BenchmarkId",
                table: "BenchmarkResults",
                column: "BenchmarkId");

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkResults_CpuId",
                table: "BenchmarkResults",
                column: "CpuId");

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkResults_ElpidaId",
                table: "BenchmarkResults",
                column: "ElpidaId");

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkResults_OsId",
                table: "BenchmarkResults",
                column: "OsId");

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkResults_TopologyId",
                table: "BenchmarkResults",
                column: "TopologyId");

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkStatistics_BenchmarkId",
                table: "BenchmarkStatistics",
                column: "BenchmarkId");

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkStatistics_CpuId",
                table: "BenchmarkStatistics",
                column: "CpuId");

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkTaskModel_BenchmarkId",
                table: "BenchmarkTaskModel",
                column: "BenchmarkId");

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkTaskModel_TaskId",
                table: "BenchmarkTaskModel",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskResults_BenchmarkResultId",
                table: "TaskResults",
                column: "BenchmarkResultId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskResults_CpuId",
                table: "TaskResults",
                column: "CpuId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskResults_TaskId",
                table: "TaskResults",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskResults_TopologyId",
                table: "TaskResults",
                column: "TopologyId");

            migrationBuilder.CreateIndex(
                name: "IX_Topologies_CpuId",
                table: "Topologies",
                column: "CpuId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BenchmarkStatistics");

            migrationBuilder.DropTable(
                name: "BenchmarkTaskModel");

            migrationBuilder.DropTable(
                name: "TaskResults");

            migrationBuilder.DropTable(
                name: "BenchmarkResults");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Benchmarks");

            migrationBuilder.DropTable(
                name: "Elpidas");

            migrationBuilder.DropTable(
                name: "Oses");

            migrationBuilder.DropTable(
                name: "Topologies");

            migrationBuilder.DropTable(
                name: "Cpus");
        }
    }
}