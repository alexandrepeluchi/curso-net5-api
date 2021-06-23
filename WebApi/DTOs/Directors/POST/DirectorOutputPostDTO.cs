using WebApi.Models;

namespace WebApi.DTOs.Directors.POST
{
    public class DirectorOutputPostDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public DirectorOutputPostDTO(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public static DirectorOutputPostDTO ToDirectorDTOMap(Director director)
        {
            return new DirectorOutputPostDTO(director.Id, director.Name);
        }
    }
}