using Minne.Models;
using Minne.ViewModels;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Minne.Views
{
    //[QueryProperty("Entry", "entry")]
    public partial class ToDoListPage : ContentPage
    {
        private string? Json;

        //public string Entry
        //{
        //    set => Json = Uri.UnescapeDataString(value);
        //}

        public ToDoListPage()
        {
            InitializeComponent();

            //Task.Run(async () => await LoadToDosAsync().ConfigureAwait(false));
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

                if(todo is null)
                {
                    return;
                }

                bool isItemExists = vm.ToDoList.Any(t => t.Id == todo.Id);
                bool isTheSameItemExists = vm.ToDoList.Any(t => t.Title == todo.Title);

                if (isItemExists && !isTheSameItemExists)
                {
                    vm.UpdateTask(todo);
                }
                else if (!isTheSameItemExists)
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

            if ((BindingContext is ToDoListPageViewModel vm) && btn.CommandParameter is ToDoModel toDoModel && vm.DeleteCommand.CanExecute(toDoModel))
            {
                vm.DeleteCommand.Execute(toDoModel);
            }
        }

        private void Edit(object sender, EventArgs e)
        {
            var btn = (ImageButton)sender;

            string todoJson = JsonConvert.SerializeObject((ToDoModel)btn.CommandParameter);
            MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync($"todolist/todocreate?entry={todoJson}").ConfigureAwait(false));
        }

        private void Completed(object sender, EventArgs e)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var btn = (ImageButton)sender;

            if ((BindingContext is ToDoListPageViewModel vm) && btn.CommandParameter is ToDoModel toDoModel && vm.CompletedCommand.CanExecute(toDoModel))
            {
                Task.WhenAll(

               ).ConfigureAwait(false);

                vm.CompletedCommand.Execute(toDoModel);

                collectionView.ScrollTo(toDoModel, position: ScrollToPosition.Center, animate: false);
            }

            Console.WriteLine($"############################################################### Sort Time: {stopwatch.ElapsedMilliseconds}");
        }
    }
}