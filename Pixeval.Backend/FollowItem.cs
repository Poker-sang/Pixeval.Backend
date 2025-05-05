using Microsoft.EntityFrameworkCore;
using Pixeval.Backend.Models;

namespace Pixeval.Backend;

[PrimaryKey(nameof(UserId), nameof(FollowedUserId))]
public class FollowItem
{
    public required long UserId { get; set; }

    public required long FollowedUserId { get; set; }

    public User User { get; set; } = null!;

    public User FollowedUser { get; set; } = null!;
}
