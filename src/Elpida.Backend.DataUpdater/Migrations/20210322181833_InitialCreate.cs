using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elpida.Backend.DataUpdater.Migrations
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
                    Features = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cpus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Topologies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CpuBrand = table.Column<string>(type: "TEXT", nullable: false),
                    TopologyHash = table.Column<string>(type: "TEXT", nullable: false),
                    TotalLogicalCores = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalPhysicalCores = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalDepth = table.Column<int>(type: "INTEGER", nullable: false),
                    Root = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topologies", x => x.Id);
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
                    OutputProperties = table.Column<string>(type: "TEXT", nullable: true),
                    BenchmarkModelId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Benchmarks_BenchmarkModelId",
                        column: x => x.BenchmarkModelId,
                        principalTable: "Benchmarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CpuCacheModel",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Associativity = table.Column<string>(type: "TEXT", nullable: false),
                    Size = table.Column<long>(type: "INTEGER", nullable: false),
                    LinesPerTag = table.Column<int>(type: "INTEGER", nullable: false),
                    LineSize = table.Column<int>(type: "INTEGER", nullable: false),
                    CpuModelId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpuCacheModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CpuCacheModel_Cpus_CpuModelId",
                        column: x => x.CpuModelId,
                        principalTable: "Cpus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Affinity = table.Column<string>(type: "TEXT", nullable: false),
                    ElpidaVersion = table.Column<string>(type: "TEXT", nullable: false),
                    CompilerVersion = table.Column<string>(type: "TEXT", nullable: false),
                    CompilerName = table.Column<string>(type: "TEXT", nullable: false),
                    OsCategory = table.Column<string>(type: "TEXT", nullable: false),
                    OsName = table.Column<string>(type: "TEXT", nullable: false),
                    OsVersion = table.Column<string>(type: "TEXT", nullable: false),
                    MemorySize = table.Column<long>(type: "INTEGER", nullable: false),
                    PageSize = table.Column<long>(type: "INTEGER", nullable: false),
                    CpuId = table.Column<long>(type: "INTEGER", nullable: true),
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
                        name: "FK_Results_Cpus_CpuId",
                        column: x => x.CpuId,
                        principalTable: "Cpus",
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
                    MarginOfError = table.Column<double>(type: "REAL", nullable: false),
                    ResultModelId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskResultModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskResultModel_Results_ResultModelId",
                        column: x => x.ResultModelId,
                        principalTable: "Results",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskResultModel_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CpuCacheModel_CpuModelId",
                table: "CpuCacheModel",
                column: "CpuModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_BenchmarkId",
                table: "Results",
                column: "BenchmarkId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_CpuId",
                table: "Results",
                column: "CpuId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_TopologyId",
                table: "Results",
                column: "TopologyId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskResultModel_ResultModelId",
                table: "TaskResultModel",
                column: "ResultModelId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskResultModel_TaskId",
                table: "TaskResultModel",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_BenchmarkModelId",
                table: "Tasks",
                column: "BenchmarkModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CpuCacheModel");

            migrationBuilder.DropTable(
                name: "TaskResultModel");

            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Cpus");

            migrationBuilder.DropTable(
                name: "Topologies");

            migrationBuilder.DropTable(
                name: "Benchmarks");
        }
    }
}
