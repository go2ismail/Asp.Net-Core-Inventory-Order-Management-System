using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.VendorContactManager.Queries;

public record GetVendorContactByVendorIdListDto
{
    public string? Id { get; init; }
    public string? Name { get; set; }
    public string? Number { get; set; }
    public string? JobTitle { get; set; }
    public string? PhoneNumber { get; set; }
    public string? EmailAddress { get; set; }
    public string? Description { get; set; }
    public string? VendorId { get; set; }
    public string? VendorName { get; set; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetVendorContactByVendorIdListProfile : Profile
{
    public GetVendorContactByVendorIdListProfile()
    {
        CreateMap<VendorContact, GetVendorContactByVendorIdListDto>()
            .ForMember(
                dest => dest.VendorName,
                opt => opt.MapFrom(src => src.Vendor != null ? src.Vendor.Name : string.Empty)
            );

    }
}

public class GetVendorContactByVendorIdListResult
{
    public List<GetVendorContactByVendorIdListDto>? Data { get; init; }
}

public class GetVendorContactByVendorIdListRequest : IRequest<GetVendorContactByVendorIdListResult>
{
    public string? VendorId { get; init; }
}


public class GetVendorContactByVendorIdListHandler : IRequestHandler<GetVendorContactByVendorIdListRequest, GetVendorContactByVendorIdListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetVendorContactByVendorIdListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetVendorContactByVendorIdListResult> Handle(GetVendorContactByVendorIdListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .VendorContact
            .AsNoTracking()
            .ApplyIsDeletedFilter(false)
            .Include(x => x.Vendor)
            .Where(x => x.VendorId == request.VendorId)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetVendorContactByVendorIdListDto>>(entities);

        return new GetVendorContactByVendorIdListResult
        {
            Data = dtos
        };
    }


}



