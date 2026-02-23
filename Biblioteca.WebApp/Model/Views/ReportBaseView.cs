using System.ComponentModel.DataAnnotations;

namespace IFL.WebApp.Model.Views
{
    public class ReportBaseView
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
    }
}
