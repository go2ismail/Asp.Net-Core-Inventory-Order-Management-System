using Application.Common.Services.SecurityManager;
using FluentValidation;
using MediatR;

namespace Application.Features.SecurityManager.Commands;


public class DeleteUserResult
{
    public DeleteUserResultDto? Data { get; set; }
}

public class DeleteUserRequest : IRequest<DeleteUserResult>
{
    public string? UserId { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteUserValidator : AbstractValidator<DeleteUserRequest>
{
    public DeleteUserValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public class DeleteUserHandler : IRequestHandler<DeleteUserRequest, DeleteUserResult>
{
    private readonly ISecurityService _securityService;

    public DeleteUserHandler(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    public async Task<DeleteUserResult> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _securityService.DeleteUserAsync(
            request.UserId ?? "",
            request.DeletedById ?? "",
            cancellationToken
            );

        return new DeleteUserResult
        {
            Data = result
        };
    }
}


