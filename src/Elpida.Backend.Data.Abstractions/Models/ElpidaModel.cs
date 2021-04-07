using Elpida.Backend.Data.Abstractions.Interfaces;

namespace Elpida.Backend.Data.Abstractions.Models
{
    public class ElpidaModel : Entity
    {
        public int VersionMajor { get; set; }
        public int VersionMinor { get; set; }
        public int VersionRevision { get; set; }
        public int VersionBuild { get; set; }

        public string CompilerVersion { get; set; } = default!;
        public string CompilerName { get; set; } = default!;
    }
}