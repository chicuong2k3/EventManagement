using Evently.Modules.Events.Presentation.ApiResults;

namespace EventManagement.Events.Api.Responses
{
    public static class ResultExtensions
    {
        public static IResult Match(
            this Result result,
            Func<IResult> onSuccess)
        {
            return result.IsSuccess ? onSuccess() : ApiResponses.Problem(result);
        }

        public static IResult Match<TIn>(
            this Result<TIn> result,
            Func<TIn, IResult> onSuccess)
        {
            return result.IsSuccess ? onSuccess(result.Value) : ApiResponses.Problem(result);
        }
    }
}
