namespace EventManagement.Users.Domain.Entities
{
    public sealed class Role
    {
        public string Name { get; private set; }
        private Role(string name)
        {
            Name = name;
        }

        public static readonly Role Admin = new Role("Admin");
        public static readonly Role Member = new Role("Member");
    }
}
