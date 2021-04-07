using Elpida.Backend.Data.Abstractions.Interfaces;

namespace Elpida.Backend.Data.Abstractions.Models
{
    public class OsModel : Entity
    {
        public string Category { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
    }
}