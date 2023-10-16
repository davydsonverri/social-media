namespace CQRS.Core.Identity
{
    public static class IdGenerator
    {
        public static Guid NewId()
        {
            return Guid.NewGuid();
        }
    }
}
