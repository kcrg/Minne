using Acr.UserDialogs;
using Minne.Models;
using Minne.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Minne.ViewModels
{
    public class ToDoListPageViewModel : BindableBase, INavigatedAware
    {
        private readonly IRestService restService;

        private bool _isLoading = true;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private bool _isRefreshing;

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public ObservableCollection<ToDoModel> ToDoList { get; set; }
        public DelegateCommand AddTaskCommand { get; }
        public DelegateCommand<ToDoModel> DeleteCommand { get; set; }
        public DelegateCommand<ToDoModel> CompletedCommand { get; set; }
        public DelegateCommand RefreshCommand { get; }

        public ToDoListPageViewModel(IRestService restService)
        {
            this.restService = restService;

            ToDoList = new ObservableCollection<ToDoModel>();

            AddTaskCommand = new DelegateCommand(() => MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync("todolist/todocreate").ConfigureAwait(false)));

            DeleteCommand = new DelegateCommand<ToDoModel>((toDoModel) => DeleteTask(toDoModel));
            CompletedCommand = new DelegateCommand<ToDoModel>((toDoModel) => SortList(toDoModel));

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
                    SortList(toDoModel);

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

        private void DeleteTask(ToDoModel toDoModel)
        {
            bool isSuccess = restService.DeleteToDo(toDoModel.Id);

            if (isSuccess)
            {
                try
                {
                    ToDoList.Remove(toDoModel);

                    UserDialogs.Instance.Toast($"Task with ID {toDoModel.Id} was deleted.");
                }
                catch
                {
                }
            }
        }

        private void SortList(ToDoModel toDoModel)
        {
            try
            {
                int prevTodoIndex = ToDoList.ToList().FindLastIndex(t => t.Id < toDoModel.Id && t.Completed == toDoModel.Completed);
                int nextTodoIndex = ToDoList.ToList().FindIndex(t => t.Id > toDoModel.Id && t.Completed == toDoModel.Completed);
                int sortedTodoIndex = ToDoList.IndexOf(toDoModel);

                if (prevTodoIndex >= ToDoList.Count - 1)
                {
                    ToDoList.Remove(toDoModel);
                    ToDoList.Add(toDoModel);
                }
                else if (prevTodoIndex >= 0 && nextTodoIndex >= 0)
                {
                    if (sortedTodoIndex < prevTodoIndex && prevTodoIndex >= 0)
                    {
                        ToDoList.Remove(toDoModel);
                        ToDoList.Insert(prevTodoIndex, toDoModel);
                    }
                    else if (sortedTodoIndex > nextTodoIndex && nextTodoIndex >= 0)
                    {
                        ToDoList.Remove(toDoModel);
                        ToDoList.Insert(nextTodoIndex, toDoModel);
                    }
                }
                else if (prevTodoIndex < 0)
                {
                    if (nextTodoIndex == 0)
                    {
                        ToDoList.Move(sortedTodoIndex, nextTodoIndex);
                    }
                    else
                    {
                        ToDoList.Move(sortedTodoIndex, nextTodoIndex - 1);
                    }
                }
                else if (nextTodoIndex < 0)
                {
                    ToDoList.Move(sortedTodoIndex, prevTodoIndex + 1);
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

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            // Not used
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await LoadDataAsync().ConfigureAwait(false);
        }
    }
}
