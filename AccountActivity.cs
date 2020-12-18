using System;
using System.IO;
using Android.App;
using Android.OS;
using Android.Widget;
using ReactiveUI;
using Android.Content;
using SQLite;
using System.Linq;
using ClassLibrary2.ViewModels;
using ClassLibrary2.Validation;
using ClassLibrary2.Models;

namespace SpectrumTest
{
    [Activity(Label = "AccountActivity")]
    public class AccountActivity : ReactiveActivity, IViewFor<AccountViewModel>
    {
        private EditText FirstNameEditText { get; set; }
        private EditText LastNameEditText { get; set; }
        private EditText PhoneEditText { get; set; }
        private Button DateButton { get; set; }
        private EditText UsernameEditText { get; set; }
        private EditText PasswordEditText { get; set; }
        private Button SignupButton { get; set; }

        private AccountValidator AccountValidator => new AccountValidator();

        private string DBPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3");

        private IObservable<bool> CanSubmit
        {
            get
            {
                return this.WhenAnyValue(x => x.FirstNameEditText.Text,
                                         x => x.LastNameEditText.Text,
                                         x => x.PasswordEditText.Text,
                                         x => x.UsernameEditText.Text,
                                         x => x.PhoneEditText.Text,
                                         (firstName, lastName, password, username, phoneNumber) =>
                                         !string.IsNullOrWhiteSpace(firstName) &&
                                         !string.IsNullOrWhiteSpace(lastName) &&
                                         !string.IsNullOrWhiteSpace(password) &&
                                         !string.IsNullOrWhiteSpace(username) &&
                                         !string.IsNullOrWhiteSpace(phoneNumber));
            }
        }

        AccountViewModel _ViewModel;
        public AccountViewModel ViewModel
        {
            get { return _ViewModel; }
            set { this.RaiseAndSetIfChanged(ref _ViewModel, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (AccountViewModel)value; }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_account);

            FirstNameEditText = FindViewById<EditText>(Resource.Id.account_firstname);
            LastNameEditText = FindViewById<EditText>(Resource.Id.account_lastname);
            PhoneEditText = FindViewById<EditText>(Resource.Id.account_phone);
            PasswordEditText = FindViewById<EditText>(Resource.Id.account_password);
            UsernameEditText = FindViewById<EditText>(Resource.Id.account_username);
            DateButton = FindViewById<Button>(Resource.Id.account_date);

            SignupButton = FindViewById<Button>(Resource.Id.account_signup);

            ViewModel = new AccountViewModel();

            ViewModel.CreateAccountCommand = ReactiveCommand.Create(CreateAccount, CanSubmit);
            ViewModel.AppearDateDialogCommand = ReactiveCommand.Create(AppearDateDialog);

            this.BindCommand(this.ViewModel, vm => vm.CreateAccountCommand, v => v.SignupButton);
            this.BindCommand(this.ViewModel, vm => vm.AppearDateDialogCommand, v => v.DateButton);

            this.Bind(ViewModel, x => x.UserName, x => x.UsernameEditText.Text);
            this.Bind(ViewModel, x => x.Password, x => x.PasswordEditText.Text);
            this.Bind(ViewModel, x => x.PhoneNumber, x => x.PhoneEditText.Text);
            this.Bind(ViewModel, x => x.FirstName, x => x.FirstNameEditText.Text);
            this.Bind(ViewModel, x => x.LastName, x => x.LastNameEditText.Text);
        }

        private void CreateAccount()
        {
            var results = AccountValidator.Validate(ViewModel);

            if (!results.IsValid)
            {
                Android.Support.V7.App.AlertDialog alertDialog = null;
                Android.Support.V7.App.AlertDialog.Builder builder = new Android.Support.V7.App.AlertDialog.Builder(this)
                    .SetTitle("Invalid Model")
                    .SetMessage("- " + string.Join(" \n -", results.Errors.Select(x => x.ErrorMessage).ToList()))
                    .SetPositiveButton("Ok", (object s, Android.Content.DialogClickEventArgs dialogClickEventArgs) =>
                    {
                        alertDialog.Hide();
                    });

                alertDialog = builder.Create();
                alertDialog.Show();
            }
            else
            {
                try
                {
                    var db = new SQLiteConnection(DBPath);

                    var insert = db.Insert(new Account()
                    {
                        Password = ViewModel.Password,
                        UserName = ViewModel.UserName,
                        FirstName = ViewModel.FirstName,
                        LastName = ViewModel.LastName,
                        PhoneNumber = ViewModel.PhoneNumber,
                        ServiceDate = ViewModel.ServiceStartDate.Value
                    });

                    if (insert == 1)
                    {
                        var intent = new Intent(this, typeof(ConfirmationActivity));
                        StartActivity(intent);
                    }
                    else
                    {
                        //TODO: Let the user know the database wasn't updated
                    }
                }
                catch (Exception ex)
                {
                    //TODO: Let the user know something went wrong
                }
            }
        }

        private void AppearDateDialog()
        {
            DatePickerDialog datePickerDialog = new DatePickerDialog(this);
            Android.Support.V7.App.AlertDialog datePickerAlertDialog = null;
            Android.Support.V7.App.AlertDialog.Builder builder = new Android.Support.V7.App.AlertDialog.Builder(this)
                .SetTitle("Invalid Date")
                .SetMessage("Too early to create an account. Please select an earlier date under 30 days.")
                .SetPositiveButton("Ok", (object s, Android.Content.DialogClickEventArgs dialogClickEventArgs) =>
                {
                    datePickerAlertDialog.Show();
                });

            datePickerAlertDialog = builder.Create();

            datePickerDialog.DateSet += (s, e) =>
            {
                if (e.Date > DateTime.Now.AddDays(30))
                    datePickerAlertDialog.Show();
                else
                {
                    ViewModel.ServiceStartDate = e.Date;
                    DateButton.Text = e.Date.ToShortDateString();
                }
            };

            datePickerDialog.Show();
        }
    }
}
