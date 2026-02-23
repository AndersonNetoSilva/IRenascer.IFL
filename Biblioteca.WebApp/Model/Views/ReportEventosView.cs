using System.ComponentModel.DataAnnotations;

namespace IFL.WebApp.Model.Views
{
    public class ReportEventosView
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Local { get; set; }
        public string Data { get; set; }
        public string TipoEvento { get; set; }
        public bool Encerrado { get; set; }

    }
}
