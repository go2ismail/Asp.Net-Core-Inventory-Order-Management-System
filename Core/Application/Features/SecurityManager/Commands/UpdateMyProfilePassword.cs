using Application.Common.Services.SecurityManager;
using FluentValidation;
using MediatR;

namespace Application.Features.SecurityManager.Commands;



public class UpdateMyProfilePasswordResult
{
    public string? Data { get; init; }
}

public class UpdateMyProfilePasswordRequest : IRequest<UpdateMyProfilePasswordResult>
{
    public string? UserId { get; init; }
    public string? OldPassword { get; init; }
    public string? NewPassword { get; init; }
    public string? ConfirmNewPassword { get; init; }
}

public class UpdateMyProfilePasswordValidator : AbstractValidator<UpdateMyProfilePasswordRequest>
{
    public UpdateMyProfilePasswordValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.OldPassword).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty();
        RuleFor(x => x.ConfirmNewPassword).NotEmpty();
    }
}

public class UpdateMyProfilePasswordHandler : IRequestHandler<UpdateMyProfilePasswordRequest, UpdateMyProfilePasswordResult>
{
    private readonly ISecurityService _securityService;

    public UpdateMyProfilePasswordHandler(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    public async Task<UpdateMyProfilePasswordResult> Handle(UpdateMyProfilePasswordRequest request, CancellationToken cancellationToken)
    {
        await _securityService.ChangePasswordAsync(
            request.UserId ?? "",
            request.OldPassword ?? "",
            request.NewPassword ?? "",
            request.ConfirmNewPassword ?? "",
            cancellationToken
            );

        return new UpdateMyProfilePasswordResult
        {
            Data = "Update Password Success"
        };
    }
}

