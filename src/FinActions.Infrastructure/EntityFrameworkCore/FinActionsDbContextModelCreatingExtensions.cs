using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace FinActions.Infrastructure.EntityFrameworkCore;

public static class FinActionsDbContextModelCreatingExtensions
{
    public static void ConfigureFinActions(this ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
