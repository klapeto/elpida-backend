using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elpida.Backend.Data.Sqlite.Migrations
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
                name: "ElpidaVersions",
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
                    table.PrimaryKey("PK_ElpidaVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperatingSystems",
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
                    table.PrimaryKey("PK_OperatingSystems", x => x.Id);
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
                    FrequencyClasses = table.Column<string>(type: "TEXT", nullable: false)
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
                    ElpidaVersionId = table.Column<long>(type: "INTEGER", nullable: false),
                    OperatingSystemId = table.Column<long>(type: "INTEGER", nullable: false),
                    TopologyId = table.Column<long>(type: "INTEGER", nullable: false),
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
                        name: "FK_BenchmarkResults_ElpidaVersions_ElpidaVersionId",
                        column: x => x.ElpidaVersionId,
                        principalTable: "ElpidaVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BenchmarkResults_OperatingSystems_OperatingSystemId",
                        column: x => x.OperatingSystemId,
                        principalTable: "OperatingSystems",
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
                name: "TaskResultModel",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                    table.PrimaryKey("PK_TaskResultModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskResultModel_BenchmarkResults_BenchmarkResultId",
                        column: x => x.BenchmarkResultId,
                        principalTable: "BenchmarkResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskResultModel_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkResults_BenchmarkId",
                table: "BenchmarkResults",
                column: "BenchmarkId");

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkResults_ElpidaVersionId",
                table: "BenchmarkResults",
                column: "ElpidaVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkResults_OperatingSystemId",
                table: "BenchmarkResults",
                column: "OperatingSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkResults_TopologyId",
                table: "BenchmarkResults",
                column: "TopologyId");

            migrationBuilder.CreateIndex(
                name: "IX_Benchmarks_Uuid",
                table: "Benchmarks",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkStatistics_BenchmarkId",
                table: "BenchmarkStatistics",
                column: "BenchmarkId");

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkStatistics_CpuId_BenchmarkId",
                table: "BenchmarkStatistics",
                columns: new[] { "CpuId", "BenchmarkId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkTaskModel_BenchmarkId",
                table: "BenchmarkTaskModel",
                column: "BenchmarkId");

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkTaskModel_TaskId",
                table: "BenchmarkTaskModel",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Cpus_Vendor_ModelName_AdditionalInfo",
                table: "Cpus",
                columns: new[] { "Vendor", "ModelName", "AdditionalInfo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ElpidaVersions_VersionMajor_VersionMinor_VersionRevision_VersionBuild_CompilerName_CompilerVersion",
                table: "ElpidaVersions",
                columns: new[] { "VersionMajor", "VersionMinor", "VersionRevision", "VersionBuild", "CompilerName", "CompilerVersion" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OperatingSystems_Category_Name_Version",
                table: "OperatingSystems",
                columns: new[] { "Category", "Name", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskResultModel_BenchmarkResultId",
                table: "TaskResultModel",
                column: "BenchmarkResultId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskResultModel_TaskId",
                table: "TaskResultModel",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Uuid",
                table: "Tasks",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topologies_CpuId_TopologyHash",
                table: "Topologies",
                columns: new[] { "CpuId", "TopologyHash" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BenchmarkStatistics");

            migrationBuilder.DropTable(
                name: "BenchmarkTaskModel");

            migrationBuilder.DropTable(
                name: "TaskResultModel");

            migrationBuilder.DropTable(
                name: "BenchmarkResults");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Benchmarks");

            migrationBuilder.DropTable(
                name: "ElpidaVersions");

            migrationBuilder.DropTable(
                name: "OperatingSystems");

            migrationBuilder.DropTable(
                name: "Topologies");

            migrationBuilder.DropTable(
                name: "Cpus");
        }
    }
}
