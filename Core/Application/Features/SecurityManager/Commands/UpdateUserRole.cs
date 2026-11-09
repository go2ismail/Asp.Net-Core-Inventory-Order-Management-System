using Application.Common.Services.SecurityManager;
using FluentValidation;
using MediatR;

namespace Application.Features.SecurityManager.Commands;



public class UpdateUserRoleResult
{
    public List<string>? Data { get; set; }
}

public class UpdateUserRoleRequest : IRequest<UpdateUserRoleResult>
{
    public string? UserId { get; init; }
    public string? RoleName { get; init; }
    public bool? AccessGranted { get; init; }
}

public class UpdateUserRoleValidator : AbstractValidator<UpdateUserRoleRequest>
{
    public UpdateUserRoleValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.RoleName).NotEmpty();
    }
}

public class UpdateUserRoleHandler : IRequestHandler<UpdateUserRoleRequest, UpdateUserRoleResult>
{
    private readonly ISecurityService _securityService;

    public UpdateUserRoleHandler(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    public async Task<UpdateUserRoleResult> Handle(UpdateUserRoleRequest request, CancellationToken cancellationToken)
    {
        var result = await _securityService.UpdateUserRoleAsync(
            request.UserId ?? "",
            request.RoleName ?? "",
            request.AccessGranted ?? true,
            cancellationToken
            );

        return new UpdateUserRoleResult
        {
            Data = result
        };
    }
}


