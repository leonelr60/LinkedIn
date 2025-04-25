using MobileChurchGroups.Models;
using MobileChurchGroups.Services;
using MobileChurchGroups.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileChurchGroups.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
        public string sUser;
        public string sName;
        
        
        public Login ()
		{
            Title = "Login";
            InitializeComponent ();
           
		}

        private async void Validate(object sender, EventArgs args)
        {
            Title = "Login";

            if (txtUser.Text is null || txtPass.Text is null)
            {
                await DisplayAlert("Alert", "Email or Password could not be empty...", "OK");
                return;
            }
            if (txtUser.Text.Length == 0 || txtPass.Text.Length == 0)
            {
                await DisplayAlert("Alert", "Email or Password could not be empty...", "OK");
                return;
            }
            Member member = new Member();
            member.Email = txtUser.Text;
            member.id_group = "0"; //0 None
            member.Password = txtPass.Text;
            member.Avatar = "";
            member.Name = "";
            member.Type = "0"; //0 is student, 1 is Leader, 2 is Admin
            //Validates the email and save the member
            LoginMockDataStore oAddLogin = new LoginMockDataStore();
            Member resultmember = await oAddLogin.GetMembersAsyncByCode(member.Email);
            if (resultmember is null)
            {
                //Invalid User
                await DisplayAlert("Alert", "Invalid User or Password...", "OK");
            }
            else
            {
                if (resultmember.Password == member.Password)
                {
                    sUser = txtUser.Text;
                    sName = resultmember.Name;
                    string sType = resultmember.Type;
                    Application.Current.Resources["sUser"] = sUser;
                    Application.Current.Resources["sType"] = sType;
                    await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ItemsPage(sUser, sName, sType)));
                }
                else
                {
                    //Invalid Password
                    await DisplayAlert("Alert", "Invalid User or Password...", "OK");
                }
            }

        }

        private async void Register(object sender, EventArgs args)
        {
            //Every user is created as student, the admin is going to decide who has a different role
            if (txtUser.Text is null || txtPass.Text is null)
            {
                await DisplayAlert("Alert", "Email or Password could not be empty...", "OK");
                return;
            }
            if (txtUser.Text.Length == 0 || txtPass.Text.Length == 0)
            {
                await DisplayAlert("Alert", "Email or Password could not be empty...", "OK");
                return;
            }
            Member member = new Member();
            member.Email = txtUser.Text;
            member.id_group = "0"; //0 None
            member.Password = txtPass.Text;
            member.Avatar = "";
            member.Name = "";
            member.Type = "0"; //0 is student, 1 is Leader, 2 is Admin

            LoginViewModel oModel = new LoginViewModel();
            LoginMockDataStore oAddLogin = new LoginMockDataStore();
            //Validates the email and save the member
            Member resultmember = await oAddLogin.GetMemberAsync(member.Email);
            if(resultmember is null)
            {
                await oAddLogin.AddMemberAsync(member);
                await CreatePhoto(txtUser.Text);
                Validate(sender, args);
            }
            else
            {
                //Already exists
                await DisplayAlert("Alert", "The user already exists...", "OK");
            }

        }

        private async Task<bool> CreatePhoto(string sUser)
        {
            LoginViewModel oModel = new LoginViewModel();
            bool bFile = await oModel.CreatePhoto(sUser);
            return await Task.FromResult(bFile);
        }

        private void btnForgotPass_Clicked(object sender, EventArgs e)
        {

        }
    }
}