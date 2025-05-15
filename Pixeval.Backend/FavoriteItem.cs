using Microsoft.EntityFrameworkCore;
using Pixeval.Backend.Models;

namespace Pixeval.Backend;

[PrimaryKey(nameof(UserId), nameof(IllustrationId))]
public class FavoriteItem
{
    public required long UserId { get; set; }

    public required long IllustrationId { get; set; }

    public required DateTime DateTime { get; set; }

    public UserEntity User { get; set; } = null!;

    public Illustration Illustration { get; set; } = null!;
}
