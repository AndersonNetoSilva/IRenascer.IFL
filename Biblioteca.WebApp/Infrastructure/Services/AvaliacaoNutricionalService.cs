using IFL.WebApp.Data;
using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Abstractions.Services;
using IFL.WebApp.Infrastructure.Exceptions;
using IFL.WebApp.Infrastructure.Extensions;
using IFL.WebApp.Model;
using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Abstractions.Services;
using IFL.WebApp.Model;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Infrastructure.Services
{
    public class AvaliacaoNutricionalService : IAvaliacaoNutricionalService
    {
        private readonly IAvaliacaoNutricionalRepository _avaliacaoRepository;
        private readonly IAtletaRepository _atletaRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public AvaliacaoNutricionalService(IAvaliacaoNutricionalRepository avaliacaoRepository,
            IAtletaRepository atletaRepository,
            ApplicationDbContext dbContext,
            IUnitOfWork unitOfWork)
        {
            _avaliacaoRepository = avaliacaoRepository;
            _atletaRepository = atletaRepository;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(
            AvaliacaoNutricional avaliacaoNutricional,           
            ArquivoVM? arquivoImagem)
        {
            //ValidarValor(livro);

            if (arquivoImagem?.FormFile?.Length > 0)
            {
                await SalvarOuSubstituirArquivoAsync(avaliacaoNutricional, arquivoImagem,
                      l => l.ArquivoImagemId == null ? null : _dbContext.Arquivos.First(x => x.Id == l.ArquivoImagemId),
                      (l, a) => l.ArquivoImagem = a);
            }
            
            _avaliacaoRepository.Add(avaliacaoNutricional); 

            await _unitOfWork.CommitAsync();
        }

        private async Task SalvarOuSubstituirArquivoAsync(
            AvaliacaoNutricional avaliacao,
            ArquivoVM arquivoVm,
            Func<AvaliacaoNutricional, Arquivo?> getter,
            Action<AvaliacaoNutricional, Arquivo> setter)
        {
            if (arquivoVm?.FormFile == null)
                return;

            using var ms = new MemoryStream();
            await arquivoVm.FormFile.CopyToAsync(ms);

            var arquivo = getter(avaliacao);

            if (arquivo != null)
            {
                arquivo.Conteudo = ms.ToArray();
                arquivo.ContentType = arquivoVm.FormFile.ContentType;
                arquivo.Tamanho = (int)arquivoVm.FormFile.Length;
                arquivo.NomeOriginal = arquivoVm.FormFile.FileName;
                arquivo.Descricao = arquivoVm.Descricao ?? arquivoVm.FormFile.FileName;
                arquivo.DataUltimaAlteracao = DateTime.UtcNow;
            }
            else
            {
                arquivo = new Arquivo
                {
                    Conteudo = ms.ToArray(),
                    ContentType = arquivoVm.FormFile.ContentType,
                    Tamanho = (int)arquivoVm.FormFile.Length,
                    NomeOriginal = arquivoVm.FormFile.FileName,
                    Descricao = arquivoVm.Descricao ?? arquivoVm.FormFile.FileName,
                    DataCriacao = DateTime.UtcNow
                };

                setter(avaliacao, arquivo);
            }
        }

        public async Task UpdateAsync(AvaliacaoNutricional AvaliacaoNutricional,
                                        ArquivoVM? arquivoImagem)
        {
            var avaliacao = await _avaliacaoRepository.GetForUpdateAsync(AvaliacaoNutricional.Id);

            if (avaliacao != null)
            {
                _dbContext.Entry(avaliacao).CurrentValues.SetValues(AvaliacaoNutricional);

                if (arquivoImagem?.FormFile?.Length > 0)
                {
                    await SalvarOuSubstituirArquivoAsync(avaliacao, arquivoImagem,
                          l => l.ArquivoImagemId == null ? null : _dbContext.Arquivos.First(x => x.Id == l.ArquivoImagemId),
                          (l, a) => l.ArquivoImagem = a);
                }
               
                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _avaliacaoRepository.ExistsAsync(x => x.Id == AvaliacaoNutricional.Id))
                    {
                        throw new KeyNotFoundException();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}
