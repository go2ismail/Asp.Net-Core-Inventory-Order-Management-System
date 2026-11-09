using Application.Common.Services.FileDocumentManager;
using FluentValidation;
using MediatR;

namespace Application.Features.FileDocumentManager.Commands;

public class CreateDocumentResult
{
    public string? DocumentName { get; init; }
}

public class CreateDocumentRequest : IRequest<CreateDocumentResult>
{
    public string? OriginalFileName { get; init; }
    public string? Extension { get; init; }
    public byte[]? Data { get; init; }
    public long? Size { get; init; }
    public string? CreatedById { get; init; }
    public string? Description { get; init; }
}

public class CreateDocumentValidator : AbstractValidator<CreateDocumentRequest>
{
    public CreateDocumentValidator()
    {
        RuleFor(x => x.OriginalFileName)
            .NotEmpty();

        RuleFor(x => x.Extension)
            .NotEmpty();

        RuleFor(x => x.Data)
            .NotEmpty();

        RuleFor(x => x.Size)
            .NotEmpty();
    }
}

public class CreateDocumentHandler : IRequestHandler<CreateDocumentRequest, CreateDocumentResult>
{
    private readonly IFileDocumentService _uploadDocument;

    public CreateDocumentHandler(IFileDocumentService uploadDocument)
    {
        _uploadDocument = uploadDocument;
    }

    public async Task<CreateDocumentResult> Handle(CreateDocumentRequest request, CancellationToken cancellationToken)
    {
        var result = await _uploadDocument.UploadAsync(
            request.OriginalFileName,
            request.Extension,
            request.Data,
            request.Size,
            request.Description,
            request.CreatedById,
            cancellationToken);

        return new CreateDocumentResult { DocumentName = result };
    }
}

