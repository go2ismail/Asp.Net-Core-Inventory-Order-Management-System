using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TransferInManager.Queries;

public record GetTransferInListDto
{
    public string? Id { get; init; }
    public string? Number { get; init; }
    public DateTime? TransferReceiveDate { get; init; }
    public TransferStatus? Status { get; init; }
    public string? StatusName { get; init; }
    public string? Description { get; init; }
    public string? TransferOutId { get; init; }
    public string? TransferOutNumber { get; init; }
    public string? WarehouseFromName { get; init; }
    public string? WarehouseToName { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetTransferInListProfile : Profile
{
    public GetTransferInListProfile()
    {
        CreateMap<TransferIn, GetTransferInListDto>()
            .ForMember(
                dest => dest.TransferOutNumber,
                opt => opt.MapFrom(src => src.TransferOut != null ? src.TransferOut.Number : string.Empty)
            )
            .ForMember(
                dest => dest.WarehouseFromName,
                opt => opt.MapFrom(src => src.TransferOut!.WarehouseFrom != null ? src.TransferOut.WarehouseFrom.Name : string.Empty)
            )
            .ForMember(
                dest => dest.WarehouseToName,
                opt => opt.MapFrom(src => src.TransferOut!.WarehouseTo != null ? src.TransferOut.WarehouseTo.Name : string.Empty)
            )
            .ForMember(
                dest => dest.StatusName,
                opt => opt.MapFrom(src => src.Status.HasValue ? src.Status.Value.ToFriendlyName() : string.Empty)
            );

    }
}

public class GetTransferInListResult
{
    public List<GetTransferInListDto>? Data { get; init; }
}

public class GetTransferInListRequest : IRequest<GetTransferInListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetTransferInListHandler : IRequestHandler<GetTransferInListRequest, GetTransferInListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetTransferInListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetTransferInListResult> Handle(GetTransferInListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .TransferIn
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .Include(x => x.TransferOut)
                .ThenInclude(x => x.WarehouseFrom)
            .Include(x => x.TransferOut)
                .ThenInclude(x => x.WarehouseTo)
            .AsQueryable();


        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetTransferInListDto>>(entities);

        return new GetTransferInListResult
        {
            Data = dtos
        };
    }


}



