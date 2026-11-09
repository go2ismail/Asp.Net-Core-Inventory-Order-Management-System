using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CustomerContactManager.Queries;

public record GetCustomerContactByCustomerIdListDto
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

public class GetCustomerContactByCustomerIdListProfile : Profile
{
    public GetCustomerContactByCustomerIdListProfile()
    {
        CreateMap<CustomerContact, GetCustomerContactByCustomerIdListDto>()
            .ForMember(
                dest => dest.CustomerName,
                opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Name : string.Empty)
            );

    }
}

public class GetCustomerContactByCustomerIdListResult
{
    public List<GetCustomerContactByCustomerIdListDto>? Data { get; init; }
}

public class GetCustomerContactByCustomerIdListRequest : IRequest<GetCustomerContactByCustomerIdListResult>
{
    public string? CustomerId { get; init; }
}


public class GetCustomerContactByCustomerIdListHandler : IRequestHandler<GetCustomerContactByCustomerIdListRequest, GetCustomerContactByCustomerIdListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetCustomerContactByCustomerIdListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetCustomerContactByCustomerIdListResult> Handle(GetCustomerContactByCustomerIdListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .CustomerContact
            .AsNoTracking()
            .ApplyIsDeletedFilter(false)
            .Include(x => x.Customer)
            .Where(x => x.CustomerId == request.CustomerId)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetCustomerContactByCustomerIdListDto>>(entities);

        return new GetCustomerContactByCustomerIdListResult
        {
            Data = dtos
        };
    }


}



