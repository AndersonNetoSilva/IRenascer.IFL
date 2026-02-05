using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Extensions;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Infrastructure.Repositories;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace IFL.WebApp.Areas.Admin.Pages.General.Pesagens
{
    public class CreateModel : CrudPageModel<Pesagem, IPesagemRepository>
    {
        private readonly IAtletaRepository _atletaRepository;

        private readonly IEventoRepository _eventoRepository;

        public CreateModel(IPesagemRepository repository,
                           IAtletaRepository atletaRepository,
                           IEventoRepository eventoRepository, 
                           IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            _atletaRepository = atletaRepository;
            _eventoRepository = eventoRepository;
        }

        public IActionResult OnGet()
        {
            BindSelectLists();
            return Page();
        }

        private void BindSelectLists()
        {
            Eventos = _eventoRepository.Query()
                        .Where(a=> !a.Encerrado && a.TipoEvento == TipoEvento.Competicao)
                        .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Nome })
                        .OrderBy(a => a.Text)
                        .ToList();

            Atletas = _atletaRepository.Query()
                            .Where(a => a.Ativo)
                            .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Nome })
                            .OrderBy(a => a.Text)
                            .ToList();
        }
        public List<SelectListItem> Eventos { get; set; } = new();
        public List<SelectListItem> Atletas { get; set; } = new();

        [BindProperty]
        public Pesagem Pesagem { get; set; } = default!;

        public static void ValidarValor(Pesagem pesagem)
        {
            if (!pesagem.Peso1AsString.TryParseValor(out string errorMessage, out var valorDecimal))
            {
                throw new Infrastructure.Exceptions.ValidationListException(("Pesagem.Peso1AsString", errorMessage));
            }

            pesagem.Peso1 = valorDecimal;

            if (!pesagem.Peso2AsString.IsNullOrEmpty())
            {
                if(!pesagem.Peso2AsString.TryParseValor(out string errorMessage2, out var valorDecimal2))
                {
                    throw new Infrastructure.Exceptions.ValidationListException(("Pesagem.Peso2AsString", errorMessage2));
                }

                pesagem.Peso2 = valorDecimal2;
            }

            if (!pesagem.Peso3AsString.IsNullOrEmpty())
            {
                if (!pesagem.Peso3AsString.TryParseValor(out string errorMessage3, out var valorDecimal3))
                {
                    throw new Infrastructure.Exceptions.ValidationListException(("Pesagem.Peso3AsString", errorMessage3));
                }

                pesagem.Peso3 = valorDecimal3;
            }
        }
        public static void SetCategoria(Pesagem pesagem)
        { 
           if(pesagem.Peso1.HasValue)
           {
                pesagem.Categoria1 = GetCategoria(pesagem, pesagem.Peso1.Value);
           }

           if (pesagem.Peso2.HasValue)
           {
               pesagem.Categoria2 = GetCategoria(pesagem, pesagem.Peso2.Value);
           }

           if (pesagem.Peso3.HasValue)
           {
               pesagem.Categoria3 = GetCategoria(pesagem, pesagem.Peso3.Value);
           }
        }

        private static string GetCategoria(Pesagem pesagem, decimal peso)
        {
            string categoria = string.Empty;

            if (pesagem.Atleta?.Classe == "Chupeta")
                categoria = GetCategoriaChupeta(pesagem, peso);
            else
              if (pesagem.Atleta?.Classe == "Sub-9")
                categoria = GetCategoriaSub9(pesagem, peso);
            else
              if (pesagem.Atleta?.Classe == "Sub-11")
                categoria = GetCategoriaSub11(pesagem, peso);
            else
              if (pesagem.Atleta?.Classe == "Sub-13")
                categoria = GetCategoriaSub13(pesagem, peso);
            else
              if (pesagem.Atleta?.Classe == "Sub-15")
                {
                  if(pesagem.Atleta.Sexo==Sexo.Masculino)
                    categoria = GetCategoriaSub15M(pesagem, peso);
                  else
                    categoria = GetCategoriaSub15F(pesagem, peso);
                 }                
            else
              if (pesagem.Atleta?.Classe == "Cadete")
                categoria = GetCategoriaCadete(pesagem, peso);
            else
              if (pesagem.Atleta?.Classe == "Junior")
                categoria = GetCategoriaJunior(pesagem, peso);
            else
              if (pesagem.Atleta?.Classe == "Dangai")
                categoria = GetCategoriaDangai(pesagem, peso);
            else
                categoria = "Não encontrada";

            return categoria;
        }

        private static string GetCategoriaDangai(Pesagem pesagem, decimal peso)
        {
            return "Não implementado";
        }

        private static string GetCategoriaJunior(Pesagem pesagem, decimal peso)
        {
            return "Não implementado";
        }

        private static string GetCategoriaCadete(Pesagem pesagem, decimal peso)
        {
            return "Não implementado";
        }

        private static string GetCategoriaSub15F(Pesagem pesagem, decimal peso)
        {
            switch (peso)
            {
                case <= 36.0m:
                    return "Super Ligeiro - até 36kg";
                    break;

                case >= 36m and < 40.0m:
                    return "Ligeiro - 36,1kg até 40kg";
                    break;

                case >= 40.01m and < 44:
                    return "Meio Leve - 40,1kg até 44kg";
                    break;

                case >= 44.01m and <= 48:
                    return "Leve - 44,1kg até 48kg";
                    break;

                case >= 48.01m and <= 52:
                    return "Meio Médio - 48,1kg até 52kg";
                    break;

                case >= 52.01m and <= 57:
                    return "Médio - 52,01kg até 57kg";
                    break;

                case >= 57.01m and <= 63:
                    return "Meio Pesado - 57,01kg até 63kg";
                    break;

                case >= 63.01m and <= 70:
                    return "Pesado - 63,01kg até 70kg";
                    break;

                case >= 70.01m:
                    return "Super Pesado - acima de 70kg";
                    break;

                default:
                    return "Não encontrado";
                    break;

            }
        }

        private static string GetCategoriaSub15M(Pesagem pesagem, decimal peso)
        {
            switch (peso)
            {
                case <= 40.0m:
                    return "Super Ligeiro - até 40kg";
                    break;

                case >= 40.1m and < 45.0m:
                    return "Ligeiro - 40,1kg até 45kg";
                    break;

                case >= 45.01m and < 50:
                    return "Meio Leve - 45,1kg até 50kg";
                    break;

                case >= 50.01m and <= 55:
                    return "Leve - 50,1kg até 55kg";
                    break;

                case >= 55.01m and <= 60:
                    return "Meio Médio - 50,1kg até 60kg";
                    break;

                case >= 60.01m and <= 66:
                    return "Médio - 60,1kg ate 66kg";
                    break;

                case >= 66.01m and <= 73:
                    return "Meio Pesado - 66,1kg até 73kg";
                    break;

                case >= 73.01m and <= 81:
                    return "Pesado - 73,1kg até 81kg";
                    break;

                case >= 81.01m:
                    return "Super Pesado - acima de 81,1kg";
                    break;

                default:
                    return "Não encontrado";
                    break;

            }
        }

        private static string GetCategoriaSub13(Pesagem pesagem, decimal peso)
        {
            switch (peso)
            {
                case <= 28.0m:
                    return "Super Ligeiro - até 28kg";
                    break;

                case >= 28.1m and < 31.0m:
                    return "Ligeiro - 28,1kg até 31kg";
                    break;

                case >= 31.01m and < 34:
                    return "Meio Leve - 31,1kg até 84kg";
                    break;

                case >= 34.01m and <= 38:
                    return "Leve - 34,1kg até 38kg";
                    break;

                case >= 38.01m and <= 42:
                    return "Meio Médio - 38,1kg até 42kg";
                    break;

                case >= 42.01m and <= 47:
                    return "Médio - 42,1kg até 47kg";
                    break;

                case >= 47.01m and <= 52:
                    return "Meio Pesado - 47,1kg até 52kg";
                    break;

                case >= 52.01m and <= 60:
                    return "Pesado - 52,1kg até 60kg";
                    break;

                case >= 60.01m :
                    return "Super Pesado - acima 60,1kg";
                    break;

                default:
                    return "Não encontrado";
                    break;

            }
        }

        private static string GetCategoriaSub11(Pesagem pesagem, decimal peso)
        {
            switch (peso)
            {
                case <= 28.0m:
                    return "Super Ligeiro - até 28";
                    break;

                case >= 28.1m and < 30.0m:
                    return "Ligeiro - 28,1kg até 30kg";
                    break;

                case >= 30.01m and < 33:
                    return "Meio Leve - 30,1kg até 33kg";
                    break;

                case >= 33.01m and <= 36:
                    return "Leve - 33,1kg até 36kg";
                    break;

                case >= 36.01m and <= 40:
                    return "Meio Médio - 36,1kg até 40kg";
                    break;

                case >= 40.01m and <= 45:
                    return "Médio - 40,1kg até 45kg";
                    break;

                case >= 45.01m and <= 50:
                    return "Meio Pesado - 45,1kg até 50kg";
                    break;

                case >= 50.01m and <= 55:
                    return "Pesado - 50,1kg até 55kg";
                    break;

                case >= 55.01m and <= 60:
                    return "Super Pesado - 55,1kg até 60kg";
                    break;

                case >= 60.01m:
                    return "Extra Pesado - acima de 60,1kg ";
                    break;

                default:
                    return "Não encontrado";
                    break;

            }
        }

        private static string GetCategoriaSub9(Pesagem pesagem, decimal peso)
        {
            switch (peso)
            {
                case <= 23.0m:
                    return "Super Ligeiro - até 23kg";
                    break;
                     
                case >= 23.1m and < 26.0m:
                    return "Ligeiro - 23,1kg até 26kg";
                    break;

                case >= 26.01m and < 29:
                    return "Meio Leve - 26,1kg até 29kg";
                    break;

                case >= 29.01m and <= 32:
                    return "Leve - 32,1kg até 32kg";
                    break;

                case >= 32.01m and <= 36:
                    return "Meio Médio - 32,1kg até 36kg";
                    break;

                case >= 36.01m and <= 40:
                    return "Médio - 36,1kg até 40kg";
                    break;

                case >= 40.01m and <= 45:
                    return "Meio Pesado - 40,1kg até 45kg";
                    break;

                case >= 45.01m and <= 50:
                    return "Pesado - 45,1kg até 50kg";
                    break;

                case >= 50.01m and <= 55:
                    return "Super Pesado - 50,1kg até 55kg";
                    break;

                case >= 55.01m :
                    return "Extra Pesado - acima de 55,1kg";
                    break;

                default:
                    return "Não encontrado";
                    break;

            }
        }

        private static string GetCategoriaChupeta(Pesagem pesagem, decimal peso)
        {
            return " Sem Categoria ";
 
        }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ValidarValor(Pesagem);
            Pesagem.Atleta = _atletaRepository.Query().Where(a => Pesagem.AtletaId == a.Id).FirstOrDefault();
            Pesagem.Evento = _eventoRepository.Query().Where(a => Pesagem.EventoId == a.Id).FirstOrDefault();

            if (Pesagem.Atleta != null)
              SetCategoria(Pesagem);

            if (!ModelState.IsValid)
            {
                BindSelectLists();
                return Page();
            }

            _repository.Add(Pesagem);
            await _unitOfWork.CommitAsync();

            return RedirectToPage("./Index");
        }
    }
}
