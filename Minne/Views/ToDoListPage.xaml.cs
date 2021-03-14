﻿using Minne.ViewModels;
using System;
using System.Linq;
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
            var btn = (ImageButton)sender;

            if ((BindingContext is ToDoListPageViewModel vm) && vm.DeleteCommand.CanExecute(btn.CommandParameter))
            {
                vm.DeleteCommand.Execute(btn.CommandParameter);
            }
        }

        private void Edit(object sender, EventArgs e)
        {
            //var btn = (ImageButton)sender;

            //int ID = int.Parse(btn.CommandParameter.ToString());
            //ToDoModel contactToEdit = await App.Database.GetContactAsync(ID).ConfigureAwait(false);

            //string contactJson = JsonConvert.SerializeObject(contactToEdit);
            //MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync($"todolist/todocreate?entry={""}").ConfigureAwait(false));
        }

        private void Completed(object sender, EventArgs e)
        {
            var btn = (ImageButton)sender;

            if ((BindingContext is ToDoListPageViewModel vm) && vm.CompletedCommand.CanExecute(btn.CommandParameter))
            {
                vm.CompletedCommand.Execute(btn.CommandParameter);

                //collectionView.ScrollTo(vm.ToDoList.IndexOf(vm.ToDoList.First(x => x.Id == (int)btn.CommandParameter)), position: ScrollToPosition.Center);
            }
        }
    }
}