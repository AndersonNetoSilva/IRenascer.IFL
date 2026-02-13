using IFL.WebApp.Data;
using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Abstractions.Services;
using IFL.WebApp.Infrastructure.Abstractions.Services;
using IFL.WebApp.Infrastructure.Exceptions;
using IFL.WebApp.Infrastructure.Extensions;
using IFL.WebApp.Model;
using IFL.WebApp.Model;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace IFL.WebApp.Infrastructure.Services
{
    public class EstatisticaCompeticaoService : IEstatisticaCompeticaoService
    {
        private readonly IEstatisticaCompeticaoRepository _estatisticaRepository;
        private readonly IAtletaRepository _atletaRepository;
        private readonly IEventoRepository _eventoRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public EstatisticaCompeticaoService(IEstatisticaCompeticaoRepository estatisticaRepository,
            IAtletaRepository atletaRepository,
            IEventoRepository eventoRepository,
            ApplicationDbContext dbContext,
            IUnitOfWork unitOfWork)
        {
            _estatisticaRepository = estatisticaRepository;
            _atletaRepository = atletaRepository;
            _eventoRepository = eventoRepository;   
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(EstatisticaCompeticao estatistica,
                                   IEnumerable<EstatisticaCompeticaoDetalheVM> detalhes
                                  )
        {

            SyncDetalhes(estatistica, detalhes);

            _estatisticaRepository.Add(estatistica);

            await _unitOfWork.CommitAsync();
        }

        private void SyncDetalhes(EstatisticaCompeticao estatistica, IEnumerable<EstatisticaCompeticaoDetalheVM> detalhes)
        {
            var idsParaExclusao = detalhes
                                    .Where(x => x.Id.HasValue && x.MarcadoParaExclusao)
                                    .Select(x => x.Id.Value)
                                    .ToHashSet();

            estatistica.Detalhes.RemoveAll(p => idsParaExclusao.Contains(p.Id));

            var indice = 0;

            foreach (var detalhe in detalhes)
            {
                if (!detalhe.MarcadoParaExclusao)
                {
                    if (detalhe.Id.HasValue && detalhe.Id.Value > 0)
                    {
                        var estatisticaCompeticaoDetalhe = estatistica.Detalhes.First(p => p.Id == detalhe.Id);
                        estatisticaCompeticaoDetalhe.Ippon       = detalhe.Ippon;
                        estatisticaCompeticaoDetalhe.Shido       = detalhe.Shido;
                        estatisticaCompeticaoDetalhe.Yuko        = detalhe.Yuko;
                        estatisticaCompeticaoDetalhe.Wazari      = detalhe.Wazari;
                        estatisticaCompeticaoDetalhe.Hansokumake = detalhe.Hansokumake;
                        estatisticaCompeticaoDetalhe.Vitoria     = detalhe.Vitoria;
                        estatisticaCompeticaoDetalhe.GoldenScore = detalhe.GoldenScore;
                        estatisticaCompeticaoDetalhe.TempoDaLuta = detalhe.TempoDaLuta;
                        estatisticaCompeticaoDetalhe.TempoDoGoldenScore = detalhe.TempoDoGoldenScore;
                        estatisticaCompeticaoDetalhe.TecnicaAplicou     = detalhe.TecnicaAplicou;
                        estatisticaCompeticaoDetalhe.TecnicaRecebeu     = detalhe.TecnicaRecebeu;

                    }
                    else
                    {
                        estatistica.Detalhes.Add(new EstatisticaCompeticaoDetalhe
                        {
                            Ippon = detalhe.Ippon,
                            Shido = detalhe.Shido,
                            Yuko = detalhe.Yuko,
                            Wazari = detalhe.Wazari,
                            Hansokumake = detalhe.Hansokumake,
                            Vitoria = detalhe.Vitoria,
                            GoldenScore = detalhe.GoldenScore,
                            TempoDaLuta = detalhe.TempoDaLuta,
                            TempoDoGoldenScore = detalhe.TempoDoGoldenScore,
                            TecnicaAplicou = detalhe.TecnicaAplicou,
                            TecnicaRecebeu = detalhe.TecnicaRecebeu
                        });
                    }
                }

                indice++;
            }
        }




        public async Task UpdateAsync(EstatisticaCompeticao Estatistica,
                                      IEnumerable<EstatisticaCompeticaoDetalheVM> detalhes
                                        )
        {
            var estatistica = await _estatisticaRepository.GetForUpdateAsync(Estatistica.Id);

            if (estatistica != null)
            {
                _dbContext.Entry(estatistica).CurrentValues.SetValues(Estatistica);
                SyncDetalhes(estatistica, detalhes);

                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _estatisticaRepository.ExistsAsync(x => x.Id == Estatistica.Id))
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
