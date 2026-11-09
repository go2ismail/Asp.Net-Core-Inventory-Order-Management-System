using Application.Features.FileImageManager.Commands;
using Application.Features.FileImageManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using Infrastructure.FileImageManager;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class FileImageController : BaseApiController
{
    public FileImageController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("UploadImage")]
    public async Task<ActionResult<CreateImageResult>> UploadImageAsync(IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Invalid file.");
        }

        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream, cancellationToken);
            var fileData = memoryStream.ToArray();
            var extension = Path.GetExtension(file.FileName).TrimStart('.');

            var command = new CreateImageRequest
            {
                OriginalFileName = file.FileName,
                Extension = extension,
                Data = fileData,
                Size = fileData.Length
            };

            var result = await _sender.Send(command, cancellationToken);

            if (result?.ImageName == null)
            {
                return StatusCode(500, "An error occurred while uploading the image.");
            }

            return Ok(new ApiSuccessResult<CreateImageResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(UploadImageAsync)}",
                Content = result
            });
        }
    }


    [Authorize]
    [HttpGet("GetImage")]
    public async Task<IActionResult> GetImageAsync(
        [FromQuery] string imageName,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(imageName) || Path.GetExtension(imageName) == string.Empty)
        {
            imageName = "noimage.png";
        }

        var request = new GetImageRequest
        {
            ImageName = imageName
        };

        var result = await _sender.Send(request, cancellationToken);

        if (result?.Data == null)
        {
            return NotFound("Image not found.");
        }

        var extension = Path.GetExtension(imageName).ToLower();
        var mimeType = FileImageHelper.GetMimeType(extension);

        return File(result.Data, mimeType);
    }


}


