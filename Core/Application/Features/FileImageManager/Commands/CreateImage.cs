using Application.Common.Services.FileImageManager;
using FluentValidation;
using MediatR;

namespace Application.Features.FileImageManager.Commands;

public class CreateImageResult
{
    public string? ImageName { get; init; }
}

public class CreateImageRequest : IRequest<CreateImageResult>
{
    public string? OriginalFileName { get; init; }
    public string? Extension { get; init; }
    public byte[]? Data { get; init; }
    public long? Size { get; init; }
    public string? CreatedById { get; init; }
    public string? Description { get; init; }
}

public class CreateImageValidator : AbstractValidator<CreateImageRequest>
{
    public CreateImageValidator()
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

public class CreateImageHandler : IRequestHandler<CreateImageRequest, CreateImageResult>
{
    private readonly IFileImageService _uploadImage;

    public CreateImageHandler(IFileImageService uploadImage)
    {
        _uploadImage = uploadImage;
    }

    public async Task<CreateImageResult> Handle(CreateImageRequest request, CancellationToken cancellationToken)
    {
        var result = await _uploadImage.UploadAsync(
            request.OriginalFileName,
            request.Extension,
            request.Data,
            request.Size,
            request.Description,
            request.CreatedById,
            cancellationToken);

        return new CreateImageResult { ImageName = result };
    }
}

