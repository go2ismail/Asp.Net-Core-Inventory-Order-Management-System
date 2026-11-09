using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Common.Base;

[ApiController]
[Route("api/[controller]/[action]")]
public abstract class BaseApiController : ControllerBase
{
    protected readonly ISender _sender;

    protected BaseApiController(ISender sender)
    {
        _sender = sender;
    }
}
