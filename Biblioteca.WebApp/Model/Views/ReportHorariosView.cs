using System.ComponentModel.DataAnnotations;

namespace IFL.WebApp.Model.Views
{
    public class ReportHorariosView
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
        public bool Domingo { get; set; }
        public bool Segunda { get; set; }
        public bool Terca { get; set; }
        public bool Quarta { get; set; }
        public bool Quinta { get; set; }
        public bool Sexta { get; set; }
        public bool Sabado { get; set; }
        public bool Ativo { get; set; }
        

    }
}
