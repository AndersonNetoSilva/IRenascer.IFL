using System.ComponentModel.DataAnnotations;

namespace IFL.WebApp.Model.Views
{
    public class ReportAvaliacoesNutricionaisView
    {
        public int? Id { get; set; }
        public string? Nome { get; set; }
        public int? ArquivoImagemFrenteId { get; set; }
        public int? ArquivoImagemCostasId { get; set; }
        public decimal? Peso { get; set; } = 0;
        public decimal? Altura { get; set; } = 0;
        public decimal? PctGordura { get; set; } = 0;
        public decimal? MassaMuscular { get; set; } = 0;
        public decimal? GorduraViceral { get; set; } = 0;
        public decimal? MassaLivre { get; set; } = 0;
        public decimal? AguaCorporal { get; set; } = 0;
        public string? Obs { get; set; }
        public string? Data { get; set; }
    }
}
