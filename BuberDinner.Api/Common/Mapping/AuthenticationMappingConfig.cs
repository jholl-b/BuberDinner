using BuberDinner.Application.Authentication.Common;
using BuberDinner.Contracts.Authentication;
using Mapster;

namespace BuberDinner.Api.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
  public void Register(TypeAdapterConfig config)
  {
    config.NewConfig<RegisterRequest, AuthenticationResponse>();
    config.NewConfig<LoginRequest, AuthenticationResponse>();
    config.NewConfig<AuthenticationResult, AuthenticationResponse>()
      .Map(dest => dest, src => src.User);
  }
}