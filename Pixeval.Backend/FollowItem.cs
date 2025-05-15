using Microsoft.EntityFrameworkCore;
using Pixeval.Backend.Models;

namespace Pixeval.Backend;

[PrimaryKey(nameof(UserId), nameof(FollowedUserId))]
public class FollowItem
{
    public required long UserId { get; set; }

    public required long FollowedUserId { get; set; }

    public required DateTime DateTime { get; set; }

    public UserEntity User { get; set; } = null!;

    public UserEntity FollowedUser { get; set; } = null!;
}
