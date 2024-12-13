namespace SmartWeather.Api.Controllers.User;

using Microsoft.AspNetCore.Mvc;
using SmartWeather.Api.Contract;
using SmartWeather.Api.Controllers.User.Dtos;
using SmartWeather.Services.Authentication;
using SmartWeather.Services.Users;
using SmartWeather.Api.Controllers.User.Dtos.Converters;
using SmartWeather.Api.Helpers;
using SmartWeather.Api.Controllers.Station.Dtos;
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

        try
        {
            Entities.User.User createdUser = _userService.AddNewUser(request.Name, request.Email, request.Password);
            formattedResponse = UserCreateResponseConverter.ConvertUserToUserCreateResponse(createdUser, _authenticationService.GenerateToken(createdUser));
            response = ApiResponse<UserCreateResponse>.Success(formattedResponse);
            return Ok(response);
        }
        catch (Exception ex) when (ex is EntityCreationException)
        {
            response = ApiResponse<UserCreateResponse>.Failure(BaseResponses.VALIDATION_ERROR, Status.VALIDATION_ERROR);
            return BadRequest(response);
        }
        catch (Exception ex) when (ex is EntitySavingException)
        {
            response = ApiResponse<UserCreateResponse>.Failure(BaseResponses.DATABASE_ERROR, Status.DATABASE_ERROR);
            return BadRequest(response);
        }
        catch
        {
            response = ApiResponse<UserCreateResponse>.Failure();
            return BadRequest(response);
        }
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

        try
        {
            User updatedUser = _userService.UpdateUser(request.Id, request.Password, request.Email, request.Name);
            formattedResponse = UserResponseConverter.ConvertUserToUserResponse(updatedUser);
            response = ApiResponse<UserResponse>.Success(formattedResponse);
            return Ok(response);
        }
        catch (Exception ex) when (ex is EntityCreationException)
        {
            response = ApiResponse<UserResponse>.Failure(BaseResponses.VALIDATION_ERROR, Status.VALIDATION_ERROR);
            return BadRequest(response);
        }
        catch (Exception ex) when (ex is EntitySavingException)
        {
            response = ApiResponse<UserResponse>.Failure(BaseResponses.DATABASE_ERROR, Status.DATABASE_ERROR);
            return BadRequest(response);
        }
        catch
        {
            response = ApiResponse<UserResponse>.Failure();
            return BadRequest(response);
        }
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

        try
        {
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
        catch
        {
            response = ApiResponse<EmptyResponse>.Failure();
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

        try
        {
            User userRetreived = _userService.GetUserById(idUser);
            response = ApiResponse<UserResponse>.Success(UserResponseConverter.ConvertUserToUserResponse(userRetreived));
            return Ok(response);
        }
        catch (Exception ex) when (ex is EntityFetchingException)
        {
            response = ApiResponse<UserResponse>.NoContent();
            return Ok(response);
        }
        catch
        {
            response = ApiResponse<UserResponse>.Failure();
            return BadRequest(response);
        }

    }

    [HttpGet(nameof(GetAll))]
    public ActionResult<ApiResponse<UserListResponse>> GetAll()
    {
        ApiResponse<UserListResponse> response;

        var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

        try
        {
            Role userRole = _authenticationService.GetUserRoleFromToken(token);

            if (!RoleAccess.ADMINISTRATORS.Contains(userRole))
            {
                throw new UnauthorizedAccessException();
            }
        }
        catch (Exception ex) when (ex is SecurityTokenException ||
                                  ex is UnauthorizedAccessException)
        {
            response = ApiResponse<UserListResponse>.Failure();
            return Unauthorized(response);
        }

        try
        {
            IEnumerable<User> usersRetreived = _userService.GetUserList(null);
            response = ApiResponse<UserListResponse>.Success(UserResponseConverter.ConvertUserListToUserListResponse(usersRetreived));
            return Ok(response);
        }
        catch (Exception ex) when (ex is EntityFetchingException)
        {
            response = ApiResponse<UserListResponse>.NoContent();
            return Ok(response);
        }
        catch
        {
            response = ApiResponse<UserListResponse>.Failure();
            return BadRequest(response);
        }

    }
}
