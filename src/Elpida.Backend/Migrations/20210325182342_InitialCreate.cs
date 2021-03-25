using System;
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
                    Name = table.Column<string>(type: "TEXT", nullable: false)
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
                    Vendor = table.Column<string>(type: "TEXT", nullable: false),
                    Brand = table.Column<string>(type: "TEXT", nullable: false),
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
                name: "Topologies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CpuId = table.Column<long>(type: "INTEGER", nullable: false),
                    TopologyHash = table.Column<string>(type: "TEXT", nullable: false),
                    TotalLogicalCores = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalPhysicalCores = table.Column<int>(type: "INTEGER", nullable: false),
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
                name: "BenchmarkModelTaskModel",
                columns: table => new
                {
                    BenchmarksId = table.Column<long>(type: "INTEGER", nullable: false),
                    TasksId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenchmarkModelTaskModel", x => new { x.BenchmarksId, x.TasksId });
                    table.ForeignKey(
                        name: "FK_BenchmarkModelTaskModel_Benchmarks_BenchmarksId",
                        column: x => x.BenchmarksId,
                        principalTable: "Benchmarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BenchmarkModelTaskModel_Tasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Affinity = table.Column<string>(type: "TEXT", nullable: false),
                    ElpidaVersionMajor = table.Column<int>(type: "INTEGER", nullable: false),
                    ElpidaVersionMinor = table.Column<int>(type: "INTEGER", nullable: false),
                    ElpidaVersionRevision = table.Column<int>(type: "INTEGER", nullable: false),
                    ElpidaVersionBuild = table.Column<int>(type: "INTEGER", nullable: false),
                    CompilerVersion = table.Column<string>(type: "TEXT", nullable: false),
                    CompilerName = table.Column<string>(type: "TEXT", nullable: false),
                    OsCategory = table.Column<string>(type: "TEXT", nullable: false),
                    OsName = table.Column<string>(type: "TEXT", nullable: false),
                    OsVersion = table.Column<string>(type: "TEXT", nullable: false),
                    MemorySize = table.Column<long>(type: "INTEGER", nullable: false),
                    PageSize = table.Column<long>(type: "INTEGER", nullable: false),
                    TopologyId = table.Column<long>(type: "INTEGER", nullable: true),
                    NotifyOverhead = table.Column<double>(type: "REAL", nullable: false),
                    WakeupOverhead = table.Column<double>(type: "REAL", nullable: false),
                    SleepOverhead = table.Column<double>(type: "REAL", nullable: false),
                    NowOverhead = table.Column<double>(type: "REAL", nullable: false),
                    LockOverhead = table.Column<double>(type: "REAL", nullable: false),
                    LoopOverhead = table.Column<double>(type: "REAL", nullable: false),
                    JoinOverhead = table.Column<double>(type: "REAL", nullable: false),
                    TargetTime = table.Column<double>(type: "REAL", nullable: false),
                    BenchmarkId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Results_Benchmarks_BenchmarkId",
                        column: x => x.BenchmarkId,
                        principalTable: "Benchmarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Results_Topologies_TopologyId",
                        column: x => x.TopologyId,
                        principalTable: "Topologies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskResultModel",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResultId = table.Column<long>(type: "INTEGER", nullable: false),
                    TaskId = table.Column<long>(type: "INTEGER", nullable: true),
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
                        name: "FK_TaskResultModel_Results_ResultId",
                        column: x => x.ResultId,
                        principalTable: "Results",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskResultModel_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkModelTaskModel_TasksId",
                table: "BenchmarkModelTaskModel",
                column: "TasksId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_BenchmarkId",
                table: "Results",
                column: "BenchmarkId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_TopologyId",
                table: "Results",
                column: "TopologyId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskResultModel_ResultId",
                table: "TaskResultModel",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskResultModel_TaskId",
                table: "TaskResultModel",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Topologies_CpuId",
                table: "Topologies",
                column: "CpuId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BenchmarkModelTaskModel");

            migrationBuilder.DropTable(
                name: "TaskResultModel");

            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Benchmarks");

            migrationBuilder.DropTable(
                name: "Topologies");

            migrationBuilder.DropTable(
                name: "Cpus");
        }
    }
}
