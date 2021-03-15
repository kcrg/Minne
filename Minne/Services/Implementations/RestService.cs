using Minne.Models;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minne.Services.Implementations
{
    public class RestService : IRestService
    {
        private readonly RestClient restClient = new("https://jsonplaceholder.typicode.com"); //https://mockend.com/kcrg/minne

        public RestService()
        {
        }

        public async Task<ToDoModel> GetToDoAsync(int id)
        {
            var request = new RestRequest($"todos/{id}", Method.GET, DataFormat.Json);
            return await restClient.GetAsync<ToDoModel>(request).ConfigureAwait(false);
        }

        public async Task<List<ToDoModel>> GetToDosAsync()
        {
            var request = new RestRequest("todos", Method.GET, DataFormat.Json);
            return await restClient.GetAsync<List<ToDoModel>>(request).ConfigureAwait(false);
        }

        public bool CreateToDo(ToDoModel todo)
        {
            var request = new RestRequest("todos", Method.POST, DataFormat.Json);
            request.AddObject(todo);
            var response = restClient.Post(request);
            return response.IsSuccessful;
        }

        public bool UpdateToDo(ToDoModel todo)
        {
            var request = new RestRequest($"todos/{todo.Id}", Method.PUT, DataFormat.Json);
            request.AddObject(todo);
            var response = restClient.Put(request);
            return response.IsSuccessful;
        }

        public bool DeleteToDo(int id)
        {
            var request = new RestRequest($"todos/{id}", Method.DELETE, DataFormat.Json);
            var response = restClient.Delete(request);
            return response.IsSuccessful;
        }
    }
}
