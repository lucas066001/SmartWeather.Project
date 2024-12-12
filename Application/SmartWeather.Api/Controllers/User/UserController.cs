using Microsoft.AspNetCore.Mvc;
using SmartWeather.Api.Contract;
using SmartWeather.Api.Controllers.User.Dtos;
using SmartWeather.Services.Authentication;
using SmartWeather.Services.Users;
using SmartWeather.Api.Controllers.User.Dtos.Converters;
using Microsoft.AspNetCore.Authorization;
using SmartWeather.Entities.User;

namespace SmartWeather.Api.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly AuthenticationService _authenticationService;

        public UserController(UserService userService, AuthenticationService authenticationService)
        {
            _userService = userService;
            _authenticationService = authenticationService;
        }

        [HttpPost(nameof(Create))]
        public ActionResult<ApiResponse<UserCreateResponse>> Create(UserCreateRequest request)
        {
            ApiResponse<UserCreateResponse> response;
            UserCreateResponse formattedResponse;

            if (String.IsNullOrWhiteSpace(request.Name) ||
                String.IsNullOrWhiteSpace(request.Email)|| 
                String.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(ApiResponse<UserCreateResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
            }

            try
            {
                Entities.User.User createdUser = _userService.AddNewUser(request.Name, request.Email, request.Password);
                formattedResponse = UserCreateResponseConverter.ConvertUserToUserCreateResponse(createdUser, _authenticationService.GenerateToken(createdUser));
                response = ApiResponse<UserCreateResponse>.Success(formattedResponse);
            }
            catch (Exception ex)
            {
                response = ApiResponse<UserCreateResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut(nameof(Update))]
        public ActionResult<ApiResponse<UserResponse>> Update(UserUpdateRequest request)
        {
            ApiResponse<UserResponse> response;
            UserResponse formattedResponse;

            if (!(request.Id > 0) ||
                String.IsNullOrWhiteSpace(request.Name) ||
                String.IsNullOrWhiteSpace(request.Email) ||
                String.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(ApiResponse<UserCreateResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
            }

            try
            {
                Entities.User.User updatedUser = _userService.UpdateUser(request.Id, request.Password, request.Email, request.Name);
                formattedResponse = UserResponseConverter.ConvertUserToUserResponse(updatedUser);
                response = ApiResponse<UserResponse>.Success(formattedResponse);
            }
            catch (Exception ex)
            {
                response = ApiResponse<UserResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete(nameof(Delete))]
        public ActionResult<ApiResponse<EmptyResponse>> Delete(int idUser)
        {
            // Later will need to restrict this to admin or current token user privilege
            ApiResponse<EmptyResponse> response;

            if (!(idUser > 0))
            {
                return BadRequest(ApiResponse<EmptyResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
            }

            try
            {
                if (_userService.DeleteUser(idUser))
                {
                    response = ApiResponse<EmptyResponse>.Success(null);
                }
                else
                {
                    response = ApiResponse<EmptyResponse>.Failure(BaseResponses.INTERNAL_ERROR);
                }

            }
            catch (Exception ex)
            {
                response = ApiResponse<EmptyResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet(nameof(GetById))]
        public ActionResult<ApiResponse<UserResponse>> GetById(int idUser)
        {
            // Later will need to restrict this to admin or current token user privilege
            ApiResponse<UserResponse> response;

            if (!(idUser > 0))
            {
                return BadRequest(ApiResponse<UserResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
            }

            try
            {
                SmartWeather.Entities.User.User userRetreived = _userService.GetUserById(idUser);
                if (userRetreived != null)
                {
                    response = ApiResponse<UserResponse>.Success(UserResponseConverter.ConvertUserToUserResponse(userRetreived));
                }
                else
                {
                    response = ApiResponse<UserResponse>.Failure(BaseResponses.INTERNAL_ERROR);
                }

            }
            catch (Exception ex)
            {
                response = ApiResponse<UserResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet(nameof(GetAll))]
        public ActionResult<ApiResponse<UserListResponse>> GetAll()
        {
            // Later will need to restrict this to admin or current token user privilege
            ApiResponse<UserListResponse> response;

            try
            {
                IEnumerable<SmartWeather.Entities.User.User> usersRetreived = _userService.GetUserList(null);
                response = ApiResponse<UserListResponse>.Success(UserResponseConverter.ConvertUserListToUserListResponse(usersRetreived));
            }
            catch (Exception ex)
            {
                response = ApiResponse<UserListResponse>.Failure(String.Format(BaseResponses.FORMAT_ERROR, ex.Message));
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
