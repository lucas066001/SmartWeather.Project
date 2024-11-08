namespace SmartWeather.Api.Controllers.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWeather.Api.Contract;
using SmartWeather.Services.Users;
using SmartWeather.Api.Controllers.Authentication.Dtos;
using SmartWeather.Api.Controllers.Authentication.Dtos.Converters;
using SmartWeather.Services.Authentication;
using SmartWeather.Entities.User;

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

        try
        {
            Tuple<User, string> registerResponse = _authenticationService.Register(request.Name, request.Email, request.Password);
            formattedResponse = UserRegisterResponseConverter.ConvertUserToUserRegisterResponse(registerResponse.Item1, registerResponse.Item2);
            response = ApiResponse<UserRegisterResponse>.Success(formattedResponse);
        }
        catch (Exception ex)
        {
            response = ApiResponse<UserRegisterResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost(nameof(Signin))]
    public ActionResult<ApiResponse<UserSigninResponse>> Signin(UserSigninRequest request)
    {
        ApiResponse<UserSigninResponse> response;
        UserSigninResponse formattedResponse;

        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(ApiResponse<UserSigninResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        try
        {
            Tuple<User, string> signinResponse = _authenticationService.Signin(request.Email, request.Password);
            formattedResponse = UserSigninResponseConverter.ConvertUserToUserSigninResponse(signinResponse.Item1, signinResponse.Item2);
            response = ApiResponse<UserSigninResponse>.Success(formattedResponse);
        }
        catch (Exception ex)
        {
            response = ApiResponse<UserSigninResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR, ex.Message));
            return BadRequest(response);
        }

        return Ok(response);
    }

}