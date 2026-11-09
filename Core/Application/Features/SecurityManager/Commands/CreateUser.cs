using Application.Common.Services.SecurityManager;
using FluentValidation;
using MediatR;

namespace Application.Features.SecurityManager.Commands;


public class CreateUserResult
{
    public CreateUserResultDto? Data { get; set; }
}

public class CreateUserRequest : IRequest<CreateUserResult>
{
    public string? Email { get; init; }
    public string? Password { get; init; }
    public string? ConfirmPassword { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public bool? EmailConfirmed { get; init; }
    public bool? IsBlocked { get; init; }
    public bool? IsDeleted { get; init; }
    public string? CreatedById { get; init; }
}

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.ConfirmPassword).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}

public class CreateUserHandler : IRequestHandler<CreateUserRequest, CreateUserResult>
{
    private readonly ISecurityService _securityService;

    public CreateUserHandler(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    public async Task<CreateUserResult> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _securityService.CreateUserAsync(
            request.Email ?? "",
            request.Password ?? "",
            request.ConfirmPassword ?? "",
            request.FirstName ?? "",
            request.LastName ?? "",
            request.EmailConfirmed ?? true,
            request.IsBlocked ?? false,
            request.IsDeleted ?? false,
            request.CreatedById ?? "",
            cancellationToken
            );

        return new CreateUserResult
        {
            Data = result
        };
    }
}
