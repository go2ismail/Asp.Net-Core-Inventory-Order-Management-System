using Application.Common.CQS.Queries;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TodoItemManager.Queries;

public record GetTodoItemSingleDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? TodoName { get; init; }
}

public class GetTodoItemSingleProfile : Profile
{
    public GetTodoItemSingleProfile()
    {
        CreateMap<TodoItem, GetTodoItemSingleDto>()
            .ForMember(
                dest => dest.TodoName,
                opt => opt.MapFrom(src => src.Todo != null ? src.Todo.Name : string.Empty)
            );
    }
}

public class GetTodoItemSingleResult
{
    public GetTodoItemSingleDto? Data { get; init; }
}

public class GetTodoItemSingleRequest : IRequest<GetTodoItemSingleResult>
{
    public string? Id { get; init; }
}

public class GetTodoItemSingleValidator : AbstractValidator<GetTodoItemSingleRequest>
{
    public GetTodoItemSingleValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetTodoItemSingleHandler : IRequestHandler<GetTodoItemSingleRequest, GetTodoItemSingleResult>
{
    private readonly IQueryContext _context;
    private readonly IMapper _mapper;

    public GetTodoItemSingleHandler(
        IQueryContext context,
        IMapper mapper
        )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetTodoItemSingleResult> Handle(GetTodoItemSingleRequest request, CancellationToken cancellationToken)
    {
        var query = _context
            .TodoItem
            .AsNoTracking()
            .Include(x => x.Todo)
            .AsQueryable();

        query = query
            .Where(x => x.Id == request.Id);

        var entity = await query.SingleOrDefaultAsync(cancellationToken);

        var dto = _mapper.Map<GetTodoItemSingleDto>(entity);

        return new GetTodoItemSingleResult
        {
            Data = dto
        };
    }
}