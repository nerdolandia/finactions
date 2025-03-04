using FinActions.Domain.Identity;

namespace FinActions.Domain.Base;

public abstract class UserBaseEntity : BaseEntity
{
    public AppUser User { get; set; }
    public Guid UserId { get; set; }
}
