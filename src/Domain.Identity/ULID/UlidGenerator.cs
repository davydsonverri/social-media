namespace Domain.Identity.ULID
{
    public class UlidGenerator : IDomainIdentity
    {
        public Did NewId()
        {
            return Did.NewDid();
        }
    }
}
