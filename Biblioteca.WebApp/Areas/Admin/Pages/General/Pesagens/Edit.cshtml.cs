using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Infrastructure.Repositories;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.General.Pesagens
{
    public class EditModel : CrudPageModel<Pesagem, IPesagemRepository>
    {
        private readonly IAtletaRepository _atletaRepository;

        private readonly IEventoRepository _eventoRepository;

        public EditModel(IPesagemRepository repository,
                         IAtletaRepository atletaRepository,
                         IEventoRepository eventoRepository, 
                         IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            _atletaRepository = atletaRepository;
            _eventoRepository = eventoRepository;
        }

        [BindProperty]
        public Pesagem Pesagem { get; set; } = default!;

        private void BindSelectLists()
        {
            Eventos = _eventoRepository.Query()
                        .Where(a => !a.Encerrado && a.TipoEvento == TipoEvento.Competicao)
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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            BindSelectLists();

            if (id == null)
            {
                return NotFound();
            }

            //Método com o Include
            var pesagem = await _repository.GetForUpdateAsync(id ?? throw new ArgumentException(nameof(id)));

            if (pesagem == null)
            {
                return NotFound();
            }

            if(pesagem.Peso1.HasValue)
                pesagem.Peso1AsString = pesagem.Peso1.Value.ToString(new System.Globalization.CultureInfo("pt-BR"));

            if(pesagem.Peso2.HasValue)
                pesagem.Peso2AsString = pesagem.Peso2.Value.ToString(new System.Globalization.CultureInfo("pt-BR"));

            if(pesagem.Peso3.HasValue)
                pesagem.Peso3AsString = pesagem.Peso3.Value.ToString(new System.Globalization.CultureInfo("pt-BR"));

            IFL.WebApp.Areas.Admin.Pages.General.Pesagens.CreateModel.SetCategoria(pesagem);

            Pesagem = pesagem;

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Pesagem.Atleta = _atletaRepository.Query().Where(a => Pesagem.AtletaId == a.Id).FirstOrDefault();
            Pesagem.Evento = _eventoRepository.Query().Where(a => Pesagem.EventoId == a.Id).FirstOrDefault();

            IFL.WebApp.Areas.Admin.Pages.General.Pesagens.CreateModel.ValidarValor(Pesagem);

            if(Pesagem.Atleta != null)
                IFL.WebApp.Areas.Admin.Pages.General.Pesagens.CreateModel.SetCategoria(Pesagem);

            if (!ModelState.IsValid)
            {
                BindSelectLists();
                return Page();
            }

            try
            {
                _repository.Update(Pesagem);
                await _unitOfWork.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _repository.ExistsAsync(x => x.Id == Pesagem.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
