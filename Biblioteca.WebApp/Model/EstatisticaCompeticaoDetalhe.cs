using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

namespace IFL.WebApp.Model
{
    public class EstatisticaCompeticaoDetalhe : EntityBase
    {
        [Required]
        [ForeignKey(nameof(EstatisticaCompeticao))]
        [Display(Name = "Estatistica da Competição")]
        public int EstatisticaCompeticaoId { get; set; }

        [Display(Name = "Estatistica da Competição")]
        public EstatisticaCompeticao? EstatisticaCompeticao { get; set; }

        [Display(Name = "Vitória")]
        public bool? Vitoria { get; set; } 

        [Range(0, 5)]
        [Display(Name = "Yuko")]
        public int? Yuko { get; set; }

        [Range(0, 2)]
        [Display(Name = "Wazari")]
        public int? Wazari { get; set; }

        [Range(0, 1)]
        [Display(Name = "Ippon")]
        public int? Ippon { get; set; }

        [Range(0, 5)]
        [Display(Name = "Shido")]
        public int? Shido { get; set; }

        [Range(0, 1)]
        [Display(Name = "Hansoku-make")]
        public int? Hansokumake { get; set; }

        [Display(Name = "Golden Score")]
        public bool? GoldenScore { get; set; }=false;

        public TimeSpan TempoDaLuta { get; set; }
        public TimeSpan TempoDoGoldenScore { get; set; }

        [Display(Name = "Tecnica que Aplicou")]
        public required Tecnica TecnicaAplicou { get; set; }

        [Display(Name = "Tecnica que Recebeu")]
        public required Tecnica TecnicaRecebeu { get; set; }
    }

    public enum Tecnica
    {
        // -------- Te-waza --------

        [Display(Name = "Ippon Seoi Nage")]
        [Description("Te-waza")]
        IpponSeoiNage,

        [Display(Name = "Morote Seoi Nage")]
        [Description("Te-waza")]
        MoroteSeoiNage,

        [Display(Name = "Seoi Otoshi")]
        [Description("Te-waza")]
        SeoiOtoshi,

        [Display(Name = "Tai Otoshi")]
        [Description("Te-waza")]
        TaiOtoshi,

        [Display(Name = "Kata Guruma")]
        [Description("Te-waza")]
        KataGuruma,

        [Display(Name = "Sukui Nage")]
        [Description("Te-waza")]
        SukuiNage,

        [Display(Name = "Uki Otoshi")]
        [Description("Te-waza")]
        UkiOtoshi,

        [Display(Name = "Sumi Otoshi")]
        [Description("Te-waza")]
        SumiOtoshi,


        // -------- Koshi-waza --------

        [Display(Name = "O Goshi")]
        [Description("Koshi-waza")]
        OGoshi,

        [Display(Name = "Uki Goshi")]
        [Description("Koshi-waza")]
        UkiGoshi,

        [Display(Name = "Koshi Guruma")]
        [Description("Koshi-waza")]
        KoshiGuruma,

        [Display(Name = "Tsuri Komi Goshi")]
        [Description("Koshi-waza")]
        TsuriKomiGoshi,

        [Display(Name = "Sode Tsuri Komi Goshi")]
        [Description("Koshi-waza")]
        SodeTsuriKomiGoshi,

        [Display(Name = "Harai Goshi")]
        [Description("Koshi-waza")]
        HaraiGoshi,

        [Display(Name = "Hane Goshi")]
        [Description("Koshi-waza")]
        HaneGoshi,


        // -------- Ashi-waza --------

        [Display(Name = "O Soto Gari")]
        [Description("Ashi-waza")]
        OSotoGari,

        [Display(Name = "O Uchi Gari")]
        [Description("Ashi-waza")]
        OUchiGari,

        [Display(Name = "Ko Uchi Gari")]
        [Description("Ashi-waza")]
        KoUchiGari,

        [Display(Name = "Ko Soto Gari")]
        [Description("Ashi-waza")]
        KoSotoGari,

        [Display(Name = "Uchi Mata")]
        [Description("Ashi-waza")]
        UchiMata,

        [Display(Name = "De Ashi Harai (ou Barai)")]
        [Description("Ashi-waza")]
        DeAshiHarai,

        [Display(Name = "Hiza Guruma")]
        [Description("Ashi-waza")]
        HizaGuruma,

        [Display(Name = "Sasae Tsurikomi Ashi")]
        [Description("Ashi-waza")]
        SasaeTsurikomiAshi,

        [Display(Name = "Okuri Ashi Harai")]
        [Description("Ashi-waza")]
        OkuriAshiHarai,

        [Display(Name = "Ashi Guruma")]
        [Description("Ashi-waza")]
        AshiGuruma,


        // -------- Sutemi-waza --------

        [Display(Name = "Tomoe Nage")]
        [Description("Sutemi-waza")]
        TomoeNage,

        [Display(Name = "Sumi Gaeshi")]
        [Description("Sutemi-waza")]
        SumiGaeshi,

        [Display(Name = "Tani Otoshi")]
        [Description("Sutemi-waza")]
        TaniOtoshi,

        [Display(Name = "Yoko Tomoe Nage")]
        [Description("Sutemi-waza")]
        YokoTomoeNage,

        [Display(Name = "Ura Nage")]
        [Description("Sutemi-waza")]
        UraNage,


        // -------- Imobilizações --------

        [Display(Name = "Kesa Gatame")]
        [Description("Imobilizações")]
        KesaGatame,

        [Display(Name = "Yoko Shiho Gatame")]
        [Description("Imobilizações")]
        YokoShihoGatame,

        [Display(Name = "Kami Shiho Gatame")]
        [Description("Imobilizações")]
        KamiShihoGatame,

        [Display(Name = "Juji Gatame (Ude-hishigi-juji-gatame)")]
        [Description("Imobilizações")]
        JujiGatame,

        [Display(Name = "Hadaka Jime")]
        [Description("Imobilizações")]
        HadakaJime
    }

    public class EstatisticaCompeticaoDetalheVM : IPermiteMarcarParaExclusao
    {
        public int? Id { get; set; }

        [Display(Name = "Estatistica de Competição")]
        public int EstatisticaCompeticaoId { get; set; }

        public Tecnica Tecnica { get; set; }
        public bool Vitoria { get; set; } = false;
        public int? Yuko { get; set; }
        public int? Wazari { get; set; }
        public int? Ippon { get; set; }
        public int? Shido { get; set; }
        public int? Hansokumake { get; set; }
        public bool GoldenScore { get; set; } = false;
        public TimeSpan TempoDaLuta { get; set; }
        public TimeSpan TempoDoGoldenScore { get; set; }
        public Tecnica TecnicaAplicou { get; set; }
        public Tecnica TecnicaRecebeu { get; set; }
        public bool MarcadoParaExclusao { get; set; } = false;
    }

 
}
