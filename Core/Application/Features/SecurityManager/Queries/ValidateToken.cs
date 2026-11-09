using FluentValidation;
using MediatR;

namespace Application.Features.SecurityManager.Queries;



public class ValidateTokenResult
{
    public string? Data { get; init; }
}

public class ValidateTokenRequest : IRequest<ValidateTokenResult>
{
}

public class ValidateTokenValidator : AbstractValidator<ValidateTokenRequest>
{
    public ValidateTokenValidator()
    {
    }
}

public class ValidateTokenHandler : IRequestHandler<ValidateTokenRequest, ValidateTokenResult>
{

    public ValidateTokenHandler()
    {
    }

    public async Task<ValidateTokenResult> Handle(ValidateTokenRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        return new ValidateTokenResult
        {
            Data = "Token is valid"
        };
    }
}
