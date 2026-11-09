using Application.Features.SecurityManager.Commands;
using Application.Features.SecurityManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.SecurityManager.Tokens;
using Microsoft.Extensions.Options;

namespace ASPNET.BackEnd.Controllers;


[Route("api/[controller]")]
public class SecurityController : BaseApiController
{
    private readonly IConfiguration _configuration;
    private readonly TokenSettings _tokenSettings;
    public SecurityController(ISender sender, IConfiguration configuration, IOptions<TokenSettings> tokenSettings) : base(sender)
    {
        _configuration = configuration;
        _tokenSettings = tokenSettings.Value;
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<ActionResult<ApiSuccessResult<LoginResult>>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        // Set HttpOnly cookie for server-side page authorization
        if (response?.Data?.AccessToken != null)
        {
            Response.Cookies.Append("accessToken", response.Data.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddMinutes(_tokenSettings.ExpireInMinute)
            });
        }

        return Ok(new ApiSuccessResult<LoginResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(LoginAsync)}",
            Content = response
        });
    }

    [AllowAnonymous]
    [HttpPost("Logout")]
    public async Task<ActionResult<ApiSuccessResult<LogoutResult>>> LogoutAsync(LogoutRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        // Clear HttpOnly cookie on logout
        Response.Cookies.Append("accessToken", "", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddDays(-1)
        });

        return Ok(new ApiSuccessResult<LogoutResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(LogoutAsync)}",
            Content = response
        });
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<ActionResult<ApiSuccessResult<RegisterResult>>> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken
        )
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<RegisterResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(RegisterAsync)}",
            Content = response
        });
    }

    [AllowAnonymous]
    [HttpGet("ConfirmEmail")]
    public async Task<ActionResult<ApiSuccessResult<ConfirmEmailResult>>> ConfirmEmailAsync(
        [FromQuery] string email,
        [FromQuery] string code,
        CancellationToken cancellationToken
        )
    {
        var request = new ConfirmEmailRequest { Email = email, Code = code };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<ConfirmEmailResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(ConfirmEmailAsync)}",
            Content = response
        });
    }


    [AllowAnonymous]
    [HttpPost("ForgotPassword")]
    public async Task<ActionResult<ApiSuccessResult<ForgotPasswordResult>>> ForgotPasswordAsync(
        ForgotPasswordRequest request,
        CancellationToken cancellationToken
        )
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<ForgotPasswordResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(ForgotPasswordAsync)}",
            Content = response
        });
    }

    [AllowAnonymous]
    [HttpGet("ForgotPasswordConfirmation")]
    public async Task<ActionResult<ApiSuccessResult<ForgotPasswordConfirmationResult>>> ForgotPasswordConfirmationAsync(
        [FromQuery] string email,
        [FromQuery] string code,
        [FromQuery] string tempPassword,
        CancellationToken cancellationToken)
    {
        var request = new ForgotPasswordConfirmationRequest { Email = email, TempPassword = tempPassword, Code = code };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<ForgotPasswordConfirmationResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(ForgotPasswordConfirmationAsync)}",
            Content = response
        });
    }

    [AllowAnonymous]
    [HttpPost("RefreshToken")]
    public async Task<ActionResult<ApiSuccessResult<RefreshTokenResult>>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        // Update HttpOnly cookie with new access token
        if (response?.Data?.AccessToken != null)
        {
            Response.Cookies.Append("accessToken", response.Data.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddMinutes(_tokenSettings.ExpireInMinute)
            });
        }

        return Ok(new ApiSuccessResult<RefreshTokenResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(RefreshTokenAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("ValidateToken")]
    public async Task<ActionResult<ApiSuccessResult<ValidateTokenResult>>> ValidateTokenAsync(
        ValidateTokenRequest request,
        CancellationToken cancellationToken
        )
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<ValidateTokenResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(ValidateTokenAsync)}",
            Content = response
        });
    }


    [Authorize]
    [HttpGet("GetMyProfileList")]
    public async Task<ActionResult<ApiSuccessResult<GetMyProfileListResult>>> GetMyProfileListAsync(
        [FromQuery] string userId,
        CancellationToken cancellationToken
        )
    {
        var request = new GetMyProfileListRequest { UserId = userId };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetMyProfileListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetMyProfileListAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateMyProfile")]
    public async Task<ActionResult<ApiSuccessResult<UpdateMyProfileResult>>> UpdateMyProfileAsync(
        UpdateMyProfileRequest request,
        CancellationToken cancellationToken
        )
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateMyProfileResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateMyProfileAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateMyProfilePassword")]
    public async Task<ActionResult<ApiSuccessResult<UpdateMyProfilePasswordResult>>> UpdateMyProfilePasswordAsync(
        UpdateMyProfilePasswordRequest request,
        CancellationToken cancellationToken
        )
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateMyProfilePasswordResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateMyProfilePasswordAsync)}",
            Content = response
        });
    }


    [Authorize]
    [HttpGet("GetRoleList")]
    public async Task<ActionResult<ApiSuccessResult<GetRoleListResult>>> GetRoleListAsync(
        CancellationToken cancellationToken
        )
    {
        var request = new GetRoleListRequest { };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetRoleListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetRoleListAsync)}",
            Content = response
        });
    }


    [Authorize(Roles = "Users")]
    [HttpGet("GetUserList")]
    public async Task<ActionResult<ApiSuccessResult<GetUserListResult>>> GetUserListAsync(
        CancellationToken cancellationToken
        )
    {
        var request = new GetUserListRequest { };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetUserListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetUserListAsync)}",
            Content = response
        });
    }

    [Authorize(Roles = "Users")]
    [HttpPost("CreateUser")]
    public async Task<ActionResult<ApiSuccessResult<CreateUserResult>>> CreateUserAsync(
        CreateUserRequest request,
        CancellationToken cancellationToken
        )
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<CreateUserResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(CreateUserAsync)}",
            Content = response
        });
    }


    [Authorize(Roles = "Users")]
    [HttpPost("UpdateUser")]
    public async Task<ActionResult<ApiSuccessResult<UpdateUserResult>>> UpdateUserAsync(
        UpdateUserRequest request,
        CancellationToken cancellationToken
        )
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateUserResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateUserAsync)}",
            Content = response
        });
    }

    [Authorize(Roles = "Users")]
    [HttpPost("DeleteUser")]
    public async Task<ActionResult<ApiSuccessResult<DeleteUserResult>>> DeleteUserAsync(
    DeleteUserRequest request,
    CancellationToken cancellationToken
    )
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeleteUserResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeleteUserAsync)}",
            Content = response
        });
    }


    [Authorize(Roles = "Users")]
    [HttpPost("UpdatePasswordUser")]
    public async Task<ActionResult<ApiSuccessResult<UpdatePasswordUserResult>>> UpdatePasswordUserAsync(
    UpdatePasswordUserRequest request,
    CancellationToken cancellationToken
    )
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdatePasswordUserResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdatePasswordUserAsync)}",
            Content = response
        });
    }

    [Authorize(Roles = "Users")]
    [HttpPost("GetUserRoles")]
    public async Task<ActionResult<ApiSuccessResult<GetUserRolesResult>>> GetUserRolesAsync(GetUserRolesRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetUserRolesResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetUserRolesAsync)}",
            Content = response
        });
    }

    [Authorize(Roles = "Users")]
    [HttpPost("UpdateUserRole")]
    public async Task<ActionResult<ApiSuccessResult<UpdateUserRoleResult>>> UpdateUserRoleAsync(UpdateUserRoleRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateUserRoleResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateUserRoleAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("UpdateMyProfileAvatar")]
    public async Task<ActionResult<ApiSuccessResult<UpdateMyProfileAvatarResult>>> UpdateMyProfileAvatarAsync(UpdateMyProfileAvatarRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<UpdateMyProfileAvatarResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(UpdateMyProfileAvatarAsync)}",
            Content = response
        });
    }

}

