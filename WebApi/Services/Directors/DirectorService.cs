using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Services
{
    public class DirectorService : IDirectorService
    {
        private readonly Context _context;
        public DirectorService(Context context)
        {
            _context = context;
        }

        public async Task<List<Director>> GetAll() 
        {
            var diretores = await _context.Directors.ToListAsync();

            if (!diretores.Any()) {
                throw new System.Exception("There are no registered Directors.");
            }

            return diretores;
        }
    }
}