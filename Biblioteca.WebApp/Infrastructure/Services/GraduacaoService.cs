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
    public class GraduacaoService : IGraduacaoService
    {
        private readonly IHorarioRepository _horarioRepository;
        private readonly IModalidadeRepository _modalidadeRepository;
        private readonly IAtletaRepository _atletaRepository;
        private readonly IGraduacaoRepository _graduacaoRepository;

        private readonly ApplicationDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public GraduacaoService(IHorarioRepository horarioRepository,
                                IModalidadeRepository modalidadeRepository,
                                IAtletaRepository atletaRepository,
                                IGraduacaoRepository graduacaoRepository,
                                ApplicationDbContext dbContext,
                                IUnitOfWork unitOfWork)
        {
            _graduacaoRepository    = graduacaoRepository;
            _horarioRepository      = horarioRepository;
            _modalidadeRepository   = modalidadeRepository;
            _atletaRepository       = atletaRepository;
            _dbContext              = dbContext;
            _unitOfWork             = unitOfWork;
        }

        public async Task AddAsync( Graduacao graduacao,
                                    IEnumerable<GraduacaoAtletaVM> graduacaoAtletaVMs)
        {

            _graduacaoRepository.Add(graduacao);

           // await SyncAnexosAsync(graduacao, graduacaoAtletaVMs);

            await _unitOfWork.CommitAsync();
        }

        public bool ExisteGraduacaoNoPeriodo(GraduacaoVM graduacao)
        {
           return   _graduacaoRepository
                        .Query()
                        .Where(x=> x.HorarioId == graduacao.HorarioId &&
                                   x.Data.Year == graduacao.Data.Value.Year).Any();
            
        }

        private async Task SyncAnexosAsync(Graduacao graduacao, IEnumerable<GraduacaoAtletaVM> graduacaoAtletaListVM)
        {
            var idsParaExclusao = graduacaoAtletaListVM
                        .Where(x => x.Id.HasValue && x.MarcadoParaExclusao)
                        .Select(x => x.Id.Value)
                        .ToHashSet();

            graduacao.GraduacaoAtletas.RemoveAll(p => idsParaExclusao.Contains(p.Id));

            var indice = 0;

            foreach (var graducaoAtletaVM in graduacaoAtletaListVM)
            {
                if (!graducaoAtletaVM.MarcadoParaExclusao)
                {
                    if (graducaoAtletaVM.Id.HasValue && graducaoAtletaVM.Id.Value > 0)
                    {
                        var anexoDoLivro = graduacao.GraduacaoAtletas.First(p => p.Id == graducaoAtletaVM.Id);

                        await SalvarOuSubstituirAnexoAsync(graduacao, anexoDoLivro, graducaoAtletaVM);
                    }
                    else
                    {
                        var anexo = await SalvarOuSubstituirAnexoAsync(graduacao, null, graducaoAtletaVM);

                        if (anexo != null)
                            graduacao.GraduacaoAtletas.Add(anexo);
                    }
                }

                indice++;
            }

        }

        private async Task<GraduacaoAtleta> SalvarOuSubstituirAnexoAsync(Graduacao graduacao, 
                                            GraduacaoAtleta graduacaoAtleta, 
                                            GraduacaoAtletaVM graduacaoAtletaMV)
        {
            if (graduacaoAtletaMV?.FormFile == null || graduacaoAtletaMV?.FormFile?.Length <= 0)
                return graduacaoAtleta;

            using var ms = new MemoryStream();

            await graduacaoAtletaMV.FormFile.CopyToAsync(ms);

            if (graduacaoAtleta?.Arquivo != null)
            {
                graduacaoAtleta.Arquivo.Conteudo = ms.ToArray();
                graduacaoAtleta.Arquivo.ContentType = graduacaoAtletaMV.FormFile.ContentType;
                graduacaoAtleta.Arquivo.Tamanho = (int)graduacaoAtletaMV.FormFile.Length;
                graduacaoAtleta.Arquivo.NomeOriginal = graduacaoAtletaMV.FormFile.FileName;
                graduacaoAtleta.Arquivo.Descricao =  graduacaoAtletaMV.FormFile.FileName;
                graduacaoAtleta.Arquivo.DataUltimaAlteracao = DateTime.UtcNow; 
                graduacaoAtleta.Tipo = IsImageFileName(graduacaoAtleta.Arquivo.NomeOriginal) ? TipoANexo.Imagem : TipoANexo.Arquivo;
            }
            else
            {
                var anexo = new Arquivo
                {
                    Conteudo        = ms.ToArray(),
                    ContentType     = graduacaoAtletaMV.FormFile.ContentType,
                    Tamanho         = (int)graduacaoAtletaMV.FormFile.Length,
                    NomeOriginal    = graduacaoAtletaMV.FormFile.FileName,
                    Descricao       = graduacaoAtletaMV.FormFile.FileName,
                    DataCriacao     = DateTime.UtcNow
                };

                graduacaoAtleta = new GraduacaoAtleta
                {
                    Arquivo = anexo,                    
                    Tipo = IsImageFileName(anexo.NomeOriginal) ? TipoANexo.Imagem : TipoANexo.Arquivo,
                    FaixaNova = graduacaoAtletaMV.FaixaNova
                };

                graduacao.GraduacaoAtletas.Add(graduacaoAtleta);
            }

            return graduacaoAtleta;
        }

        private bool IsImageFileName(string nomeOriginal)
        {
            if (string.IsNullOrWhiteSpace(nomeOriginal))
                return false;

            string extension = Path.GetExtension(nomeOriginal).ToLowerInvariant();

            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".webp" };

            return Array.Exists(imageExtensions, ext => ext == extension);
        }

        public async Task UpdateAsync(Graduacao Graduacao,
                                      IEnumerable<GraduacaoAtletaVM> graduacaoAtletaVMs)
        {
            var graduacao = await _graduacaoRepository.GetForUpdateAsync(Graduacao.Id);

            if (graduacao != null)
            {
                _dbContext.Entry(graduacao).CurrentValues.SetValues(Graduacao);
                
                await SyncAtletasAsync(graduacao, graduacaoAtletaVMs);
                await SyncAnexosAsync(graduacao, graduacaoAtletaVMs);

                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _graduacaoRepository.ExistsAsync(x => x.Id == Graduacao.Id))
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

        private async Task SyncAtletasAsync(Graduacao graduacao,
                                 IEnumerable<GraduacaoAtletaVM> graduacaoAtletaVMs)
        {
            var idsParaExclusao = graduacaoAtletaVMs
                                    .Where(x => x.Id.HasValue && x.MarcadoParaExclusao)
                                    .Select(x => x.Id.Value)
                                    .ToHashSet();

            graduacao.GraduacaoAtletas.RemoveAll(p => idsParaExclusao.Contains(p.Id));

            var indice = 0;

            foreach (var graduacaoAtleta in graduacaoAtletaVMs)
            {
                if (!graduacaoAtleta.MarcadoParaExclusao)
                {
                    if (graduacaoAtleta.Id.HasValue && graduacaoAtleta.Id.Value > 0)
                    {
                        var g = graduacao.GraduacaoAtletas.First(p => p.Id == graduacaoAtleta.Id);
                        g.Aprovado    = graduacaoAtleta.Aprovado;
                        g.AtletaId    = graduacaoAtleta.AtletaId;
                        g.NotaPratica = graduacaoAtleta.NotaPratica;
                        g.NotaEscrita = graduacaoAtleta.NotaEscrita;
                        g.FaixaNova   = graduacaoAtleta.FaixaNova;
                        g.Atleta      = _atletaRepository.Query().Where(x => x.Id == graduacaoAtleta.AtletaId).FirstOrDefault();
                    }
                    else
                    {
                        var atleta = _atletaRepository.Query().Where(x => x.Id == graduacaoAtleta.AtletaId).FirstOrDefault();

                        graduacao.GraduacaoAtletas.Add(new GraduacaoAtleta
                        {
                            AtletaId    = graduacaoAtleta.AtletaId,
                            Aprovado    = graduacaoAtleta.Aprovado,
                            GraducaoId  = graduacaoAtleta.GraduacaoId,
                            NotaEscrita = graduacaoAtleta.NotaEscrita,
                            NotaPratica = graduacaoAtleta.NotaPratica,
                            FaixaNova   = graduacaoAtleta.FaixaNova,
                            Atleta      = atleta
                            
                        });
                    }
                }

                indice++;
            } 
        }

    }
}
