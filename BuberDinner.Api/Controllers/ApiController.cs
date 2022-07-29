using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BuberDinner.Api.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
  protected IActionResult Problem(List<Error> errors)
  {
    var firstError = errors[0];

    var statusCode = firstError.Type switch
    {
      ErrorType.Conflict => StatusCodes.Status409Conflict,
      ErrorType.Validation => StatusCodes.Status400BadRequest,
      ErrorType.NotFound => StatusCodes.Status404NotFound,
      _ => StatusCodes.Status500InternalServerError
    };

    var modelState = new ModelStateDictionary();
    foreach (var error in errors)
    {
      modelState.AddModelError(error.Code, error.Description);
    }

    return ValidationProblem(
      statusCode: statusCode,
      title: firstError.Description,
      modelStateDictionary: modelState
    );
  }
}