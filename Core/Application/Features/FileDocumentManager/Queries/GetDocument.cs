using Application.Common.Services.FileDocumentManager;
using FluentValidation;
using MediatR;

namespace Application.Features.FileDocumentManager.Queries;


public class GetDocumentResult
{
    public byte[]? Data { get; init; }
}

public class GetDocumentRequest : IRequest<GetDocumentResult>
{
    public string? DocumentName { get; init; }
}

public class GetDocumentValidator : AbstractValidator<GetDocumentRequest>
{
    public GetDocumentValidator()
    {
        RuleFor(x => x.DocumentName)
            .NotEmpty();
    }
}

public class GetDocumentHandler : IRequestHandler<GetDocumentRequest, GetDocumentResult>
{
    private readonly IFileDocumentService _documentService;

    public GetDocumentHandler(IFileDocumentService documentService)
    {
        _documentService = documentService;
    }

    public async Task<GetDocumentResult> Handle(GetDocumentRequest request, CancellationToken cancellationToken)
    {
        var result = await _documentService.GetFileAsync(request.DocumentName ?? "", cancellationToken);

        return new GetDocumentResult { Data = result };
    }
}


