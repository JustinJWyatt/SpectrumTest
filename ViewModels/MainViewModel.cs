using System;
using System.Reactive;
using ReactiveUI;

namespace SpectrumTest.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        private string _username = null;
        private string _password = null;
        private bool _isValid;

        public MainViewModel()
        {
        }

        public ReactiveCommand<Unit, Unit> SignInCommand { get; set; }
        public ReactiveCommand<Unit, Unit> SignUpCommand { get; set; }

        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                _isValid = value;
                this.RaiseAndSetIfChanged(ref _isValid, value);
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
                this.RaiseAndSetIfChanged(ref _password, value);
            }
        }
    }
}
