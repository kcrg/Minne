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

        public bool CreateToDoAsync(ToDoModel contact)
        {
            var request = new RestRequest("todos", Method.POST, DataFormat.Json);
            var response = restClient.Post(request);
            return response.IsSuccessful;
        }

        public bool UpdateToDoAsync(ToDoModel contact)
        {
            var request = new RestRequest("todos", Method.PUT, DataFormat.Json);
            var response = restClient.Put(request);
            return response.IsSuccessful;
        }

        public bool DeleteToDoAsync(int id)
        {
            var request = new RestRequest($"todos/{id}", Method.DELETE, DataFormat.Json);
            var response = restClient.Delete(request);
            return response.IsSuccessful;
        }
    }
}
