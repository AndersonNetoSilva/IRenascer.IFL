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
        public Tecnica TecnicaAplicou { get; set; }

        [Display(Name = "Tecnica que Recebeu")]
        public Tecnica TecnicaRecebeu { get; set; }
    }

    public enum Tecnica
    {

        [Display(Name = "Não informada")]
        [Description("Não informada")]
        NaoInformado = 0,

        // -------- Te-waza --------

        [Display(Name = "Ippon Seoi Nage")]
        [Description("Te-waza")]
        IpponSeoiNage = 1,

        [Display(Name = "Morote Seoi Nage")]
        [Description("Te-waza")]
        MoroteSeoiNage = 2,

        [Display(Name = "Seoi Otoshi")]
        [Description("Te-waza")]
        SeoiOtoshi = 3,

        [Display(Name = "Tai Otoshi")]
        [Description("Te-waza")]
        TaiOtoshi = 4,

        [Display(Name = "Kata Guruma")]
        [Description("Te-waza")]
        KataGuruma = 5,

        [Display(Name = "Sukui Nage")]
        [Description("Te-waza")]
        SukuiNage = 6,

        [Display(Name = "Uki Otoshi")]
        [Description("Te-waza")]
        UkiOtoshi = 7,

        [Display(Name = "Sumi Otoshi")]
        [Description("Te-waza")]
        SumiOtoshi = 8,


        // -------- Koshi-waza --------

        [Display(Name = "O Goshi")]
        [Description("Koshi-waza")]
        OGoshi = 9,

        [Display(Name = "Uki Goshi")]
        [Description("Koshi-waza")]
        UkiGoshi = 10,

        [Display(Name = "Koshi Guruma")]
        [Description("Koshi-waza")]
        KoshiGuruma = 11,

        [Display(Name = "Tsuri Komi Goshi")]
        [Description("Koshi-waza")]
        TsuriKomiGoshi = 12,

        [Display(Name = "Sode Tsuri Komi Goshi")]
        [Description("Koshi-waza")]
        SodeTsuriKomiGoshi = 13,

        [Display(Name = "Harai Goshi")]
        [Description("Koshi-waza")]
        HaraiGoshi = 14,

        [Display(Name = "Hane Goshi")]
        [Description("Koshi-waza")]
        HaneGoshi = 15,


        // -------- Ashi-waza --------

        [Display(Name = "O Soto Gari")]
        [Description("Ashi-waza")]
        OSotoGari = 16,

        [Display(Name = "O Uchi Gari")]
        [Description("Ashi-waza")]
        OUchiGari = 17,

        [Display(Name = "Ko Uchi Gari")]
        [Description("Ashi-waza")]
        KoUchiGari = 18,

        [Display(Name = "Ko Soto Gari")]
        [Description("Ashi-waza")]
        KoSotoGari = 19,

        [Display(Name = "Uchi Mata")]
        [Description("Ashi-waza")]
        UchiMata = 20,

        [Display(Name = "De Ashi Harai (ou Barai)")]
        [Description("Ashi-waza")]
        DeAshiHarai = 21,

        [Display(Name = "Hiza Guruma")]
        [Description("Ashi-waza")]
        HizaGuruma = 22,

        [Display(Name = "Sasae Tsurikomi Ashi")]
        [Description("Ashi-waza")]
        SasaeTsurikomiAshi = 23,

        [Display(Name = "Okuri Ashi Harai")]
        [Description("Ashi-waza")]
        OkuriAshiHarai = 24,

        [Display(Name = "Ashi Guruma")]
        [Description("Ashi-waza")]
        AshiGuruma = 25,


        // -------- Sutemi-waza --------

        [Display(Name = "Tomoe Nage")]
        [Description("Sutemi-waza")]
        TomoeNage = 26,

        [Display(Name = "Sumi Gaeshi")]
        [Description("Sutemi-waza")]
        SumiGaeshi = 27,

        [Display(Name = "Tani Otoshi")]
        [Description("Sutemi-waza")]
        TaniOtoshi = 28,

        [Display(Name = "Yoko Tomoe Nage")]
        [Description("Sutemi-waza")]
        YokoTomoeNage = 29,

        [Display(Name = "Ura Nage")]
        [Description("Sutemi-waza")]
        UraNage = 30,


        // -------- Imobilizações --------

        [Display(Name = "Kesa Gatame")]
        [Description("Imobilizações")]
        KesaGatame = 31,

        [Display(Name = "Yoko Shiho Gatame")]
        [Description("Imobilizações")]
        YokoShihoGatame = 32,

        [Display(Name = "Kami Shiho Gatame")]
        [Description("Imobilizações")]
        KamiShihoGatame = 33,

        [Display(Name = "Juji Gatame (Ude-hishigi-juji-gatame)")]
        [Description("Imobilizações")]
        JujiGatame = 34,

        [Display(Name = "Hadaka Jime")]
        [Description("Imobilizações")]
        HadakaJime = 35
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
