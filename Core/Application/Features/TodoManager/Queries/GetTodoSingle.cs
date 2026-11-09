using Application.Common.CQS.Queries;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TodoManager.Queries;

public record GetTodoSingleDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
}

public class GetTodoSingleProfile : Profile
{
    public GetTodoSingleProfile()
    {
        CreateMap<Todo, GetTodoSingleDto>();
    }
}

public class GetTodoSingleResult
{
    public GetTodoSingleDto? Data { get; init; }
}

public class GetTodoSingleRequest : IRequest<GetTodoSingleResult>
{
    public string? Id { get; init; }
}

public class GetTodoSingleValidator : AbstractValidator<GetTodoSingleRequest>
{
    public GetTodoSingleValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetTodoSingleHandler : IRequestHandler<GetTodoSingleRequest, GetTodoSingleResult>
{
    private readonly IQueryContext _context;
    private readonly IMapper _mapper;

    public GetTodoSingleHandler(
        IQueryContext context,
        IMapper mapper
        )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetTodoSingleResult> Handle(GetTodoSingleRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .Todo
            .AsNoTracking()
            .AsQueryable();

        query = query
            .Where(x => x.Id == request.Id);

        var entity = await query.SingleOrDefaultAsync(cancellationToken);

        var dto = _mapper.Map<GetTodoSingleDto>(entity);

        return new GetTodoSingleResult
        {
            Data = dto
        };
    }
}