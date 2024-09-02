using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartWeather.Api.Contract;
using SmartWeather.Api.Controllers.User.Dtos.Converters;
using SmartWeather.Api.Controllers.User.Dtos;
using SmartWeather.Services.Users;
using SmartWeather.Api.Controllers.Authentication.Dtos;
using SmartWeather.Services.Authentication;
using Microsoft.AspNetCore.Authentication;
using SmartWeather.Api.Controllers.Authentication.Dtos.Converters;

namespace SmartWeather.Api.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly Services.Authentication.AuthenticationService _authenticationService;

        public AuthenticationController(UserService userService, Services.Authentication.AuthenticationService authenticationService)
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

            if (String.IsNullOrWhiteSpace(request.Name) ||
                String.IsNullOrWhiteSpace(request.Email) ||
                String.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(ApiResponse<UserRegisterResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
            }

            try
            {
                Tuple<Entities.User.User, string> registerResponse = _authenticationService.Register(request.Name, request.Email, request.Password);
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

            if (String.IsNullOrWhiteSpace(request.Email) ||
                String.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(ApiResponse<UserSigninResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
            }

            try
            {
                Tuple<Entities.User.User, string> signinResponse = _authenticationService.Signin(request.Email, request.Password);
                formattedResponse = UserSigninResponseConverter.ConvertUserToUserSigninResponse(signinResponse.Item1, signinResponse.Item2);
                response = ApiResponse<UserSigninResponse>.Success(formattedResponse);
            }
            catch (Exception ex)
            {
                response = ApiResponse<UserSigninResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
                return BadRequest(response);
            }

            return Ok(response);
        }

    }
}
