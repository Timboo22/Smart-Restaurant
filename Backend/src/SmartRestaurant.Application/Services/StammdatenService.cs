using SmartRestaurant.Application.Common;
using SmartRestaurant.Application.Dtos;
using SmartRestaurant.Application.Interfaces.Persistence;
using SmartRestaurant.Application.Interfaces.Services;

namespace SmartRestaurant.Application.Services;

public sealed class StammdatenService(
    ITischRepository tischRepository,
    IMitarbeiterRepository mitarbeiterRepository,
    IUnitOfWork unitOfWork) : IStammdatenService
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

    public async Task<Result<TischResponse>> UpdateTischStatusAsync(
        int tischId,
        TischStatusUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        var tisch = await tischRepository.GetByIdAsync(tischId, cancellationToken);
        if (tisch is null)
        {
            return Result<TischResponse>.Failure(
                ErrorType.NotFound,
                $"Tisch mit Id {tischId} wurde nicht gefunden.");
        }

        tisch.Status = request.IstBelegt;
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<TischResponse>.Success(new TischResponse
        {
            TischId = tisch.Id,
            Plaetze = tisch.Plaetze,
            IstBelegt = tisch.Status
        });
    }
}
