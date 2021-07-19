using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IDirectorService
    {
        Task<List<Director>> GetAll();
    }
}