using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.VendorContactManager.Queries;

public record GetVendorContactListDto
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

public class GetVendorContactListProfile : Profile
{
    public GetVendorContactListProfile()
    {
        CreateMap<VendorContact, GetVendorContactListDto>()
            .ForMember(
                dest => dest.VendorName,
                opt => opt.MapFrom(src => src.Vendor != null ? src.Vendor.Name : string.Empty)
            );

    }
}

public class GetVendorContactListResult
{
    public List<GetVendorContactListDto>? Data { get; init; }
}

public class GetVendorContactListRequest : IRequest<GetVendorContactListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetVendorContactListHandler : IRequestHandler<GetVendorContactListRequest, GetVendorContactListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetVendorContactListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetVendorContactListResult> Handle(GetVendorContactListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .VendorContact
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .Include(x => x.Vendor)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetVendorContactListDto>>(entities);

        return new GetVendorContactListResult
        {
            Data = dtos
        };
    }


}



