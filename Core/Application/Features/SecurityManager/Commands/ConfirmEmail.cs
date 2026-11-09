using Application.Common.Services.SecurityManager;
using FluentValidation;
using MediatR;

namespace Application.Features.SecurityManager.Commands;


public class ConfirmEmailResult
{
    public string? Email { get; init; }
}

public class ConfirmEmailRequest : IRequest<ConfirmEmailResult>
{
    public required string Email { get; init; }
    public required string Code { get; init; }
}

public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailRequest>
{
    public ConfirmEmailValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Code)
            .NotEmpty();
    }
}


public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailRequest, ConfirmEmailResult>
{
    private readonly ISecurityService _securityService;

    public ConfirmEmailHandler(
        ISecurityService securityService
        )
    {
        _securityService = securityService;
    }

    public async Task<ConfirmEmailResult> Handle(ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        var result = await _securityService.ConfirmEmailAsync(
            request.Email,
            request.Code,
            cancellationToken
            );

        return new ConfirmEmailResult { Email = result };
    }
}

