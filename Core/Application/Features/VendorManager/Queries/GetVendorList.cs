using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.VendorManager.Queries;

public record GetVendorListDto
{
    public string? Id { get; init; }
    public string? Name { get; set; }
    public string? Number { get; set; }
    public string? Description { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FaxNumber { get; set; }
    public string? EmailAddress { get; set; }
    public string? Website { get; set; }
    public string? WhatsApp { get; set; }
    public string? LinkedIn { get; set; }
    public string? Facebook { get; set; }
    public string? Instagram { get; set; }
    public string? TwitterX { get; set; }
    public string? TikTok { get; set; }
    public string? VendorGroupId { get; set; }
    public string? VendorGroupName { get; set; }
    public string? VendorCategoryId { get; set; }
    public string? VendorCategoryName { get; set; }
    public string? CreatedById { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetVendorListProfile : Profile
{
    public GetVendorListProfile()
    {
        CreateMap<Vendor, GetVendorListDto>()
            .ForMember(
                dest => dest.VendorGroupName,
                opt => opt.MapFrom(src => src.VendorGroup != null ? src.VendorGroup.Name : string.Empty)
            )
            .ForMember(
                dest => dest.VendorCategoryName,
                opt => opt.MapFrom(src => src.VendorCategory != null ? src.VendorCategory.Name : string.Empty)
            );

    }
}

public class GetVendorListResult
{
    public List<GetVendorListDto>? Data { get; init; }
}

public class GetVendorListRequest : IRequest<GetVendorListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetVendorListHandler : IRequestHandler<GetVendorListRequest, GetVendorListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetVendorListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetVendorListResult> Handle(GetVendorListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .Vendor
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .Include(x => x.VendorGroup)
            .Include(x => x.VendorCategory)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetVendorListDto>>(entities);

        return new GetVendorListResult
        {
            Data = dtos
        };
    }


}



