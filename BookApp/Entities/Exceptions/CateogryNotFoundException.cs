namespace Entities.Exceptions
{
    public sealed class CateogryNotFoundException : NotFoundException
    {
        public CateogryNotFoundException(int CateogryId)
        : base($"The Cateogry with id: {CateogryId} doesn't exist in the database.") 
        {
        }
    }
}
