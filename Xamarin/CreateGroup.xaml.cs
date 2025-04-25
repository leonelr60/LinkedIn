using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MobileChurchGroups.Models;
using MobileChurchGroups.ViewModels;
using MobileChurchGroups.Services;

namespace MobileChurchGroups.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateGroup : ContentPage
	{
        public Group Group { get; set; }

        public CreateGroup ()
		{
			InitializeComponent ();
            Group = new Group
            {
                Id = "",
                Text = ""
            };

            BindingContext = this;
            txtNumber.Focus();
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        async void ButtonSave_Clicked(object sender, EventArgs e)
        {
            
            GMockDataStore gMockDataStore = new GMockDataStore();
            Group = new Group
            {
                Id = txtNumber.Text,
                Text = txtName.Text 
            };
            if (gMockDataStore.GetGroupAsyncByCode(txtNumber.Text).Result is null)
            {
                await gMockDataStore.AddGroupAsync(Group);
            }
            else
            {
                if (gMockDataStore.GetGroupAsyncByCode(txtNumber.Text).Result.Text.Length > 0)
                {
                    await gMockDataStore.UpdateGroupAsync(Group);
                }
                else
                {
                    await gMockDataStore.AddGroupAsync(Group);
                }
            }
            
            
            Group = new Group
            {
                Id = "",
                Text = ""
            };

            BindingContext = this;
            txtNumber.Text = "";
            txtName.Text = "";
            txtNumber.Focus();
        }

        async void ButtonDelete_Clicked(object sender, EventArgs e)
        {
            GMockDataStore gMockDataStore = new GMockDataStore();
            Group = new Group
            {
                Id = txtNumber.Text,
                Text = txtName.Text
            };
            if (gMockDataStore.GetGroupAsyncByCode(txtNumber.Text).Result.Text.Length > 0)
            {
                await gMockDataStore.DeleteGroupAsync(Group);
            }
            

            Group = new Group
            {
                Id = "",
                Text = ""
            };

            BindingContext = this;
            txtNumber.Text = "";
            txtName.Text = "";
            txtNumber.Focus();
        }
    }
}