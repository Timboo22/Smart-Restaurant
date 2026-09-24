using SmartRestaurant.Application.Common;
using SmartRestaurant.Application.Dtos;

namespace SmartRestaurant.Application.Interfaces.Services;

public interface IStammdatenService
{
    Task<List<TischResponse>> GetTischeAsync(CancellationToken cancellationToken = default);
    Task<List<MitarbeiterResponse>> GetMitarbeiterAsync(CancellationToken cancellationToken = default);

    Task<Result<TischResponse>> UpdateTischStatusAsync(
        int tischId,
        TischStatusUpdateRequest request,
        CancellationToken cancellationToken = default);
}
