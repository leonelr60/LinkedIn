using MobileChurchGroups.Models;
using MobileChurchGroups.Views;
using MobileChurchGroups.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileChurchGroups.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Leaders : ContentPage
	{
        LeadersViewModel viewModel;
        public string sValType = "";
        public string sEmail = "";
        public string sGroup = "";
        public Leaders ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new LeadersViewModel();
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            try
            {
                var leader = args.SelectedItem as Leader;

                if (leader.Id is null)
                {
                    return;
                }
                await Navigation.PushAsync(new MemberProfiles(leader.Id, true,sEmail, sValType));


                // Manually deselect item.
                LeadersListView.SelectedItem = null;
            }
            catch (Exception ex)
            {
                string smess = ex.Message;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            sEmail = Application.Current.Resources["sUser"].ToString();
            sValType = Application.Current.Resources["sType"].ToString();
            if(Application.Current.Resources["sMemberGroup"] != null)
            {
                sGroup = Application.Current.Resources["sMemberGroup"].ToString();
            }
            
            if (viewModel.Leaders.Count == 0)
                viewModel.LoadLeadersCommand.Execute(null);

            
        }
    }
}