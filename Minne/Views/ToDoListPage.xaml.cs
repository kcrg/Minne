﻿using Minne.ViewModels;
using System;
using Xamarin.Forms;

namespace Minne.Views
{
    public partial class ToDoListPage : ContentPage
    {
        public ToDoListPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            LoadDatabase();
        }

        private void LoadDatabase()
        {
            if ((BindingContext is ToDoListPageViewModel vm) && vm.LoadCommand.CanExecute())
            {
                vm.LoadCommand.Execute();
            }
        }

        private void Delete(object sender, EventArgs e)
        {
            //var btn = (ImageButton)sender;

            //int Id = int.Parse(btn.CommandParameter.ToString());
            //ToDoModel contactToDelete = await App.Database.GetContactAsync(ID).ConfigureAwait(false);

            //await App.Database.DeleteContactAsync(contactToDelete).ConfigureAwait(false);

            LoadDatabase();
        }

        private void Edit(object sender, EventArgs e)
        {
            //var btn = (ImageButton)sender;

            //int ID = int.Parse(btn.CommandParameter.ToString());
            //ToDoModel contactToEdit = await App.Database.GetContactAsync(ID).ConfigureAwait(false);

            //string contactJson = JsonConvert.SerializeObject(contactToEdit);
            //MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync($"todolist/todocreate?entry={""}").ConfigureAwait(false));
        }
    }
}