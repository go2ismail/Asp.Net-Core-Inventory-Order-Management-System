using Application.Common.Services.SecurityManager;
using FluentValidation;
using MediatR;

namespace Application.Features.SecurityManager.Commands;



public class UpdateMyProfileAvatarResult
{
    public string? Data { get; init; }
}

public class UpdateMyProfileAvatarRequest : IRequest<UpdateMyProfileAvatarResult>
{
    public string? UserId { get; init; }
    public string? Avatar { get; init; }
}

public class UpdateMyProfileAvatarValidator : AbstractValidator<UpdateMyProfileAvatarRequest>
{
    public UpdateMyProfileAvatarValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Avatar).NotEmpty();
    }
}

public class UpdateMyProfileAvatarHandler : IRequestHandler<UpdateMyProfileAvatarRequest, UpdateMyProfileAvatarResult>
{
    private readonly ISecurityService _securityService;

    public UpdateMyProfileAvatarHandler(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    public async Task<UpdateMyProfileAvatarResult> Handle(UpdateMyProfileAvatarRequest request, CancellationToken cancellationToken)
    {
        await _securityService.ChangeAvatarAsync(
            request.UserId ?? "",
            request.Avatar ?? "",
            cancellationToken
            );

        return new UpdateMyProfileAvatarResult
        {
            Data = "Update Avatar Success"
        };
    }
}

