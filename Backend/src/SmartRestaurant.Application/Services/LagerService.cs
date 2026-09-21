using SmartRestaurant.Application.Common;
using SmartRestaurant.Application.Dtos;
using SmartRestaurant.Application.Interfaces.Persistence;
using SmartRestaurant.Application.Interfaces.Services;

namespace SmartRestaurant.Application.Services;

public sealed class LagerService(
    ILagerbestandRepository lagerbestandRepository,
    IUnitOfWork unitOfWork) : ILagerService
{
    public async Task<List<LagerbestandResponse>> GetLagerbestandAsync(CancellationToken cancellationToken = default)
    {
        var lagerbestaende = await lagerbestandRepository.GetAllWithZutatAsync(cancellationToken);

        return lagerbestaende.Select(ToResponse).ToList();
    }

    public async Task<Result<LagerbestandResponse>> UpdateLagerbestandAsync(
        int zutatenId,
        LagerbestandUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        var lagerbestand = await lagerbestandRepository.GetByZutatIdAsync(zutatenId, cancellationToken);
        if (lagerbestand is null)
        {
            return Result<LagerbestandResponse>.Failure(
                ErrorType.NotFound,
                $"Lagerbestand für Zutat mit Id {zutatenId} wurde nicht gefunden.");
        }

        lagerbestand.Soll = request.Soll;
        lagerbestand.Ist = request.Ist;
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<LagerbestandResponse>.Success(ToResponse(lagerbestand));
    }

    private static LagerbestandResponse ToResponse(Domain.Entities.Lagerbestand lagerbestand) => new()
    {
        ZutatenId = lagerbestand.ZutatId,
        ZutatenName = lagerbestand.Zutat.Name,
        Soll = lagerbestand.Soll,
        Ist = lagerbestand.Ist,
        NachbestellenErforderlich = lagerbestand.Ist < lagerbestand.Soll
    };
}
