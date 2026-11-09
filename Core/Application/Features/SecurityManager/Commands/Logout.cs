using Application.Common.Services.SecurityManager;
using FluentValidation;
using MediatR;

namespace Application.Features.SecurityManager.Commands;

public class LogoutResult
{
    public LogoutResultDto? Data { get; set; }
}

public class LogoutRequest : IRequest<LogoutResult>
{
    public string? UserId { get; init; }
}

public class LogoutValidator : AbstractValidator<LogoutRequest>
{
    public LogoutValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public class LogoutHandler : IRequestHandler<LogoutRequest, LogoutResult>
{
    private readonly ISecurityService _securityService;

    public LogoutHandler(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    public async Task<LogoutResult> Handle(LogoutRequest request, CancellationToken cancellationToken)
    {
        var result = await _securityService.LogoutAsync(
            request.UserId ?? "",
            cancellationToken
            );

        return new LogoutResult
        {
            Data = result
        };
    }
}