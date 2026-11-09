using Application.Common.CQS.Queries;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CompanyManager.Queries;

public record GetCompanySingleDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? Currency { get; init; }
    public string? Street { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? ZipCode { get; init; }
    public string? Country { get; init; }
    public string? PhoneNumber { get; init; }
    public string? FaxNumber { get; init; }
    public string? EmailAddress { get; init; }
    public string? Website { get; init; }
}

public class GetCompanySingleProfile : Profile
{
    public GetCompanySingleProfile()
    {
        CreateMap<Company, GetCompanySingleDto>();
    }
}

public class GetCompanySingleResult
{
    public GetCompanySingleDto? Data { get; init; }
}

public class GetCompanySingleRequest : IRequest<GetCompanySingleResult>
{
    public string? Id { get; init; }
}

public class GetCompanySingleValidator : AbstractValidator<GetCompanySingleRequest>
{
    public GetCompanySingleValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetCompanySingleHandler : IRequestHandler<GetCompanySingleRequest, GetCompanySingleResult>
{
    private readonly IQueryContext _context;
    private readonly IMapper _mapper;

    public GetCompanySingleHandler(
        IQueryContext context,
        IMapper mapper
        )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetCompanySingleResult> Handle(GetCompanySingleRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .Company
            .AsNoTracking()
            .AsQueryable();

        query = query
            .Where(x => x.Id == request.Id);

        var entity = await query.SingleOrDefaultAsync(cancellationToken);

        var dto = _mapper.Map<GetCompanySingleDto>(entity);

        return new GetCompanySingleResult
        {
            Data = dto
        };
    }
}