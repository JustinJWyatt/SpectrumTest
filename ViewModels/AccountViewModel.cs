using System;
using System.Reactive;
using System.Text.RegularExpressions;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;

namespace SpectrumTest.ViewModels
{
    public class AccountViewModel : ReactiveObject
    {
        private string _phoneNumber;
        private string _username;
        private string _password;
        private string _firstName;
        private string _lastName;
        private DateTime? _serviceStartDate;

        //ValidationContext IValidatableViewModel.ValidationContext => new ValidationContext();

        public ReactiveCommand<Unit, Unit> CreateAccountCommand { get; set; }
        public ReactiveCommand<Unit, Unit> AppearDateDialogCommand { get; set; }

        public AccountViewModel()
        {
            var specialChracters = new Regex(@"^[^!@#$%^&]+$");
            var lowerCaseLetter = new Regex(@"(?=.*[a-z])");
            var upperCaseLetter = new Regex(@"(?=.*[A-Z])");

            //this.ValidationRule(
            //    viewModel => viewModel.FirstName,
            //    name => !specialChracters.IsMatch(name),
            //    "First name should not have a special character");

            //this.ValidationRule(
            //    viewModel => viewModel.LastName,
            //    name => !specialChracters.IsMatch(name),
            //    "Last name should not have a special character");

            //this.ValidationRule(
            //    viewModel => viewModel.Password,
            //    password => password.Length < 8 || password.Length > 15,
            //    "Password must be between 8 and 15 characters");

            //this.ValidationRule(
            //    viewModel => viewModel.Password,
            //    password => !lowerCaseLetter.IsMatch(password) || !upperCaseLetter.IsMatch(password),
            //    "Password must have at least one lowercase and uppercase letter");

            //this.ValidationRule(
            //    viewModel => viewModel.Password,
            //    password => (password + password).IndexOf(password, 1) != password.Length,
            //    "Password must not have any repetitive sequence of characters");

            //this.ValidationRule(
            //    viewModel => viewModel.ServiceStartDate,
            //    date => ServiceStartDate.HasValue && ServiceStartDate.Value.Date < DateTime.Now.Date,
            //    "Past date is not allowed for service start date");
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                this.RaiseAndSetIfChanged(ref _phoneNumber, value);
            }
        }

        public string UserName
        {
            get { return _username; }
            set
            {
                _username = value;
                this.RaiseAndSetIfChanged(ref _username, value);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                this.RaiseAndSetIfChanged(ref _username, value);
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                this.RaiseAndSetIfChanged(ref _firstName, value);

            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                this.RaiseAndSetIfChanged(ref _lastName, value);
            }
        }

        public DateTime? ServiceStartDate
        {
            get { return _serviceStartDate; }
            set
            {
                _serviceStartDate = value;
                this.RaiseAndSetIfChanged(ref _serviceStartDate, value);
            }
        }
    }
}
