namespace WebApi.DTOs.Directors.PUT
{
    public class DirectorOutputPutDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public DirectorOutputPutDTO(long id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}