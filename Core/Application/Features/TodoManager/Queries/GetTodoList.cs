using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TodoManager.Queries;

public record GetTodoListDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetTodoListProfile : Profile
{
    public GetTodoListProfile()
    {
        CreateMap<Todo, GetTodoListDto>();
    }
}

public class GetTodoListResult
{
    public List<GetTodoListDto>? Data { get; init; }
}

public class GetTodoListRequest : IRequest<GetTodoListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetTodoListHandler : IRequestHandler<GetTodoListRequest, GetTodoListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetTodoListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetTodoListResult> Handle(GetTodoListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .Todo
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetTodoListDto>>(entities);

        return new GetTodoListResult
        {
            Data = dtos
        };
    }


}



