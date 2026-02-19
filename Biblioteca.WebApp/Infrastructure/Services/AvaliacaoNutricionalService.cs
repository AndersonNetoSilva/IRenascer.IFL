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
                                    ArquivoVM? arquivoImagem,
                                    ArquivoVM? arquivoImagemCostas,
                                    IEnumerable<AvaliacaoNutricionalAnexoVM> anexos
                                    )
        {

            //Salvar Arquivo de Frente
            if (arquivoImagem?.FormFile?.Length > 0)
            {
                await SalvarOuSubstituirArquivoAsync(avaliacaoNutricional, arquivoImagem,
                      l => l.ArquivoImagemId == null ? null : _dbContext.Arquivos.First(x => x.Id == l.ArquivoImagemId),
                      (l, a) => l.ArquivoImagem = a);
            }

            //Salvar Arquivo de costas
            if (arquivoImagemCostas?.FormFile?.Length > 0)
            {
                await SalvarOuSubstituirArquivoAsync(avaliacaoNutricional, arquivoImagemCostas,
                      l => l.ArquivoImagemCostasId == null ? null : _dbContext.Arquivos.First(x => x.Id == l.ArquivoImagemCostasId),
                      (l, a) => l.ArquivoImagemCostas = a);
            }

            _avaliacaoRepository.Add(avaliacaoNutricional);

            await SyncAnexosAsync(avaliacaoNutricional, anexos);

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

        private async Task SyncAnexosAsync(AvaliacaoNutricional avaliacao, IEnumerable<AvaliacaoNutricionalAnexoVM> anexoList)
        {
            var idsParaExclusao = anexoList
                        .Where(x => x.Id.HasValue && x.MarcadoParaExclusao)
                        .Select(x => x.Id.Value)
                        .ToHashSet();

            avaliacao.Anexos.RemoveAll(p => idsParaExclusao.Contains(p.Id));

            var indice = 0;

            foreach (var anexoVM in anexoList)
            {
                if (!anexoVM.MarcadoParaExclusao)
                {
                    if (anexoVM.Id.HasValue && anexoVM.Id.Value > 0)
                    {
                        var anexoDoLivro = avaliacao.Anexos.First(p => p.Id == anexoVM.Id);

                        await SalvarOuSubstituirAnexoAsync(avaliacao, anexoDoLivro, anexoVM);
                    }
                    else
                    {
                        var anexo = await SalvarOuSubstituirAnexoAsync(avaliacao, null, anexoVM);

                        if (anexo != null)
                            avaliacao.Anexos.Add(anexo);
                    }
                }

                indice++;
            }

        }

        private async Task<AvaliacaoNutricionalAnexo> SalvarOuSubstituirAnexoAsync(AvaliacaoNutricional avaliacao, AvaliacaoNutricionalAnexo avaliacaoAnexo, AvaliacaoNutricionalAnexoVM anexoMV)
        {
            if (anexoMV?.FormFile == null || anexoMV?.FormFile?.Length <= 0)
                return avaliacaoAnexo;

            using var ms = new MemoryStream();

            await anexoMV.FormFile.CopyToAsync(ms);

            if (avaliacaoAnexo?.Anexo != null)
            {
                avaliacaoAnexo.Anexo.Conteudo = ms.ToArray();
                avaliacaoAnexo.Anexo.ContentType = anexoMV.FormFile.ContentType;
                avaliacaoAnexo.Anexo.Tamanho = (int)anexoMV.FormFile.Length;
                avaliacaoAnexo.Anexo.NomeOriginal = anexoMV.FormFile.FileName;
                avaliacaoAnexo.Anexo.Descricao = anexoMV.Descricao ?? anexoMV.FormFile.FileName;
                avaliacaoAnexo.Anexo.DataUltimaAlteracao = DateTime.UtcNow;
                avaliacaoAnexo.Descricao = anexoMV.Descricao ?? anexoMV.FormFile.FileName;
                avaliacaoAnexo.Tipo = IsImageFileName(avaliacaoAnexo.Anexo.NomeOriginal) ? TipoANexo.Imagem : TipoANexo.Arquivo;
            }
            else
            {
                var anexo = new Arquivo
                {
                    Conteudo = ms.ToArray(),
                    ContentType = anexoMV.FormFile.ContentType,
                    Tamanho = (int)anexoMV.FormFile.Length,
                    NomeOriginal = anexoMV.FormFile.FileName,
                    Descricao = anexoMV.Descricao ?? anexoMV.FormFile.FileName,
                    DataCriacao = DateTime.UtcNow
                };

                avaliacaoAnexo = new AvaliacaoNutricionalAnexo
                {
                    Anexo = anexo,
                    Descricao = anexoMV.Descricao ?? anexoMV.FormFile.FileName,
                    Tipo = IsImageFileName(anexo.NomeOriginal) ? TipoANexo.Imagem : TipoANexo.Arquivo
                };

                avaliacao.Anexos.Add(avaliacaoAnexo);
            }

            return avaliacaoAnexo;
        }

        private bool IsImageFileName(string nomeOriginal)
        {
            if (string.IsNullOrWhiteSpace(nomeOriginal))
                return false;

            string extension = Path.GetExtension(nomeOriginal).ToLowerInvariant();

            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".webp" };

            return Array.Exists(imageExtensions, ext => ext == extension);
        }

        public async Task UpdateAsync(AvaliacaoNutricional AvaliacaoNutricional,
                                        ArquivoVM? arquivoImagem,
                                        ArquivoVM? arquivoImagemCostas,
                                        IEnumerable<AvaliacaoNutricionalAnexoVM> anexos
                                        )
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

                if (arquivoImagemCostas?.FormFile?.Length > 0)
                {
                    await SalvarOuSubstituirArquivoAsync(avaliacao, arquivoImagemCostas,
                          l => l.ArquivoImagemCostasId == null ? null : _dbContext.Arquivos.First(x => x.Id == l.ArquivoImagemCostasId),
                          (l, a) => l.ArquivoImagemCostas = a);
                }

                await SyncAnexosAsync(avaliacao, anexos);

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
