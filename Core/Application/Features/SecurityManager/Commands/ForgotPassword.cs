using Application.Common.Services.SecurityManager;
using FluentValidation;
using MediatR;

namespace Application.Features.SecurityManager.Commands;


public class ForgotPasswordResult
{
    public string? Data { get; init; }
}

public class ForgotPasswordRequest : IRequest<ForgotPasswordResult>
{
    public required string Email { get; init; }
}

public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}


public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordRequest, ForgotPasswordResult>
{
    private readonly ISecurityService _securityService;

    public ForgotPasswordHandler(
        ISecurityService securityService
        )
    {
        _securityService = securityService;
    }

    public async Task<ForgotPasswordResult> Handle(ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        var result = await _securityService.ForgotPasswordAsync(
            request.Email,
            cancellationToken
            );

        return new ForgotPasswordResult
        {
            Data = result
        };
    }
}
