using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BuberDinner.Api.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
  protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count is 0)
            return Problem();

        var modelState = new ModelStateDictionary();
        foreach (var error in errors)
        {
            modelState.AddModelError(error.Code, error.Description);
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
            return ValidationProblem(modelState);

        return Problems(modelState, errors[0]);
    }

    private IActionResult Problems(ModelStateDictionary modelState, Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        return ValidationProblem(
          statusCode: statusCode,
          title: error.Description,
          modelStateDictionary: modelState
        );
    }
}