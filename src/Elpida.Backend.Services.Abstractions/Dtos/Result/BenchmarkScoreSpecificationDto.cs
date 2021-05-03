namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
    public class BenchmarkScoreSpecificationDto
    {
        public string Unit { get; set; } = default!;
        public int Comparison { get; set; }
    }
}