using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation.TabbedPages;
using Prism.Navigation;

namespace Minne.ViewModels
{
    public class TabbedPageViewModel : BindableBase
    {
        //private INavigationService navigationService;

        public TabbedPageViewModel(INavigationService navigationService)
        {
            //this.navigationService = navigationService;
        }

        //async void SelectTab(object parameters)
        //{
        //    var result = await navigationService.SelectTabAsync("Tab3");
        //}
    }
}
