using Application.Common.Services.SecurityManager;
using FluentValidation;
using MediatR;

namespace Application.Features.SecurityManager.Queries;



public class GetRoleListResult
{
    public List<GetRoleListResultDto>? Data { get; init; }
}

public class GetRoleListRequest : IRequest<GetRoleListResult>
{
}

public class GetRoleListValidator : AbstractValidator<GetRoleListRequest>
{
    public GetRoleListValidator()
    {
    }
}

public class GetRoleListHandler : IRequestHandler<GetRoleListRequest, GetRoleListResult>
{
    private readonly ISecurityService _securityService;

    public GetRoleListHandler(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    public async Task<GetRoleListResult> Handle(GetRoleListRequest request, CancellationToken cancellationToken)
    {
        var result = await _securityService.GetRoleListAsync(
            cancellationToken
            );

        return new GetRoleListResult
        {
            Data = result
        };
    }
}

