using IFL.WebApp.Data;
using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Abstractions.Services;
using IFL.WebApp.Infrastructure.Abstractions.Services;
using IFL.WebApp.Infrastructure.Exceptions;
using IFL.WebApp.Infrastructure.Extensions;
using IFL.WebApp.Infrastructure.Repositories;
using IFL.WebApp.Model;
using IFL.WebApp.Model;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Infrastructure.Services
{
    public class AtletaService : IAtletaService
    {
        private readonly IAtletaRepository _atletaRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public AtletaService(
            IAtletaRepository atletaRepository,
            ApplicationDbContext dbContext,
            IUnitOfWork unitOfWork)
        {
            _atletaRepository = atletaRepository;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(Atleta atleta, IEnumerable<AtletaGradeVM> grade, ArquivoVM? arquivoImagem)
        {

            if (arquivoImagem?.FormFile?.Length > 0)
            {
                await SalvarOuSubstituirArquivoAsync(atleta, arquivoImagem,
                      l => l.ArquivoImagemId == null ? null : _dbContext.Arquivos.First(x => x.Id == l.ArquivoImagemId),
                      (l, a) => l.ArquivoImagem = a);
            }

            SyncGrade(atleta, grade);

            _atletaRepository.Add(atleta); 

            await _unitOfWork.CommitAsync();
        }

        private void SyncGrade(Atleta atleta, IEnumerable<AtletaGradeVM> grades)
        {
            var idsParaExclusao = grades
                                    .Where(x => x.Id.HasValue && x.MarcadoParaExclusao)
                                    .Select(x => x.Id.Value)
                                    .ToHashSet();

            atleta.AtletaGrades.RemoveAll(p => idsParaExclusao.Contains(p.Id));

            var indice = 0;

            foreach (var gradeAtleta in grades)
            {
                if (!gradeAtleta.MarcadoParaExclusao)
                {
                    if (gradeAtleta.Id.HasValue && gradeAtleta.Id.Value > 0)
                    {
                        var grade = atleta.AtletaGrades.First(p => p.Id == gradeAtleta.Id);
                        grade.ModalidadeId = gradeAtleta.ModalidadeId;
                        grade.HorarioId    = gradeAtleta.HorarioId;
                    }
                    else
                    {
                        atleta.AtletaGrades.Add(new AtletaGrade
                        {
                            AtletaId = gradeAtleta.AtletaId,
                            ModalidadeId = gradeAtleta.ModalidadeId,
                            HorarioId = gradeAtleta.HorarioId
                        });
                    }
                }

                indice++;
            }
        }


        private async Task SalvarOuSubstituirArquivoAsync(
            Atleta atleta,
            ArquivoVM arquivoVm,
            Func<Atleta, Arquivo?> getter,
            Action<Atleta, Arquivo> setter)
        {
            if (arquivoVm?.FormFile == null)
                return;

            using var ms = new MemoryStream();
            await arquivoVm.FormFile.CopyToAsync(ms);

            var arquivo = getter(atleta);

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

                setter(atleta, arquivo);
            }
        }

        public async Task UpdateAsync(Atleta Atleta, IEnumerable<AtletaGradeVM> grade, ArquivoVM? arquivoImagem)
        {
            var atleta = await _atletaRepository.GetForUpdateAsync(Atleta.Id);

            if (atleta != null)
            {
                _dbContext.Entry(atleta).CurrentValues.SetValues(Atleta);

                if (arquivoImagem?.FormFile?.Length > 0)
                {
                    await SalvarOuSubstituirArquivoAsync(atleta, arquivoImagem,
                          l => l.ArquivoImagemId == null ? null : _dbContext.Arquivos.First(x => x.Id == l.ArquivoImagemId),
                          (l, a) => l.ArquivoImagem = a);
                }

                SyncGrade(atleta, grade);

                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _atletaRepository.ExistsAsync(x => x.Id == Atleta.Id))
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
