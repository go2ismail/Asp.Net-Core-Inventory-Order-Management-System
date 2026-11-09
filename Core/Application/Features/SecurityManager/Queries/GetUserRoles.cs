using Application.Common.Services.SecurityManager;
using FluentValidation;
using MediatR;

namespace Application.Features.SecurityManager.Queries;




public class GetUserRolesResult
{
    public List<string>? Data { get; init; }
}

public class GetUserRolesRequest : IRequest<GetUserRolesResult>
{
    public string? UserId { get; init; }
}

public class GetUserRolesValidator : AbstractValidator<GetUserRolesRequest>
{
    public GetUserRolesValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public class GetUserRolesHandler : IRequestHandler<GetUserRolesRequest, GetUserRolesResult>
{
    private readonly ISecurityService _securityService;

    public GetUserRolesHandler(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    public async Task<GetUserRolesResult> Handle(GetUserRolesRequest request, CancellationToken cancellationToken)
    {
        var result = await _securityService.GetUserRolesAsync(
            request.UserId ?? "",
            cancellationToken
            );

        return new GetUserRolesResult
        {
            Data = result
        };
    }
}



