using SmartRestaurant.Application.Dtos;
using SmartRestaurant.Application.Interfaces.Persistence;
using SmartRestaurant.Application.Interfaces.Services;

namespace SmartRestaurant.Application.Services;

public sealed class StammdatenService(
    ITischRepository tischRepository,
    IMitarbeiterRepository mitarbeiterRepository) : IStammdatenService
{
    public async Task<List<TischResponse>> GetTischeAsync(CancellationToken cancellationToken = default)
    {
        var tische = await tischRepository.GetAllAsync(cancellationToken);

        return tische.Select(t => new TischResponse
        {
            TischId = t.Id,
            Plaetze = t.Plaetze,
            IstBelegt = t.Status
        }).ToList();
    }

    public async Task<List<MitarbeiterResponse>> GetMitarbeiterAsync(CancellationToken cancellationToken = default)
    {
        var mitarbeiter = await mitarbeiterRepository.GetAllAsync(cancellationToken);

        return mitarbeiter.Select(m => new MitarbeiterResponse
        {
            MitarbeiterId = m.Id,
            Name = m.Name,
            Benutzername = m.Benutzername,
            Rolle = m.Rolle
        }).ToList();
    }
}
