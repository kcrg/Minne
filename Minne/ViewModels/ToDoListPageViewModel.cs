using Acr.UserDialogs;
using Minne.Models;
using Minne.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services;
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
        public DelegateCommand SettingsCommand { get; }
        public DelegateCommand<object> DeleteCommand { get; set; }
        public DelegateCommand<object> CompletedCommand { get; set; }
        public DelegateCommand<IReadOnlyList<object>> ItemTappedCommand { get; }

        public DelegateCommand LoadCommand { get; }
        public DelegateCommand RefreshCommand { get; }

        public ToDoListPageViewModel(IPageDialogService dialogService, IRestService restService)
        {
            this.restService = restService;

            ToDoList = new ObservableCollection<ToDoModel>();

            AddTaskCommand = new DelegateCommand(() => MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync("todolist/todocreate").ConfigureAwait(false)));
            SettingsCommand = new DelegateCommand(() => MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync("todolist/settings").ConfigureAwait(false)));
            ItemTappedCommand = new DelegateCommand<IReadOnlyList<object>>(async (o) => await ShowDetailsAsync(o, dialogService).ConfigureAwait(false));

            DeleteCommand = new DelegateCommand<object>((todoId) => DeleteTask(int.Parse(todoId.ToString())));
            CompletedCommand = new DelegateCommand<object>((todoId) => SortList(int.Parse(todoId.ToString())));

            LoadCommand = new DelegateCommand(async () => await LoadDataAsync().ConfigureAwait(false));
            RefreshCommand = new DelegateCommand(async () => await RefreshDataAsync().ConfigureAwait(false));
        }

        private void DeleteTask(int todoId)
        {
            bool isSuccess = restService.DeleteToDoAsync(int.Parse(todoId.ToString()));

            if (isSuccess)
            {
                var todoToDelete = ToDoList.Single(todo => todo.Id == todoId);
                ToDoList.Remove(todoToDelete);

                UserDialogs.Instance.Toast($"Task with ID {todoId} was deleted.");
            }
        }

        private void SortList(int todoId)
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

            try
            {
                var closestTodo = separatedToDos.OrderBy(x => Math.Abs((long)x.Id - checkedTodo.Id)).First();
                int closestTodoIndex = ToDoList.IndexOf(closestTodo);
                int checkedTodoIndex = ToDoList.IndexOf(checkedTodo);
                ToDoList.Move(checkedTodoIndex, closestTodoIndex);
            }
            catch
            {
                UserDialogs.Instance.Toast("Oops... Something went wrong, please try again.");
            }
        }

        public async Task ShowDetailsAsync(IReadOnlyList<object> obj, IPageDialogService dialogService)
        {
            //if (obj is null)
            //{
            //    return;
            //}

            //var list = (List<object>)obj;
            //var todo = list.ConvertAll(item => (ToDoModel)item).ToArray();

            //if (todo == null)
            //{
            //    throw new ArgumentException("Given contact is null");
            //}
            //string message = $"{todo[0].Title}\nCompleted: {todo[0].Completed}";

            //await dialogService.DisplayAlertAsync("Task", message, "Close").ConfigureAwait(false);
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

            var sortedList = response.OrderBy(x => x.Completed);

            foreach (var todo in sortedList)
            {
                ToDoList.Add(todo);
            }
            IsLoading = false;
        }
    }
}
