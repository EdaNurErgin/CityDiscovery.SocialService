using System;

namespace IdentityService.Shared.MessageBus.Identity
{
    public class UserLoggedInEvent
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string? DeviceId { get; set; }
        public DateTime LoggedInAtUtc { get; set; }

        public UserLoggedInEvent()
        {
        }

        public UserLoggedInEvent(Guid userId, string userName, string email, string role, string? deviceId, DateTime loggedInAtUtc)
        {
            UserId = userId;
            UserName = userName;
            Email = email;
            Role = role;
            DeviceId = deviceId;
            LoggedInAtUtc = loggedInAtUtc;
        }
    }
}

