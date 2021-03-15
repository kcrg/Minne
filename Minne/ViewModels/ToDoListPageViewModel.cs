using Acr.UserDialogs;
using Minne.Models;
using Minne.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Minne.ViewModels
{
    public class ToDoListPageViewModel : BindableBase
    {
        private readonly IRestService restService;

        private bool _isLoading = true;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private bool _isRefreshing = false;

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public ObservableCollection<ToDoModel> ToDoList { get; set; }
        public DelegateCommand AddTaskCommand { get; }
        public DelegateCommand<object> DeleteCommand { get; set; }
        public DelegateCommand<object> CompletedCommand { get; set; }
        public DelegateCommand RefreshCommand { get; }

        public ToDoListPageViewModel(IRestService restService)
        {
            this.restService = restService;

            ToDoList = new ObservableCollection<ToDoModel>();

            AddTaskCommand = new DelegateCommand(() => MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync("todolist/todocreate").ConfigureAwait(false)));

            DeleteCommand = new DelegateCommand<object>((todoId) => DeleteTask(int.Parse(todoId.ToString())));
            CompletedCommand = new DelegateCommand<object>((todoId) => SortList(int.Parse(todoId.ToString())));

            RefreshCommand = new DelegateCommand(async () => await RefreshDataAsync().ConfigureAwait(false));
        }

        public void AddTaskToList(ToDoModel toDoModel)
        {
            try
            {
                bool isSuccess = restService.CreateToDo(toDoModel);

                if (isSuccess)
                {
                    ToDoList.Add(toDoModel);
                    SortList(toDoModel.Id);

                    UserDialogs.Instance.Toast($"Task with ID {toDoModel.Id} was created.");
                }
            }
            catch
            {
                UserDialogs.Instance.Toast("Oops... Something went wrong, please try again.");
            }
        }

        public void UpdateTask(ToDoModel toDoModel)
        {
            try
            {
                int taskToUpdateIndex = ToDoList.ToList().FindIndex(x => x.Id == toDoModel.Id);

                if (ToDoList.First(x => x.Id == toDoModel.Id).Title == toDoModel.Title)
                {
                    UserDialogs.Instance.Toast("There is nothing to update.");
                    return;
                }

                bool isSuccess = restService.UpdateToDo(toDoModel);

                if (isSuccess)
                {
                    ToDoList[taskToUpdateIndex] = toDoModel;

                    UserDialogs.Instance.Toast($"Task with ID {toDoModel.Id} was updated.");
                }
                else
                {
                    UserDialogs.Instance.Toast("This task ID is higher than any task in placeholder REST server, task can not be updated.");
                }
            }
            catch
            {
                UserDialogs.Instance.Toast("Oops... Something went wrong, please try again.");
            }
        }

        private void DeleteTask(int todoId)
        {
            bool isSuccess = restService.DeleteToDo(todoId);

            if (isSuccess)
            {
                try
                {
                    var todoToDelete = ToDoList.Single(todo => todo.Id == todoId);
                    ToDoList.Remove(todoToDelete);

                    UserDialogs.Instance.Toast($"Task with ID {todoId} was deleted.");
                }
                catch
                {
                }
            }
        }

        private void SortList(int todoId)
        {
            try
            {
                var checkedTodo = ToDoList.First(x => x.Id == todoId);
                var separatedToDos = new List<ToDoModel>();
                foreach (var todo in ToDoList)
                {
                    if (todo.Completed == checkedTodo.Completed && todo.Id != checkedTodo.Id)
                    {
                        separatedToDos.Add(todo);
                    }
                }

                var prevTodo = separatedToDos.LastOrDefault(t => t.Id < checkedTodo.Id);
                var nextTodo = separatedToDos.Find(t => t.Id > checkedTodo.Id);
                int prevTodoIndex = ToDoList.IndexOf(prevTodo);
                int nextTodoIndex = ToDoList.IndexOf(nextTodo);
                int checkedTodoIndex = ToDoList.IndexOf(checkedTodo);

                if (prevTodoIndex >= ToDoList.Count - 1)
                {
                    ToDoList.Remove(checkedTodo);
                    ToDoList.Add(checkedTodo);
                }
                else if (prevTodoIndex >= 0 && nextTodoIndex >= 0)
                {
                    if (checkedTodoIndex < prevTodoIndex && prevTodoIndex >= 0)
                    {
                        ToDoList.Remove(checkedTodo);
                        ToDoList.Insert(prevTodoIndex, checkedTodo);
                    }
                    else if (checkedTodoIndex > nextTodoIndex && nextTodoIndex >= 0)
                    {
                        ToDoList.Remove(checkedTodo);
                        ToDoList.Insert(nextTodoIndex, checkedTodo);
                    }
                }
                else if (prevTodoIndex < 0)
                {
                    if (nextTodoIndex == 0)
                    {
                        ToDoList.Move(checkedTodoIndex, nextTodoIndex);
                    }
                    else
                    {
                        ToDoList.Move(checkedTodoIndex, nextTodoIndex - 1);
                    }
                }
                else if (nextTodoIndex < 0)
                {
                    ToDoList.Move(checkedTodoIndex, prevTodoIndex + 1);
                }
            }
            catch (Exception)
            {
                UserDialogs.Instance.Toast("Oops... Something went wrong, please try again.");
            }
        }

        public async Task RefreshDataAsync()
        {
            IsRefreshing = true;
            ToDoList.Clear();
            await LoadDataAsync().ConfigureAwait(false);
            IsRefreshing = false;
        }

        public async Task LoadDataAsync()
        {
            if (ToDoList.Count > 0)
            {
                return;
            }

            var response = await restService.GetToDosAsync().ConfigureAwait(false);

            if (response is null)
            {
                return;
            }

            foreach (var todo in response.OrderBy(x => x.Completed))
            {
                ToDoList.Add(todo);
            }
            IsLoading = false;
        }
    }
}
