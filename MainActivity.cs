using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Android.Views;
using System.IO;
using SQLite;
using Android.Content;
using ReactiveUI;
using ClassLibrary2.ViewModels;
using ClassLibrary2.Models;

namespace SpectrumTest
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : ReactiveActivity, IViewFor<MainViewModel>
    {
        private EditText UserNameEditText { get; set; }
        private EditText PasswordEditText { get; set; }
        private Button SigninButton { get; set; }
        private TextView SignupButton { get; set; }

        private string DBPath => Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3");

        MainViewModel _ViewModel;
        public MainViewModel ViewModel
        {
            get { return _ViewModel; }
            set { this.RaiseAndSetIfChanged(ref _ViewModel, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (MainViewModel)value; }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.activity_main);

            UserNameEditText = FindViewById<EditText>(Resource.Id.username);
            PasswordEditText = FindViewById<EditText>(Resource.Id.password);

            SigninButton = FindViewById<Button>(Resource.Id.signin);
            SignupButton = FindViewById<TextView>(Resource.Id.signup);

            ViewModel = new MainViewModel();

            var canSubmit = this.WhenAnyValue(x => x.UserNameEditText.Text, x => x.PasswordEditText.Text, (username, password) => !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password));

            ViewModel.SignInCommand = ReactiveCommand.Create(SignIn, canSubmit);
            ViewModel.SignUpCommand = ReactiveCommand.Create(SignUp);

            this.BindCommand(this.ViewModel, vm => vm.SignInCommand, v => v.SigninButton);
            this.BindCommand(this.ViewModel, vm => vm.SignUpCommand, v => v.SignupButton);

            this.Bind(this.ViewModel, x => x.UserName, x => x.UserNameEditText.Text);
            this.Bind(this.ViewModel, x => x.Password, x => x.PasswordEditText.Text);

            CreateDatabase(DBPath);
        }

        private void CreateDatabase(string path)
        {
            var db = new SQLiteConnection(path);
            db.CreateTable<Account>();
        }

        private void SignUp()
        {
            var intent = new Intent(this, typeof(AccountActivity));
            StartActivity(intent);
        }

        private void SignIn()
        {
            var db = new SQLiteConnection(DBPath);
            var accounts = db.Table<Account>();
            var account = accounts.Where(x => x.UserName == ViewModel.UserName).FirstOrDefault();

            Android.Support.V7.App.AlertDialog alertDialog = null;
            Android.Support.V7.App.AlertDialog.Builder builder = new Android.Support.V7.App.AlertDialog.Builder(this)
                .SetTitle("Account Information")
                .SetPositiveButton("Ok", (object s, Android.Content.DialogClickEventArgs dialogClickEventArgs) =>
                {
                    alertDialog.Show();
                });

            alertDialog = builder.Create();

            if (account == null)
                alertDialog.SetTitle("This account does not exist.");
            else if (account.Password != ViewModel.Password)
                alertDialog.SetTitle("Password is incorrect.");
            else
                alertDialog.SetTitle("User successfully found.");

            alertDialog.Show();
        }
    }
}