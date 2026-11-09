using Application.Common.Services.SecurityManager;
using FluentValidation;
using MediatR;

namespace Application.Features.SecurityManager.Commands;

public class RegisterResult
{
    public RegisterResultDto? Data { get; set; }
}

public class RegisterRequest : IRequest<RegisterResult>
{
    public string? Email { get; init; }
    public string? Password { get; init; }
    public string? ConfirmPassword { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? CompanyName { get; init; }
}

public class RegisterValidator : AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.ConfirmPassword).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}

public class RegisterHandler : IRequestHandler<RegisterRequest, RegisterResult>
{
    private readonly ISecurityService _securityService;

    public RegisterHandler(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    public async Task<RegisterResult> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _securityService.RegisterAsync(
            request.Email ?? "",
            request.Password ?? "",
            request.ConfirmPassword ?? "",
            request.FirstName ?? "",
            request.LastName ?? "",
            request.CompanyName ?? "",
            cancellationToken
            );

        return new RegisterResult
        {
            Data = result
        };
    }
}