using Minne.Models;
using Newtonsoft.Json;
using System;
using Xamarin.Forms;

namespace Minne.Views
{
    [QueryProperty("Entry", "entry")]
    public partial class ToDoCreatePage : ContentPage
    {
        private string? Json;

        public string Entry
        {
            set => Json = Uri.UnescapeDataString(value);
        }

        public ToDoCreatePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!string.IsNullOrEmpty(Json))
            {
                Title = "Edit";
                var contactToEdit = JsonConvert.DeserializeObject<ToDoModel>(Json);

                if (contactToEdit is null)
                {
                    return;
                }

                IDLabel.Text = contactToEdit.Id.ToString();
                TitleEntry.Text = contactToEdit.Title;
                checkBox.IsChecked = contactToEdit.Completed;
            }
            else
            {
                Title = "Add";
                checkBoxWithShadows.IsVisible = false;
                welcomeLabel.Text = "You are adding a new task.";
            }
        }
    }
}