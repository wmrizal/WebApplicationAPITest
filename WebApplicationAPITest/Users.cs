using System.Runtime.Serialization;

namespace WebApplicationAPITest
{
    public class Users
    {
        public Guid? Id { get; set; } = Guid.Empty;
        public string FullName { get; set; } = null;
        public string Email { get; set; } = null;
        public string? PhoneNumber { get; set; } = null;
        public int? Age { get; set; } = -1;
    }
}
