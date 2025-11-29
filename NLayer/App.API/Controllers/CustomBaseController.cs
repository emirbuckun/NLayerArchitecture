using System.Net;
using App.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class CustomBaseController : ControllerBase {
        [NonAction]
        public IActionResult CreateActionResult<T>(ServiceResult<T> result) {
            return result.StatusCode switch {
                HttpStatusCode.NoContent => NoContent(),
                HttpStatusCode.Created => Created(result.UrlAsCreated, result),
                _ => StatusCode((int)result.StatusCode, result)
            };
        }

        [NonAction]
        public IActionResult CreateActionResult(ServiceResult result) {
            if (result.StatusCode == HttpStatusCode.NoContent) {
                return StatusCode((int)result.StatusCode, null);
            }

            return StatusCode((int)result.StatusCode, result);
        }
    }
}