namespace WebApi.DTOs.Director
{
    public class DirectorOutputDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public DirectorOutputDTO(long id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}