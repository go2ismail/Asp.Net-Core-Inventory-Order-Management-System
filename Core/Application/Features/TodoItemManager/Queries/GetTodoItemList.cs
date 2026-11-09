using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TodoItemManager.Queries;

public record GetTodoItemListDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? TodoId { get; init; }
    public string? TodoName { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetTodoItemListProfile : Profile
{
    public GetTodoItemListProfile()
    {
        CreateMap<TodoItem, GetTodoItemListDto>()
            .ForMember(
                dest => dest.TodoName,
                opt => opt.MapFrom(src => src.Todo != null ? src.Todo.Name : string.Empty)
            );

    }
}

public class GetTodoItemListResult
{
    public List<GetTodoItemListDto>? Data { get; init; }
}

public class GetTodoItemListRequest : IRequest<GetTodoItemListResult>
{
    public bool IsDeleted { get; init; } = false;
}


public class GetTodoItemListHandler : IRequestHandler<GetTodoItemListRequest, GetTodoItemListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetTodoItemListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetTodoItemListResult> Handle(GetTodoItemListRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .TodoItem
            .AsNoTracking()
            .ApplyIsDeletedFilter(request.IsDeleted)
            .Include(x => x.Todo)
            .AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GetTodoItemListDto>>(entities);

        return new GetTodoItemListResult
        {
            Data = dtos
        };
    }


}



