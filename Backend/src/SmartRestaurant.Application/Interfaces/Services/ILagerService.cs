using SmartRestaurant.Application.Common;
using SmartRestaurant.Application.Dtos;

namespace SmartRestaurant.Application.Interfaces.Services;

public interface ILagerService
{
    Task<List<LagerbestandResponse>> GetLagerbestandAsync(CancellationToken cancellationToken = default);

    Task<Result<LagerbestandResponse>> UpdateLagerbestandAsync(
        int zutatenId,
        LagerbestandUpdateRequest request,
        CancellationToken cancellationToken = default);
}
