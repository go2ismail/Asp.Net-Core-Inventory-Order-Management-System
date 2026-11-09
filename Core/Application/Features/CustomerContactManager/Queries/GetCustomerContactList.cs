using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CustomerContactManager.Queries;

public record GetCustomerContactListDto
{
    public string? Id { get; init; }
    public string? Name { get; set; }
    public string? Number { get; set; }
    public string? JobTitle { get; set; }
    public string? PhoneNumber { get; set; }
    public string? EmailAddress { get; set; }
    public string? Description { get; set; }
    public string? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetCustomerContactListProfile : Profile
{
    public GetCustomerContactListProfile()
    {
        CreateMap<CustomerContact, GetCustomerContactListDto>()
            .ForMember(
                dest => dest.CustomerName,
                opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Name : string.Empty)
            );

    }
}

public class GetCustomerContactListResult
{
    public List<GetCustomerContactListDto>? Data { get; init; }
}

public class GetCustomerContactListRequest : IRequest<GetCustomerContactListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetCustomerContactListHandler : IRequestHandler<GetCustomerContactListRequest, GetCustomerContactListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetCustomerContactListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetCustomerContactListResult> Handle(GetCustomerContactListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .CustomerContact
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .Include(x => x.Customer)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetCustomerContactListDto>>(entities);

        return new GetCustomerContactListResult
        {
            Data = dtos
        };
    }


}



