using Minne.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Minne.ViewModels
{
    internal class ToDoListPageViewModel : BindableBase
    {
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
        public DelegateCommand<string>? DeleteTaskCommand { get; set; }
        public DelegateCommand<IReadOnlyList<object>> ItemTappedCommand { get; }

        public DelegateCommand LoadCommand { get; }

        public DelegateCommand RefreshCommand { get; }

        public ToDoListPageViewModel(IPageDialogService dialogService)
        {
            ToDoList = new ObservableCollection<ToDoModel>();

            AddTaskCommand = new DelegateCommand(() => MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync("todolist/todocreate").ConfigureAwait(false)));
            SettingsCommand = new DelegateCommand(() => MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync("todolist/settings").ConfigureAwait(false)));
            ItemTappedCommand = new DelegateCommand<IReadOnlyList<object>>(async (o) => await ShowDetailsAsync(o, dialogService).ConfigureAwait(false));

            LoadCommand = new DelegateCommand(async () => await LoadDataAsync().ConfigureAwait(false));
            RefreshCommand = new DelegateCommand(async () => await RefreshDataAsync().ConfigureAwait(false));
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
            string message = $"{todo[0].Description}\nCompleted: {todo[0].Completed}\nCreated: {todo[0].CreatedAt}";

            await dialogService.DisplayAlertAsync(todo[0].Title, message, "Close").ConfigureAwait(false);
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

            var client = new RestClient("https://mockend.com/kcrg/minne");
            var request = new RestRequest("todos", DataFormat.Json);
            var response = await client.GetAsync<List<ToDoModel>>(request).ConfigureAwait(false);

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
