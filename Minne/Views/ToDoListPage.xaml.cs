using Minne.Models;
using Minne.ViewModels;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Minne.Views
{
    [QueryProperty("Entry", "entry")]
    public partial class ToDoListPage : ContentPage
    {
        private string? Json;

        public string Entry
        {
            set => Json = Uri.UnescapeDataString(value);
        }

        public ToDoListPage()
        {
            InitializeComponent();

            Task.Run(async () => await LoadToDosAsync().ConfigureAwait(false));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (string.IsNullOrEmpty(Json))
            {
                return;
            }

            if (BindingContext is ToDoListPageViewModel vm)
            {
                var todo = JsonConvert.DeserializeObject<ToDoModel>(Json);
                bool isItemExists = vm.ToDoList.Any(t => t.Id == todo.Id);
                bool isTheSameItemExists = vm.ToDoList.Any(t => t.Title == todo.Title);

                if (isItemExists && !isTheSameItemExists)
                {
                    vm.UpdateTask(todo);
                }
                else if(!isTheSameItemExists)
                {
                    todo.Id = vm.ToDoList.Max(p => p.Id) + 1;
                    vm.AddTaskToList(todo);

                    collectionView.ScrollTo(todo, position: ScrollToPosition.Center, animate: false);
                }
            }
        }

        private async Task LoadToDosAsync()
        {
            if (BindingContext is ToDoListPageViewModel vm)
            {
                await vm.LoadDataAsync().ConfigureAwait(false);
            }
        }

        private void Delete(object sender, EventArgs e)
        {
            var btn = (ImageButton)sender;

            if ((BindingContext is ToDoListPageViewModel vm) && vm.DeleteCommand.CanExecute(btn.CommandParameter))
            {
                vm.DeleteCommand.Execute(btn.CommandParameter);
            }
        }

        private void Edit(object sender, EventArgs e)
        {
            var btn = (ImageButton)sender;
            var todoToEdit = btn.CommandParameter as ToDoModel;

            string todoJson = JsonConvert.SerializeObject(todoToEdit);
            MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync($"todolist/todocreate?entry={todoJson}").ConfigureAwait(false));
        }

        private void Completed(object sender, EventArgs e)
        {
            var btn = (ImageButton)sender;

            if ((BindingContext is ToDoListPageViewModel vm) && vm.CompletedCommand.CanExecute(btn.CommandParameter))
            {
                vm.CompletedCommand.Execute(btn.CommandParameter);

                collectionView.ScrollTo(vm.ToDoList.First(x => x.Id == (int)btn.CommandParameter), position: ScrollToPosition.Center, animate: false);
            }
        }
    }
}