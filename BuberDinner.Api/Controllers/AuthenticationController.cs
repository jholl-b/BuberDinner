using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Domain.Common.Errors;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[Route("auth")]
public class AuthenticationController : ApiController
{
  private readonly IAuthenticationService _authenticationService;

  public AuthenticationController(IAuthenticationService authenticationService)
  {
    _authenticationService = authenticationService;
  }

  [HttpPost("register")]
  public IActionResult Register(RegisterRequest request)
  {
    var authResult = _authenticationService.Register(
      request.FirstName, 
      request.LastName,
      request.Email,
      request.Password);

    return authResult.Match(
      authResult => Ok(MapAuthResult(authResult)),
      errors => Problem(errors)
    );
  }

  [HttpPost("login")]
  public IActionResult Login(LoginRequest request)
  {
    var authResult = _authenticationService.Login(
      request.Email,
      request.Password
    );

    if (authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidCredentials)
      return Problem(
        statusCode: StatusCodes.Status401Unauthorized,
        title: authResult.FirstError.Description
      );

    return authResult.Match(
      authResult => Ok(MapAuthResult(authResult)),
      errors => Problem(errors)
    );
  }

  private AuthenticationResponse MapAuthResult(AuthenticationResult authResult) 
    => new AuthenticationResponse(
      Id: authResult.User.Id,
      FirstName: authResult.User.FirstName,
      LastName: authResult.User.LastName,
      Email: authResult.User.Email,
      Token: authResult.Token
    );
}