using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TransferOutManager.Queries;

public record GetTransferOutListDto
{
    public string? Id { get; init; }
    public string? Number { get; init; }
    public DateTime? TransferReleaseDate { get; init; }
    public TransferStatus? Status { get; init; }
    public string? StatusName { get; init; }
    public string? Description { get; init; }
    public string? WarehouseFromId { get; init; }
    public string? WarehouseFromName { get; init; }
    public string? WarehouseToId { get; init; }
    public string? WarehouseToName { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetTransferOutListProfile : Profile
{
    public GetTransferOutListProfile()
    {
        CreateMap<TransferOut, GetTransferOutListDto>()
            .ForMember(
                dest => dest.WarehouseFromName,
                opt => opt.MapFrom(src => src.WarehouseFrom != null ? src.WarehouseFrom.Name : string.Empty)
            )
            .ForMember(
                dest => dest.WarehouseToName,
                opt => opt.MapFrom(src => src.WarehouseTo != null ? src.WarehouseTo.Name : string.Empty)
            )
            .ForMember(
                dest => dest.StatusName,
                opt => opt.MapFrom(src => src.Status.HasValue ? src.Status.Value.ToFriendlyName() : string.Empty)
            );

    }
}

public class GetTransferOutListResult
{
    public List<GetTransferOutListDto>? Data { get; init; }
}

public class GetTransferOutListRequest : IRequest<GetTransferOutListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetTransferOutListHandler : IRequestHandler<GetTransferOutListRequest, GetTransferOutListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetTransferOutListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetTransferOutListResult> Handle(GetTransferOutListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .TransferOut
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .Include(x => x.WarehouseFrom)
            .Include(x => x.WarehouseTo)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetTransferOutListDto>>(entities);

        return new GetTransferOutListResult
        {
            Data = dtos
        };
    }


}



