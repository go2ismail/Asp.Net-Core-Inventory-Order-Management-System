using Application.Features.FileDocumentManager.Commands;
using Application.Features.FileDocumentManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using Infrastructure.FileDocumentManager;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class FileDocumentController : BaseApiController
{
    public FileDocumentController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("UploadDocument")]
    public async Task<ActionResult<CreateDocumentResult>> UploadDocumentAsync(IFormFile file, CancellationToken cancellationToken)
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

            var command = new CreateDocumentRequest
            {
                OriginalFileName = file.FileName,
                Extension = extension,
                Data = fileData,
                Size = fileData.Length
            };

            var result = await _sender.Send(command, cancellationToken);

            if (result?.DocumentName == null)
            {
                return StatusCode(500, "An error occurred while uploading the document.");
            }

            return Ok(new ApiSuccessResult<CreateDocumentResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(UploadDocumentAsync)}",
                Content = result
            });
        }
    }


    [Authorize]
    [HttpGet("GetDocument")]
    public async Task<IActionResult> GetDocumentAsync(
        [FromQuery] string documentName,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(documentName) || Path.GetExtension(documentName) == string.Empty)
        {
            documentName = "nodocument.txt";
        }

        var request = new GetDocumentRequest
        {
            DocumentName = documentName
        };

        var result = await _sender.Send(request, cancellationToken);

        if (result?.Data == null)
        {
            return NotFound("Document not found.");
        }

        var extension = Path.GetExtension(documentName).ToLower();
        var mimeType = FileDocumentHelper.GetMimeType(extension);

        return File(result.Data, mimeType);
    }


}


