using Minne.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Minne.ViewModels
{
    public class ToDoCreatePageViewModel : BindableBase
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public bool Completed { get; set; }

        public string? _validateMessage;
        public string? ValidateMessage
        {
            get => _validateMessage;
            set => SetProperty(ref _validateMessage, value);
        }

        public DelegateCommand SaveContactCommand { get; }

        public DelegateCommand BackCommand { get; }

        public ToDoCreatePageViewModel()
        {
            SaveContactCommand = new DelegateCommand(AddToDo);

            BackCommand = new DelegateCommand(() => MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync($"..?entry={string.Empty}").ConfigureAwait(false)));
        }

        public void AddToDo()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                ValidateMessage = "Please complete the entry.";
            }
            else
            {
                var todoToCreate = new ToDoModel()
                {
                    Id = Id,
                    Title = Title.Replace("\n", " "),
                    UserId = default,
                    Completed = Completed
                };

                string todoJson = JsonConvert.SerializeObject(todoToCreate);
                MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync($"..?entry={todoJson}").ConfigureAwait(false));
            }
        }
    }
}
