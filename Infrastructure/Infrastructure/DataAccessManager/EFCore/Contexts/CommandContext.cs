using Application.Common.CQS.Commands;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccessManager.EFCore.Contexts;
public class CommandContext : DataContext, ICommandContext
{
    public CommandContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }
}

