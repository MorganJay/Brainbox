using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ApiBad(ModelStateDictionary? modelState = null, [FromQuery] List<string>? errors = null, string message = "A required parameter is missing from the input. See errors")
        {
            var modelErrors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            var r = new Error
            {
                Message = modelState is null ? message : modelErrors[0],
                Errors = modelState is null ? errors is null ? null : errors : modelErrors
            };

            return BadRequest(r);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ApiUnauthorized(string message = "Unauthorized")
        {
            var r = new Error
            {
                Message = message,
            };
            return Unauthorized(r);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        protected IActionResult ApiOk<T>(T obj, string message = "SUCCESS") => Ok(new Result<T>(obj, message));

        [ApiExplorerSettings(IgnoreApi = true)]
        protected IActionResult ApiCreated<T>(string actionName, object value, T data, string message = "Successfully Created")
        {
            return CreatedAtAction(actionName, value, new Result<T>(data, message));
        }
    }
}
