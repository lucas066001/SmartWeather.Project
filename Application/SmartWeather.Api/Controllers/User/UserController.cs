namespace SmartWeather.Api.Controllers.User;

using Microsoft.AspNetCore.Mvc;
using SmartWeather.Api.Contract;
using SmartWeather.Api.Controllers.User.Dtos;
using SmartWeather.Services.Authentication;
using SmartWeather.Services.Users;
using SmartWeather.Api.Controllers.User.Dtos.Converters;
using SmartWeather.Api.Helpers;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Entities.User;
using Microsoft.IdentityModel.Tokens;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly AuthenticationService _authenticationService;
    private readonly AccessManagerHelper _accessManagerHelper;

    public UserController(UserService userService, AuthenticationService authenticationService, AccessManagerHelper accessManagerHelper)
    {
        _userService = userService;
        _authenticationService = authenticationService;
        _accessManagerHelper = accessManagerHelper;
    }

    [HttpPost(nameof(Create))]
    public ActionResult<ApiResponse<UserCreateResponse>> Create(UserCreateRequest request)
    {
        ApiResponse<UserCreateResponse> response;
        UserCreateResponse formattedResponse;

        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Email)|| 
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(ApiResponse<UserCreateResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        var createdUser = _userService.AddNewUser(request.Name, request.Email, request.Password);

        if (createdUser.IsFailure)
        {
            response = ApiResponse<UserCreateResponse>.Failure(createdUser.ErrorMessage);
            return BadRequest(response);
        }

        var generatedToken = _authenticationService.GenerateToken(createdUser.Value);

        if(generatedToken.IsFailure)
        {
            response = ApiResponse<UserCreateResponse>.Failure(generatedToken.ErrorMessage);
            return BadRequest(response);
        }

        formattedResponse = UserCreateResponseConverter.ConvertUserToUserCreateResponse(createdUser.Value, generatedToken.Value);
        response = ApiResponse<UserCreateResponse>.Success(formattedResponse);
        
        return Ok(response);
    }

    [HttpPut(nameof(Update))]
    public ActionResult<ApiResponse<UserResponse>> Update(UserUpdateRequest request)
    {
        ApiResponse<UserResponse> response;
        UserResponse formattedResponse;

        if (!(request.Id > 0) ||
            string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(ApiResponse<UserResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        if (!_accessManagerHelper.ValidateUserAccess<User>(this, request.Id, RoleAccess.ADMINISTRATORS))
        {
            response = ApiResponse<UserResponse>.Failure();
            return Unauthorized(response);
        }

        var updatedUser = _userService.UpdateUser(request.Id, request.Password, request.Email, request.Name);

        if (updatedUser.IsFailure)
        {
            response = ApiResponse<UserResponse>.Failure(updatedUser.ErrorMessage);
            return BadRequest(response);
        }

        formattedResponse = UserResponseConverter.ConvertUserToUserResponse(updatedUser.Value);
        response = ApiResponse<UserResponse>.Success(formattedResponse);
        
        return Ok(response);
    }

    [HttpDelete(nameof(Delete))]
    public ActionResult<ApiResponse<EmptyResponse>> Delete(int idUser)
    {
        ApiResponse<EmptyResponse> response;

        if (!(idUser > 0))
        {
            return BadRequest(ApiResponse<EmptyResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        if (!_accessManagerHelper.ValidateUserAccess<User>(this, idUser, RoleAccess.ADMINISTRATORS))
        {
            response = ApiResponse<EmptyResponse>.Failure();
            return Unauthorized(response);
        }

        if (_userService.DeleteUser(idUser))
        {
            response = ApiResponse<EmptyResponse>.Success(new EmptyResponse());
            return Ok(response);
        }
        else
        {
            response = ApiResponse<EmptyResponse>.Failure(BaseResponses.DATABASE_ERROR);
            return BadRequest(response);
        }
    }

    [HttpGet(nameof(GetById))]
    public ActionResult<ApiResponse<UserResponse>> GetById(int idUser)
    {
        ApiResponse<UserResponse> response;

        if (!(idUser > 0))
        {
            return BadRequest(ApiResponse<UserResponse>.Failure(BaseResponses.ARGUMENT_ERROR));
        }

        if (!_accessManagerHelper.ValidateUserAccess<User>(this, idUser, RoleAccess.GLOBAL_READING_ACCESS))
        {
            response = ApiResponse<UserResponse>.Failure();
            return Unauthorized(response);
        }

        var userRetreived = _userService.GetUserById(idUser);

        if (userRetreived.IsFailure)
        {
            response = ApiResponse<UserResponse>.Failure(userRetreived.ErrorMessage);
            return BadRequest(response);
        }

        response = ApiResponse<UserResponse>.Success(UserResponseConverter.ConvertUserToUserResponse(userRetreived.Value));
        
        return Ok(response);
    }

    [HttpGet(nameof(GetAll))]
    public ActionResult<ApiResponse<UserListResponse>> GetAll()
    {
        ApiResponse<UserListResponse> response;

        var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

        var userRole = _authenticationService.GetUserRoleFromToken(token);

        if(userRole == Role.Unauthorized || !RoleAccess.ADMINISTRATORS.Contains(userRole))
        {
            response = ApiResponse<UserListResponse>.Failure();
            return Unauthorized(response);
        }

        var usersRetreived = _userService.GetUserList(null);

        if (usersRetreived.IsFailure)
        {
            response = ApiResponse<UserListResponse>.Failure(usersRetreived.ErrorMessage);
            return BadRequest(response);
        }

        response = ApiResponse<UserListResponse>.Success(UserResponseConverter.ConvertUserListToUserListResponse(usersRetreived.Value));
        
        return Ok(response);
    }
}
