using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Extensions;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Infrastructure.Repositories;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                        .Where(a=> !a.Encerrado )
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

            if (!pesagem.Peso2AsString.TryParseValor(out string errorMessage2, out var valorDecimal2))
            {
                throw new Infrastructure.Exceptions.ValidationListException(("Pesagem.Peso2AsString", errorMessage2));
            }

            pesagem.Peso2 = valorDecimal2;

            if (!pesagem.Peso3AsString.TryParseValor(out string errorMessage3, out var valorDecimal3))
            {
                throw new Infrastructure.Exceptions.ValidationListException(("Pesagem.Peso3AsString", errorMessage3));
            }

            pesagem.Peso3 = valorDecimal3;
        }
        public static void SetCategoria(Pesagem pesagem)
        { 
           if(!pesagem.Peso1.HasValue)
           {
                pesagem.Categoria1 = GetCategoria(pesagem, pesagem.Peso1.Value);
           }

           if (!pesagem.Peso2.HasValue)
           {
               pesagem.Categoria2 = GetCategoria(pesagem, pesagem.Peso2.Value);
           }

           if (!pesagem.Peso3.HasValue)
           {
               pesagem.Categoria3 = GetCategoria(pesagem, pesagem.Peso3.Value);
           }
        }

        private static string GetCategoria(Pesagem pesagem, decimal peso)
        {
            string categoria = string.Empty;

            if (pesagem.Atleta.Classe == "Chupeta")
                categoria = GetCategoriaChupeta(pesagem, peso);
            else
              if (pesagem.Atleta.Classe == "Sub-9")
                categoria = GetCategoriaSub9(pesagem, peso);
            else
              if (pesagem.Atleta.Classe == "Sub-11")
                categoria = GetCategoriaSub11(pesagem, peso);
            else
              if (pesagem.Atleta.Classe == "Sub-13")
                categoria = GetCategoriaSub13(pesagem, peso);
            else
              if (pesagem.Atleta.Classe == "Sub-15")
                {
                  if(pesagem.Atleta.Sexo==Sexo.Masculino)
                    categoria = GetCategoriaSub15M(pesagem, peso);
                  else
                    categoria = GetCategoriaSub15F(pesagem, peso);
                 }                
            else
              if (pesagem.Atleta.Classe == "Cadete")
                categoria = GetCategoriaCadete(pesagem, peso);
            else
              if (pesagem.Atleta.Classe == "Junior")
                categoria = GetCategoriaJunior(pesagem, peso);
            else
              if (pesagem.Atleta.Classe == "Dangai")
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
                    return "Super Ligeiro";
                    break;

                case >= 36m and < 40.0m:
                    return "Ligeiro";
                    break;

                case >= 40.01m and < 44:
                    return "Meio Leve";
                    break;

                case >= 44.01m and <= 48:
                    return "Leve";
                    break;

                case >= 48.01m and <= 52:
                    return "Meio Médio";
                    break;

                case >= 52.01m and <= 57:
                    return "Médio";
                    break;

                case >= 57.01m and <= 63:
                    return "Meio Pesado";
                    break;

                case >= 63.01m and <= 70:
                    return "Pesado";
                    break;

                case >= 70.01m and <= 80:
                    return "Super Pesado";
                    break;

                case >= 80.01m:
                    return "Baleia";
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
                    return "Super Ligeiro";
                    break;

                case >= 40.1m and < 45.0m:
                    return "Ligeiro";
                    break;

                case >= 45.01m and < 50:
                    return "Meio Leve";
                    break;

                case >= 50.01m and <= 55:
                    return "Leve";
                    break;

                case >= 55.01m and <= 60:
                    return "Meio Médio";
                    break;

                case >= 60.01m and <= 66:
                    return "Médio";
                    break;

                case >= 66.01m and <= 73:
                    return "Meio Pesado";
                    break;

                case >= 73.01m and <= 81:
                    return "Pesado";
                    break;

                case >= 81.01m and <= 100:
                    return "Super Pesado";
                    break;

                case >= 100.01m:
                    return "Godzilla";
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
                    return "Super Ligeiro";
                    break;

                case >= 28.1m and < 31.0m:
                    return "Ligeiro";
                    break;

                case >= 31.01m and < 34:
                    return "Meio Leve";
                    break;

                case >= 34.01m and <= 38:
                    return "Leve";
                    break;

                case >= 38.01m and <= 42:
                    return "Meio Médio";
                    break;

                case >= 42.01m and <= 47:
                    return "Médio";
                    break;

                case >= 47.01m and <= 52:
                    return "Meio Pesado";
                    break;

                case >= 52.01m and <= 60:
                    return "Pesado";
                    break;

                case >= 60.01m :
                    return "Super Pesado";
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
                    return "Super Ligeiro";
                    break;

                case >= 28.1m and < 30.0m:
                    return "Ligeiro";
                    break;

                case >= 30.01m and < 33:
                    return "Meio Leve";
                    break;

                case >= 33.01m and <= 36:
                    return "Leve";
                    break;

                case >= 36.01m and <= 40:
                    return "Meio Médio";
                    break;

                case >= 40.01m and <= 45:
                    return "Médio";
                    break;

                case >= 45.01m and <= 50:
                    return "Meio Pesado";
                    break;

                case >= 50.01m and <= 55:
                    return "Pesado";
                    break;

                case >= 55.01m and <= 60:
                    return "Super Pesado";
                    break;

                case >= 60.01m:
                    return "Extra Pesado";
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
                    return "Super Ligeiro";
                    break;
                     
                case >= 23.1m and < 26.0m:
                    return "Ligeiro";
                    break;

                case >= 26.01m and < 29:
                    return "Meio Leve";
                    break;

                case >= 29.01m and <= 32:
                    return "Leve";
                    break;

                case >= 32.01m and <= 36:
                    return "Meio Médio";
                    break;

                case >= 36.01m and <= 40:
                    return "Médio";
                    break;

                case >= 40.01m and <= 45:
                    return "Meio Pesado";
                    break;

                case >= 45.01m and <= 50:
                    return "Pesado";
                    break;

                case >= 50.01m and <= 55:
                    return "Super Pesado";
                    break;

                case >= 55.01m :
                    return "Extra Pesado";
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
