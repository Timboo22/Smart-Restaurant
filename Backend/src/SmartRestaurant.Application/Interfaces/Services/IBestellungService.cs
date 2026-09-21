using SmartRestaurant.Application.Common;
using SmartRestaurant.Application.Dtos;

namespace SmartRestaurant.Application.Interfaces.Services;

public interface IBestellungService
{
    Task<Result<BestellungResponse>> CreateBestellungAsync(
        NeueBestellungRequest request,
        CancellationToken cancellationToken = default);

    Task<BestellungResponse?> GetBestellungAsync(int id, CancellationToken cancellationToken = default);

    Task<Result<BestellungResponse>> UpdateStatusAsync(
        int id,
        BestellungStatusRequest request,
        CancellationToken cancellationToken = default);
}
