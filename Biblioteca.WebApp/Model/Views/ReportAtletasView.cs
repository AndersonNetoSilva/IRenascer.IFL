using System.ComponentModel.DataAnnotations;

namespace IFL.WebApp.Model.Views
{
    public class ReportAtletasView
    {
        public int? Id { get; set; }
        public string? Nome { get; set; }
        public string? Responsavel { get; set; }
        public string? DataNascimento { get; set; }
        public string? TelefonePrincipal { get; set; }
        public string? Endereco { get; set; }
        public int? Idade { get; set; }
        public int? ArquivoImagemId { get; set; }
        public string? Graduacao { get; set; }
        public string? Evento { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Data { get; set; }
    }
}
