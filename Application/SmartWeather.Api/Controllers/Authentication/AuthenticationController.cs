namespace SmartWeather.Api.Controllers.Authentication;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWeather.Api.Contract;
using SmartWeather.Services.Users;
using SmartWeather.Api.Controllers.Authentication.Dtos;
using SmartWeather.Api.Controllers.Authentication.Dtos.Converters;
using SmartWeather.Services.Authentication;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly UserService _userService;
    private readonly AuthenticationService _authenticationService;

    public AuthenticationController(UserService userService, AuthenticationService authenticationService)
    {
        _userService = userService;
        _authenticationService = authenticationService;
    }

    [AllowAnonymous]
    [HttpPost(nameof(Register))]
    public ActionResult<ApiResponse<UserRegisterResponse>> Register(UserRegisterRequest request)
    {
        ApiResponse<UserRegisterResponse> response;
        UserRegisterResponse formattedResponse;

        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(ApiResponse<UserRegisterResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        var registerResult = _authenticationService.Register(request.Name, request.Email, request.Password);

        if (registerResult.IsFailure)
        {
            response = ApiResponse<UserRegisterResponse>.Failure(registerResult.ErrorMessage);
            return BadRequest(response);
        }

        formattedResponse = UserRegisterResponseConverter.ConvertUserToUserRegisterResponse(registerResult.Value.Item1, registerResult.Value.Item2);
        response = ApiResponse<UserRegisterResponse>.Success(formattedResponse);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost(nameof(Signin))]
    public ActionResult<ApiResponse<UserSigninResponse>> Signin(UserSigninRequest request)
    {
        ApiResponse<UserSigninResponse> response;

        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(ApiResponse<UserSigninResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        var signinResult = _authenticationService.Signin(request.Email, request.Password);

        if (signinResult.IsFailure)
        {
            response = ApiResponse<UserSigninResponse>.Failure(signinResult.ErrorMessage);
            return BadRequest(response);
        }

        var formattedResponse = UserSigninResponseConverter.ConvertUserToUserSigninResponse(signinResult.Value.Item1, signinResult.Value.Item2);
        response = ApiResponse<UserSigninResponse>.Success(formattedResponse);
        return Ok(response);
    }

}