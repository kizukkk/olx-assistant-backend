using olx_assistant_domain.Common;

namespace olx_assistant_domain;
public class User : BaseEntity
{
    public required string Login { get; set; }
    public required string PasswordHash { get; set; }
    public required string Email { get; set; }
    public required string TelegramUsername { get; set; }
    public List<Target>? Targets { get; set; }

}
