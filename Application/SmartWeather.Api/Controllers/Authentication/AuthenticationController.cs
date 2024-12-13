namespace SmartWeather.Api.Controllers.Authentication;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWeather.Api.Contract;
using SmartWeather.Services.Users;
using SmartWeather.Api.Controllers.Authentication.Dtos;
using SmartWeather.Api.Controllers.Authentication.Dtos.Converters;
using SmartWeather.Services.Authentication;
using SmartWeather.Entities.User;
using SmartWeather.Entities.Common.Exceptions;

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
            return Ok(response);
        }
        catch (Exception ex) when (ex is EntityCreationException)
        {
            response = ApiResponse<UserRegisterResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR, 
                                                                                "Unable to create User from given informations"), 
                                                                 Status.VALIDATION_ERROR);
            return Ok(response);
        }
        catch (Exception ex) when (ex is EntitySavingException)
        {
            response = ApiResponse<UserRegisterResponse>.Failure(BaseResponses.DATABASE_ERROR,
                                                                 Status.DATABASE_ERROR);
            return Ok(response);
        }
        catch
        {
            response = ApiResponse<UserRegisterResponse>.Failure();
            return BadRequest(response);
        }
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
            return Ok(response);
        }
        catch (Exception ex) when (ex is EntityFetchingException)
        {
            response = ApiResponse<UserSigninResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR, 
                                                                            "Unable to retreive User from given credentials"),
                                                               Status.VALIDATION_ERROR);
            return Ok(response);
        }
        catch (Exception ex) when (ex is EntityCreationException)
        {
            response = ApiResponse<UserSigninResponse>.Failure(string.Format(BaseResponses.FORMAT_ERROR,
                                                                            "Unable to create token from detected User"),
                                                               Status.VALIDATION_ERROR);
            return Ok(response);
        }
        catch
        {
            response = ApiResponse<UserSigninResponse>.Failure();
            return BadRequest(response);
        }
    }

}