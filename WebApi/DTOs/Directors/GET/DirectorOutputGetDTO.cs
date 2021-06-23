using WebApi.Models;

namespace WebApi.DTOs.Directors.GET
{
    public class DirectorOutputGetDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public DirectorOutputGetDTO(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public static DirectorOutputGetDTO ToDirectorDTOMap(Director director)
        {
            return new DirectorOutputGetDTO(director.Id, director.Name);
        }
    }
}