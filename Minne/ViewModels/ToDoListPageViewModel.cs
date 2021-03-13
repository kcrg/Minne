using Acr.UserDialogs;
using Minne.Models;
using Minne.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services;
using RestSharp;
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

                UserDialogs.Instance.Toast($"To Do with id {todoId} was deleted.");
            }
        }

        public async Task ShowDetailsAsync(IReadOnlyList<object> obj, IPageDialogService dialogService)
        {
            if (obj is null)
            {
                return;
            }

            var list = (List<object>)obj;
            var todo = list.ConvertAll(item => (ToDoModel)item).ToArray();

            if (todo == null)
            {
                throw new ArgumentException("Given contact is null");
            }
            string message = $"{todo[0].Title}\nCompleted: {todo[0].Completed}";

            await dialogService.DisplayAlertAsync("Task", message, "Close").ConfigureAwait(false);
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

            foreach (var todo in response)
            {
                ToDoList.Add(todo);
            }
            IsLoading = false;
        }
    }
}
