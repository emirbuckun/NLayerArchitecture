using System.Net;
using App.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Services.Filters {
    public class NotFoundFilter<T, TId>(IGenericRepository<T, TId> genericRepository) : Attribute, IAsyncActionFilter
        where T : BaseEntity<TId> where TId : struct {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
            var idValue = context.ActionArguments.TryGetValue("request", out var requestObj) ?
                (requestObj as dynamic)?.Id :
                context.ActionArguments.TryGetValue("id", out var idObj) ? idObj : null;

            if (idValue is not TId) {
                await next();
                return;
            }

            if (await genericRepository.AnyAsync((TId)idValue)) {
                await next();
                return;
            }

            var entityName = typeof(T).Name;
            var actionName = context.ActionDescriptor.RouteValues["action"];

            var result = ServiceResult.Fail($"{entityName} with id: {idValue} not found in {actionName}", HttpStatusCode.NotFound);
            context.Result = new NotFoundObjectResult(result);
            return;
        }
    }
}