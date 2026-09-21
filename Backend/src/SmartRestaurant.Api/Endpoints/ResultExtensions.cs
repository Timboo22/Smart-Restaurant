using SmartRestaurant.Application.Common;

namespace SmartRestaurant.Api.Endpoints;

public static class ResultExtensions
{
    public static IResult ToHttpResult<T>(this Result<T> result, Func<T, IResult> onSuccess) =>
        result switch
        {
            { IsSuccess: true } => onSuccess(result.Value!),
            { ErrorType: ErrorType.Validation } => Results.BadRequest(new ErrorResponse { Message = result.Error! }),
            { ErrorType: ErrorType.NotFound } => Results.NotFound(new ErrorResponse { Message = result.Error! }),
            _ => Results.Problem(result.Error)
        };
}
