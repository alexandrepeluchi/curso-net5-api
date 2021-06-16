using System.Collections.Generic;

namespace WebApi.Models
{
    public class Director
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<Movie> Movies { get; set; }

        public Director()
        {
            Movies = new List<Movie>();
        }
    }
}