namespace Domain.Identity
{
    public interface IDomainIdentity
    {
        Did NewId();
    }
}