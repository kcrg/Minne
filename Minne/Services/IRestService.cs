using Minne.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minne.Services
{
    public interface IRestService
    {
        Task<List<ToDoModel>> GetToDosAsync();
        Task<ToDoModel> GetToDoAsync(int id);
        bool CreateToDo(ToDoModel contact);
        bool UpdateToDo(ToDoModel contact);
        bool DeleteToDo(int id);
    }
}
